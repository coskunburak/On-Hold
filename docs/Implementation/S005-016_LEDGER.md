# S005–S016 playable foundation ledger

Scope: existing Solo authority, one CharacterController, existing command gate and bounded grip forces. Preserve all pre-existing untracked foundation and unrelated project changes. No S017 or co-op work.

Initial audit (2026-09-10): S004 has local host without Relay and two passing historical PlayMode tests; rerun pending. PlayerBody is the only movement driver. FoundationFrontend owns one camera and Input Action instance. FoundationWorld owns physics simulation. CarryableBody and ItemState own physical grips and lifecycle.

| Sprint | Audit | Implementation | Verification | Final |
| --- | --- | --- | --- | --- |
| S005 | PARTIAL: authored spawn; duplicate callback recreates player | NOT_STARTED | NOT_STARTED | NOT_STARTED |
| S006 | PARTIAL: mouse delta correct; stick scaling and pitch constants | NOT_STARTED | NOT_STARTED | NOT_STARTED |
| S007 | PARTIAL: yaw-relative normalized movement; UI stale movement | NOT_STARTED | NOT_STARTED | NOT_STARTED |
| S008 | PARTIAL: gravity exists; jump absent | NOT_STARTED | NOT_STARTED | NOT_STARTED |
| S009 | PARTIAL: CharacterController; no scenario tests | NOT_STARTED | NOT_STARTED | NOT_STARTED |
| S010 | PARTIAL: default slope/step settings; no controlled fixtures | NOT_STARTED | NOT_STARTED | NOT_STARTED |
| S011 | PARTIAL: JSON config; geometry/gravity/pitch scattered | NOT_STARTED | NOT_STARTED | NOT_STARTED |
| S012 | PARTIAL: private camera ray; fixed mask; no gameplay result API | NOT_STARTED | NOT_STARTED | NOT_STARTED |
| S013 | PARTIAL: no explicit bounded CharacterController push | NOT_STARTED | NOT_STARTED | NOT_STARTED |
| S014 | PARTIAL: existing validated authoritative grip | NOT_STARTED | NOT_STARTED | NOT_STARTED |
| S015 | PARTIAL: existing disposable grip cleanup | NOT_STARTED | NOT_STARTED | NOT_STARTED |
| S016 | PARTIAL: capped spring/damping; no distance break or runtime coverage | NOT_STARTED | NOT_STARTED | NOT_STARTED |

Execution: baseline → movement/config/input implementation → compile and physical traversal tests → interaction/carry → complete regressions → bounded runtime acceptance and repeat sessions → final evidence/handoff.

## Progress after physical and real Input System tests

- Baseline: 2/2 PlayMode PASS.
- Movement: 16/16 PASS after correcting stepOffset at descending step edges.
- Real input: synthetic keyboard/mouse routed through the actual Input System and production frontend; Space, no repeat on hold, no air jump, UI suppression, Escape, mouse pitch/sensitivity and one-click/repeated Solo PASS. Test-only focus configuration is restored in teardown.
- Current integrated run: 26/27 PASS; heavy push under investigation. Twenty physical grip cycles and twelve long-cargo carry routes PASS, including contacts and clean release.
- Fixed: duplicate spawn callback, safe spawn fallback, gravity/jump/air-control configuration, immediate stop command, Input Action jump, Solo auto-transition, gameplay ray API with occlusion and zero-allocation warmed query test, bounded grip error/distance break/numerical guards, post-contact speed limits.
- Generation, final complete regressions, standalone runtime acceptance and handoff remain pending.
