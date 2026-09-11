---
doc_id: ASK-028
title: "Komut protokolü, tekrarlar ve sıralama"
version: 0.1.0
status: proposed
owner_role: Network
last_reviewed: 2026-09-07
dependencies: ["ASK-015","ASK-027","ASK-029","ASK-034"]
---

# Komut protokolü, tekrarlar ve sıralama

## Önerilen zarf
CommandEnvelope: protocolVersion, sessionEpoch, connectionGeneration, requestId, sequence, targetId, expectedRevision, commandType, boundedPayload. Actor kimliği payload'dan değil doğrulanmış bağlantı eşlemesinden alınır. ClientTime yalnız tanı veya sınırlandırılmış tahmin için olabilir; zaman aşımını geriye almak için güvenilmez.

## İşleme
Parse ve boyut sınırı → oturum/protokol → actor eşlemesi → rate limit → request dedup → komut özel doğrulaması → domain transaction → revision/event → sonuç. Yanıt requestId, accepted/rejected, reasonCode, authoritativeRevision ve gerekirse küçük düzeltme state'i taşır.

## Tekrar yönetimi
Kontrol komutları için bağlantı başına sınırlı dedup cache tutulur; başlangıç hedefi son 256 request ve en az 10 saniyelik pencere, gerçek trafikle ayarlanır. Sequence eski veya cache dışı tekrarları reddetmek için de kullanılır; reconnect aynı sessionEpoch içinde yeni connectionGeneration ve sequence bağlamıyla başlar. Teslimat ve kalıcı ödeme için yalnız bu geçici cache yeterli değildir: item/run ve transactionId kayıtları kalıcı semantik idempotency sağlar.

## Revision politikası
Destructive veya münhasır eylem güncel revision gerektirir. İkinci Grab hedefinin revision'ı değişmişse yeni state üzerinde boş yuva ve erişim yeniden değerlendirilerek kontrollü kabul edilebilir; bunu genel “stale her zaman kabul” kuralına çevirmeyin. Release kendi holder hakkını kaldırırken eski revision nedeniyle kilitlenmez. Secure ve satın alma stale ise açık ret ve güncelleme gerekir.

## Paket türleri
Sık hareket niyetleri sıra numaralı, eski veri atılabilir kanalda; yaşam döngüsü ve sonuçlar güvenilir kanalda taşınabilir. Güvenilir kanal kayıp altında head-of-line gecikmesi üretebileceğinden bütün transform'ları güvenilir yapmak doğru değildir. Kesin kanal seçimi seçilen framework'ün davranışıyla doğrulanır.

## Fuzz testleri
NaN/Infinity hedef, eksi ID, çok uzun string, bilinmeyen enum, 1 MB payload, sahte actor, eski epoch, yinelenen sequence. Hepsi sınırlı kaynak kullanımıyla reddedilmeli; exception storm veya log diski dolması yaratmamalıdır. Kütle/fiyat/hasar miktarı istemciden kabul edilen alan değildir.

## Gereksinim ve test izleri
- REQ-003 → TC-003 — İstemci paylaşılan state'i doğrudan değiştirememeli
- REQ-010 → TC-010 — Anchor işlemi tek ve atomik olmalı
- REQ-022 → TC-022 — Bozuk komut sınırlı kaynakla reddedilmeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-015 — Etkileşim sorgusu ve komut sözleşmesi](../01_gameplay/015_interaction_contract.md)
- [ASK-027 — Otorite, sahiplik ve veri sınırları](027_authority_matrix.md)
- [ASK-029 — Çoğaltma, snapshot ve ağ ilgisi](029_replication_snapshot.md)
- [ASK-034 — Ağ güvenliği ve kötüye kullanım sınırları](034_security_abuse.md)

