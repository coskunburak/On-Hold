---
doc_id: ASK-051
title: "Karakter, rig ve el IK standardı"
version: 0.1.0
status: proposed
owner_role: Animation
last_reviewed: 2026-09-07
dependencies: ["ASK-013","ASK-046","ASK-047","ASK-052"]
---

# Karakter, rig ve el IK standardı

## Karakter ailesi
Universal Base Characters ortak humanoid taban adayıdır. Tek iskelet standardı, uyumlu boy aralığı ve dört okunur renk varyantı hedeflenir. Renk seçimi collider veya kütleyi değiştirmez. Ürün sayfasındaki poligon/saç/karakter sayısı ücretsiz tier'ın eksiksiz içeriği sayılmaz; indirilen dosyalar envanterde doğrulanacaktır.

## Rig kabulü
Bir root, tutarlı pelvis/spine/limb hiyerarşisi, uygulanmış ölçek, eksen dönüşümü belgeli; T/A-pose uyumu; hatalı weight, boş vertex ve aşırı deformation kontrolü. Parmak kemikleri mevcutsa sayısı/isim eşlemesi kayda girer. Humanoid avatar yeşil görünmesi taşıma pozunun iyi göründüğü anlamına gelmez.

## Oyun temsili
V1 tam active ragdoll controller önermez. Karakter hareketi kontrollü controller; animasyon görseldir. Düşme sırasında kısa cosmetic ragdoll ancak ağ ve konfor bütçesi uygunsa eklenir. Birinci kişi kolları ile diğer oyuncuların gördüğü tam beden ayrı rig/presentation olabilir, fakat eşya socket'i ve el hedefleri aynı dünya anlamını korur.

## IK
El hedefi authoritative holder bağlantısından türetilen grip socket'idir. İstemci görsel IK ağırlığını yumuşatır; IK eşyanın fizik konumunu belirlemez. Omuz erişim sınırı aşılırsa el sonsuza uzamaz; tutma hedefi/kamera sınırlaması devreye girer. Dirsek hint ve yerel el rotasyon offset'leri her grip ailesi için test edilir. [Unity Two Bone IK](https://docs.unity3d.com/Packages/com.unity.animation.rigging%401.1/manual/constraints/TwoBoneIKConstraint.html) temel araçtır; paket sürüm uyumu ayrıca seçilir.

## Test pozları
Boş yürüyüş, ağır eşya, baş üstü olmayan yüksek tutuş, ters iki holder, vinç paneli, merdiven ve platform eğimi. Kamera yakınında kol mesh'inin clipping'i ve uzaktan ayak kayması ayrı kontrol edilir. En uzun/kısa avatar aynı kapıdan geçer.

## Kabul
Bir animasyon klibi tüm onaylı karakterlerde deformasyon kontrolünden geçer; saç/aksesuar sayısı skinned render maliyetini aşmaz. Yeniden bağlanmada eski el IK hedefi silinmiş item'a bağlı kalmaz.

## Gereksinim ve test izleri
- REQ-027 → TC-027 — Tek rig bütün karakterlerde çalışmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-013 — Kontroller, kamera ve giriş niyetleri](../01_gameplay/013_controls_camera.md)
- [ASK-046 — Sanat yönetimi ve paket uyumu](046_art_direction.md)
- [ASK-047 — Seçilecek paketler ve kullanım sınırları](047_package_selection.md)
- [ASK-052 — Animasyon grafiği ve gerekli klipler](052_animation_graph.md)

