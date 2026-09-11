# S017–S018 — Interaction feedback and authored grip targeting

## Execution ledger

2026-09-10, Unity 6000.5.0f1. Existing repository changes retained; no networking files, packages, motor, input bindings, carry force tuning, scene or prefab assets intentionally changed.

- Audit: production `InteractionRay.Detect` uses closest-hit occlusion and `GetComponentInParent<CarryableBody>`. The existing HUD called it again from OnGUI and displayed unconditional Grab/Secure text. `CarryableBody.CreateGrip` selected nearest unoccupied authored grip to actor eye, with array index breaking ties. No separate HUD prefab or EventSystem exists; frontend uses IMGUI.
- Baseline: 3/3 focused PlayMode tests PASS (ray/occlusion, real Space binding, real mouse grab/release/menu/session cleanup).
- S017 EditMode: 7/7 PASS.
- S017 PlayMode: 8/8 PASS, including Space and mouse regressions. 1000 warmed feedback queries: 0 bytes; no transform count growth or item state mutation.
- S017 visible acceptance: PASS. Direct CUA screenshots verified empty/floor/wall neutral reticle, eligible cargo [LMB] Grab, look-away/out-of-range clearance, occluder clearance, menu suppression and resume/reacquisition at 1280×720 and 1280×800. QA fixture poses were arranged with F6 through actual Play Solo.
- S017: PASS, closed before starting S018.
- S018: IN PROGRESS after S017 gate passed.

## S017 architecture

`InteractionRay.QueryFeedback` → existing `Detect` / logical owner → `FoundationWorld.CanGrab` → shared `ItemState.CanGrab` and `CarryableBody` selection → immutable `InteractionFeedback` → persistent `InteractionFeedbackPresenter`.

The frontend refreshes after view/input changes and in LateUpdate. OnGUI consumes cached state; it performs no interaction query. Existing Domain commands identify available actions. A deliberate notice enum maps only player-facing reasons. Neutral dot, actionable cross and centralized text provide structural differences beyond color. Binding text comes from the instantiated InputAction's effective binding. Hold versus toggle semantics are distinct.

No action is authorized by UI: the existing command is submitted and authority validates eligibility again. Menus clear state synchronously. No delayed target history or hysteresis is used, so wall/range invalidation is immediate. Child colliders resolve to one existing owner.

The presenter is an ordinary persistent object owned by the production frontend: generator changes are unnecessary, and there is no additional EventSystem/HUD hierarchy. `InteractionFeedbackAcceptance` is compiled only with `ONHOLD_FEEDBACK_ACCEPTANCE` into a separately named QA player; F6 arranges explicit production-world test scenarios after the human/agent clicks Play Solo. It does not replace gameplay or submit Grab.

## Current evidence

- `Evidence/S017-baseline-playmode.xml`
- `Evidence/S017-interaction-feedback-editmode.xml`
- `Evidence/S017-interaction-feedback-playmode.xml`

S019 and networking/co-op remain frozen.
