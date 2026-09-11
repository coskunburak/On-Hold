---
doc_id: ASK-001
title: "ASKIDA — Üretim bilgi tabanı başlangıcı"
version: 0.1.0
status: proposed
owner_role: Product
last_reviewed: 2026-09-07
dependencies: ["ASK-004","ASK-007","ASK-009","ASK-076","ASK-080"]
---

# ASKIDA — Üretim bilgi tabanı başlangıcı

## Bu paket neyi teslim ediyor?
ASKIDA, 2–4 arkadaşın apartman cephelerinde ağır ve biçimsiz eşyaları bir kaldırma platformuyla taşıdığı, fizik tabanlı bir co-op oyun önerisidir. Oyuncular platform üzerinde yer değiştirerek karşı ağırlık olur; yükü birlikte döndürür, sabitler ve teslim eder. Hedef, oyuncunun neyin yanlış gittiğini anlayabildiği, paylaşılabilir komik anlar üreten bir Steam oyunudur. Satış başarısı garanti değildir.

Bu arşiv çalışır bir Unity projesi, satın alınmış asset koleksiyonu veya onaylanmış nihai tasarım değildir. 80 kaynak Markdown belgesi, yapılandırılmış gereksinim/test/iş kayıtları, yerel doğrulama ve arama yardımcıları ile türetilmiş notebook aktarım dosyaları içerir. Bütün tasarım kararları aksi belirtilmedikçe öneridir. Henüz motor üzerinde performans, multiplayer veya eğlence doğrulaması yapılmadı.

## İlk okuma ve ilk çalışma
1. Ürün hedefi için [ASK-004 — Proje bildirgesi ve başarı tanımı](docs/00_foundation/004_project_charter.md), [ASK-005 — Vizyon, özgün kanca ve deneyim](docs/00_foundation/005_vision_hook.md) ve [ASK-007 — Kapsam, sürümler ve kesme sırası](docs/00_foundation/007_scope_matrix.md).
2. Onay bekleyen kararlar için [ASK-009 — Karar kaydı ve onay bekleyenler](docs/00_foundation/009_decision_register.md); özellikle motor, ekip kapasitesi ve görsel yön.
3. Oynanış sözleşmesi için [ASK-011 — Temel oyun döngüsü ve zaman çizgisi](docs/01_gameplay/011_core_loop.md), [ASK-016 — Eşya semantiği ve değişmezler](docs/01_gameplay/016_item_state_model.md), [ASK-018 — Platform, destek kütlesi ve kontrollü eğim](docs/01_gameplay/018_platform_balance.md), [ASK-022 — Teslimat doğrulaması ve tek seferlik sonuç](docs/01_gameplay/022_delivery_scoring.md).
4. Teknik başlangıç için [ASK-026 — Ağ çözümü seçimi ve risk prototipi](docs/02_network/026_network_adr_spike.md), [ASK-036 — Unity deposu ve dosya hiyerarşisi](docs/03_engineering/036_unity_repository.md), [ASK-040 — Unity fizik uygulaması ve teknik sınırlar](docs/03_engineering/040_physics_integration.md).
5. İlk çalışma paketi için [ASK-074 — Başlangıç backlog'u ve ilk iki sprint](docs/07_production/074_backlog_first_sprints.md); doğrudan tam oyun üretimine başlamayın.
6. Notebook ve Codex kullanımı için [ASK-076 — Notebook aktarımı ve tek kaynak ilkesi](docs/08_knowledge/076_notebook_import.md), [ASK-077 — Codex görev bağlamı ve güvenilir iş akışı](docs/08_knowledge/077_codex_task_workflow.md), [ASK-078 — Salt okunur MCP bilgi servisi şartnamesi](docs/08_knowledge/078_mcp_readonly_spec.md).

## Arşiv düzeni
Kökte README.md ve AGENTS.md bulunur. docs/00_foundation ürün ve kararları; 01_gameplay kuralları; 02_network çevrimiçi sözleşmeleri; 03_engineering uygulama tasarımını; 04_art sanat üretimini; 05_content seviyeleri ve arayüzü; 06_quality testleri; 07_production üretimi; 08_knowledge bilgi yönetimini barındırır. data/ makine tarafından okunabilir kayıtları, tools/ bağımlılıksız yardımcıları, notebook_exports/ on adet türetilmiş metin cildini taşır. Kanonik dosya listesi data/manifest.json içindedir.

## Güvenli kullanım
Zip'i açtıktan sonra kendi Git deponuza alın; depo oluşturma veya uzak servise gönderme bu teslimatta yapılmadı. Yardımcı komutlar Python 3 standart kütüphanesini kullanır. Kök dizinde “python3 tools/validate.py” yapısal kontrolleri, “python3 tools/search_docs.py platform” yerel aramayı çalıştırır. Doğrulayıcı oyun testlerini çalıştırmaz ve tasarımın doğruluğunu kanıtlamaz.

Belge değişikliğinde bağımlı sözleşmeleri, veri kayıtlarını ve testleri birlikte güncelleyin; ardından “python3 tools/build_exports.py” ile manifest hash'lerini ve notebook kopyalarını yeniden üretin. “validation_report.json” yalnızca teslim edilen belge paketinin otomatik kontrol sonucudur.

## Onay kapısı
İlk toplantının çıktısı üç şey olmalıdır: seçilen motor/ekip kapasitesi, iki haftalık risk prototipinin sınırı, başarısız olursa hangi kapsamın kesileceği. Üretime hazır kaliteye ancak uygulama, oyun testleri, ölçüm, lisans denetimi ve yayın kapıları geçilerek ulaşılır.

## Gereksinim ve test izleri
- REQ-001 → TC-001 — Motor ve sürüm kararı kanıtla seçilmeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](data/requirements.json) ve [test kayıtları](data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-004 — Proje bildirgesi ve başarı tanımı](docs/00_foundation/004_project_charter.md)
- [ASK-007 — Kapsam, sürümler ve kesme sırası](docs/00_foundation/007_scope_matrix.md)
- [ASK-009 — Karar kaydı ve onay bekleyenler](docs/00_foundation/009_decision_register.md)
- [ASK-076 — Notebook aktarımı ve tek kaynak ilkesi](docs/08_knowledge/076_notebook_import.md)
- [ASK-080 — Kaynaklar, doğrulama sınırları ve açık kanıtlar](docs/08_knowledge/080_sources_evidence_limits.md)

