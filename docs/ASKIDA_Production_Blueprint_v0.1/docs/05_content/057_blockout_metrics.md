---
doc_id: ASK-057
title: "Blockout ve oynanabilir alan metrikleri"
version: 0.1.0
status: proposed
owner_role: LevelDesign
last_reviewed: 2026-09-07
dependencies: ["ASK-013","ASK-017","ASK-018","ASK-056","ASK-058"]
---

# Blockout ve oynanabilir alan metrikleri

## Gri kutu önce gelir
İlk seviye yalnız kutu ve renklerden oluşabilir, ancak gerçek item bounds, platform ölçüsü ve controller capsule'ı kullanmalıdır. Küçük placeholder kutuyla kanıtlanan kapıdan final piano geçeceği varsayılmaz. Blockout art onayı değil mekanik doğrulama aracıdır.

## Zorunlu alanlar
Güvenli brifing zemini; iki kişinin eşya çevresine eriştiği kaynak oda; platforma yükleme eşiği; operatör paneli; karşı ağırlık için açık deck; hedef balkon; tam delivery bounds alanı; iki güvenli item recovery noktası. Her alan işlevi nedeniyle boyutlandırılır.

## Rota ilkeleri
Tek bir daralma ilk öğreticide yeterlidir. Aynı anda kapı, yüksek eğim ve zaman baskısı tanıtılmaz. Dengeleyici oyuncunun deck kenarını görmesi, operatörün tehlikeyi okuyabilmesi gerekir. Kaynak ile hedef arasında kaybolmayı önleyen siluet ve yön ipuçları bulunur.

## Ölçüm kartı
Kapı net genişlik/yükseklik; köşe dönüş yarıçapı; eşik yüksekliği; platform durak farkı; güvenli yaya geçişi; item teslimat hacmi; vinç görüş çizgisi. Her ölçüm metre olarak level kartına yazılır. “Biraz geniş” revizyon talebi kabul edilmez; gözlenen sorun ve yeni ölçü belirtilir.

## Debug görünümleri
Collision-only, item deliveryBounds, grip socket, platform support alanı, kill/recovery volume, occlusion ve network relevance. Görsel balkonun dışındaki invisible collider oyuncu güvenini kırar; hata gizlenmez. Drop zone gerçek yürüme alanıyla çakışmaz.

## Kabul
İki farklı ekip dört prototip eşyayı rota üzerinden taşıyabilir; en az bir ağır eşya iki kişiyle dönüş gerektirir ama fiziksel imkânsızlık yoktur. Dört oyuncu kritik panel önünde kalıcı kilit oluşturmaz. Video ve ölçü revizyonları saklanır.

## Gereksinim ve test izleri
- REQ-032 → TC-032 — Slice rotası iki/dört oyuncuyla geçilmeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-013 — Kontroller, kamera ve giriş niyetleri](../01_gameplay/013_controls_camera.md)
- [ASK-017 — Ortak taşıma ve kuvvet modeli](../01_gameplay/017_shared_carrying.md)
- [ASK-018 — Platform, destek kütlesi ve kontrollü eğim](../01_gameplay/018_platform_balance.md)
- [ASK-056 — Modüler dünya kiti ve apartman kimliği](056_modular_world_kit.md)
- [ASK-058 — Seviye 1 — Avlu apartmanı](058_level_one_courtyard.md)

