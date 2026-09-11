---
doc_id: ASK-075
title: "Steam hazırlığı, demo ve yayın kontrolü"
version: 0.1.0
status: proposed
owner_role: Product
last_reviewed: 2026-09-07
dependencies: ["ASK-006","ASK-048","ASK-069","ASK-070","ASK-080"]
---

# Steam hazırlığı, demo ve yayın kontrolü

## Ürün sunumu
Kısa video ilk saniyelerde dış cephe yük taşıma ve oyuncunun karşı ağırlık oluşunu göstermeli. Sadece çevre gezisi veya logo açılışı özgün kancayı açıklamaz. Mağaza görseli gerçek oynanabilir deneyimi temsil eder; AI konseptini çalışır oyun görüntüsü gibi kullanmayın. Satış garantisi veya doğrulanmamış ödül/review iddiası yoktur.

## Platform işleri
Steamworks erişimi, uygulama yapılandırması, branch/depot düzeni, build yükleme, mağaza varlıkları, içerik anketi, destek bağlantısı, minimum/önerilen sistem gereksinimi ve yayın inceleme süreci güncel resmi belgelerden tekrar doğrulanır. Hesap, ücret, vergi ve tarih kuralları burada sabitlenmedi; kullanıcı adına dış işlem yapılmadı.

## Demo
Bir bina/üç sözleşme önerisi; arkadaş daveti, açıklayıcı co-op gereksinimi, onboarding, geri bildirim yolu ve açık known limitations. Demo ile tam oyun farklı content manifest taşıyabilir; uyumsuz oyuncular aynı run'a alınmaz. Demo save ilerlemesinin tam oyuna taşınıp taşınmayacağı ayrı ADR ister; otomatik vaat edilmez.

## AI beyanı
Oyuncuya ulaşan AI destekli görsel, ses veya başka içerik envanteri tutulur; yayın anında Steam anketinin güncel gereksinimi kontrol edilir. Kodla üretilmiş mesh “nasıl olsa kod” diye otomatik kapsam dışı sayılmaz. [Steam içerik anketi](https://partner.steamgames.com/doc/gettingstarted/contentsurvey). Genel geliştirme verimliliği ile oyuncunun tükettiği içerik ayrımı uygulanırken platformun geçerli metni esas alınır.

## Release kapısı
Kritik bug yok; save ve rollback testli; gerçek internet co-op; hedef donanım verisi; lisans/provenance; dil/erişilebilirlik; store açıklamasıyla özellik uyumu; destek sorumlusu. Yayın takvimi harici inceleme sürelerini ve olası ret düzeltmelerini kapsar.

## Ticari karar
Fiyat, indirim, çıkış tarihi ve içerik üretici erişimi kullanıcı onayı ve güncel pazar verisi gerektirir. Bu paket ücret ödeme, mağaza açma veya mesaj gönderme yetkisi değildir.

## Gereksinim ve test izleri
- REQ-040 → TC-040 — Yayın iddiaları kanıta ve lisansa uymalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-006 — Hedef kitle ve ticari doğrulama](../00_foundation/006_audience_market_validation.md)
- [ASK-048 — Lisans, kaynak kaydı ve AI kullanımı](../04_art/048_licenses_provenance.md)
- [ASK-069 — Milestone kalite kapıları ve bitmiş iş](../06_quality/069_quality_gates.md)
- [ASK-070 — Regresyon, destek ve yayın sonrası triyaj](../06_quality/070_release_regression_support.md)
- [ASK-080 — Kaynaklar, doğrulama sınırları ve açık kanıtlar](../08_knowledge/080_sources_evidence_limits.md)

