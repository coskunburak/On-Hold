---
doc_id: ASK-043
title: "Save şeması, migration ve cloud çatışması"
version: 0.1.0
status: proposed
owner_role: Backend
last_reviewed: 2026-09-07
dependencies: ["ASK-024","ASK-033","ASK-042"]
---

# Save şeması, migration ve cloud çatışması

## Kayıt bölümleri
Campaign: schemaVersion, campaignId, revision, balance, upgrades, unlockedContracts, completedRuns, contentVersion. Settings: dil, ses, kamera, input tercihleri. Bunlar yaşam döngüsü ve senkronizasyon politikası farklı olduğu için ayrıdır. Runtime rigidbody/joint snapshot'ı campaign kaydı değildir.

## Migration
Sürüm N'den N+1'e açık, test edilebilir dönüştürücü bulunur. Orijinal dosya başarılı dönüşüm doğrulanana kadar korunur. Bilinmeyen ileri schema okunursa eski build üzerine yazmaz; daha yeni sürüm gerektiğini bildirir. Content ID kaldırılmışsa eşleme veya güvenli fallback tanımlanır; aynı ID başka anlamla kullanılmaz.

## Doğrulama
JSON parse başarısı yeterli değildir: sayı aralıkları, enumlar, referanslar, revision, cüzdan taşması ve duplicate transaction kayıtları kontrol edilir. Checksum yanlışlıkla bozulmayı saptar; kullanıcı hilesine karşı güvenlik kanıtı değildir. Kullanıcı kaydı bozuksa backup ve dışa aktarma yolu sunulur, sessiz wipe yapılmaz.

## Cloud
Steam Cloud dosya senkronizasyonu aktif multiplayer transaction veya host migration değildir. [Steam Cloud belgeleri](https://partner.steamgames.com/doc/features/cloud). Hangi dosyaların hangi aşamada senkronize edileceği platform entegrasyonunda belirlenir. Aynı kampanyanın iki cihazda farklı ilerlemesi için revision tek başına küresel zaman sırası kanıtı olmaz; last-modified, cihaz ve içerik özetiyle kullanıcıya çatışma seçimi gerekebilir.

## Test fixture'ları
Boş yeni kayıt, N-1 kayıt, eksik opsiyonel alan, bozuk JSON, ileri sürüm, negatif bakiye, çok büyük sayı, silinmiş content ID, farklı iki cihaz dalı. Fixture'lar gerçek kullanıcı kişisel verisi taşımaz.

## Kabul
Migration iki kez çalışınca değer iki kez artmaz; başarısız dönüşüm orijinali korur; eski build yeni kaydı bozmaz. Cloud kapalıyken yerel kampanya çalışır. Geri yükleme UX'i test edilmeden “veri güvenli” denmez.

## Gereksinim ve test izleri
- REQ-017 → TC-017 — Host kaybı checkpoint sınırını korumalı
- REQ-018 → TC-018 — Kayıt atomik ve migrate edilebilir olmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-024 — İlerleme ve küçük ölçekli ekonomi](../01_gameplay/024_progression_economy.md)
- [ASK-033 — Kampanya işlemleri ve kayıt otoritesi](../02_network/033_campaign_transaction.md)
- [ASK-042 — Build, CI ve bağımlılık kilitleme](042_build_ci_release.md)

