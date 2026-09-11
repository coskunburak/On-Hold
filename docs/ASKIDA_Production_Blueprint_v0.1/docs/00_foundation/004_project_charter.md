---
doc_id: ASK-004
title: "Proje bildirgesi ve başarı tanımı"
version: 0.1.0
status: proposed
owner_role: Product
last_reviewed: 2026-09-07
dependencies: ["ASK-005","ASK-007","ASK-009","ASK-071"]
---

# Proje bildirgesi ve başarı tanımı

## Yetki ve mevcut durum
Kullanıcı tarafından istenenler: üretim kalitesinde hazırlanabilir Steam co-op oyunu, nesnelerde görsel uyum, optimizasyon, Unity veya Unreal ile uygulanabilirlik, AI destekli varlık üretim olanağı, ayrıntılı Markdown bilgi tabanı ve ZIP teslimi. Bunlar ihtiyaçtır. ASKIDA adı, dış cephe taşıma teması, Unity/URP, içerik miktarı ve takvim ise onaya açık çalışma önerileridir.

## Ürün vaadi
Bir oyuncu “Sola geç, dolap kayıyor!” dediğinde diğerinin fiziksel konumu anlamlı biçimde sonucu değiştirmelidir. Oyuncular başarıyı koordinasyonlarına, başarısızlığı anlaşılır kararlarına bağlamalıdır. Temel sosyal deneyim arkadaş grubudur; rastgele eşleştirme zorunlu çekirdek özellik değildir.

## Başarı katmanları
Teknik başarı: hedef cihazda kararlı kare süreleri; ağ koşullarında çelişmeyen eşya sahipliği; teslimatta ve kayıtta çoğaltılamayan ödül. Deneyim başarısı: kısa açıklamayla ilk eşyayı taşıyabilme, rol değiştirebilme, başarısızlıktan sonra yeniden deneme isteği. Ürün başarısı: mağaza ziyaretçisi kısa videoda özgün kancayı anlayabilmeli. Ticari başarı: net gelir üretim ve işletim maliyetlerini karşılamalı; fiyat ve dönüşüm verisi olmadan satış tahmini verilmez.

## Sorumluluk
Ürün sahibi kapsam ve harcamayı; teknik sorumlu motor/ağ kararını; sanat sorumlusu stil kabulünü; QA sorumlusu kanıt yeterliliğini onaylar. Küçük ekipte aynı kişi birden fazla rol üstlenebilir, ancak kendi ürettiği değişikliğe bağımsız kontrol yapılacak yöntem tanımlanmalıdır. Ekip sayısı henüz bilinmiyor.

## Durdurma koşulları
İki haftalık risk çalışmasında uzaktan platform deneyimi kabul edilebilir seviyeye yaklaşmıyorsa içerik üretimi büyütülmez. Denge mekaniği testte okunamıyorsa ilk çözüm daha fazla eşya veya seviye eklemek değildir; kütle işaretleri ve hareket aralığı yeniden tasarlanır. Lisansı belirsiz kritik asset alternatifsiz biçimde üretime sokulmaz.

## Onay çıktısı
Bir sayfalık “devam / daralt / yön değiştir” kararı; gerekçeli test klipleri; ekip kapasitesi; maliyet üst sınırı; karar sahibi ve tarih. Bu belge tek başına geliştirme bütçesi harcama yetkisi vermez.

## Gereksinim ve test izleri
- REQ-001 → TC-001 — Motor ve sürüm kararı kanıtla seçilmeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-005 — Vizyon, özgün kanca ve deneyim](005_vision_hook.md)
- [ASK-007 — Kapsam, sürümler ve kesme sırası](007_scope_matrix.md)
- [ASK-009 — Karar kaydı ve onay bekleyenler](009_decision_register.md)
- [ASK-071 — Üretim yol haritası ve kapasite senaryoları](../07_production/071_roadmap_capacity.md)

