---
doc_id: ASK-066
title: "Başlangıç asset, render ve fizik bütçeleri"
version: 0.1.0
status: proposed
owner_role: Performance
last_reviewed: 2026-09-07
dependencies: ["ASK-018","ASK-040","ASK-049","ASK-053","ASK-065"]
---

# Başlangıç asset, render ve fizik bütçeleri

## Rakamların durumu
Aşağıdaki bütçeler erken tasarım alarmıdır; ölçülmüş platform limiti değildir. ArtValidation ve 4 oyunculu benchmark sonrasında daraltılabilir veya gevşetilebilir. Bir varlığın bütçeyi geçmesi otomatik ret yerine gerekçeli performans incelemesi açar; toplam sahne maliyeti belirleyicidir.

| Kategori | Başlangıç alarmı |
|---|---|
| Yakın kahraman eşya LOD0 | 3k–12k triangle |
| Küçük dekor | 100–1.500 triangle |
| Karakter LOD0 | 10k–18k triangle; aksesuar dahil ölç |
| Taşınabilir prefab | Genelde 1–2 material slot |
| Ana eşya collision | Genelde 2–6 primitive parça |
| Aynı anda uyanık carryable | Önce 12–20 ile test; 30 stres |
| Gameplay rigidbody toplamı | Player/platform dahil ayrı sayaç |
| Texture | Ekran alanına göre 256–2K; ortak atlas önceliği |

## Render maliyeti
Triangle sayısı tek sınır değildir: draw calls, skinned mesh, transparan overdraw, gölgeli ışık, shader varyantı ve texture bandwidth ölçülür. Özellikle cama ve parçacığa eklenen transparan katmanlar düşük-poly avantajını silebilir. Uzak cephelerde LOD/occlusion maliyet kazancı gerçek sahnede doğrulanır.

## Fizik maliyeti
Awake body sayısı, contact pair, solver iteration, CCD ve joint sayısı izlenir. Çok parçalı compound collider şekil sayısı arttıkça maliyet büyüyebilir. Her küçük dekoru fizik nesnesi yapma. Kullanıcı tutabilsin diye bütün şehir props'larını network actor yapmak v1 dışıdır.

## Ağ maliyeti
Physics budget ile replicated object budget aynı değildir. Uyuyan item düşük frekanslı gönderilebilir, fakat yaşam döngüsü ve teslimat bilgisi korunur. Ağ optimizasyonu semantik görünmezlik yaratmamalıdır.

## İstisna kartı
Asset ID, bütçe aşımı, oyuncuya getirisi, measured CPU/GPU/network etkisi, alternatif, onay sahibi ve yeniden ölçüm tarihi. Örneğin piano 14k tris ama tek material ile sahnede önemsiz maliyet olabilir; ölçüm yoksa karar yoktur.

## Kabul
Üç farklı paket kaynağından gelen adaylar aynı bütçe kartıyla değerlendirilir. Düşük kalite seçeneği oyuncuya daha avantajlı collision veya denge sağlamaz. LOD geçişinde grip noktası ve gameplay bounds değişmez.

## Gereksinim ve test izleri
- REQ-029 → TC-029 — Materyaller uyumlu ve bütçe içinde olmalı
- REQ-037 → TC-037 — Hedef donanım performansı ölçülmeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-018 — Platform, destek kütlesi ve kontrollü eğim](../01_gameplay/018_platform_balance.md)
- [ASK-040 — Unity fizik uygulaması ve teknik sınırlar](../03_engineering/040_physics_integration.md)
- [ASK-049 — Her nesne için üretim şartnamesi](../04_art/049_prop_asset_spec.md)
- [ASK-053 — Malzeme, texture ve aydınlatma standardı](../04_art/053_materials_textures_lighting.md)
- [ASK-065 — Performans hedefi ve ölçüm disiplini](065_performance_targets.md)

