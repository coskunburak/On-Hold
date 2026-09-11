---
doc_id: ASK-061
title: "Varlık envanteri ve üretim kuyruğu"
version: 0.1.0
status: proposed
owner_role: ArtLead
last_reviewed: 2026-09-07
dependencies: ["ASK-047","ASK-049","ASK-056","ASK-060","ASK-074"]
---

# Varlık envanteri ve üretim kuyruğu

## Envanter alanları
assetId, kategori, stage, gameplay rolü, kaynak adayı, lisans/provenance, owner, estimate range, source path, export path, prefab ID, acceptance state, bağımlılıklar. “Done” için yalnız mesh bitmiş olması yeterli değildir; texture/rig/collider/prefab/test durumları ayrı işaretlenir.

## Slice varlık listesi
| Varlık | Kaynak/üretim | Öncelik |
|---|---|---|
| Platform deck + korkuluk + ankraj | Özel parametrik üretim | P0 |
| Vinç gövdesi + panel + görsel halat | Özel/kit türevi | P0 |
| Sandık, kanepe, buzdolabı, piano | Prototip + stil normalize | P0 |
| Tek rig, dört renk varyantı | Quaternius aday | P0 |
| Oynanabilir balkon/oda kapısı | Ana kit + özel collision | P0 |
| Teslimat alanı işareti ve HUD | Özel UI/VFX | P0 |
| Grip/secure/delivery sesleri | Lisanslı kaynak + miks | P0 |
| Klima, tente, kapı panosu | Özel tamamlayıcı | P1 |
| Uzak cephe dolgu varyantları | Ana kit | P1 |

## V1 büyüme
Yeni model miktarı sözleşme çeşitliliğiyle ilişkilendirilir. Altı davranış ailesi 60 benzersiz mesh gerektirmez. Aynı rig ve materyal ailesinin yeniden kullanımı ilk stratejidir. Kullanılmayan kütüphane varlıkları “hazır içerik” olarak iş bitirme hesabına katılmaz.

## Üretim akışı
Brief → blockout → art review → geometry/UV → material → collision/socket → engine prefab → network/physics test → approval. Rig/animasyon gereken varlıkta ilgili kapılar eklenir. Her aşamada geri dönüş maliyeti görünürdür; üçgen model final onayından sonra ölçü değiştirmenin maliyeti backlog'a girer.

## Satın alma kararı
Ücretli Source tier yalnız kaynak düzenleme veya motor materyalleri gerçekten zaman kazandırıyorsa düşünülür. Bedel dışında lisans, dönüştürme saati, uyum ve bakım hesaplanır. Bu çalışma hiçbir paket satın almadı veya indirmedi.

## Kabul
Slice'taki her görünür gameplay nesnesi inventory'de bir satıra ve ASK-049 kabul kartına sahiptir. “Temporary” etiketleri release öncesi aramayla bulunur. data/assets.json bu başlangıç listesinin makine okunur halidir.

## Gereksinim ve test izleri
- REQ-024 → TC-024 — Shipping varlığı lisansa izlenebilir olmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-047 — Seçilecek paketler ve kullanım sınırları](../04_art/047_package_selection.md)
- [ASK-049 — Her nesne için üretim şartnamesi](../04_art/049_prop_asset_spec.md)
- [ASK-056 — Modüler dünya kiti ve apartman kimliği](056_modular_world_kit.md)
- [ASK-060 — On iki sözleşmelik içerik taslağı](060_contract_catalog.md)
- [ASK-074 — Başlangıç backlog'u ve ilk iki sprint](../07_production/074_backlog_first_sprints.md)

