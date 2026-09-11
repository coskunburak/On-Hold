---
doc_id: ASK-040
title: "Unity fizik uygulaması ve teknik sınırlar"
version: 0.1.0
status: proposed
owner_role: Gameplay
last_reviewed: 2026-09-07
dependencies: ["ASK-017","ASK-018","ASK-020","ASK-026","ASK-066"]
---

# Unity fizik uygulaması ve teknik sınırlar

## Fizik sahnesi
Fixed-step başlangıç önerisi 50 Hz; renderer 60 FPS hedefinden bağımsızdır. Maksimum catch-up ve aşırı host yükünde davranış profille seçilir. Sahneye daha fazla substep eklemek yanlış kuvvet modelini düzeltmez. Dünya küçük ölçekte tutulur; kilometrelerce şehir koordinatı v1 gereği değildir.

## Collider standardı
Dinamik eşyalarda basit compound primitive collider veya uygun convex mesh tercih edilir. Girintili mobilyanın görsel triangle mesh'ini otomatik dinamik collider yapmak yasaktır. Bir eşya kökü tek Rigidbody, altında collision parçaları taşır. Statik bina collider'ları oynanabilir yüzeyi destekler; küçük süs çıkıntıları gereksiz takılma yaratmaz. [Unity compound collider](https://docs.unity3d.com/6000.0/Documentation/Manual/compound-colliders-introduction.html).

## Collision layer önerisi
WorldStatic, Player, Carryable, Platform, Trigger, Cosmetic. Collision matrix bilinçli tanımlanır; Cosmetic fizik simülasyonuna girmez. Player-Player çarpışması spike sonucuna göre seçilir; çarpışmayı kapatmak item-player sorununu da çözmüş sayılmaz. Trigger ve collider callback'leri domain event kuyruğuna veri sağlar.

## Servo ve platform
Force/torque FixedUpdate veya seçilen network physics tick ile tek yerde uygulanır. NetworkTransform, parent hareketi ve custom servo aynı body'yi farklı zamanlarda sürmez. Platform taşıma hızı ve dönme noktasal hızı karakter controller'a yalnız bir kez eklenir. CCD yalnız hızlı/kritik item'larda ölçümle açılır; her objede pahalı ayar kullanmak varsayılan değildir.

## Kararlılık araçları
NaN guard, hız/tork üst sınırı, penetration tanısı, sleeping body sayacı, joint sayacı, temas çifti görselleştirmesi. Guard fizik patlamasını gizli teleportlarla sürekli örtmemelidir; reasonCode ve ölçüm bırakır. Fizik deterministik lockstep kabul edilmez, state host'tan çoğaltılır.

## Kabul
Kapı sıkışması, üst üste yük, kenarda tek teker temas, düşen piano ve platform üzerindeki dört oyuncu benchmark sahnesinde test edilir. 10 dakika boyunca enerji birikmesi veya sürekli correction döngüsü varsa sanat üretim kapısı açılmaz.

## Gereksinim ve test izleri
- REQ-005 → TC-005 — Platform destek kütlesini bir kere saymalı
- REQ-012 → TC-012 — Hasar temas sayısıyla çoğalmamalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-017 — Ortak taşıma ve kuvvet modeli](../01_gameplay/017_shared_carrying.md)
- [ASK-018 — Platform, destek kütlesi ve kontrollü eğim](../01_gameplay/018_platform_balance.md)
- [ASK-020 — Sabitleme, kayış ve anchor sözleşmesi](../01_gameplay/020_straps_anchors.md)
- [ASK-026 — Ağ çözümü seçimi ve risk prototipi](../02_network/026_network_adr_spike.md)
- [ASK-066 — Başlangıç asset, render ve fizik bütçeleri](../06_quality/066_render_physics_asset_budgets.md)

