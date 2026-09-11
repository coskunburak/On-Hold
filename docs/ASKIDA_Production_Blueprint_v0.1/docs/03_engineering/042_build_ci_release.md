---
doc_id: ASK-042
title: "Build, CI ve bağımlılık kilitleme"
version: 0.1.0
status: proposed
owner_role: DevOps
last_reviewed: 2026-09-07
dependencies: ["ASK-036","ASK-039","ASK-041","ASK-067","ASK-075"]
---

# Build, CI ve bağımlılık kilitleme

## Tekrarlanabilir build
Editor exact version, paket manifest/lock, platform SDK sürümü, source commit, content hash ve build configuration her build kaydına girer. “Unity 6” tek başına tekrar üretim bilgisi değildir. Unity build lisansı ve CI agent erişimi ayrıca doğrulanır; ücretsiz/sınırsız build altyapısı varsayılmaz.

## Önerilen pipeline
Checkout + LFS doğrulama → dependency cache anahtarı kontrolü → statik veri/asset validation → EditMode → PlayMode smoke → Windows development build → artifact hash ve test raporu. Release yolu ayrıca gerçek multiplayer smoke, lisans taraması ve manuel onay gerektirir. İlk aşamada self-hosted build makinesi yeterli olabilir; maliyet/anahtar güvenliği değerlendirilir.

## Cache
Cache hızlandırmadır, doğruluk kaynağı değildir. Editor ve lock hash'i değişince uygun cache anahtarı değişir. Aralıklı temiz build referansı tutulur. Cache kaynaklı başarı ile temiz checkout başarısı ayrılır. Shipping pakete Library klasörü veya kaynak .blend dosyaları yanlışlıkla girmez.

## Branch ve review
Kısa ömürlü feature branch, küçük PR, test ve belge bağlantısı önerilir. Ağ/ekonomi değişikliği en az ilgili test kaydıyla incelenir. Aynı scene dosyasını iki kişinin uzun süre düzenlemesi yerine prefab ve additive çalışma sınırları planlanır. Bu teslimatta GitHub veya CI servisine bağlanılmadı.

## Release artifact
BuildId, commit, editor version, package hash, content hash, save schema, protocol version, desteklenen OS, bilinen hatalar ve rollback build kimliği. Steam upload yetkisi CI'dan ayrı kontrollü olabilir. Üretim anahtarları repo veya notebook'a konmaz.

## Kabul
Yeni makine belgeli kurulumla aynı içerik hash'li build üretir; binary byte-for-byte determinism ayrıca ölçülmedikçe iddia edilmez. Test başarısızlığı pipeline'ı durdurur; release onayı olmadan otomatik public branch yayınlanmaz.

## Gereksinim ve test izleri
- REQ-002 → TC-002 — Temiz checkout tekrar açılmalı
- REQ-038 → TC-038 — Build/release yolu kontrollü olmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-036 — Unity deposu ve dosya hiyerarşisi](036_unity_repository.md)
- [ASK-039 — Veri şemaları ve içerik kimlikleri](039_data_schemas.md)
- [ASK-041 — Editör araçları ve asset kabul otomasyonu](041_editor_asset_validation.md)
- [ASK-067 — Test stratejisi ve gereksinim izlenebilirliği](../06_quality/067_test_strategy_traceability.md)
- [ASK-075 — Steam hazırlığı, demo ve yayın kontrolü](../07_production/075_steam_launch_plan.md)

