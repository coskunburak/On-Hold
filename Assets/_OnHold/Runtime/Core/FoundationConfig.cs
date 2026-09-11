using System;

namespace OnHold.Core
{
    // Original ASK-010 / data/parameters.json baseline. Extra tuning is experimental.
    [Serializable]
    public sealed class FoundationConfig
    {
        public int PhysicsHz = 50, SnapshotHz = 20, HoldersMax = 2, RecoveryMax = 2;
        public float PlayerMass = 80, DeckWidth = 2.4f, DeckDepth = 1.8f, TiltMax = 12;
        public float ReconnectSeconds = 60, DeliveryDwell = 1, DeliverySpeed = .15f;
        public float DeliveryAngularDegrees = 5, IntegrityMin = 20, WinchSecureSpeed = .05f, RecoveryPenalty = 15;
        public float Reach = 3, CarryDistance = 1.6f, CarrySpring = 900, CarryDamping = 100;
        public float HandForceMax = 1400, TotalForceMax = 2400, TotalTorqueMax = 800;
        public float BodySpeedMax = 8, AngularSpeedMax = 6, SupportEnter = .06f, SupportExit = .14f;
        public float BaseMass = 120, TiltResponse = 3, TiltSpeed = 15;
        public float WinchSpeed = .6f, WinchAcceleration = 1, LeaseTimeout = .5f;
        public float DeliveryTolerance = .015f, DamageThreshold = 2.5f, DamageScale = 8, DamageMax = 40, DamageCooldown = .25f;
        public float VoteSeconds = 15, VoteCooldown = 5, PlayerSpeed = 3, RecoveryWait = 5;
        public float AirControl = .25f, Gravity = 20, JumpHeight = 1, FallSpeedMax = 30;
        public float PlayerHeight = 1.8f, PlayerRadius = .3f, EyeHeight = 1.6f;
        public float SlopeLimit = 45, StepHeight = .3f, SkinWidth = .03f, GroundProbe = .12f;
        public float PitchMin = -85, PitchMax = 85, StickLookSpeed = 150;
        public float PushForceMax = 1200, PushSpeedMax = 3;
        public int InteractionMask = (1 << 8) | (1 << 10) | (1 << 11);
        // 0 = UseGlobal, 1 = Ignore, 2 = Collide (Unity QueryTriggerInteraction).
        public int InteractionTriggers = 1;
        public float CarryErrorMax = 1.5f, CarryBreakDistance = 4.5f, CarryAngularDamping = 100;
        public float ManipulationSensitivity = .2f, ManipulationSpeed = 120, ManipulationLeadMax = 75;
        public float ManipulationResponse = 6, ManipulationTorqueMax = 400;
        // S020 — Carry Handling & Release Safety.
        public float ReleasePenetrationTolerance = .03f;
        public float SafeReleaseSearchDistance = .5f;
        public float MaxReleaseLinearSpeed = 5, MaxReleaseAngularSpeed = 4;
        public void Validate()
        {
            foreach (var f in GetType().GetFields())
            {
                var n = Convert.ToDouble(f.GetValue(this));
                if (!Numbers.Finite(n) || (f.Name != nameof(PitchMin) && f.Name != nameof(InteractionTriggers) && n <= 0)) throw new ArgumentException("Invalid configuration: " + f.Name);
            }
            if (PhysicsHz > 120 || SnapshotHz > PhysicsHz || HoldersMax != 2 || IntegrityMin > 100 || TiltMax > 20)
                throw new ArgumentException("Configuration outside foundation limits");
            if (ManipulationSensitivity > 5 || ManipulationSpeed > 360 || ManipulationLeadMax > 90 ||
                ManipulationResponse > 20 || ManipulationTorqueMax > TotalTorqueMax)
                throw new ArgumentException("Invalid manipulation limits");
            if (AirControl > 1 || PlayerHeight <= 2 * PlayerRadius || EyeHeight >= PlayerHeight - SkinWidth ||
                EyeHeight <= PlayerRadius || SkinWidth >= PlayerRadius || StepHeight >= PlayerHeight / 2 ||
                GroundProbe >= PlayerRadius || SlopeLimit >= 85 || PitchMin >= PitchMax || PitchMin < -85 || PitchMax > 85 ||
                InteractionTriggers < 0 || InteractionTriggers > 2 || CarryDistance >= CarryBreakDistance || Reach >= CarryBreakDistance)
                throw new ArgumentException("Invalid player/interaction configuration relationship");
            if (SafeReleaseSearchDistance > .5f ||
                ReleasePenetrationTolerance > .5f || MaxReleaseLinearSpeed > BodySpeedMax || MaxReleaseAngularSpeed > AngularSpeedMax)
                throw new ArgumentException("Invalid release safety limits");
        }
    }
    public static class Numbers
    {
        public static bool Finite(double n) => !double.IsNaN(n) && !double.IsInfinity(n);
        public static double Clamp(double n, double min, double max) => Math.Max(min, Math.Min(max, n));
        public static bool Id(string id) => !string.IsNullOrEmpty(id) && id.Length <= 96 && IsAscii(id);
        static bool IsAscii(string id) { foreach (char c in id) if (!(c >= 'a' && c <= 'z') && !(c >= 'A' && c <= 'Z') && !(c >= '0' && c <= '9') && c != '.' && c != '-' && c != '_') return false; return true; }
    }
    public enum Reason { Accepted, AlreadyApplied, TooFar, Blocked, Busy, InvalidState, StaleRevision,
        RateLimited, WrongSession, UnsupportedAction, InvalidPayload, OldSequence, OldConnection,
        AuthFailed, BuildMismatch, ContentMismatch, LobbyFull, SessionInProgress, ReservationExpired,
        LoadFailed, NotReady, UnsafeSpeed, AttachmentFailed, InsufficientFunds, InvalidContent, SaveFailed, VoteRequired }
    public readonly struct Actor : IEquatable<Actor>
    {
        public readonly int Id;
        public readonly uint Generation;
        public Actor(int id, uint generation) { Id = id; Generation = generation; }
        public bool Equals(Actor other) => Id == other.Id && Generation == other.Generation;
        public override bool Equals(object obj) => obj is Actor other && Equals(other);
        public override int GetHashCode() => (Id * 397) ^ (int)Generation;
    }
}
