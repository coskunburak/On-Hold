# S017 visible acceptance — PASS

Direct CUA inspection in `Builds/FeedbackQA/On Hold Feedback.app`, Unity 6000.5.0f1. Same production Bootstrap, frontend and Solo world. Separately compiled QA fixture arranges poses using F6 after Play Solo; it does not replace detection, eligibility or HUD. Visible screenshots are in the task tool history; corresponding stage/target/action diagnostics are in S017-visible-player.log.

Observed: empty sky, floor and wall = neutral dot; eligible cargo = cross and [LMB] Grab; looking away and out of range = no action; QA occluder = no action; menu = no reticle/prompt, cursor unlocked; resume = [LMB] Grab restored. UI remained centered/readable at 1280×720 (16:9) and 1280×800 (16:10). No obvious repeated target flicker in stationary inspection. Collider seams and repeated transitions also covered automatically.

First full-screen CUA attempt could not reliably address the window; windowed QA build resolved it. External user interactions briefly interrupted observations; fresh state was retrieved before continuing. QA occluder initially used a pink unsupported default material; this affects the test fixture only and is corrected in the next QA build by reusing an existing material. Production cargo/HUD rendered normally.

S017 build: 0 C# errors/warnings; 347 existing package/build warnings (mostly Sentis Metal shaders plus Unity Services unlinked-project warning). No warning suppression or package changes.
