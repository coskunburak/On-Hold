---
doc_id: ASK-033
title: "Kampanya işlemleri ve kayıt otoritesi"
version: 0.1.0
status: proposed
owner_role: Backend
last_reviewed: 2026-09-07
dependencies: ["ASK-012","ASK-022","ASK-024","ASK-043"]
---

# Kampanya işlemleri ve kayıt otoritesi

## Kalıcı sınır
Diskteki kampanya yalnız host'a aittir. Active turdaki fizik dünyası kaydedilmez. Kalıcı değişiklikler tur sonucu commit'i ve Hub satın alımıdır. Bu sınırlama tekrar oynanabilirliği korur ve canlı joint/transform geri yükleme karmaşıklığını v1'den çıkarır.

## Tur sonu işlem
ContractRunUUID önceki committedRunIds içinde varsa mevcut sonucu döndür. Yoksa doğrulanmış teslimat defterinden sonuç hesapla, başarı bonusunu uygula, bakiye ve açılımları türet, yeni campaignRevision üret. Yeni checkpoint transactionId, schemaVersion, contentVersion, result summary ve dedup run kaydı taşır. Kalıcı yazma başarılı olmadan Results “kaydedildi” göstermez.

## Atomiklik
Öneri geçici dosya + flush + aynı hacimde güvenli replace + son iyi backup yaklaşımıdır; exact OS davranışı uygulamada doğrulanmalıdır. Birden fazla bağımsız JSON dosyasına para ve ilerleme ayrı yazılmaz. Günlük/journal tasarlanırsa recovery kuralları ve disk büyüme sınırı bulunur; “journal var” tek başına atomiklik kanıtı değildir.

## Çökme pencereleri
Sonuç hesaplandı/yazılmadı: önceki checkpoint. Geçici dosya yazıldı/replace olmadı: geçerli ana kayıt veya doğrulanmış kurtarma. Replace oldu/UI görmedi: run UUID tekrarında ek para yok. Backup bozuk: hata görünür, geçersiz veri sessizce yeni oyun diye üzerine yazılmaz.

## Misafir deneyimi
Konuk sonuç ekranında grup kazancını görür; kendi cüzdanına eklendiği izlenimi yaratılmaz. Kendi ses/kamera/tuş tercihleri ayrı yerel ayar dosyasıdır. Host kimliği değiştiğinde başka kampanya açıldığı anlaşılır.

## Kabul
Her disk aşamasına yapay hata enjekte edilir. Aynı result 100 kere uygulanınca para bir kez artar. Negatif fiyat ve taşan integer reddedilir. Gerçek para ekonomisi veya sunucu destekli değer bulunmadığından ağır güvenlik iddiası yoktur; buna rağmen kullanıcı verisi korunur.

## Gereksinim ve test izleri
- REQ-011 → TC-011 — Teslimat ve ödeme yinelenmemeli
- REQ-018 → TC-018 — Kayıt atomik ve migrate edilebilir olmalı
- REQ-019 → TC-019 — Satın alma güvenli ve softlock'suz olmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-012 — Oturum ve görev durum makinesi](../01_gameplay/012_session_state_machine.md)
- [ASK-022 — Teslimat doğrulaması ve tek seferlik sonuç](../01_gameplay/022_delivery_scoring.md)
- [ASK-024 — İlerleme ve küçük ölçekli ekonomi](../01_gameplay/024_progression_economy.md)
- [ASK-043 — Save şeması, migration ve cloud çatışması](../03_engineering/043_save_schema_migration.md)

