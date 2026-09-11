---
doc_id: ASK-024
title: "İlerleme ve küçük ölçekli ekonomi"
version: 0.1.0
status: proposed
owner_role: GameDesign
last_reviewed: 2026-09-07
dependencies: ["ASK-022","ASK-023","ASK-033","ASK-043"]
---

# İlerleme ve küçük ölçekli ekonomi

## Sahiplik
Kampanya host'a aittir. Misafir oyuncular host cüzdanının kopyasını kendi ilerlemeleri gibi kaydetmez. V1 karakter renkleri açıktır ve güç avantajı vermez. Kalıcı misafir ekonomisi, çapraz kampanya transferi ve çevrimiçi pazar yoktur. Bu tercih UI ve Steam açıklamasında anlaşılır olmalıdır.

## İlerleme amacı
İlerleme temel oyunu açmak için grind zorunluluğu değil, kısa vadeli hedef üretir. Temel vinç ve kayışlar ücretsiz görev ekipmanıdır. Önerilen yükseltmeler park ışığı okunurluğu gibi salt görsel şeylerden ibaret kalmamalı, fakat başarısız grubun oyunu oynayamamasına da yol açmamalıdır. İlk v1 adayları sınırlı vinç hız kademesi, kayış kapasitesi varyantı ve görev seçimi açılımıdır; her biri ayrı denge testi ister.

## Harcama işlemi
Yükseltme yalnız Hub'da host tarafından alınır. Command; upgradeId ve beklenen campaignRevision taşır. Host tanımı ve fiyatı kendi verisinden okur; yeterli bakiye, mevcut seviye ve bağımlılıkları doğrular. Bakiye düşümü, yükseltme seviyesi ve revision artışı tek kalıcı işlemde olur. İstemci fiyat göndererek indirim elde edemez.

## Softlock önleme
Kampanya negatif cüzdana düşmez; hata cezası borç yaratmaz. Her zaman başlangıç ekipmanıyla oynanabilir en az bir sözleşme vardır. Eşya kaybı yeni zorunlu ekipman satın alma döngüsü yaratmaz. Yükseltme etkisi küçük hız kazancından çok koordinasyonun yerine geçiyorsa yeniden tasarlanır.

## Denge tablosu
Her yükseltme için maliyet, etkilediği parametre, üst seviye, beklenen kazanma tur aralığı ve iki/dört kişi sonuç farkı tutulur. Bu arşiv nihai fiyat listesi değildir; önce teslimat değer dağılımı ve tur süresi ölçülmelidir. Ekonomi sayıları data sürümüne girer ve kayıttaki içerik sürümüyle ilişkilendirilir.

## Kabul
Arka arkaya başarısız olan ekip tekrar oynayabilir; satın alma spam'i tek harcama üretir; disk hatasında bakiye düşüp yükseltmenin kaybolması mümkün olmaz. Misafir ayrıldığında host ilerlemesi değişmez.

## Gereksinim ve test izleri
- REQ-019 → TC-019 — Satın alma güvenli ve softlock'suz olmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-022 — Teslimat doğrulaması ve tek seferlik sonuç](022_delivery_scoring.md)
- [ASK-023 — Sözleşme tanımı, başarı ve bitirme](023_contract_rules.md)
- [ASK-033 — Kampanya işlemleri ve kayıt otoritesi](../02_network/033_campaign_transaction.md)
- [ASK-043 — Save şeması, migration ve cloud çatışması](../03_engineering/043_save_schema_migration.md)

