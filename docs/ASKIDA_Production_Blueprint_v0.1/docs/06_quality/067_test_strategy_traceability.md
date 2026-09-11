---
doc_id: ASK-067
title: "Test stratejisi ve gereksinim izlenebilirliği"
version: 0.1.0
status: proposed
owner_role: QA
last_reviewed: 2026-09-07
dependencies: ["ASK-016","ASK-022","ASK-035","ASK-069","ASK-074"]
---

# Test stratejisi ve gereksinim izlenebilirliği

## Test katmanları
Saf domain: durum geçişleri, teslimat idempotency, ekonomi, komut önkoşulları. EditMode: content registry, prefab ve source metadata. PlayMode: collision, controller, scene lifecycle. Multi-process integration: host + istemciler, snapshot/kopma. İnsan playtest: eğlence, okunurluk, stil, konfor. Hiçbiri diğerinin yerine geçmez.

## Kayıtlar
data/requirements.json REQ kimliği, zorunluluk, kaynak ASK ve kabul testlerini taşır. data/test_cases.json test adımı, beklenen sonuç, katman, kanıt ve mevcut durumu taşır. data/backlog.json W kimliği ve REQ ilişkisini taşır. Bu teslimatta oyun testlerinin hepsi NOT_RUN veya PLANNED; belge kontrolü ayrı rapordadır.

## Test yazım standardı
Given açık başlangıç state'i; When tek belirgin işlem/arıza; Then ölçülebilir sonuç ve yasak yan etki. “Co-op düzgün” test adı değildir. Örnek: iki oyuncu aynı anchor'a Secure yollar; bir Accepted, bir Busy; tek anchor ilişkisi; iki cüzdan değişmez; fizik joint sayısı geçerli aralıkta.

## Regresyon
Bir bug düzeltmesinde önce tekrar üretim kaydı, sonra mümkünse otomatik test, sonra düzeltme. Flaky test quarantine edilirse owner/tarih ve manuel telafi kapısı bulunur; yeşil rapor için silinmez. Ağ zamanlaması testinde seed/emülasyon profili kaydedilir.

## Kapsam ölçümü
Kod coverage yardımcı ölçüttür; yüzde yüz line coverage, fizik hissi veya yarış koşullarının kapsandığı anlamına gelmez. Kritik değişmezlerin ayrı test listesi gerekir. Requirement ile test eşlemesi boşsa kalite kapısı geçmez.

## Kabul
Her P0 gereksinim en az bir test ve sorumlu belgeye bağlanır. Test sonucu build kimliği taşır. “Planlandı” ile “geçti” UI/raporlarda açıkça ayrıdır. Paket doğrulayıcısı bu bağlantıları denetler; gerçek Unity testlerini çalıştırmaz.

## Gereksinim ve test izleri
- REQ-009 → TC-009 — Eşya değişmezleri korunmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-016 — Eşya semantiği ve değişmezler](../01_gameplay/016_item_state_model.md)
- [ASK-022 — Teslimat doğrulaması ve tek seferlik sonuç](../01_gameplay/022_delivery_scoring.md)
- [ASK-035 — Multiplayer test matrisi ve kanıt](../02_network/035_network_test_matrix.md)
- [ASK-069 — Milestone kalite kapıları ve bitmiş iş](069_quality_gates.md)
- [ASK-074 — Başlangıç backlog'u ve ilk iki sprint](../07_production/074_backlog_first_sprints.md)

