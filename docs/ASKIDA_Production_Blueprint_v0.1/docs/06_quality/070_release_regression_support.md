---
doc_id: ASK-070
title: "Regresyon, destek ve yayın sonrası triyaj"
version: 0.1.0
status: proposed
owner_role: QA
last_reviewed: 2026-09-07
dependencies: ["ASK-032","ASK-043","ASK-069","ASK-075"]
---

# Regresyon, destek ve yayın sonrası triyaj

## Release regresyonu
Temiz kurulum; ilk açılış; controller/fare; iki/dört oyuncu daveti; sürüm uyuşmazlığı; bütün görevlerin başlama/bitmesi; save migration; cloud kapalı; kopma/reconnect; host kaybı; offline hata; ayarlar; alt-tab; çözünürlük; uzun oturum. Steam özel build branch'i ve test hesapları yayın öncesi hazırlanır, burada kurulmadı.

## Destek paketi
Build ID, platform/OS, hata kodu, isteğe bağlı anonymized log bundle ve yeniden üretim adımları. Oyuncudan parola veya Steam ticket istenmez. Kayıt dosyası istenirse içeriği ve saklama amacı açıklanır. Kullanıcının özel bilgileri herkese açık issue'ya kopyalanmaz.

## Öncelik
Çökme ve veri kaybı hızlı triage; yaygın ağ bağlantı sorunu servis/build ayrımı; tek asset clipping art kuyruğu. Hata sayısı dışında etkilenen kullanıcı sayısı ve yeniden üretilebilirlik izlenir. Aynı kök neden farklı rapor başlıklarında birleşebilir.

## Hotfix
En küçük düzeltme; ilgili regresyon; save/protocol uyum kontrolü; staging branch; manuel yayın onayı; rollback seçeneği. Yeni içerik ile acil ağ düzeltmesini aynı hotfix'e yükleme. İki farklı protocol build'inin aynı lobide eşleşmesi engellenir.

## Canlı destek sınırı
V1 live-service içerik takvimi vaat etmez. Yine de yayın sonrası hata düzeltme ve iletişim kapasitesi bütçelenir. Ekip sahibi haftalık destek saatini ve kritik durum muhatabını belirler. Oyuncuya tarih verilmeden önce hata kapsamı anlaşılmalıdır.

## Kabul
Rollback prosedürü kuru testten geçer; eski build yeni save'i bozamaz; yayın notları gerçek düzeltmeleri anlatır. Destek işi roadmap kapasitesinden düşülür, görünmez ücretsiz emek sayılmaz.

## Gereksinim ve test izleri
- REQ-038 → TC-038 — Build/release yolu kontrollü olmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-032 — Kopma, yeniden bağlanma ve host kaybı](../02_network/032_disconnect_reconnect_hostloss.md)
- [ASK-043 — Save şeması, migration ve cloud çatışması](../03_engineering/043_save_schema_migration.md)
- [ASK-069 — Milestone kalite kapıları ve bitmiş iş](069_quality_gates.md)
- [ASK-075 — Steam hazırlığı, demo ve yayın kontrolü](../07_production/075_steam_launch_plan.md)

