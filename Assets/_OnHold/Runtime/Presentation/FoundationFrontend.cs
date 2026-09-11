using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using OnHold.Core;
using OnHold.Domain;
using OnHold.Gameplay;
using OnHold.Networking;
using OnHold.Persistence;

namespace OnHold.Presentation
{
    public sealed class FoundationFrontend : MonoBehaviour
    {
        public NetworkSession Network;
        public FoundationWorld World;
        public Camera ViewCamera;
        public InputActionAsset Actions;
        LocalSettings settings = new LocalSettings();
        CampaignStore store;
        Campaign campaign;
        bool menu = true, settingsOpen, abortConfirm, holding, wasFocused = true;
        string endpoint = "127.0.0.1", code = "", feedback = "", saveError = "";
        double nextInput, feedbackUntil;
        float yaw, pitch;
        InputActionMap gameplay;
        InputAction move, look, grab, interact, secure, pause, ping, jump, manipulate;
        CarryableBody heldCargo;
        bool manipulationActive;
        public bool ManipulationActive => manipulationActive && heldCargo && heldCargo.CanManipulate(Network.LocalActor);
        bool soloStarting, feedbackGamepad;
        SessionPhase? soloSentPhase;
        Vector2 lastMove;
        PlayerBody hiddenPlayer;
        uint readyRevision = uint.MaxValue;
        SessionPhase previousPhase;
        public string SaveDirectory { get; private set; }
        readonly InteractionFeedbackPresenter interactionPresenter = new InteractionFeedbackPresenter();
        public InteractionFeedback InteractionFeedback { get; private set; }
        public string InteractionPrompt => interactionPresenter.Prompt;
        public bool GripMarkerVisible => interactionPresenter.MarkerVisible;
        [Range(8, 24)] public float GripMarkerSize = 14;
        public bool MenuOpen => menu;
        public LocalSettings Settings => settings;
        void Awake()
        {
            SaveDirectory = Argument("--save-dir") ?? Application.persistentDataPath;
            store = new CampaignStore(Path.Combine(SaveDirectory, "campaign.json"));
            try { campaign = store.Load(); } catch (IOException e) { saveError = e.Message; }
            try
            {
                var path = Path.Combine(SaveDirectory, "settings.json");
                if (File.Exists(path)) { settings = JsonUtility.FromJson<LocalSettings>(File.ReadAllText(path)); settings.Validate(); }
            }
            catch (Exception e) when (e is IOException || e is ArgumentException) { feedback = "Settings could not be read. Defaults are active."; settings = new LocalSettings(); }
            Actions = Instantiate(Actions); if (!string.IsNullOrEmpty(settings.InputOverrides)) Actions.LoadBindingOverridesFromJson(settings.InputOverrides);
            gameplay = Actions.FindActionMap("Gameplay", true);
            move = gameplay.FindAction("Move"); look = gameplay.FindAction("Look"); grab = gameplay.FindAction("Grab"); interact = gameplay.FindAction("Interact"); secure = gameplay.FindAction("Secure"); ping = gameplay.FindAction("Ping");
            jump = gameplay.FindAction("Jump", true); manipulate = gameplay.FindAction("Manipulate", true);
            pause = Actions.FindAction("UI/Pause"); pause.Enable();
            Network.Rejected += Rejection; World.SettlementRequested += Settle;
            Application.runInBackground = true; SetMenu(true);
        }
        async void Start()
        {
            Network.Profile = Argument("--profile") ?? "N0";
            var role = Argument("--role");
            if (role != null) await Network.StartConnection(role == "host", Argument("--relay") != null, Argument("--solo") != null ? OnHold.Domain.SessionMode.Solo : OnHold.Domain.SessionMode.Coop, Argument("--relay") ?? Argument("--address") ?? "127.0.0.1", ushort.Parse(Argument("--port") ?? "7777"), Argument("--auth-profile") ?? "default");
        }
        public async System.Threading.Tasks.Task PlaySolo()
        {
            if (Network.Busy || Network.Connected || soloStarting) return;
            soloStarting = true; soloSentPhase = null;
            await Network.StartConnection(true, false, SessionMode.Solo, "127.0.0.1");
            if (!Network.Connected) soloStarting = false;
        }
        public static string Argument(string key)
        {
            var args = Environment.GetCommandLineArgs(); int i = Array.IndexOf(args, key); return i >= 0 && i + 1 < args.Length ? args[i + 1] : null;
        }
        WorldFrame Frame => World.IsAuthority && World.Session != null ? World.Capture() : World.LatestFrame;
        void Update()
        {
            var frame = Frame; var phase = frame == null ? SessionPhase.Lobby : (SessionPhase)frame.Phase;
            if (phase != previousPhase) { previousPhase = phase; SetMenu(phase != SessionPhase.Active); }
            if (Network.Connected && phase == SessionPhase.Loading && readyRevision != (uint)phase)
            { readyRevision = (uint)phase; Network.Send(CommandType.Ready); }
            if (phase != SessionPhase.Loading) readyRevision = uint.MaxValue;
            if (soloStarting && Network.Connected && soloSentPhase != phase)
            {
                soloSentPhase = phase;
                if (phase == SessionPhase.Lobby || phase == SessionPhase.Loading || phase == SessionPhase.Briefing) Network.Send(CommandType.Start);
                if (phase == SessionPhase.Active) soloStarting = false;
            }
            if (!Network.Connected) { if (!Network.Busy) soloStarting = false; holding = false; hiddenPlayer = null; if (!menu) SetMenu(true); }
            if (pause.WasPressedThisFrame()) SetMenu(!menu);
            if (wasFocused && !Application.isFocused) SetMenu(true); wasFocused = Application.isFocused;
            if (World.IsAuthority)
            {
                CarryableBody nextHeld = null;
                foreach (var item in World.Items) if (item && item.State != null && item.State.IsHeldBy(Network.LocalActor)) { nextHeld = item; break; }
                if (heldCargo != nextHeld) { StopManipulation(); heldCargo = nextHeld; }
                holding = heldCargo != null;
            }
            if (!menu && Network.Connected && phase == SessionPhase.Active)
            {
                Vector2 delta = look.ReadValue<Vector2>();
                RouteLookInput(delta, look.activeControl?.device is Gamepad, Time.deltaTime, manipulate.IsPressed());
                var input = move.ReadValue<Vector2>();
                bool driver = frame.Driver == Network.LocalActor.Id;
                if (Time.realtimeSinceStartupAsDouble >= nextInput || input != lastMove)
                {
                    nextInput = Time.realtimeSinceStartupAsDouble + .05; lastMove = input;
                    Network.Send(driver ? CommandType.Winch : CommandType.Move, input: driver ? new Vector3(0, input.y, 0) : new Vector3(input.x, 0, input.y), yaw: yaw, pitch: pitch);
                }
                if (look.WasPerformedThisFrame()) feedbackGamepad = look.activeControl?.device is Gamepad;
                if (move.WasPerformedThisFrame()) feedbackGamepad = move.activeControl?.device is Gamepad;
                if (grab.WasPerformedThisFrame()) feedbackGamepad = grab.activeControl?.device is Gamepad;
                RefreshInteractionFeedback();
                if (!driver && jump.WasPressedThisFrame()) Network.Send(CommandType.Jump);
                if (grab.WasPressedThisFrame())
                {
                    if (settings.ToggleHold && holding) Release();
                    else { var item = Target(); if (item && (!World.IsAuthority || InteractionFeedback.Actionable)) { Network.Send(CommandType.Grab, item.NetworkId, Revision(item)); if (!World.IsAuthority) holding = true; } }
                }
                if (!settings.ToggleHold && grab.WasReleasedThisFrame()) Release();
                if (interact.WasPressedThisFrame()) Network.Send(driver ? CommandType.ReleaseLease : CommandType.Lease);
                if (secure.WasPressedThisFrame())
                {
                    var item = Target();
                    if (item) { var state = frame.Items.First(f => f.Id == item.NetworkId); Network.Send(state.Handling == (int)Handling.Secured ? CommandType.Unsecure : CommandType.Secure, item.NetworkId, state.Revision); }
                }
                if (ping.WasPressedThisFrame()) Network.Send(CommandType.Ping);
            }
            ViewCamera.fieldOfView = settings.FieldOfView; AudioListener.volume = settings.MasterVolume;
        }
        void LateUpdate()
        {
            if (Network.Connected && World.Players.TryGetValue(Network.LocalActor.Id, out var local) && local)
            {
                ViewCamera.transform.SetPositionAndRotation(local.Eye, Quaternion.Euler(pitch, yaw, 0));
                if (hiddenPlayer != local)
                { hiddenPlayer = local; foreach (var renderer in local.GetComponentsInChildren<Renderer>()) renderer.enabled = false; }
            }
            else ViewCamera.transform.SetPositionAndRotation(new Vector3(9, 7, -10), Quaternion.Euler(25, -35, 0));
            RefreshInteractionFeedback();
        }
        public void RouteLookInput(Vector2 delta, bool stick, float dt, bool requested)
        {
            bool eligible = !menu && Network.Connected && isActiveAndEnabled && gameplay.enabled &&
                heldCargo && heldCargo.CanManipulate(Network.LocalActor);
            bool next = eligible && requested;
            // Discard the transition sample on both boundaries; a mouse delta is never replayed to the camera.
            if (next != manipulationActive)
            {
                manipulationActive = next;
                if (next) heldCargo.UpdateManipulation(Network.LocalActor, true, Vector2.zero, ViewCamera.transform.rotation, dt);
                else if (heldCargo) heldCargo.EndManipulation(Network.LocalActor);
                return;
            }
            if (next)
            {
                float scale = stick ? World.Config.ManipulationSpeed * dt : World.Config.ManipulationSensitivity;
                if (!Numbers.Finite(delta.x) || !Numbers.Finite(delta.y) || !Numbers.Finite(scale)) return;
                // Clamp before float multiplication, including synthetic or malformed input magnitudes.
                var degrees = new Vector2((float)Numbers.Clamp((double)delta.x * scale, -360, 360),
                    (float)Numbers.Clamp((double)delta.y * scale, -360, 360));
                if (settings.InvertY) degrees.y = -degrees.y;
                heldCargo.UpdateManipulation(Network.LocalActor, true, degrees, ViewCamera.transform.rotation, dt);
            }
            else ApplyLook(delta, stick, dt);
        }
        void StopManipulation()
        {
            if (heldCargo && Network) heldCargo.EndManipulation(Network.LocalActor);
            manipulationActive = false;
        }
        public void ApplyLook(Vector2 delta, bool stick, float dt)
        {
            if (menu || !Network.Connected || !World.Players.TryGetValue(Network.LocalActor.Id, out var local) || !local || !Numbers.Finite(delta.x) || !Numbers.Finite(delta.y)) return;
            float scale = stick ? World.Config.StickLookSpeed * dt : settings.Sensitivity;
            yaw = Mathf.Repeat(yaw + delta.x * scale, 360);
            pitch = Mathf.Clamp(pitch + delta.y * scale * (settings.InvertY ? 1 : -1), World.Config.PitchMin, World.Config.PitchMax);
            ViewCamera.transform.SetPositionAndRotation(local.Eye, Quaternion.Euler(pitch, yaw, 0));
        }
        public void RefreshInteractionFeedback()
        {
            InteractionFeedback = InteractionRay.QueryFeedback(World, Network.LocalActor, ViewCamera.transform.position,
                ViewCamera.transform.forward, !menu && Network.Connected);
            interactionPresenter.Update(InteractionFeedback, grab, settings.ToggleHold,
                feedbackGamepad);
        }
        CarryableBody Target() => InteractionFeedback.Target.Item;
        uint Revision(CarryableBody item) => Frame?.Items.FirstOrDefault(f => f.Id == item.NetworkId)?.Revision ?? 0;
        void Release()
        {
            // Authority reconciliation in Update clears control only after the queued release commits.
            Network.Send(CommandType.Release);
            if (!World.IsAuthority) holding = false;
        }
        public void SetMenu(bool value)
        {
            menu = value || !Network.Connected || Frame == null || (SessionPhase)Frame.Phase != SessionPhase.Active;
            nextInput = 0; lastMove = Vector2.zero;
            if (menu) { StopManipulation(); Network.Send(CommandType.StopInput); Release(); Network.Send(CommandType.ReleaseLease); gameplay?.Disable(); }
            else gameplay?.Enable();
            Cursor.lockState = menu ? CursorLockMode.None : CursorLockMode.Locked; Cursor.visible = menu;
            RefreshInteractionFeedback();
        }
        void Rejection(Reason reason) { feedback = Explain(reason); feedbackUntil = Time.realtimeSinceStartupAsDouble + 4; }
        string Explain(Reason r)
        {
            switch (r)
            {
                case Reason.TooFar: return "Move closer to the item or winch panel.";
                case Reason.Busy: return "Both grips or the selected control are occupied.";
                case Reason.InvalidState: return "Release the item first and check the current phase.";
                case Reason.UnsafeSpeed: return "Stop the winch before changing straps.";
                case Reason.Blocked: return "Move the item clear of obstructions and try again.";
                case Reason.NotReady: return "A new run needs at least two players, with everyone loaded and ready.";
                case Reason.StaleRevision: return "The item changed. Look at it and try again.";
                case Reason.VoteRequired: return "Vote recorded. Host and a majority must agree.";
                default: return "Action unavailable. Try again.";
            }
        }
        void Settle(RunResult result)
        {
            if (!Network.IsHost) return;
            try { campaign = store.Settle(result); World.CompleteSettlement(campaign.Balance, campaign.Revision); saveError = ""; }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException || e is ArgumentException || e is OverflowException)
            { saveError = "Save failed. Previous checkpoint is preserved. Retry uses the same run transaction."; }
        }
        void SaveSettings()
        {
            try
            {
                settings.InputOverrides = Actions.SaveBindingOverridesAsJson(); settings.Validate(); Directory.CreateDirectory(SaveDirectory);
                string path = Path.Combine(SaveDirectory, "settings.json"), pending = path + ".pending";
                File.WriteAllText(pending, JsonUtility.ToJson(settings, true)); if (File.Exists(path)) File.Replace(pending, path, path + ".bak"); else File.Move(pending, path);
                feedback = "Local settings saved."; feedbackUntil = Time.realtimeSinceStartupAsDouble + 3;
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException || e is ArgumentException) { feedback = "Local settings could not be saved."; }
        }
        void OnGUI()
        {
            GUI.matrix = Matrix4x4.Scale(Vector3.one * settings.UiScale);
            GUI.skin.label.fontSize = 16; GUI.skin.button.fontSize = 16; GUI.skin.textField.fontSize = 16;
            var frame = Frame;
            if (!menu && frame != null)
            {
                GUILayout.BeginArea(new Rect(24, 20, 440, 160), GUI.skin.box);
                GUILayout.Label("ON HOLD  /  FOUNDATION");
                GUILayout.Label($"{TimeSpan.FromSeconds(frame.Remaining):mm\\:ss}   •   Delivered {frame.Delivered} / 2   •   Provisional {frame.Provisional}");
                GUILayout.Label(frame.Driver == Network.LocalActor.Id ? "WINCH • Move up/down to travel • Interact to leave" : "Carry together • Balance the deck • Deliver to the green bay");
                if (frame.MandatoryLost) GUILayout.Label("Mandatory item lost. Partial payout is still possible at timeout.");
                GUILayout.EndArea();
                interactionPresenter.Draw(settings.UiScale, ViewCamera, GripMarkerSize);
            }
            if (menu)
            {
                GUILayout.BeginArea(new Rect(28, 28, 490, Mathf.Min(760, Screen.height / settings.UiScale - 56)), GUI.skin.box);
                GUILayout.Label("ON HOLD", new GUIStyle(GUI.skin.label) { fontSize = 34, fontStyle = FontStyle.Bold });
                GUILayout.Label("Cooperative lifting · foundation test build"); GUILayout.Space(12);
                GUILayout.Label(Network.Status);
                if (!Network.Connected)
                {
                    GUI.enabled = !Network.Busy;
                    if (GUILayout.Button("PLAY SOLO", GUILayout.Height(36))) _ = PlaySolo();
                    GUILayout.Space(12);
                    if (GUILayout.Button("Host with Unity Relay", GUILayout.Height(36))) _ = Network.StartConnection(true, true, OnHold.Domain.SessionMode.Coop, "");
                    GUILayout.Label("Relay join code"); code = GUILayout.TextField(code, 12);
                    if (GUILayout.Button("Join Relay", GUILayout.Height(32))) _ = Network.StartConnection(false, true, OnHold.Domain.SessionMode.Coop, code);
                    GUILayout.Space(8); GUILayout.Label("Local process test"); endpoint = GUILayout.TextField(endpoint, 64);
                    GUILayout.BeginHorizontal(); if (GUILayout.Button("Local host")) _ = Network.StartConnection(true, false, OnHold.Domain.SessionMode.Coop, "127.0.0.1"); if (GUILayout.Button("Local client")) _ = Network.StartConnection(false, false, OnHold.Domain.SessionMode.Coop, endpoint); GUILayout.EndHorizontal(); GUI.enabled = true;
                    if (Network.Busy && GUILayout.Button("Cancel connection")) Network.Stop();
                }
                else if (frame != null)
                {
                    var phase = (SessionPhase)frame.Phase;
                    GUILayout.Label(phase + "  •  " + frame.Connections + " / 4 players");
                    if (Network.IsHost && !string.IsNullOrEmpty(Network.JoinCode)) GUILayout.Label("Relay code: " + Network.JoinCode);
                    GUILayout.Label("Long load + crate · quota 2 · mandatory long load · 10 minutes");
                    if (phase == SessionPhase.Lobby || phase == SessionPhase.Loading || phase == SessionPhase.Briefing)
                    {
                        if (GUILayout.Button("Ready")) Network.Send(CommandType.Ready);
                        if (Network.IsHost && GUILayout.Button(phase == SessionPhase.Lobby ? "Load foundation" : phase == SessionPhase.Loading ? "Continue to briefing" : "Begin contract")) Network.Send(CommandType.Start);
                    }
                    if (phase == SessionPhase.Active)
                    {
                        if (GUILayout.Button("Resume", GUILayout.Height(36))) SetMenu(false);
                        if (GUILayout.Button("Vote to finish (requires completed objectives)")) Network.Send(CommandType.VoteFinish);
                        if (Network.IsHost && GUILayout.Button("Abort contract…")) abortConfirm = true;
                        if (abortConfirm) { GUILayout.Label("Abort pays nothing for this run. Previous campaign remains."); if (GUILayout.Button("Confirm abort")) { Network.Send(CommandType.Abort); abortConfirm = false; } }
                    }
                    if (phase == SessionPhase.Settling || phase == SessionPhase.Results)
                    {
                        GUILayout.Label(frame.Saved ? "Checkpoint saved" : "Settlement pending — not saved");
                        GUILayout.Label("Host campaign balance: " + frame.Balance);
                        if (Network.IsHost && !frame.Saved && GUILayout.Button("Retry save")) Settle(World.Run.Result);
                        if (Network.IsHost && frame.Saved && GUILayout.Button("Return to Hub")) Network.Send(CommandType.Hub);
                    }
                    if (phase == SessionPhase.Hub)
                    {
                        GUILayout.Label("Basic winch and straps are always free."); GUILayout.Label("Paid upgrade pricing awaits balance validation.");
                        if (Network.IsHost && GUILayout.Button("Load next contract")) Network.Send(CommandType.NewRun);
                    }
                    if (GUILayout.Button("Leave session")) { Network.Stop(); SetMenu(true); }
                }
                if (!string.IsNullOrEmpty(saveError))
                {
                    GUILayout.Label(saveError);
                    if (GUILayout.Button("Restore validated campaign backup"))
                    { try { campaign = store.RestoreBackup(); saveError = "Backup restored."; } catch (IOException e) { saveError = e.Message; } }
                }
                if (GUILayout.Button(settingsOpen ? "Close settings" : "Local settings")) settingsOpen = !settingsOpen;
                if (settingsOpen)
                {
                    GUILayout.Label("Vertical field of view: " + (int)settings.FieldOfView); settings.FieldOfView = GUILayout.HorizontalSlider(settings.FieldOfView, 70, 100);
                    GUILayout.Label("Look sensitivity"); settings.Sensitivity = GUILayout.HorizontalSlider(settings.Sensitivity, .02f, .3f);
                    settings.ToggleHold = GUILayout.Toggle(settings.ToggleHold, "Toggle hold"); settings.InvertY = GUILayout.Toggle(settings.InvertY, "Invert look Y");
                    GUILayout.Label("UI scale"); settings.UiScale = GUILayout.HorizontalSlider(settings.UiScale, .8f, 1.5f);
                    GUILayout.Label("Volume"); settings.MasterVolume = GUILayout.HorizontalSlider(settings.MasterVolume, 0, 1);
                    if (GUILayout.Button("Save settings")) SaveSettings();
                }
                GUILayout.Label("Move: WASD / left stick • Look: mouse / right stick\nJump: Space / LB • Grab: left mouse / right trigger • Interact: E / A\nRotate held cargo: hold right mouse / left trigger + look\nSecure: R / X • Ping: Q / Y • Menu: Esc / Start");
                GUILayout.EndArea();
            }
            if (Time.realtimeSinceStartupAsDouble < feedbackUntil) GUI.Label(new Rect(28, Screen.height / settings.UiScale - 45, 850, 30), feedback);
            if (Debug.isDebugBuild && frame != null)
            {
                if (World.Players.TryGetValue(Network.LocalActor.Id, out var observed) && observed)
                    GUI.Label(new Rect(Screen.width / settings.UiScale - 390, 155, 380, 65),
                        $"Player {observed.transform.position:F2} · grounded {observed.Grounded}\nVertical {observed.VerticalSpeed:F2} m/s · cursor {Cursor.lockState}");
                GUI.Label(new Rect(Screen.width / settings.UiScale - 390, 20, 380, 130), $"{Network.Profile} (ASK-035 v0.1) · actual RTT {Network.Rtt} ms\nEpoch {frame.Epoch} / tick {frame.Tick}\nHandles {World.ActiveHandles} / anchors {World.Platform.AttachmentCount}\nPhysics {World.PhysicsMilliseconds:F2} ms / rejects {Network.Rejects}\nPayload TX {Network.SentBytes} B / RX {Network.ReceivedBytes} B\nSupport {frame.SupportMass:F0} kg · C ({frame.CentreX:F2}, {frame.CentreZ:F2})");
            }
        }
        void OnDisable()
        {
            StopManipulation();
            if (Network && Network.Connected) SetMenu(true);
            else gameplay?.Disable();
        }
        void OnDestroy()
        {
            Network.Rejected -= Rejection; World.SettlementRequested -= Settle;
            if (Actions) { Actions.Disable(); Destroy(Actions); }
            Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
        }
    }
}
