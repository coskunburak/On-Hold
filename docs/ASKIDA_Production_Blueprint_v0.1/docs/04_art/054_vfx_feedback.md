---
doc_id: ASK-054
title: "VFX, geri bildirim ve havuzlama"
version: 0.1.0
status: proposed
owner_role: VFX
last_reviewed: 2026-09-07
dependencies: ["ASK-021","ASK-022","ASK-046","ASK-053","ASK-066"]
---

# VFX, geri bildirim ve havuzlama

## Olay kataloğu
Hafif darbe: küçük toz + kısa ses. Ağır darbe: daha yoğun ama görüşü kapatmayan toz ve metal vurgu. Kritik integrity: lokal hasar belirtisi. Secure: kısa kilit ışığı/ikon. Delivery: kontrollü konfeti veya yıldız vurgusu. Recovery: anlaşılır yeniden belirme geçişi. Efektler oyunun state'ini yaratmaz, state'i anlatır.

## Kaynak
[Kenney Particle Pack](https://kenney.nl/assets/particle-pack) CC0 texture kaynağı adayıdır; hazır Unity URP prefab paketi sayılmaz. Texture'dan kendi particle system/material kurulur. [Cartoon FX Remaster Free](https://assetstore.unity.com/packages/vfx/particles/cartoon-fx-remaster-free-109565) alternatif inceleme kaynağıdır; ürün sayfasındaki eski uyumluluk tablosu hedef Unity sürümünün doğrulandığı anlamına gelmez.

## Teknik profil
Her efekt süre, maksimum parçacık, emit şekli, material, ışık kullanımı, gölge, ekran kaplama ve pool kapasitesi taşır. Basit efektlerde pahalı ışık veya collision varsayılan kapalıdır. GPU maliyeti çoğunlukla overdraw ve shader olabilir; sadece particle adedine bakılmaz.

## Ağ
Host önemli event'e kimlik verir; istemci aynı event'i bir kez gösterir. Kozmetik darbe sesi/partikül yerelde tahmin edilebilirse hasar onayıyla çelişmemelidir. Her particle için RPC gönderilmez. Reconnect'te eski delivery konfeti tekrar oynatılmaz, kalıcı hasar görünümü snapshot'tan kurulur.

## Havuz
Pool başlangıç ve tavanı ölçümle belirlenir. Kapasite dolunca düşük öncelikli efekt düşürülebilir; gameplay state düşürülemez. Pool reset; timer, renk, emitter target ve callback temizliğini içerir. Sonsuz yaşayan child particle bellek sızıntısı sayılır.

## Kabul
Dört oyuncu ve çoklu darbe sahnesinde eşya kenarı/prompt görünür kalır. Hareket azaltma ve ekran sarsıntısı kapatma çalışır. Tüm VFX kapalıyken oyun semantiği doğru işler; alternatif UI/ses bilgisi vardır.

## Gereksinim ve test izleri
- REQ-030 → TC-030 — VFX tek olay ve sınırlı havuz kullanmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-021 — Hasar ve kontrollü kırılma](../01_gameplay/021_damage_breakage.md)
- [ASK-022 — Teslimat doğrulaması ve tek seferlik sonuç](../01_gameplay/022_delivery_scoring.md)
- [ASK-046 — Sanat yönetimi ve paket uyumu](046_art_direction.md)
- [ASK-053 — Malzeme, texture ve aydınlatma standardı](053_materials_textures_lighting.md)
- [ASK-066 — Başlangıç asset, render ve fizik bütçeleri](../06_quality/066_render_physics_asset_budgets.md)

