---
doc_id: ASK-046
title: "Sanat yönetimi ve paket uyumu"
version: 0.1.0
status: proposed
owner_role: ArtLead
last_reviewed: 2026-09-07
dependencies: ["ASK-047","ASK-049","ASK-053","ASK-056","ASK-066"]
---

# Sanat yönetimi ve paket uyumu

## Görsel hedef
Stilize, ölçüleri okunur, hafif yumuşatılmış kenarlar ve kontrollü yüzey aşınması. Tam fotogerçekçi PBR ile oyuncak gibi çok düşük poligon parçalar aynı sahnede doğrudan karıştırılmaz. Bina arka planı düşük kontrast; etkileşimli ekipman turuncu/sarı vurgu; teslimat bilgisi biçim ve ikonla ayrıca okunur.

## Ana görsel aile
Quaternius Downtown City MegaKit dış cephe/modüler çekirdek adayıdır. Türkiye apartman hissi için balkon korkuluğu, klima dış ünitesi, tente, duvar kablosu ve kapı etiketi gibi project-owned tamamlayıcılar gerekir. Paket Boston/NYC kökenli görünümüyle tek başına Türk apartmanı kimliği sağlamaz. Fake interior shader gerçek oynanabilir oda değildir.

## Ortak kurallar
Gerçek ölçüye yakın kapı/eşya oranları; tek bevel ölçeği ailesi; aynı ışık altında benzer roughness; sınırlı malzeme paleti; benzer kenar aşınması yoğunluğu; belirlenmiş texel-density sınıfları. Karakterin baş/el oranı, mobilyanın köşe dili ve duvar detay sıklığı birlikte değerlendirilir. Aynı artist'in farklı yıllarda ürettiği paketler otomatik tutarlı sayılmaz.

## Stil doğrulama sahnesi
Bir cephe modülü, kapı, balkon, platform, iki farklı eşya ve karakter aynı kamera/ışık altında gösterilir. Nötr ışık kontrolü ile hedef oyun ışığı kontrolü ayrı yapılır. Yakın, orta ve oyun kamerası mesafeleri; gri ton görünüm; silhouette ve renk körlüğü simülasyonu değerlendirilir. Aday paketin kendi demo ışığı kullanılmaz.

## Kabul kartı
Ölçek 1–5, siluet 1–5, yüzey dili 1–5, renk/malzeme uyumu 1–5, oynanış okunurluğu 1–5, teknik maliyet pass/fail. Skorlar araç değil uzman değerlendirmesidir. İki alanda belirgin uyumsuz varlık “bedava” diye geçmez; yeniden üret, normalize et veya çıkar.

## Sanat kapısı
Ana platform ve bir kahraman eşya final kalitede görünmeden 100 prop üretimine geçilmez. Güzel konsept görseli, çalışır UV/LOD/collider/prefab teslimi yerine geçmez.

## Gereksinim ve test izleri
- REQ-023 → TC-023 — Kaynak paketler ortak stil kapısından geçmeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-047 — Seçilecek paketler ve kullanım sınırları](047_package_selection.md)
- [ASK-049 — Her nesne için üretim şartnamesi](049_prop_asset_spec.md)
- [ASK-053 — Malzeme, texture ve aydınlatma standardı](053_materials_textures_lighting.md)
- [ASK-056 — Modüler dünya kiti ve apartman kimliği](../05_content/056_modular_world_kit.md)
- [ASK-066 — Başlangıç asset, render ve fizik bütçeleri](../06_quality/066_render_physics_asset_budgets.md)

