# S005–S016 Playable Local Gameplay Foundation

Verification date: 2026-09-10. Unity 6000.5.0f1 (88b47c5e7076).

Status: automated gameplay verification PASS; visible runtime acceptance PASS. S005–S016 PLAYABLE LOCAL GAMEPLAY FOUNDATION: PASS.

## Retained architecture and scope

One production `PlayerBody` using CharacterController. `FoundationWorld` remains the authority and owns the fixed physics timeline (50 Hz); Solo uses the same session, command gate, item state and grip implementation as the existing co-op foundation. The existing custom-message world registry remains intact. No NGO migration, Relay development, co-op synchronization, S017, production content or final feel work.

The existing PLAY SOLO control now advances the existing Lobby → Loading/Ready → Briefing → Active sequence automatically. It starts a local host, one actor and the existing camera without invoking service initialization or Relay. Existing manual session commands remain available and their smoke test stays green.

## Sprint matrix

| Sprint | Existing implementation / audit | Change | Verification | Final |
| --- | --- | --- | --- | --- |
| S005 | Authored spawn points, player prefab, world-owned registry, one camera/listener; PARTIAL | Idempotent same-actor spawn callback, grounded unobstructed spawn fallback, configured eye/body, one-click Solo, session target cleanup | Spawn fallback and duplicate callback; real frontend startup and three repeated sessions | PASS |
| S006 | Frame-independent mouse delta, local frontend camera and settings; PARTIAL | Configured ±85° pitch, wrapped yaw, separate stick rate, current aim pose, cached local renderers | Real mouse events, sensitivity, pitch clamps, 30/60/144 frame partitions, non-authority rejection; native cursor lock | PASS |
| S007 | Single fixed-tick CharacterController, yaw-relative normalized input; PARTIAL | Conservative air steering, reliable immediate StopInput when UI owns input | All four axes and diagonal at 30/50/100 simulated physics Hz; real W/Escape; UI suppression | PASS |
| S008 | Gravity and kill-plane recovery; no jump action; PARTIAL | Space/LB Jump action, reliable edge command, grounded-only queued jump, configured analytic apex/gravity/fall cap, head collision | Eight repeated jumps, apex, air-jump rejection, actual Space press/hold/repress, ceiling, ledge/fall/land and recovery | PASS |
| S009 | CharacterController collisions; PARTIAL | Configured geometry, bounded contact pushes, stable ground adhesion, finite input/state checks | Wall, inside corner, narrow doorway, seam, ceiling, airborne wall contact, cargo contacts | PASS |
| S010 | Default slope and step values; PARTIAL | Tangential travel separated from vertical adhesion in the same motor; descending edge preserves stepping; 0.1° contact-normal tolerance; generated traversal annex | 20° and exact 45° uphill/idle/downhill; 60° rejection; 0.12/0.28 m steps accepted; 0.8 m rejected | PASS |
| S011 | FoundationConfig JSON and LocalSettings; PARTIAL | Movement/body/ground/jump/interaction/push/carry bounds centralized and validated; preserve authored JSON on generation | 16 new configuration/command EditMode cases, runtime config on existing prefab and scene | PASS |
| S012 | Private camera ray with fixed mask; PARTIAL | UI-independent InteractionRay/InteractionTarget, configured mask/triggers, nearest hit preserves wall/platform occlusion | Visible/nearest/blocked/range/layer/trigger/invalid origin; 1,000 warmed queries within 128 B allocation budget | PASS |
| S013 | CharacterController contact without bounded push policy; PARTIAL | One capped force per body per tick, speed-aware cap; static/kinematic/held/secured bodies excluded | 5 kg body versus 100 kg long cargo; heavy moves less; static walls and kinematic cargo stay bounded | PASS |
| S014 | Transactional ItemState and physical GripHandle; PARTIAL | Preserve state/authority checks; calculate grips from Rigidbody pose; reject mismatched actor and kinematic body; reconcile frontend held state | Valid/duplicate/wrong-generation/busy/blocked/out-of-range/terminal targets; real mouse press | PASS |
| S015 | Disposable grip removes holder without moving cargo; EXISTING_VALID after verification | Existing cleanup retained; input mode, UI exit and session cleanup exercised | 20 physical cycles; exact pose/velocity unchanged at release; hold and toggle mouse semantics; exit while holding then restart | PASS |
| S016 | Capped force/torque spring/damping model; PARTIAL | Cap grip error; separate angular damping; break beyond envelope; finite guards; bounded post-contact speeds | Twelve long-cargo lift/walk/strafe/90° turn/doorway/jump/reverse/release routes, distance break and invalid calculation guards | PASS |

## Effective tuning and inputs

- CharacterController height 1.8 m, radius 0.3 m, skin 0.03 m, eye height 1.6 m. One movement authority; there is no second controller or transform-driven Rigidbody carry path.
- Walk 3 m/s. Air steering accelerates toward intended horizontal velocity at `PlayerSpeed × AirControl`, with AirControl 0.25. Grounding uses a sphere sweep with walkable-normal classification (0.12 m probe envelope), rather than treating a wall as ground.
- Gravity 20 m/s² downward, jump height 1 m, terminal vertical speed 30 m/s. A grounded Space edge queues one jump; airborne presses and a held key cannot generate another jump. Ceiling contact cancels upward speed.
- Slope limit 45° with 0.1° numerical contact tolerance; step height 0.3 m. Tangential travel and downward adhesion are two collision moves within the same authoritative tick.
- Mouse sensitivity remains in the existing LocalSettings (default 0.12°/pixel), pitch −85° to +85°. Mouse delta is not multiplied by deltaTime. Gamepad look uses 150°/second.
- Move: WASD / left stick. Look: mouse / right stick. Jump: Space / left shoulder. Grab: left mouse / right trigger. Release: release that control by default, or press again in the existing Toggle Hold option. Interact: E / gamepad south (existing winch interaction). Escape / Start transitions UI input ownership. Existing Secure R/X and Ping Q/Y remain unchanged.
- Interaction range 3 m; mask WorldStatic + Carryable + Platform; triggers ignored by default. Detection casts from the current local gameplay camera. Host eligibility independently checks reach/occlusion.
- Contact push ≤1,200 N per body per tick, with a 3 m/s target and a mass/timestep cap that prevents a light body's next push from overshooting the target. Existing compound-contact friction makes the 100 kg long cargo substantially harder to move than the 5 kg test body.
- Carry distance 1.6 m; spring 900 N/m; velocity damping 100; angular damping 100; grip error influence ≤1.5 m. Per-hand force ≤1,400 N; total force ≤2,400 N; total torque ≤800 Nm. Break/release if a grip is more than 4.5 m from its holder's eye. Body speed ≤8 m/s and angular speed ≤6 rad/s, enforced after contact resolution as well as through Rigidbody limits. Continuous collision detection is enabled for authoritative cargo.

## Verification notes

The first baseline run passed 2/2 PlayMode tests. Failures found during implementation were retained in intermediate XML/log evidence: step-edge disabling, exact-limit slope behavior, stale Transform grip poses, heavy contact friction and post-contact angular velocity. Each was fixed and rerun. Tests were not removed or converted to ignored tests to obtain a passing count.

The Input System's default Editor focus policy routes keyboard/mouse events away from a headless Game View. Input tests temporarily select the documented test-compatible focus policy, inject actual keyboard/mouse events, and restore it in teardown. Gameplay still uses the production action asset and frontend. Headless Unity reports native cursor lock as None, so native cursor assertions are retained for non-batch execution and visible runtime verification is required separately.

Generated scene validation runs the builder twice and checks stable object/fixture counts, unique world/manager/camera/listener, zero scene-spawned players before a session, one controller on the prefab, Space binding, and stable unique item IDs. The authoring scene contains 51 objects, including 14 traversal fixture objects. Authored existing config values are preserved.

Automated checks exercise local PhysX behavior. They do not establish cross-machine determinism or co-op synchronization. Final carry feel remains future polish.
