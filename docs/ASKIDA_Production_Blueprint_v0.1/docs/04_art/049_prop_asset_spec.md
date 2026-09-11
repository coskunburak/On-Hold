---
doc_id: ASK-049
title: "Her nesne için üretim şartnamesi"
version: 0.1.0
status: proposed
owner_role: TechnicalArtist
last_reviewed: 2026-09-07
dependencies: ["ASK-016","ASK-041","ASK-046","ASK-050","ASK-066"]
---

# Her nesne için üretim şartnamesi

## Asset kartı
Kimlik, rol, oyun ölçüsü, massKg, davranış ailesi, etkileşim noktaları, teslimat sınırı, kaynak/lisans, poly/texture/material bütçesi, collision planı ve LOD ihtiyacı üretimden önce yazılır. Görsel sanatçı kütleyi hacimden kendiliğinden çıkarmaz; gameplay tasarım verisi kaynaktır.

## DCC standardı
Kaynak sahnede metre ve transform disiplini; export öncesi uygulanmış scale; temiz hierarchy; isimli mesh parçaları; dışa bakan normal; gereksiz iç geometri temizliği. Pivot yerleştirme amacıyla açıklanır: taşınabilir eşyada alt merkez tercih edilebilir, fizik centerOfMass ayrı alan olabilir. Pivot ile kütle merkezi aynı kavram değildir.

## Geometri/UV
Yakın tutulacak kahraman eşyada siluet önceliklidir. Ufacık vida geometriyle değil gerektiğinde normal/texture veya tamamen atlanarak çözülür. UV kanal amacı, texel-density sınıfı ve bake gereksinimi belirtilir. Lightmap UV yalnız kullanılan aydınlatma yoluna göre gerekir; dinamik eşya için körlemesine statik flag açılmaz.

## Gameplay socket'leri
Grip_L/Grip_R veya isimlendirilmiş grip dizisi; kayış temas noktaları; damage VFX noktaları; authored deliveryBounds; gerektiğinde audio emitter. Socket test mesh'iyle görünür doğrulanır. Tutma animasyonu collider'ın içindeki imkânsız noktaya hedeflenmez.

## Prefab
Tek root Rigidbody; doğru collision layer; domain/network adapter; görsel child; LODGroup gerekiyorsa; materials; ItemDefinition referansı; provenance kimliği. Ortak taşıma socket'leri, collider ve visible mesh aynı metre ölçeğinde eşleşir.

## Kabul masası
Ölçü küpü yanında görünüm; kapı geçişi; iki oyuncu tutma; drop test; platforma secure; delivery zone dönüş testi; uzaktan LOD; texture ve draw-call incelemesi. Her adım build ve asset sürümüyle kayda geçer. FBX dosyasının Unity'ye girmesi tek başına production kabulü değildir.

## Gereksinim ve test izleri
- REQ-025 → TC-025 — Prop tam teknik kabul almalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-016 — Eşya semantiği ve değişmezler](../01_gameplay/016_item_state_model.md)
- [ASK-041 — Editör araçları ve asset kabul otomasyonu](../03_engineering/041_editor_asset_validation.md)
- [ASK-046 — Sanat yönetimi ve paket uyumu](046_art_direction.md)
- [ASK-050 — Astra/Codex ile Blender üretimi ve export](050_ai_blender_export.md)
- [ASK-066 — Başlangıç asset, render ve fizik bütçeleri](../06_quality/066_render_physics_asset_budgets.md)

