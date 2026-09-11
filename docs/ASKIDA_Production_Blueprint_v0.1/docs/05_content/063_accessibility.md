---
doc_id: ASK-063
title: "Erişilebilirlik ve kamera konforu"
version: 0.1.0
status: proposed
owner_role: UX
last_reviewed: 2026-09-07
dependencies: ["ASK-013","ASK-054","ASK-055","ASK-062","ASK-068"]
---

# Erişilebilirlik ve kamera konforu

## Minimum hedefler
Tuş yeniden atama, gamepad desteği, tutma toggle/basılı seçimi, fare hassasiyeti, kamera sarsıntısını kapatma, FOV seçeneği, ayrı ses kanalları, UI ölçeği ve renk dışı durum işaretleri. Bunlar final ürün için önerilen minimumdur; uygulanmış özellik iddiası değildir.

## Hareket konforu
Kamera roll zorlaması kapalı; head bob ve motion blur varsayılanı konfor testine göre düşük/kapalı. FOV'un dikey/yatay anlamı UI'da tutarlı olur. Düşme ve recovery geçişlerinde aşırı hızlandırılmış kamera yerine kısa kontrollü geçiş kullanılır. Platform görsel eğimiyle karakter kamerası aynı şekilde dönmek zorunda değildir.

## Motor beceri yükü
Tekrarlı hızlı tuş basma veya sürekli yüksek basınç gerektiren mini oyun yoktur. Secure belirli koşullarda tek niyettir; uzun zorunlu mash değildir. Toggle tutma menü/odak kaybı ve reconnect cleanup ile güvenli çalışmalıdır.

## İşitsel/görsel eşdeğer
Vinç kilidi, kritik hasar ve delivery ret nedeni hem UI/ikon hem uygun sesle sunulur. Ping komutları mikrofon olmadan yön ve yardım ihtiyacını anlatır. Oyuncu renginin yanında isim/simge kullanılır. Yanıp sönme ve büyük ekran dolumu sınırlanır.

## Test katılımı
Konfor veya erişim ihtiyacı olan oyuncular uygun bilgilendirmeyle test edilir; “tüm engeller için erişilebilir” gibi doğrulanmamış iddia yapılmaz. Ayarların menüye ulaşmadan önce erişilebilir olması gerekebilir; ilk açılış erişim ekranı düşünülür.

## Kabul
Ses kapalı, renk ayrımı sınırlı ve gamepad-only testlerinde temel görev tamamlanabilir. Tutma toggle kullanıcıyı eşya üzerinde kilitlemez. En büyük UI ölçeğinde kritik onay düğmesi ekran dışına çıkmaz. QA bulguları kozmetik diye otomatik düşük öncelik almaz.

## Gereksinim ve test izleri
- REQ-035 → TC-035 — Erişim ayarları temel görevi desteklemeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-013 — Kontroller, kamera ve giriş niyetleri](../01_gameplay/013_controls_camera.md)
- [ASK-054 — VFX, geri bildirim ve havuzlama](../04_art/054_vfx_feedback.md)
- [ASK-055 — Ses tasarımı, miks ve sesli iletişim](../04_art/055_audio_sound_design.md)
- [ASK-062 — Arayüz ve bilgi hiyerarşisi](062_ui_information.md)
- [ASK-068 — Oyuncu testi, eğlence ve okunurluk](../06_quality/068_playtest_protocol.md)

