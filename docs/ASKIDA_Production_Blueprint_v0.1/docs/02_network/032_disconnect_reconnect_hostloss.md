---
doc_id: ASK-032
title: "Kopma, yeniden bağlanma ve host kaybı"
version: 0.1.0
status: proposed
owner_role: Network
last_reviewed: 2026-09-07
dependencies: ["ASK-012","ASK-016","ASK-019","ASK-029","ASK-033"]
---

# Kopma, yeniden bağlanma ve host kaybı

## Misafir kopması
Host bağlantı kaybını belirleyince holder ve driver haklarını temizler, avatarı gameplay temasından çıkarır ve yuvasını P-RECONNECT-SEC süresince aynı doğrulanmış oyuncu kimliğine ayırır. Dünya ve zamanlayıcı devam eder; genel pause yoktur. Holder cleanup grace süresini beklemez.

## Yeniden bağlanma
Doğrulanan aynı oyuncu, geçerli epoch ve rezerv varsa güncel snapshot alır. Yeni input sequence bağlamı açılır; eski bağlantının tampon komutları işletilmez. Oyuncu sabit güvenli spawn noktasına gelir, platform üzerinde kaldığı poz otomatik geri yüklenmez. Önceki tuttuğu eşya tekrar eline yapışmaz; bu sırada başka oyuncu onu teslim etmiş olabilir.

## Rezerv bitimi
Rezerv bittiğinde aktif turda yeni oyuncuya yuva açılmaz; sonraki Lobby/Hub aşamasına kadar başlangıç roster'ı korunur. Eski oyuncu geç gelirse tur devam ediyor mesajı görür. Oyuncu sayısına göre görev hedefleri Active sırasında değişmez.

## Yalnız kalan host
Tur zaten başlamışsa host tek başına sürdürmeyi veya abort etmeyi seçebilir. Tek kişi yeni tur başlatamaz. Tamamlama mümkün değilse abort mevcut tur kazançlarını kaybettirir; önceki kampanya ilerlemesini değil. İşlerin iki kişi tasarlandığı ve tek başına bitişin garanti olmadığı açıklanır.

## Host kaybı
V1 host migration içermez. İstemciler bağlantı kaybı ekranı ve yeniden lobi seçenekleri alır. Tur ortası fizik checkpoint'i yoktur; host yeniden açtığında son başarıyla commit edilmiş kampanya checkpoint'i yüklenir. Aktif turdaki teslimatlar ve henüz kaydedilmemiş sonuç kaybolabilir. UI bu sınırlamayı gizlemez.

## Test
Holder kopması, vinç sürücüsü kopması, reconnect snapshot sırasında ikinci kopma, aynı hesabın çift giriş denemesi, host'un Settling öncesi/sonrası kapanması. İki aktif avatar veya tekrar ödeme olmamalıdır. Bağlantı tespiti süresi transport ayarına bağlıdır; 60 saniye rezerv, ağın kopmayı anında anlayacağı garantisi değildir.

## Gereksinim ve test izleri
- REQ-016 → TC-016 — Misafir reconnect temiz snapshot almalı
- REQ-017 → TC-017 — Host kaybı checkpoint sınırını korumalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-012 — Oturum ve görev durum makinesi](../01_gameplay/012_session_state_machine.md)
- [ASK-016 — Eşya semantiği ve değişmezler](../01_gameplay/016_item_state_model.md)
- [ASK-019 — Vinç kontrolü ve kontrol kiralaması](../01_gameplay/019_winch_control.md)
- [ASK-029 — Çoğaltma, snapshot ve ağ ilgisi](029_replication_snapshot.md)
- [ASK-033 — Kampanya işlemleri ve kayıt otoritesi](033_campaign_transaction.md)

