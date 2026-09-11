---
doc_id: ASK-034
title: "Ağ güvenliği ve kötüye kullanım sınırları"
version: 0.1.0
status: proposed
owner_role: Network
last_reviewed: 2026-09-07
dependencies: ["ASK-027","ASK-028","ASK-044","ASK-078"]
---

# Ağ güvenliği ve kötüye kullanım sınırları

## Tehdit modeli
Arkadaş oturumu içinde bozuk veya değiştirilmiş istemci, paket spam'i, yanlış içerik sürümü, tekrar oynatılan komut ve hassas verinin log'a sızması ele alınır. Listen host'un kendi belleğini değiştirmesi v1'de önlenemez; rekabetçi anti-cheat vaat edilmez.

## Girdi sınırları
Komut boyutu, string uzunluğu, enum alanı, finite float, dünya koordinatı ve işlem sıklığı denetlenir. Actor bağlantıdan çözülür. Arbitrary asset path, dosya yolu, Lua/C# metni veya işletim sistemi komutu network payload olarak kabul edilmez. Unknown fields sürüm politikasıyla reddedilir veya güvenli atlanır; sessiz şema kayması yoktur.

## Kaynak tüketimi
Bağlantı başına input rate ve pahalı etkileşim sorgusu kotası farklıdır. İhlal sırasıyla sınırlama, uyarı ve koparma üretebilir. Her ret için büyük stack trace basılmaz; örnekleme ve sayıcılar kullanılır. Dedup cache, pending request ve reconnect rezervleri kapasite sınırlıdır.

## Kimlik ve gizlilik
Steam doğrulama biletleri ve servis anahtarları loglarda bulunmaz. Gameplay actorId oturumluk kimliktir; analitik için mümkün olduğunca kalıcı kişisel profil toplanmaz. Retention ve telemetry varsayılanı yayın öncesi yerel hukuk ve platform şartlarıyla doğrulanmalıdır; bu belge hukuki görüş değildir.

## Yetki kontrolleri
Yalnız host görev seçer ve kampanya harcar; driver yalnız kendi lease'i boyunca vinç yönü yollar; holder yalnız kendi hakkını bırakır. Host-local kod yolu da aynı domain koşullarına girer. Remote oyuncu Kick isteğiyle başka oyuncuyu çıkaramaz.

## Kabul
Fuzz girdilerde crash yok; 10 kat komut hızında host kaynak tüketimi kontrollü; token log taraması temiz; content hash uyuşmazlığı başlangıcı engeller. Güvenlik testi izole test oturumunda yapılır, kullanıcıya ait üçüncü taraf sistem hedeflenmez.

## Gereksinim ve test izleri
- REQ-022 → TC-022 — Bozuk komut sınırlı kaynakla reddedilmeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-027 — Otorite, sahiplik ve veri sınırları](027_authority_matrix.md)
- [ASK-028 — Komut protokolü, tekrarlar ve sıralama](028_command_protocol.md)
- [ASK-044 — Tanı, ölçüm ve gizlilik](../03_engineering/044_telemetry_debugging.md)
- [ASK-078 — Salt okunur MCP bilgi servisi şartnamesi](../08_knowledge/078_mcp_readonly_spec.md)

