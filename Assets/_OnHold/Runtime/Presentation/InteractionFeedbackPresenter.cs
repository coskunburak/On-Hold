using UnityEngine;
using UnityEngine.InputSystem;
using OnHold.Domain;
using OnHold.Gameplay;

namespace OnHold.Presentation
{
    // One persistent presenter owned by the existing frontend. No input controls or raycasts.
    public sealed class InteractionFeedbackPresenter
    {
        InteractionFeedback state;
        GUIStyle centered;
        readonly GUIContent prompt = new GUIContent();
        string bindingPath, bindingLabel;
        CommandType? previousAction;
        InteractionNotice previousNotice;
        bool previousToggle;
        public string Prompt => prompt.text;
        public void Update(InteractionFeedback next, InputAction grab, bool toggle, bool gamepad)
        {
            int index = -1;
            for (int i = 0; i < grab.bindings.Count; i++)
            {
                var path = grab.bindings[i].effectivePath;
                if (!string.IsNullOrEmpty(path) && path.StartsWith(gamepad ? "<Gamepad>" : "<Mouse>", System.StringComparison.Ordinal)) { index = i; break; }
            }
            if (index < 0 && grab.bindings.Count > 0) index = 0;
            string effective = index >= 0 ? grab.bindings[index].effectivePath : "";
            bool bindingChanged = effective != bindingPath;
            if (bindingChanged) { bindingPath = effective; bindingLabel = index >= 0 ? grab.GetBindingDisplayString(index) : "Unbound"; }
            if (bindingChanged || next.Action != previousAction || next.Notice != previousNotice || toggle != previousToggle || next.Visible != state.Visible)
            {
                prompt.text = !next.Visible ? "" : next.Action == CommandType.Grab ? "[" + bindingLabel + "] Grab" :
                    next.Action == CommandType.Release ? (toggle ? "[" + bindingLabel + "] Release" : "Release [" + bindingLabel + "] to let go") : Notice(next.Notice);
                previousAction = next.Action; previousNotice = next.Notice; previousToggle = toggle;
            }
            state = next;
        }
        static string Notice(InteractionNotice notice)
        {
            switch (notice)
            {
                case InteractionNotice.MoveCloser: return "Move closer";
                case InteractionNotice.Secured: return "Secured — unsecure before grabbing";
                case InteractionNotice.NoAvailableGrip: return "No available grip";
                case InteractionNotice.Unavailable: return "Unavailable";
                default: return "";
            }
        }
        static readonly GUIContent[] GripLabels = { new GUIContent("Grip 1"), new GUIContent("Grip 2") };
        public bool MarkerVisible => state.Visible && state.HasGrip;
        public void Draw(float scale, Camera camera, float markerSize)
        {
            if (!state.Visible || Event.current.type != EventType.Repaint) return;
            if (centered == null) centered = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter, fontSize = 18, fontStyle = FontStyle.Bold };
            float x = Screen.width / scale * .5f, y = Screen.height / scale * .5f;
            var color = GUI.color; GUI.color = Color.white;
            GUI.DrawTexture(new Rect(x - 2, y - 2, 4, 4), Texture2D.whiteTexture);
            if (state.Actionable)
            {
                GUI.DrawTexture(new Rect(x - 10, y - 1, 5, 2), Texture2D.whiteTexture);
                GUI.DrawTexture(new Rect(x + 5, y - 1, 5, 2), Texture2D.whiteTexture);
                GUI.DrawTexture(new Rect(x - 1, y - 10, 2, 5), Texture2D.whiteTexture);
                GUI.DrawTexture(new Rect(x - 1, y + 5, 2, 5), Texture2D.whiteTexture);
            }
            if (!string.IsNullOrEmpty(prompt.text))
            {
                GUI.color = new Color(0, 0, 0, .72f);
                GUI.DrawTexture(new Rect(x - 225, y + 25, 450, 36), Texture2D.whiteTexture);
                GUI.color = Color.white; GUI.Label(new Rect(x - 225, y + 25, 450, 36), prompt, centered);
            }
            if (MarkerVisible)
            {
                // World-anchored marker projected through the real local camera. No scene object,
                // collider, material, shadows or physics query; occlusion comes from gameplay state.
                var screen = camera.WorldToScreenPoint(state.Grip.WorldPose.position);
                if (screen.z > camera.nearClipPlane && screen.x >= 0 && screen.x <= Screen.width && screen.y >= 0 && screen.y <= Screen.height)
                {
                    float mx = screen.x / scale, my = (Screen.height - screen.y) / scale;
                    float radius = Mathf.Clamp(markerSize, 8, 24) * .5f;
                    GUI.color = Color.black; DrawRing(mx, my, radius + 1, 4);
                    GUI.color = new Color(1, .9f, .35f); DrawRing(mx, my, radius, 2);
                    if (state.Grip.GripIndex < GripLabels.Length)
                    {
                        GUI.color = new Color(0, 0, 0, .72f); GUI.DrawTexture(new Rect(mx - 40, my - radius - 28, 80, 24), Texture2D.whiteTexture);
                        GUI.color = Color.white; GUI.Label(new Rect(mx - 40, my - radius - 28, 80, 24), GripLabels[state.Grip.GripIndex], centered);
                    }
                }
            }
            GUI.color = color;
        }
        static void DrawRing(float x, float y, float r, float thickness)
        {
            GUI.DrawTexture(new Rect(x - r, y - r, r * 2, thickness), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(x - r, y + r - thickness, r * 2, thickness), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(x - r, y - r, thickness, r * 2), Texture2D.whiteTexture);
            GUI.DrawTexture(new Rect(x + r - thickness, y - r, thickness, r * 2), Texture2D.whiteTexture);
        }
    }
}
