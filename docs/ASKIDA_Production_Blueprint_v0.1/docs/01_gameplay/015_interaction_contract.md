---
doc_id: ASK-015
title: "Etkileşim sorgusu ve komut sözleşmesi"
version: 0.1.0
status: proposed
owner_role: Gameplay
last_reviewed: 2026-09-07
dependencies: ["ASK-016","ASK-017","ASK-020","ASK-027","ASK-028"]
---

# Etkileşim sorgusu ve komut sözleşmesi

## Sorgu ile işlem ayrımı
İstemci yakın etkileşim adaylarını gösterir; son kabul host'a aittir. Görünür prompt bir garanti değildir. Query sonucu actorId, targetId, actionId, görülen revision, izin durumu ve kullanıcıya çevrilebilir reasonCode içerir. Execute aynı önkoşulları host üzerinde yeniden değerlendirir.

## Ortak doğrulama sırası
Oturum Active mı? Bağlantı doğrulanmış oyuncuya mı ait? Hedef bu epoch içinde var mı? Komut türü bu nesnede destekleniyor mu? Mesafe ve görüş/erişim yolu geçerli mi? Hedef terminal durumda mı? Rol/holder/driver koşulu sağlanıyor mu? Hız limiti ve payload sonlu/sınırlar içinde mi? Ancak bundan sonra atomik durum değişimi yapılır. İstemcinin gönderdiği mesafe veya kütle güvenilir değildir.

## Örnek sonuç kodları
Accepted, Pending, TooFar, Blocked, Busy, InvalidState, StaleRevision, RateLimited, WrongSession, UnsupportedAction. Ham teknik detaylar log'a gider; oyuncuya “Biraz yaklaş”, “Önce vinci durdur”, “Eşyayı bırakıp sabitle” gibi eylem öneren kısa metin gösterilir.

## Eşzamanlı istek
Aynı tick içindeki istekler host alım sırası, sonra kararlı actor kimliği ile sıralanır. Sonuç arrivalOrdinal ile loglanır. Bu yöntem farklı makinelerde tüm fiziğin deterministik olduğu iddiası değildir. İlk kabul nesne revision'ını artırır; sonraki komut yeni durumla tekrar değerlendirilir. İki geçerli tutma yuvası varsa ikinci tutma kendi şartlarıyla kabul edilebilir.

## Bırakma istisnası
Release, oyuncunun mevcut holder hakkını kaldıran güvenli işlemdir. Revision eski diye oyuncu eşyaya kilitlenmez: epoch, hedef ve holder kimliği geçerliyse idempotent release uygulanır. Aktif hakkı olmayan oyuncuya başarı benzeri “zaten bırakılmış” sonucu verilebilir; başka oyuncunun holder hakkı silinmez.

## Kabul
Prompt kaybolduğu karede gönderilen komut güvenli reddedilir. Aynı requestId tekrarında ikinci fizik joint'i oluşmaz. Duvar arkasından tutma, hayali objeye komut ve NaN hedefler dünyayı değiştirmez. Bu kurallar görsel etkileşim bileşeninden bağımsız test edilebilir olmalıdır.

## Gereksinim ve test izleri
- REQ-004 → TC-004 — İki holder sınırlı kuvvetle aynı eşyayı taşıyabilmeli
- REQ-034 → TC-034 — UI otorite ve kayıt durumunu doğru göstermeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-016 — Eşya semantiği ve değişmezler](016_item_state_model.md)
- [ASK-017 — Ortak taşıma ve kuvvet modeli](017_shared_carrying.md)
- [ASK-020 — Sabitleme, kayış ve anchor sözleşmesi](020_straps_anchors.md)
- [ASK-027 — Otorite, sahiplik ve veri sınırları](../02_network/027_authority_matrix.md)
- [ASK-028 — Komut protokolü, tekrarlar ve sıralama](../02_network/028_command_protocol.md)

