---
doc_id: ASK-074
title: "Başlangıç backlog'u ve ilk iki sprint"
version: 0.1.0
status: proposed
owner_role: Producer
last_reviewed: 2026-09-07
dependencies: ["ASK-026","ASK-049","ASK-067","ASK-069","ASK-071"]
---

# Başlangıç backlog'u ve ilk iki sprint

## Önce en pahalı belirsizlik
İlk çalışma tam oyun sistemi kurmak değil, ASKIDA'nın ayırt edici fiziğinin uzaktan oynanabilirliğini kanıtlamaktır. data/backlog.json ilk 32 iş kartını içerir; tüm oyun için eksiksiz maliyet dökümü değildir. Hiçbir iş tamamlanmış işaretlenmedi.

## Sprint 0 / ilk iki hafta
İki kişi ve kişi başı 25 net saat/hafta varsayımında toplam 100 saat üst plan kapasitesi:
- W-001 karar ve sürüm matrisi: 6 saat üst tahmin.
- W-002 repository/temel build: 10 saat.
- W-003 tek otoriter eşya bağlantısı: 14 saat.
- W-004 iki holder kuvvet prototipi: 18 saat.
- W-005 kontrollü platform prototipi: 18 saat.
- W-006 ağ emülasyonu ve tanı: 10 saat.
- W-007 gerçek internet bağlantısı denemesi: 12 saat.
- W-008 test videosu, ölçüm ve ADR kararı: 12 saat.

Toplam üst tahmin 100 saattir; tüm belirsizliği karşılayan rezerv içermez. Bu yüzden hedef zaman kutusu sonunda kanıt/karar üretmektir, her özelliği bitirme garantisi değil. Tek kişi 25 saat/haftada aynı kapsam en az dört hafta kapasite ister; teknik belirsizlik ayrıca uzatabilir.

## Sprint 1 / sonraki dilim
G1 geçtiyse domain state, secure, delivery transaction, reconnect ve temel kayıt. Geçmediyse G1 sorununu daralt. Art tarafında yalnız bir platform + bir kahraman eşyanın stil onayı. UI'da prompt/ret nedeni ve basit sonuç. Bu dilimden önce bütün 12 sözleşmenin final tasarımı yapılmaz.

## İş sırası
Her W bağımlı W'leri ve REQ kimliklerini taşır. Estimate low/high planlama aralığıdır, gerçek çalışma saati değildir. Blocked nedeninin kapanması status alanını otomatik Done yapmaz; kabul kanıtı gerekir.

## Birlikte ilerleme
İlk onay görüşmesinde ADR-ENGINE/ART/BUDGET/HARDWARE seçilir. Ardından W-001 kartı üzerinde çalışılır. Her sonraki Codex görevi bir veya küçük ilişkili birkaç W ile sınırlandırılır; görev sonunda gerçek test sonucu ve doküman farkı birlikte sunulur.

## Tahmin toplamının sınırı
İlk 32 kartın toplam aralığı 408–804 net iş saatidir. Bu liste erken temel ve planlama işlerini kapsar; üç binanın tüm final varlık üretimi, bütün hata düzeltmeleri, kullanıcı test turları ve yayın sonrası destek için tam maliyet değildir. Bu toplamdan doğrudan çıkış tarihi hesaplanmaz. W-031 canlı MCP geliştirmesi gelecekte ayrıca onay gerektirir; bu doküman teslimatı onu gerçekleştirmiş sayılmaz.

## Gereksinim ve test izleri
- REQ-008 → TC-008 — Risk kapısı kanıtla kapanmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-026 — Ağ çözümü seçimi ve risk prototipi](../02_network/026_network_adr_spike.md)
- [ASK-049 — Her nesne için üretim şartnamesi](../04_art/049_prop_asset_spec.md)
- [ASK-067 — Test stratejisi ve gereksinim izlenebilirliği](../06_quality/067_test_strategy_traceability.md)
- [ASK-069 — Milestone kalite kapıları ve bitmiş iş](../06_quality/069_quality_gates.md)
- [ASK-071 — Üretim yol haritası ve kapasite senaryoları](071_roadmap_capacity.md)

