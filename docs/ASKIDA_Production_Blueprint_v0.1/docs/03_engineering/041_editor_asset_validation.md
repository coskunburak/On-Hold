---
doc_id: ASK-041
title: "Editör araçları ve asset kabul otomasyonu"
version: 0.1.0
status: proposed
owner_role: TechnicalArtist
last_reviewed: 2026-09-07
dependencies: ["ASK-036","ASK-039","ASK-049","ASK-061","ASK-066"]
---

# Editör araçları ve asset kabul otomasyonu

## İlk araç seti
AssetValidator seçili prefab veya bütün shipping registry üzerinde çalışır. Kontroller: kök ölçek, birim, pivot, bounds, kütle, collider sayısı, root Rigidbody, network bileşen profili, grip/anchor socket, malzeme/shader, texture import, LOD referansı, localization ve provenance. Araç otomatik “güzel görünür” kararı vermez; görsel kurulun yerine geçmez.

## Sonuç sınıfları
Error build'i keser: missing prefab, duplicate ID, belirsiz lisans, negatif kütle, dinamik concave collider, eksik zorunlu socket. Warning inceleme ister: üçgen/texture bütçesi aşımı, nadir materyal, eksik uzak LOD. Info öneri verir. Exception'lar kayıt altına alınmış whitelist ile owner ve son kullanma tarihi taşır; küresel ignore yoktur.

## İçe alma
Kaynak paket staging alana alınır; hash ve lisans kaydı oluşturulur; seçili asset'ler normalize edilip project-owned prefab hazırlanır. Tüm paketi shipping reference listesine koymak yoktur. Import ayarları deterministik preset veya editor script ile uygulanır; elle tıklanan istisnalar belgelenir.

## Yardımcı sahneler
ArtValidation aynı ışık/kamera altında referans duvar, karakter, platform ve aday eşyayı gösterir. PhysicsValidation merdiven, kapı, düşüş ve deck testlerini taşır. NetworkValidation iki holder ve hareketli platformu tekrar üretir. Bu sahneler ayrı amaca sahiptir; güzel art sahnesi fizik kabulü sayılmaz.

## Otomasyon güvenliği
Toplu araç dry-run sunar, değişecek GUID/path listesini gösterir. ThirdParty kaynağını yerinde değiştirmez. Yeniden adlandırma .meta ile birlikte Unity API üzerinden yapılır; ham dosya kopyalamayla duplicate GUID yaratılmaz. Her import işleminin araç sürümü tutulur.

## Kabul
Bilerek bozuk on örnek prefab validator tarafından beklenen sınıfta yakalanır. Sağlam referans prefab yanlış pozitif vermez. CI ve editör aynı kural setini kullanır. Bu teslimatta yalnız belge paketi doğrulayıcısı çalışır; burada tanımlanan Unity AssetValidator henüz yazılmadı.

## Gereksinim ve test izleri
- REQ-020 → TC-020 — Content registry hataları build'i engellemeli
- REQ-025 → TC-025 — Prop tam teknik kabul almalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-036 — Unity deposu ve dosya hiyerarşisi](036_unity_repository.md)
- [ASK-039 — Veri şemaları ve içerik kimlikleri](039_data_schemas.md)
- [ASK-049 — Her nesne için üretim şartnamesi](../04_art/049_prop_asset_spec.md)
- [ASK-061 — Varlık envanteri ve üretim kuyruğu](../05_content/061_asset_inventory.md)
- [ASK-066 — Başlangıç asset, render ve fizik bütçeleri](../06_quality/066_render_physics_asset_budgets.md)

