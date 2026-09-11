# S019 — Held Cargo Manipulation Foundation

## Scope and dependency map

The authoritative path remains `FoundationFrontend → NetworkSession.Send(Grab) → FoundationWorld.Apply → ItemState.Grab → CarryableBody.GripHandle`. S018's read-only candidate query and grip selection remain intact. `GripHandle` now owns `HeldOrientation` value state. Local input updates that state; `FoundationWorld.Step(dt) → CarryableBody.Drive(dt) → Rigidbody.AddTorque → Physics.Simulate(dt)` executes it. Release, reset and invalidation dispose the same committed handle.

`FoundationFrontend` caches the Gameplay/Manipulate action and authoritative held body alongside existing hold state. `FoundationConfig` and its existing JSON asset own validated tuning. No added scene object, attachment, parenting, kinematic manipulation, collider suppression, transform rotation write, networking protocol or release-velocity policy is involved.

The existing scene content hash was refreshed using the established generator formula. Only that line in Bootstrap.unity changed. Existing GUIDs and prefab references were retained.

## Controls and state

- Default mouse: hold RMB while holding cargo; mouse movement rotates cargo. Grab remains LMB with the existing hold/toggle preference.
- Gamepad: hold LT and move right stick; RT remains Grab. Bindings use the existing Input System overrides.
- Horizontal input rotates about camera up; vertical input rotates about negative camera right. The existing invert-Y preference applies. Quaternion composition avoids Euler accumulation.
- Camera rotation freezes only while a valid, locally authoritative held grip consumes manipulation input. Movement and jump remain available. Without eligible cargo, look works normally even if the modifier is pressed.
- The input sample at entry and exit is discarded. No deferred delta or smoothing accumulator is introduced.
- Grab initializes the target from the body's actual rotation. First manipulation entry recaptures the actual pose, avoiding a correction to an old pre-entry pose. Subsequent entry and modifier release preserve the requested world orientation. The controller remains engaged until grip disposal.
- Local authority (Solo / local host) owns this foundation. Remote guest manipulation transport is outside this sprint; guest look and existing carry behavior remain unchanged. Multiple authoritative grip corrections are averaged within the existing total torque budget.

## Physical correction

Defaults: mouse sensitivity 0.2 degrees per input unit; maximum target speed 120 degrees/s; target lead limited to 75 degrees from achieved rotation; response 6/s; manipulation torque at most 400 Nm. The existing total torque limit remains 800 Nm and existing angular velocity limit remains 6 rad/s.

The shortest quaternion error feeds an implicit critically damped PD response using the actual physics timestep and angular velocity. World-space inertia converts the angular response to torque. Torque saturation preserves slower responses for heavy/high-inertia loads. Existing grip forces and their moment remain in the same sum; the combined result uses the existing torque clamp. Existing post-simulation velocity bounds are retained. Stored orientation error cannot accumulate multiple turns against a wall.

Unity reference: [Rigidbody inertia and angular velocity](https://docs.unity3d.com/cn/6000.0/ScriptReference/Rigidbody.html) and [AddTorque](https://docs.unity3d.com/cn/6000.0/ScriptReference/Rigidbody.AddTorque.html).

## Lifecycle hardening

Disabling cargo disposes committed grips. Invalid authored grip data, disabled/missing players and invalid Rigidbody eligibility cannot continue manipulation. Disabling the frontend releases input and cargo through existing session commands. Destruction marks the cargo state lost and removes platform support; the world step, frame capture and cleanup tolerate missing held cargo. Snapshot slot count and identity are preserved with a terminal frame, maintaining the existing guest frame shape. This is invalid-reference cleanup, not S020 recovery or safe-drop behavior.

## Baseline and discrepancy

The repository was already extensively modified/untracked at entry. Existing work was preserved. A pre-edit source snapshot at `/tmp/onhold-s019-before/_OnHold` supports the sprint-only change audit.

Actual baseline: EditMode 66/66 and PlayMode 42/42 passed; no C# diagnostics. The old S017–S018 narrative says “IN PROGRESS” / “S019 frozen”, but its later evidence summary and VERIFICATION_HANDOFF mark S018 PASS and S019 next. The newer evidence and the user's S019 instruction govern this work.

The first sandboxed Unity invocation could not open its package-manager socket (EPERM). Re-running with required local process/cache access succeeded. No pre-existing test failure was found. The first S019 PlayMode run had four test-fixture failures: overlapping cargo occluded the crate, and penetration checks looked for a root collider instead of the authored child colliders. Corrected fixtures passed. A later run overlapped a visible QA Solo session on UDP port 7777 and failed on socket binding; the retained `S019-port-conflict-playmode.xml` records this environmental failure. Closing that session and rerunning yielded 60/60 PASS. The failed XML is retained as `Evidence/S019-first-focused-playmode.xml`.

## Reproduction

Unity executable: `/Applications/Unity/Hub/Editor/6000.5.0f1/Unity.app/Contents/MacOS/Unity`.

Run from the project root with `-batchmode -nographics -projectPath "$PWD"`:

- EditMode: `-runTests -testPlatform EditMode -testResults docs/Implementation/Evidence/S019-final-editmode.xml -logFile Logs/S019-final-editmode.log`
- PlayMode: corresponding `-testPlatform PlayMode` and `S019-final-playmode` filenames.
- Focused tests: add `-testFilter S019`.
- Production development build: `-executeMethod OnHold.Editor.HeldManipulationVerification.BuildDevelopment -quit -logFile Logs/S019-development-build.log`.
- Visible QA build: `-executeMethod OnHold.Editor.HeldManipulationVerification.BuildVisibleQA -quit -logFile Logs/S019-qa-build.log`.

QA-only `ONHOLD_MANIPULATION_ACCEPTANCE` arranges three scenarios after real Play Solo: long load, wall and crate. F6 changes scenario; F8 captures screenshots/telemetry. The desktop automation API cannot sustain a native held-button gesture across calls. The QA build uses the existing binding override path: F grabs/releases; F5 latches an explicitly labeled virtual F13 modifier through the Input System. F9/F10 replay 0.7-second mouse-delta sequences into an Input System mouse for visible yaw/pitch and camera checks. Native mouse movement can also drive Look while the modifier is latched. Default RMB and LT are separately exercised by real Input System events in automated tests. QA injects device input only; it does not synthesize gameplay commands or replace the production controllers. The ordinary development build excludes the fixture and keeps default bindings.

## Deferred to S020

Release clearance/obstruction validation, drop placement, inherited release-velocity policy, safe-drop search, unstuck recovery and advanced carry collision handling remain unimplemented. A bounded orientation error and existing solver collision tolerance are deliberate S019 behavior; this sprint does not promise a safe placement when the user releases into constrained geometry.

## Results

Final measured results and visible acceptance are recorded in `Evidence/S019-summary.json` and `Evidence/S019-visible-runtime.md` after validation completes.

## Final Validation (S019 Completion)

The current S019 implementation was reviewed and verified in full against the final worktree (including late snapshot/destruction modifications). 
The final full EditMode regression run yielded 80/80 PASS. 
Existing valid evidence for PlayMode (60/60 PASS), performance (0 allocated bytes per 1000 iterations), and compilation (0 C# errors) was confirmed. 
Visible acceptance using the QA player confirmed correct interaction, manipulation lifecycle, physical bounds, and cleanup.
Result: S019 production acceptance is PASS. S020 deferred tasks are explicitly logged and not implemented in this sprint.
