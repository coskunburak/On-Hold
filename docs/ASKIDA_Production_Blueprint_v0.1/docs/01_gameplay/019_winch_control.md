---
doc_id: ASK-019
title: "Vinç kontrolü ve kontrol kiralaması"
version: 0.1.0
status: proposed
owner_role: Gameplay
last_reviewed: 2026-09-07
dependencies: ["ASK-015","ASK-018","ASK-020","ASK-027","ASK-032"]
---

# Vinç kontrolü ve kontrol kiralaması

## İşlev
Vinç operatörü yükselt/alçalt/dur niyeti gönderir. V1'de platform bir tanımlı dikey rota boyunca ilerler; serbest 3D vinç inşası yoktur. Alt/üst durak, güvenli park konumu ve rota engelleri level verisidir.

## Tek sürücü
DriverLease oyuncu, platform, alınan epoch ve son yenileme zamanını taşır. Aynı anda bir sürücü vardır. İki geçerli istekte ilk host kabulü kazanır, diğerine Busy döner. Lease yalnız kontrol paneline erişim ve geçerli oyuncu durumu sürerken yenilenir. Menü, mesafe ihlali, kopma veya süre aşımı sürüş niyetini sıfırlar.

## Güvenlik duruşu
Komut akışı kesilince mevcut yükseltme niyeti sonsuza dek tutulmaz. Watchdog güvenli durdurma uygular. Duruşun anlık mı sınırlı ivmeli mi olduğu hissiyat testinde seçilir; her durumda maksimum hız ve ivme vardır. İstemci panel animasyonu platformun gerçek hareket otoritesi değildir.

## Engeller
Platform route sweep'i veya motorun fiziksel collision sonucu engel saptarsa devam komutu durumu zorla aşamaz. UI “Yukarıda engel var” der; engelin kimliği/gerekçesi tanıda tutulur. Maksimum eğim kilidi ile rota engeli farklı reasonCode'lardır. Açılma koşulu sağlanınca kendiliğinden tekrar hareket başlamaz; yeni oyuncu niyeti gerekir.

## Sabitleme ilişkisi
Yeni Secure/Unsecure için platform hızının P-WINCH-SECURE-SPEED altında olması gerekir. Hareket sırasında kopmayan görsel kayış simüle edilir; gerçek kopabilir rope veya düğüm çözme yoktur. Vinçte bekleyen oyuncu rolü bırakabilir; kontrolü alan kişi otomatik önceki yönü devralmaz.

## Test
İki oyuncu aynı karede kontrol alır; sürücü kablosu kesilir; yukarı tuşu basılıyken menü açılır; üst durakta paket tekrar gelir; engel kaldırılır. Her durumda tek sürücü, sınırlar içinde yükseklik ve güvenli sıfır niyet beklenir. Uzak istemcide panel ışığı ile gerçek driver bilgisi aynı authority kaynağından güncellenir.

## İlgili kaynak belgeler
- [ASK-015 — Etkileşim sorgusu ve komut sözleşmesi](015_interaction_contract.md)
- [ASK-018 — Platform, destek kütlesi ve kontrollü eğim](018_platform_balance.md)
- [ASK-020 — Sabitleme, kayış ve anchor sözleşmesi](020_straps_anchors.md)
- [ASK-027 — Otorite, sahiplik ve veri sınırları](../02_network/027_authority_matrix.md)
- [ASK-032 — Kopma, yeniden bağlanma ve host kaybı](../02_network/032_disconnect_reconnect_hostloss.md)

