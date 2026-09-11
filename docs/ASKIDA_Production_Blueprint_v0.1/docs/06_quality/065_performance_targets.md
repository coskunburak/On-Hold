---
doc_id: ASK-065
title: "Performans hedefi ve ölçüm disiplini"
version: 0.1.0
status: proposed
owner_role: Performance
last_reviewed: 2026-09-07
dependencies: ["ASK-009","ASK-010","ASK-040","ASK-066","ASK-069"]
---

# Performans hedefi ve ölçüm disiplini

## Hedef ve eksik bilgi
1080p/60 FPS başlangıç ürün hipotezidir. Referans CPU/GPU/RAM ve minimum cihaz henüz seçilmedi; bu yüzden Steam minimum sistem gereksinimi yazılamaz. Laptop/desktop, güç modu, sürücü ve ekran çözünürlüğü ölçüm kaydına girer. “Optimize low-poly” etiketinden donanım sonucu çıkarılmaz.

## Kare bütçesi
60 FPS için 16,67 ms frame aralığı vardır. CPU ve GPU süreleri basitçe toplanmaz; darboğaz ve pipeline davranışı incelenir. Önerilen ilk kontrol p95 CPU ve GPU frame sürelerinin hedef aralığa sığması, p99 spike'ların ayrıca raporlanmasıdır. Kesin p95/p99 kabul limiti referans cihaz seçildiğinde dondurulur; ortalama FPS yeterli değildir.

## Host ayrı profil
Host aynı anda yerel render, bütün shared physics ve diğer istemci ağ çıkışını taşır. Dört oyuncu host profili tek oyuncu editor profiliyle değiştirilemez. Remote istemci ve host'ta aynı scene/camera testleri ayrı kayda girer.

## Bellek ve yükleme
Peak RAM/VRAM, texture residency, prefab/scene yükleme süresi, GC allocation ve uzun oturum artışı ölçülür. Milestone hedefleri hardware kararıyla bağlanır. İlkel “her kare sıfır allocation” kuralı yerine hot path ve gözlenen hitch önceliklendirilir; gereksiz allocation yine giderilir.

## Kalite seçenekleri
Gölge mesafesi/kalitesi, dekor yoğunluğu, VFX yoğunluğu ve rendering ölçeği düşürülebilir. Gameplay collider, mandatory eşya veya denge hesabı düşük kalite seçeneğinde değişmez. Ağ mesaj sıklığı kullanıcı grafik seçeneğine bağlanmaz.

## Kabul
Aynı build'in kayıtlı rota replay girdisi veya kontrollü test rotasıyla karşılaştırması yapılır; deterministic fizik replay iddiası yoktur. Önce/sonra capture ve görüntü kalite karşılaştırması olmadan “optimizasyon” tamamlandı sayılmaz.

## Gereksinim ve test izleri
- REQ-037 → TC-037 — Hedef donanım performansı ölçülmeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-009 — Karar kaydı ve onay bekleyenler](../00_foundation/009_decision_register.md)
- [ASK-010 — Sözlük, birimler ve başlangıç parametreleri](../00_foundation/010_glossary_parameters.md)
- [ASK-040 — Unity fizik uygulaması ve teknik sınırlar](../03_engineering/040_physics_integration.md)
- [ASK-066 — Başlangıç asset, render ve fizik bütçeleri](066_render_physics_asset_budgets.md)
- [ASK-069 — Milestone kalite kapıları ve bitmiş iş](069_quality_gates.md)

