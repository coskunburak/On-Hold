---
doc_id: ASK-050
title: "Astra/Codex ile Blender üretimi ve export"
version: 0.1.0
status: proposed
owner_role: TechnicalArtist
last_reviewed: 2026-09-07
dependencies: ["ASK-046","ASK-048","ASK-049","ASK-053","ASK-077"]
---

# Astra/Codex ile Blender üretimi ve export

## Kısa cevap
Evet: model, Blender Python/bpy betikleri, parametrik geometri, materyal ayarları, export ve kontrol otomasyonu üretmeye yardımcı olabilir. Gerçek .blend/.fbx/.glb dosyası Blender'ın çalıştığı bir ortamda bu betiğin yürütülmesiyle oluşur. Model adı tek başına kusursuz topoloji, rig, UV, görsel uyum veya çalışan Unity prefab garantisi değildir. Bu teslimatta Blender/Unity üretimi çalıştırılmadı.

## Uygun ilk varlıklar
Platform deck'i, sandık, metal kasa, duvar braketi, basit korkuluk ve modüler kapı çerçevesi ölçülü parametrik üretime uygundur. Yüz anatomisi, yüksek kaliteli skinning, doğal kumaş deformasyonu, karmaşık el animasyonu ve kahraman karakter insan sanatçı düzeltmesi gerektirir. Tek bir resimden görülemeyen arka yüz ve fiziksel ölçü otomatik doğrulanamaz.

## Üretim döngüsü
1. ASK-049 kartı ve izinli referansları hazırla.
2. Betiği yalnız görev çalışma dizininde, dosya silmeyen ve dış ağ gerektirmeyen sınırlı işlemlerle üret.
3. Blender'da geometriyi yarat; named collection, socket, collider proxy ve materyalleri ayır.
4. Ön/yan/arka/üst ve oyun kamerası render'larını üret; siluet ve ölçeği insan kontrol etsin.
5. Mesh/UV/normals/degenerate triangle/transform validator çalıştır.
6. FBX veya doğrulanmış glTF export al; .blend kaynak ve betik sürümünü koru.
7. Unity import preset ve prefab assembler uygula.
8. Fizik/ağ/art kabul sahnelerinde test et; üretim onayı sonra ver.

## Export gerçekleri
FBX veya seçilen formatın materyal dönüşümü Blender shader grafiğini birebir taşımaz. URP materyalleri proje tarafında kurulmalıdır. Üretimde .blend otomatik import bağımlılığı yerine kontrollü export tercih edilir; [Unity model import rehberi](https://docs.unity3d.com/6000.0/Documentation/Manual/HOWTO-ImportObjectsFrom3DApps.html). GLB/glTF kullanılacaksa importer sürümü ayrıca kilitlenir.

## Örnek görev tarifi
“2,4×1,8 m deck ölçüsüne uyan görsel platform üret; 12 derece eğim kuralını geometriye gömme; tek shared palette, ayrı collider proxy, dört anchor socket, alt merkez pivot; tüm çıktılar belirtilen klasörde; önceden var olan dosyaları silme.” Bu komut bir sanat brief'idir, gerçek kaldırma ekipmanı üretim talimatı değildir.

## Kanıt ve sınır
OpenAI'nin [Astra mimari görselleştirme örneği](https://developers.openai.com/blog/architectural-visualization-with-astra) araç destekli geometri/export yaklaşımına örnektir; ASKIDA asset'inin kabul edildiği kanıtı değildir. Kaynak izinleri ve Steam oyuncuya sunulan AI içerik beyanı ayrıca takip edilir.

## Gereksinim ve test izleri
- REQ-026 → TC-026 — AI üretimi gerçek araç ve insan QA ile doğrulanmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-046 — Sanat yönetimi ve paket uyumu](046_art_direction.md)
- [ASK-048 — Lisans, kaynak kaydı ve AI kullanımı](048_licenses_provenance.md)
- [ASK-049 — Her nesne için üretim şartnamesi](049_prop_asset_spec.md)
- [ASK-053 — Malzeme, texture ve aydınlatma standardı](053_materials_textures_lighting.md)
- [ASK-077 — Codex görev bağlamı ve güvenilir iş akışı](../08_knowledge/077_codex_task_workflow.md)

