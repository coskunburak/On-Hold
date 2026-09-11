---
doc_id: ASK-047
title: "Seçilecek paketler ve kullanım sınırları"
version: 0.1.0
status: proposed
owner_role: ArtLead
last_reviewed: 2026-09-07
dependencies: ["ASK-046","ASK-048","ASK-050","ASK-061","ASK-080"]
---

# Seçilecek paketler ve kullanım sınırları

## Önerilen paket sepeti
| Kaynak | Projedeki görev | Karar |
|---|---|---|
| Quaternius Downtown City MegaKit | Cephe/modüler şehir çekirdeği | Ana aday; arşiv denetimi şart |
| Quaternius Universal Base Characters | Ortak humanoid taban | Rig/stil spike sonrası |
| Quaternius Universal Animation Library | Genel hareket başlangıcı | Klip bazlı retarget kontrolü |
| Quaternius Ultimate Furniture | Gri kutu ve boyut referansı | Final mobilya onayı değil |
| Kenney Particle Pack | Basit VFX texture kaynağı | Kendi URP efektini üret |
| Kenney Impact Sounds | Geçici/işlenmiş darbe katmanları | Ses miks kontrolü |
| Kenney Input Prompts | Kontrol simgeleri | Tek UI stiline normalize |

## Doğrudan bağlantılar
[Downtown City MegaKit](https://quaternius.itch.io/downtown-city-megakit), [Universal Base Characters](https://quaternius.itch.io/universal-base-characters), [Universal Animation Library](https://quaternius.itch.io/universal-animation-library), [Animation Library 2](https://quaternius.itch.io/universal-animation-library-2), [Ultimate Furniture](https://quaternius.com/packs/ultimatefurniture.html), [Particle Pack](https://kenney.nl/assets/particle-pack), [Impact Sounds](https://kenney.nl/assets/impact-sounds), [Input Prompts](https://kenney.nl/assets/input-prompts).

## Ücretsiz ile source ayrımı
07.09.2026 ürün sayfalarında şehir/karakter/animasyon ürünlerinde ücretsiz Standard ve ücretli Source/Pro seçenekleri ayrılıyor. “Paket 300+ parça/120+ animasyon sunuyor” ifadesi, ücretsiz indirmenin tüm içeriği veya .blend kaynaklarını içerdiği kanıtı değildir. Arşiv bu çalışma kapsamında indirilmedi; dosya bazlı kapsam ve motor uyumu henüz denetlenmedi. Satın almadan önce güncel tier içeriği tekrar okunmalıdır.

## Kullanmayacağımız karışım
Kenney Furniture Kit ve KayKit Furniture Bits yararlı alternatif/prototip kaynaklarıdır; ana setin yanına ham haliyle ikinci/üçüncü 3D aile olarak konmayacak. Photoreal mobilya veya farklı karakter oranları ana palete boyanarak otomatik uyumlu olmaz. Cartoon FX Free ancak shader ve stil dönüşümü ispatlanırsa seçilir; ana VFX tabanı zorunlu değildir.

## Production-ready yorumu
Bu bağlantılar tek başına ASKIDA için production-approved paket listesi değildir. Uygun lisans, gerekli tier, Unity sürüm uyumu, collider/rig, görsel uyum ve gerçek cihaz maliyeti doğrulandıktan sonra seçili varlık onaylanır. Ana platform, vinç, grip socket'leri ve Turkish balkon kit'i büyük olasılıkla özel üretim ister.

## Gereksinim ve test izleri
- REQ-023 → TC-023 — Kaynak paketler ortak stil kapısından geçmeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-046 — Sanat yönetimi ve paket uyumu](046_art_direction.md)
- [ASK-048 — Lisans, kaynak kaydı ve AI kullanımı](048_licenses_provenance.md)
- [ASK-050 — Astra/Codex ile Blender üretimi ve export](050_ai_blender_export.md)
- [ASK-061 — Varlık envanteri ve üretim kuyruğu](../05_content/061_asset_inventory.md)
- [ASK-080 — Kaynaklar, doğrulama sınırları ve açık kanıtlar](../08_knowledge/080_sources_evidence_limits.md)

