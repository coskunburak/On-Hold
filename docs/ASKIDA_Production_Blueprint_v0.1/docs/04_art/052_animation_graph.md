---
doc_id: ASK-052
title: "Animasyon grafiği ve gerekli klipler"
version: 0.1.0
status: proposed
owner_role: Animation
last_reviewed: 2026-09-07
dependencies: ["ASK-013","ASK-017","ASK-019","ASK-051"]
---

# Animasyon grafiği ve gerekli klipler

## Öncelik matrisi
P0 slice: idle, walk forward/back/strafe, start/stop blend, light carry pose, heavy two-hand carry, grab/release görsel geçişi, vinç kullanım pozu, düşme/geri dönüş geçişi. P1 demo: yön değiştirme, küçük denge düzeltme, başarı/başarısızlık tepkisi. P2 v1: kozmetik emote varyantları; gameplay avantajı yoktur.

## Grafiğin katmanları
Locomotion blend → upper-body carry mask → el IK → gerekli additive tepki. Oyun state'i animator state adından okunmaz. Animator tamamlanmadı diye authoritative Release geciktirilmez. Animasyon event'leri ses/yerel sunum tetikleyebilir; para, delivery veya holder yaratamaz.

## Root motion kararı
Çevrimiçi karakter hareketinde ilk öneri in-place klip + controller otoritesidir. Root motion gereken istisna ayrı senkronizasyon tasarımı ister; kaynak pakette root/no-root sürüm bulunduğu her klibin ücretsiz tier'da olduğu anlamına gelmez. Ağ hareketi ile animasyon root'unun çifte translasyonu ayak kayması ve state ayrışması yaratır.

## Retarget hattı
Kaynak klip lisansı → source avatar → target avatar → loop/seam → ayak temasları → carry mask → IK soket testi → gerçek oyun hızı. Aynı klibi farklı rig'e aktarmak son iş değildir. Düşük hızda foot sliding, omuzun mesh içine gömülmesi ve bilek dönmesi yakından incelenir.

## Senkronizasyon
Remote oyuncuya gerekli action/state parametreleri gönderilir; bütün kemik transform'ları ağdan akıtılmaz. Kısa tek-seferlik tepki eventId ile dedup edilir. Reconnect'te mevcut carry pose snapshot'tan kurulur, geçmiş tüm animasyon olayları tekrar oynatılmaz.

## Kabul
Grab reddedildiğinde eller eşyanın üzerinde kalmaz; son holder bırakınca carry katmanı kapanır; driver lease kaybında panel pozu biter. Animasyon sayısını artırmadan önce P0 klipler bütün karakterlerde ve 2/4 kişilik gerçek sahnede geçer.

## Gereksinim ve test izleri
- REQ-028 → TC-028 — Animasyon gameplay state'i belirlememeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-013 — Kontroller, kamera ve giriş niyetleri](../01_gameplay/013_controls_camera.md)
- [ASK-017 — Ortak taşıma ve kuvvet modeli](../01_gameplay/017_shared_carrying.md)
- [ASK-019 — Vinç kontrolü ve kontrol kiralaması](../01_gameplay/019_winch_control.md)
- [ASK-051 — Karakter, rig ve el IK standardı](051_character_rig.md)

