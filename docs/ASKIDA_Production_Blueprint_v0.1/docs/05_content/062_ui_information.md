---
doc_id: ASK-062
title: "Arayüz ve bilgi hiyerarşisi"
version: 0.1.0
status: proposed
owner_role: UX
last_reviewed: 2026-09-07
dependencies: ["ASK-013","ASK-015","ASK-019","ASK-022","ASK-032","ASK-063"]
---

# Arayüz ve bilgi hiyerarşisi

## Ekranlar
Frontend; lobi/davet; loading; brifing; gameplay HUD; görev panosu; sonuç; Hub yükseltme; ayarlar; bağlantı/kayıt hata ekranları. Aynı bilgi farklı ekranda farklı formülle hesaplanmaz. Cüzdan campaign state'ten, geçici tur değeri delivery ledger'dan gelir ve farklı etiketlenir.

## Gameplay hiyerarşisi
Merkezde küçük etkileşim niyeti ve ret nedeni; platform kontrolünde denge ve vinç kilidi; kenarda süre/kota; pingler dünya bağlamında. Sürekli bütün oyuncuların sayısal kütlesini göstermek zorunlu değildir. UI fiziksel silueti ve görüşü kapatmamalıdır.

## Etkileşim durumları
Uygun, niyet gönderiliyor, kabul edildi, reddedildi, meşgul. Reddedilen komut bir frame yeşil başarı göstermemeli. Ret metni düzeltilir eylem önerir: “Önce eşyayı bırak”, “Vinç hareket ediyor”, “Teslimat alanına tamamen yerleştir”. Hata kodu localization anahtarına eşlenir.

## Lobi ve kopma
Host işareti, bağlantı durumu, content/build uyuşmazlığı ve hazır bilgisi net görünür. Reconnect rezervi oyuncuya geri dönüş beklentisini açıklar; sürenin ağ kopmasını tespit sonrası başladığı uygulama ayrıntısı yanlış geri sayım üretmemelidir. Host kaybı ekranı mevcut turun korunmadığını söyler.

## Sonuç ekranı
Teslim edilen/zorunlu/kaybedilen eşyalar; hasar nedeniyle değer farkı; başarı bonusu; kaydedilme durumu; sonraki eylem. “Kaydediliyor” ve “Kaydedildi” ayrı durumdur. Başarı sesi disk commit öncesi ekonomik güvence olarak kullanılmaz.

## Kabul
1080p ve seçili düşük çözünürlükte metin okunur; gamepad odağı hiçbir modalda kaybolmaz; renk olmadan başarı/ret ayrılır. İlk oyuncu geçici tur gelirini kendi misafir cüzdanı sanmamalıdır. UI screenshot regresyonu farklı dillerle yapılır.

## Gereksinim ve test izleri
- REQ-034 → TC-034 — UI otorite ve kayıt durumunu doğru göstermeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-013 — Kontroller, kamera ve giriş niyetleri](../01_gameplay/013_controls_camera.md)
- [ASK-015 — Etkileşim sorgusu ve komut sözleşmesi](../01_gameplay/015_interaction_contract.md)
- [ASK-019 — Vinç kontrolü ve kontrol kiralaması](../01_gameplay/019_winch_control.md)
- [ASK-022 — Teslimat doğrulaması ve tek seferlik sonuç](../01_gameplay/022_delivery_scoring.md)
- [ASK-032 — Kopma, yeniden bağlanma ve host kaybı](../02_network/032_disconnect_reconnect_hostloss.md)
- [ASK-063 — Erişilebilirlik ve kamera konforu](063_accessibility.md)

