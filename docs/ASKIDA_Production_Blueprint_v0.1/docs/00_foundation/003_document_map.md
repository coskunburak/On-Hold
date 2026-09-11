---
doc_id: ASK-003
title: "Belge haritası ve okuma rotaları"
version: 0.1.0
status: proposed
owner_role: Producer
last_reviewed: 2026-09-07
dependencies: ["ASK-001","ASK-009","ASK-076"]
---

# Belge haritası ve okuma rotaları

## Bilgi mimarisi
ASK kimliği dosya adı değişse bile kalıcıdır. Bir konu tek kaynak belgede normatif olarak tanımlanır; diğer belgeler kuralı yeniden kopyalamak yerine o kaynağa bağlanır. Örneğin teslimatın tek seferlik olması ASK-022'nin, host kaybının davranışı ASK-032'nin, görsel kabul ölçütleri ASK-046'nın sorumluluğundadır.

| Aralık | Alan | Temel okuyucu |
|---|---|---|
| 001–010 | Başlangıç, ürün, kararlar | Herkes |
| 011–025 | Oyun semantiği | Tasarım, gameplay, QA |
| 026–035 | Co-op ve ağ | Network, gameplay, QA |
| 036–045 | Unity mühendisliği | Teknik ekip |
| 046–055 | Sanat, lisans, üretim | Artist, technical artist |
| 056–064 | Seviye, içerik, UX | Tasarım, sanat, QA |
| 065–070 | Kalite ve performans | QA, teknik ekip |
| 071–075 | Üretim ve yayın | Producer, ekip sahibi |
| 076–080 | Notebook, Codex, MCP, kaynaklar | Herkes |

## Rol bazlı ilk gün
Gameplay geliştiricisi durum eksenlerini, komut önkoşullarını ve teslimat işlemini okur; görsel etkileşimden para sistemine doğrudan çağrı yazmaz. Artist önce stil standardı, varlık kabul şartnamesi ve lisans kaydını okur; satın alınan paketin demosunu sanat yönetimi yerine kullanmaz. QA durum makinesi, bağlantı kaybı, kayıt işlemi ve kalite kapılarından test üretir. Producer kapsam, risk ve kapasite belgelerini birlikte kullanır.

## Değişiklik yayılımı
“İki kişi yerine üç kişi eşya tutabilsin” küçük ayar değildir: ASK-017 kuvvet sınırı, ASK-028 komut doğrulaması, ASK-029 çoğaltma, ASK-052 el pozları, ASK-062 göstergeler, ASK-067 testler etkilenir. Değişiklik isteği bu etki listesini taşımadan kabul edilmez.

## Belge yaşam döngüsü
Proposed → Reviewed → Approved → Implemented → Verified dizisi önerilir. Frontmatter içindeki status mevcut taslak durumunu anlatır; “Verified” için build ve test kanıtı şarttır. Retired kayıt silinmez, yerine geçen kimliğe bağlanır. data/manifest.json içerik hash'lerini taşır; commit kimliği bu arşivde yoktur çünkü henüz kullanıcı deposuna bağlanmadı.

## Kabul
Yeni ekip üyesi 30 dakikalık incelemede bir eşyanın yetkilisini, host kaybı davranışını ve seçilen ana asset adayını kaynak kimliğiyle bulabilmelidir. Bu erişilebilirlik hedefi henüz kullanıcı testiyle ölçülmedi.

## Tam belge listesi
- [ASK-001 — ASKIDA — Üretim bilgi tabanı başlangıcı](../../README.md) — ASKIDA — Üretim bilgi tabanı başlangıcı
- [ASK-002 — Codex çalışma sözleşmesi](../../AGENTS.md) — Codex çalışma sözleşmesi
- [ASK-003 — Belge haritası ve okuma rotaları](003_document_map.md) — Belge haritası ve okuma rotaları
- [ASK-004 — Proje bildirgesi ve başarı tanımı](004_project_charter.md) — Proje bildirgesi ve başarı tanımı
- [ASK-005 — Vizyon, özgün kanca ve deneyim](005_vision_hook.md) — Vizyon, özgün kanca ve deneyim
- [ASK-006 — Hedef kitle ve ticari doğrulama](006_audience_market_validation.md) — Hedef kitle ve ticari doğrulama
- [ASK-007 — Kapsam, sürümler ve kesme sırası](007_scope_matrix.md) — Kapsam, sürümler ve kesme sırası
- [ASK-008 — Tasarım ilkeleri ve anti-hedefler](008_design_pillars.md) — Tasarım ilkeleri ve anti-hedefler
- [ASK-009 — Karar kaydı ve onay bekleyenler](009_decision_register.md) — Karar kaydı ve onay bekleyenler
- [ASK-010 — Sözlük, birimler ve başlangıç parametreleri](010_glossary_parameters.md) — Sözlük, birimler ve başlangıç parametreleri
- [ASK-011 — Temel oyun döngüsü ve zaman çizgisi](../01_gameplay/011_core_loop.md) — Temel oyun döngüsü ve zaman çizgisi
- [ASK-012 — Oturum ve görev durum makinesi](../01_gameplay/012_session_state_machine.md) — Oturum ve görev durum makinesi
- [ASK-013 — Kontroller, kamera ve giriş niyetleri](../01_gameplay/013_controls_camera.md) — Kontroller, kamera ve giriş niyetleri
- [ASK-014 — Co-op roller ve oyuncu sayısı ölçekleme](../01_gameplay/014_cooperation_roles.md) — Co-op roller ve oyuncu sayısı ölçekleme
- [ASK-015 — Etkileşim sorgusu ve komut sözleşmesi](../01_gameplay/015_interaction_contract.md) — Etkileşim sorgusu ve komut sözleşmesi
- [ASK-016 — Eşya semantiği ve değişmezler](../01_gameplay/016_item_state_model.md) — Eşya semantiği ve değişmezler
- [ASK-017 — Ortak taşıma ve kuvvet modeli](../01_gameplay/017_shared_carrying.md) — Ortak taşıma ve kuvvet modeli
- [ASK-018 — Platform, destek kütlesi ve kontrollü eğim](../01_gameplay/018_platform_balance.md) — Platform, destek kütlesi ve kontrollü eğim
- [ASK-019 — Vinç kontrolü ve kontrol kiralaması](../01_gameplay/019_winch_control.md) — Vinç kontrolü ve kontrol kiralaması
- [ASK-020 — Sabitleme, kayış ve anchor sözleşmesi](../01_gameplay/020_straps_anchors.md) — Sabitleme, kayış ve anchor sözleşmesi
- [ASK-021 — Hasar ve kontrollü kırılma](../01_gameplay/021_damage_breakage.md) — Hasar ve kontrollü kırılma
- [ASK-022 — Teslimat doğrulaması ve tek seferlik sonuç](../01_gameplay/022_delivery_scoring.md) — Teslimat doğrulaması ve tek seferlik sonuç
- [ASK-023 — Sözleşme tanımı, başarı ve bitirme](../01_gameplay/023_contract_rules.md) — Sözleşme tanımı, başarı ve bitirme
- [ASK-024 — İlerleme ve küçük ölçekli ekonomi](../01_gameplay/024_progression_economy.md) — İlerleme ve küçük ölçekli ekonomi
- [ASK-025 — Düşme, sıkışma ve kurtarma kuralları](../01_gameplay/025_recovery_failures.md) — Düşme, sıkışma ve kurtarma kuralları
- [ASK-026 — Ağ çözümü seçimi ve risk prototipi](../02_network/026_network_adr_spike.md) — Ağ çözümü seçimi ve risk prototipi
- [ASK-027 — Otorite, sahiplik ve veri sınırları](../02_network/027_authority_matrix.md) — Otorite, sahiplik ve veri sınırları
- [ASK-028 — Komut protokolü, tekrarlar ve sıralama](../02_network/028_command_protocol.md) — Komut protokolü, tekrarlar ve sıralama
- [ASK-029 — Çoğaltma, snapshot ve ağ ilgisi](../02_network/029_replication_snapshot.md) — Çoğaltma, snapshot ve ağ ilgisi
- [ASK-030 — Gecikme, öngörü ve düzeltme sınırları](../02_network/030_latency_prediction.md) — Gecikme, öngörü ve düzeltme sınırları
- [ASK-031 — Lobi, Steam daveti ve bağlantı akışı](../02_network/031_lobby_steam_connectivity.md) — Lobi, Steam daveti ve bağlantı akışı
- [ASK-032 — Kopma, yeniden bağlanma ve host kaybı](../02_network/032_disconnect_reconnect_hostloss.md) — Kopma, yeniden bağlanma ve host kaybı
- [ASK-033 — Kampanya işlemleri ve kayıt otoritesi](../02_network/033_campaign_transaction.md) — Kampanya işlemleri ve kayıt otoritesi
- [ASK-034 — Ağ güvenliği ve kötüye kullanım sınırları](../02_network/034_security_abuse.md) — Ağ güvenliği ve kötüye kullanım sınırları
- [ASK-035 — Multiplayer test matrisi ve kanıt](../02_network/035_network_test_matrix.md) — Multiplayer test matrisi ve kanıt
- [ASK-036 — Unity deposu ve dosya hiyerarşisi](../03_engineering/036_unity_repository.md) — Unity deposu ve dosya hiyerarşisi
- [ASK-037 — Modüller, assembly sınırları ve bağımlılıklar](../03_engineering/037_module_boundaries.md) — Modüller, assembly sınırları ve bağımlılıklar
- [ASK-038 — Bootstrap, servis ömrü ve sahne geçişleri](../03_engineering/038_bootstrap_lifecycle.md) — Bootstrap, servis ömrü ve sahne geçişleri
- [ASK-039 — Veri şemaları ve içerik kimlikleri](../03_engineering/039_data_schemas.md) — Veri şemaları ve içerik kimlikleri
- [ASK-040 — Unity fizik uygulaması ve teknik sınırlar](../03_engineering/040_physics_integration.md) — Unity fizik uygulaması ve teknik sınırlar
- [ASK-041 — Editör araçları ve asset kabul otomasyonu](../03_engineering/041_editor_asset_validation.md) — Editör araçları ve asset kabul otomasyonu
- [ASK-042 — Build, CI ve bağımlılık kilitleme](../03_engineering/042_build_ci_release.md) — Build, CI ve bağımlılık kilitleme
- [ASK-043 — Save şeması, migration ve cloud çatışması](../03_engineering/043_save_schema_migration.md) — Save şeması, migration ve cloud çatışması
- [ASK-044 — Tanı, ölçüm ve gizlilik](../03_engineering/044_telemetry_debugging.md) — Tanı, ölçüm ve gizlilik
- [ASK-045 — Hata sınıfları ve güvenli bozulma](../03_engineering/045_errors_resilience.md) — Hata sınıfları ve güvenli bozulma
- [ASK-046 — Sanat yönetimi ve paket uyumu](../04_art/046_art_direction.md) — Sanat yönetimi ve paket uyumu
- [ASK-047 — Seçilecek paketler ve kullanım sınırları](../04_art/047_package_selection.md) — Seçilecek paketler ve kullanım sınırları
- [ASK-048 — Lisans, kaynak kaydı ve AI kullanımı](../04_art/048_licenses_provenance.md) — Lisans, kaynak kaydı ve AI kullanımı
- [ASK-049 — Her nesne için üretim şartnamesi](../04_art/049_prop_asset_spec.md) — Her nesne için üretim şartnamesi
- [ASK-050 — Astra/Codex ile Blender üretimi ve export](../04_art/050_ai_blender_export.md) — Astra/Codex ile Blender üretimi ve export
- [ASK-051 — Karakter, rig ve el IK standardı](../04_art/051_character_rig.md) — Karakter, rig ve el IK standardı
- [ASK-052 — Animasyon grafiği ve gerekli klipler](../04_art/052_animation_graph.md) — Animasyon grafiği ve gerekli klipler
- [ASK-053 — Malzeme, texture ve aydınlatma standardı](../04_art/053_materials_textures_lighting.md) — Malzeme, texture ve aydınlatma standardı
- [ASK-054 — VFX, geri bildirim ve havuzlama](../04_art/054_vfx_feedback.md) — VFX, geri bildirim ve havuzlama
- [ASK-055 — Ses tasarımı, miks ve sesli iletişim](../04_art/055_audio_sound_design.md) — Ses tasarımı, miks ve sesli iletişim
- [ASK-056 — Modüler dünya kiti ve apartman kimliği](../05_content/056_modular_world_kit.md) — Modüler dünya kiti ve apartman kimliği
- [ASK-057 — Blockout ve oynanabilir alan metrikleri](../05_content/057_blockout_metrics.md) — Blockout ve oynanabilir alan metrikleri
- [ASK-058 — Seviye 1 — Avlu apartmanı](../05_content/058_level_one_courtyard.md) — Seviye 1 — Avlu apartmanı
- [ASK-059 — Seviye 2–3 — Kapsamlı varyasyon planı](../05_content/059_levels_two_three.md) — Seviye 2–3 — Kapsamlı varyasyon planı
- [ASK-060 — On iki sözleşmelik içerik taslağı](../05_content/060_contract_catalog.md) — On iki sözleşmelik içerik taslağı
- [ASK-061 — Varlık envanteri ve üretim kuyruğu](../05_content/061_asset_inventory.md) — Varlık envanteri ve üretim kuyruğu
- [ASK-062 — Arayüz ve bilgi hiyerarşisi](../05_content/062_ui_information.md) — Arayüz ve bilgi hiyerarşisi
- [ASK-063 — Erişilebilirlik ve kamera konforu](../05_content/063_accessibility.md) — Erişilebilirlik ve kamera konforu
- [ASK-064 — Yerelleştirme ve öğretici tasarımı](../05_content/064_localization_onboarding.md) — Yerelleştirme ve öğretici tasarımı
- [ASK-065 — Performans hedefi ve ölçüm disiplini](../06_quality/065_performance_targets.md) — Performans hedefi ve ölçüm disiplini
- [ASK-066 — Başlangıç asset, render ve fizik bütçeleri](../06_quality/066_render_physics_asset_budgets.md) — Başlangıç asset, render ve fizik bütçeleri
- [ASK-067 — Test stratejisi ve gereksinim izlenebilirliği](../06_quality/067_test_strategy_traceability.md) — Test stratejisi ve gereksinim izlenebilirliği
- [ASK-068 — Oyuncu testi, eğlence ve okunurluk](../06_quality/068_playtest_protocol.md) — Oyuncu testi, eğlence ve okunurluk
- [ASK-069 — Milestone kalite kapıları ve bitmiş iş](../06_quality/069_quality_gates.md) — Milestone kalite kapıları ve bitmiş iş
- [ASK-070 — Regresyon, destek ve yayın sonrası triyaj](../06_quality/070_release_regression_support.md) — Regresyon, destek ve yayın sonrası triyaj
- [ASK-071 — Üretim yol haritası ve kapasite senaryoları](../07_production/071_roadmap_capacity.md) — Üretim yol haritası ve kapasite senaryoları
- [ASK-072 — Ekip rolleri, review ve çalışma ritmi](../07_production/072_team_workflow.md) — Ekip rolleri, review ve çalışma ritmi
- [ASK-073 — Risk defteri ve azaltma planı](../07_production/073_risk_register.md) — Risk defteri ve azaltma planı
- [ASK-074 — Başlangıç backlog'u ve ilk iki sprint](../07_production/074_backlog_first_sprints.md) — Başlangıç backlog'u ve ilk iki sprint
- [ASK-075 — Steam hazırlığı, demo ve yayın kontrolü](../07_production/075_steam_launch_plan.md) — Steam hazırlığı, demo ve yayın kontrolü
- [ASK-076 — Notebook aktarımı ve tek kaynak ilkesi](../08_knowledge/076_notebook_import.md) — Notebook aktarımı ve tek kaynak ilkesi
- [ASK-077 — Codex görev bağlamı ve güvenilir iş akışı](../08_knowledge/077_codex_task_workflow.md) — Codex görev bağlamı ve güvenilir iş akışı
- [ASK-078 — Salt okunur MCP bilgi servisi şartnamesi](../08_knowledge/078_mcp_readonly_spec.md) — Salt okunur MCP bilgi servisi şartnamesi
- [ASK-079 — Belge sürümleme, şablonlar ve değişiklik yönetimi](../08_knowledge/079_versioning_templates.md) — Belge sürümleme, şablonlar ve değişiklik yönetimi
- [ASK-080 — Kaynaklar, doğrulama sınırları ve açık kanıtlar](../08_knowledge/080_sources_evidence_limits.md) — Kaynaklar, doğrulama sınırları ve açık kanıtlar

## İlgili kaynak belgeler
- [ASK-001 — ASKIDA — Üretim bilgi tabanı başlangıcı](../../README.md)
- [ASK-009 — Karar kaydı ve onay bekleyenler](009_decision_register.md)
- [ASK-076 — Notebook aktarımı ve tek kaynak ilkesi](../08_knowledge/076_notebook_import.md)

