---
doc_id: ASK-053
title: "Malzeme, texture ve aydınlatma standardı"
version: 0.1.0
status: proposed
owner_role: TechnicalArtist
last_reviewed: 2026-09-07
dependencies: ["ASK-046","ASK-049","ASK-056","ASK-065","ASK-066"]
---

# Malzeme, texture ve aydınlatma standardı

## Malzeme ailesi
Env_Plaster, Env_Brick, Env_Metal; Prop_Painted, Prop_Wood, Prop_Fabric; Equipment_Accent; Glass_Simple gibi küçük shared aileler önerilir. Her prefab kendine özgü onlarca materyal açmaz. Renk varyasyonu kontrollü vertex color veya uygun shader parametresiyle çözülür; batch/instancing etkisi ölçülür.

## Görsel birlik
Bevel ölçeği, roughness aralığı, aşınma maskesinin dağılımı ve renk doygunluğu master profile'dan yönetilir. Her kenarın beyaz parlaması ve aşırı kir ana stile aykırıdır. Nötr ışıkta materyal karşılaştırması yapılmadan hedef sarı ışıkla kusurlar gizlenmez.

## Texture bütçesi
Kahraman eşya için başlangıç 1K–2K, küçük prop için 256–512, modüler yüzey için ortak tile/atlas önerisi; bunlar sabit zorunluluk değil ekranda kaplanan alana göre karar girdisidir. Texture memory dosya boyutu değildir; çözünürlük, format, mip ve platform sıkıştırmasıyla ölçülür. Aynı texture'ın kopyaları hash ile bulunur.

## Aydınlatma
Bir ana yönlü ışık ve kontrollü yerel vurgularla başla. Dinamik gölge sayısı, mesafesi ve çözünürlüğü gerçek GPU'da ölçülür. Statik geometri bake kullanıyorsa dynamic item/karakter probe stratejisi hazırlanır; balkon altındaki eşya görünmez siyaha düşmemeli. Cam/pencere fake interior gameplay oda sınırı değildir.

## Teknik dikkat
SRP Batcher, GPU instancing ve static batching aynı mekanizma değildir; hangisinin gerçekten etkin olduğu Frame Debugger/profiler ile doğrulanır. Shared material üzerine runtime global renk yazıp bütün eşyaları yanlış değiştirme. Per-instance özellik yöntemi seçilen render yolu ile test edilir.

## Kabul
Ana sahnede karakter/eşya silueti üç kalite seviyesinde okunur; farklı paket malzemeleri tek ışıkta sırıtmıyor; eksik shader ve pembe materyal yoktur. Texture/art iyileştirmesi performans kapısını aşarsa alternatif çözüm veya bütçe istisnası gerekir.

## Gereksinim ve test izleri
- REQ-029 → TC-029 — Materyaller uyumlu ve bütçe içinde olmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-046 — Sanat yönetimi ve paket uyumu](046_art_direction.md)
- [ASK-049 — Her nesne için üretim şartnamesi](049_prop_asset_spec.md)
- [ASK-056 — Modüler dünya kiti ve apartman kimliği](../05_content/056_modular_world_kit.md)
- [ASK-065 — Performans hedefi ve ölçüm disiplini](../06_quality/065_performance_targets.md)
- [ASK-066 — Başlangıç asset, render ve fizik bütçeleri](../06_quality/066_render_physics_asset_budgets.md)

