---
doc_id: ASK-080
title: "Kaynaklar, doğrulama sınırları ve açık kanıtlar"
version: 0.1.0
status: proposed
owner_role: Knowledge
last_reviewed: 2026-09-07
dependencies: ["ASK-009","ASK-026","ASK-047","ASK-048","ASK-050","ASK-076"]
---

# Kaynaklar, doğrulama sınırları ve açık kanıtlar

## Araştırma tarihi ve yöntem
Dış kaynaklar 07.09.2026 tarihinde resmi ürün, motor, platform ve yardım sayfalarından incelendi. Ürün sayfası erişimi, indirilen paket arşivinin incelenmesi veya hedef Unity sürümünde test edilmesi değildir. Fiyat/tier/uyumluluk değişebilir; satın alma ve release öncesi yeniden doğrulayın. Belgelerdeki oyun mekaniği ve kapasite rakamları özgün öneridir, dış kaynaklardan ölçülmüş sonuç değildir.

## Kaynak aileleri
Varlıklar: Quaternius ürün sayfaları; Kenney asset sayfaları. Lisanslar: Unity Asset Store şartları, Adobe Mixamo FAQ, Sonniss/Freesound koşulları. Motor: Unity NetworkRigidbody/compound collider/model import, Unreal Networked Physics. Dağıtım: Steam multiplayer, matchmaking, SDR, Cloud ve içerik anketi. Bilgi araçları: Codex AGENTS/MCP ve Google notebook kaynak desteği. Doğrudan bağlantılar ilgili belgelerde, toplu URL/kapsam kaydı data/sources.json içindedir.

## Ek inceleme kaynakları
[Kenney Furniture Kit](https://kenney.nl/assets/furniture-kit) ve [KayKit Furniture Bits](https://kaylousberg.itch.io/furniture-bits) alternatif görsel ailelerdir; ana sete otomatik eklenmez. [Poly Haven lisansı](https://polyhaven.com/license) ve [ambientCG lisansı](https://docs.ambientcg.com/license/) seçili texture/HDRI için kaynak olabilir; fotogerçekçi estetiği olduğu gibi aktarma kararı değildir. [Fab EULA](https://www.fab.com/eula) satın alınan ürünün güncel lisans bağlamıyla okunur.

## Bu teslimatta olmayan kanıt
Unity/Unreal projesi veya oyun build'i yok. Asset archive import testi yok. Ağ benchmark'ı yok. Minimum donanım testi yok. Kullanıcı playtest'i yok. Mağaza satış araştırmasından çıkarılmış talep ölçümü yok. Modelle üretilmiş/Blender'da kontrol edilmiş mesh yok. Çalışan MCP server ve otomatik notebook sync yok. Hiçbir ADR kullanıcı adına onaylanmadı.

## Bu teslimatta sağlanan kanıt
80 Markdown kaynak belgesinin varlığı, benzersiz kimliği, dahili bağlantı/dependency çözümü, gereksinim-test-iş eşleşmeleri, export içerik/hash tutarlılığı ve ZIP bütünlüğü yerel kontrollerle sınanır. Sonuç validation_report.json ve arşiv testinden görülebilir. Yapısal kontrol, tasarımın eğlenceli veya oyun kodunun production-ready olduğunu kanıtlamaz.

## Sonraki araştırma
Motor exact version ve transport adapter için çalışan örnek + lisans incelemesi; ücretsiz asset tier arşivi; gerçek hedef cihaz; 5–8 grup nitel test; bütçe ve ekip kapasitesi. Bu sorular kapanınca v0.2 tasarım tabanı hazırlanmalıdır.

## İlgili kaynak belgeler
- [ASK-009 — Karar kaydı ve onay bekleyenler](../00_foundation/009_decision_register.md)
- [ASK-026 — Ağ çözümü seçimi ve risk prototipi](../02_network/026_network_adr_spike.md)
- [ASK-047 — Seçilecek paketler ve kullanım sınırları](../04_art/047_package_selection.md)
- [ASK-048 — Lisans, kaynak kaydı ve AI kullanımı](../04_art/048_licenses_provenance.md)
- [ASK-050 — Astra/Codex ile Blender üretimi ve export](../04_art/050_ai_blender_export.md)
- [ASK-076 — Notebook aktarımı ve tek kaynak ilkesi](076_notebook_import.md)

