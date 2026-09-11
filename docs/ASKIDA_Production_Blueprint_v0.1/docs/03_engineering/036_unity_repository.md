---
doc_id: ASK-036
title: "Unity deposu ve dosya hiyerarşisi"
version: 0.1.0
status: proposed
owner_role: TechLead
last_reviewed: 2026-09-07
dependencies: ["ASK-002","ASK-037","ASK-039","ASK-041","ASK-042"]
---

# Unity deposu ve dosya hiyerarşisi

## Motor durumu
Öneri Unity 6 + URP; exact editor patch ve paket sürümleri ADR-ENGINE sonrası kilitlenecek. “Latest” bağımlılık kullanılmaz. Bu arşivde Unity projesi oluşturulmadı; aşağıdaki ağacın üretim deposunda kurulması planlanır.

## Depo ağacı
- Game/Assets/_Askida/Runtime/{Core,Gameplay,Network,UI,Audio,Presentation}
- Game/Assets/_Askida/Editor/{Validation,Import,Build}
- Game/Assets/_Askida/Tests/{EditMode,PlayMode,Integration}
- Game/Assets/_Askida/Art/{Meshes,Materials,Textures,Animations,VFX}
- Game/Assets/_Askida/Prefabs/{Players,Items,Platform,Environment,UI}
- Game/Assets/_Askida/Data/{Items,Contracts,Levels,Profiles,Localization}
- Game/Assets/_Askida/Scenes/{Bootstrap,Frontend,Hub,Levels,Validation}
- Game/Assets/ThirdParty/<Publisher>/<Package>/<Version>/
- Game/Packages/{manifest.json,packages-lock.json}
- Game/ProjectSettings/
- SourceArt/{Blender,Textures,Audio,Exports,Provenance}
- Docs/ASKIDA/ — bu arşivin bütünü, kendi README/AGENTS/data/tools düzeniyle
- Tools/{AssetBuild,Validation,Release}
- Builds/ — üretilmiş, Git dışında

## Sürüm kontrolü
Bilgi tabanı kökü ile oyun deposu kökü aynı olmak zorunda değildir. Önerilen entegrasyonda bu arşivin içeriği Docs/ASKIDA/ altına bütünüyle taşınır; göreli belge bağlantıları değişmez. Yardımcı komutları o dizinde çalıştırın. Oyun deposunun kök AGENTS.md dosyası, Docs/ASKIDA/AGENTS.md ve görevle ilgili ASK belgelerini açıkça okuma yönlendirmesi taşımalıdır; alt klasördeki AGENTS kurallarının Game/ koduna otomatik uygulanacağını varsaymayın. Kök yönergede çalışma deposunun gerçek yolları kullanılır, bu paketin data/ yolunu yanlışlıkla repo kökünde aramayın.

Assets ve .meta dosyaları birlikte sürümlenir; GUID korunur. ProjectSettings ve paket lock dosyası depoya alınır. Library, Temp, Obj, Logs, UserSettings ve build çıktıları kaynak depoya alınmaz. Büyük .blend, ses ve texture kaynaklarında Git LFS önerilir; ekip kota ve erişimini onaylamadan etkinleştirilmez. Metin serialization ve görünür meta ayarları doğrulanır.

## Dosya adları
PF_Item_Piano_A, SM_Platform_Deck_A, MAT_Equipment_Orange, SO_Item_Piano_A, SCN_Level_Courtyard gibi tür önekleri; dosyalarda ASCII ve kararlı isimler. Kod namespace Askida.Gameplay.Carrying benzeri alan tabanlıdır. Türkçe görünen metinler localization tablosunda bulunur, sınıf adlarında değil.

## Üçüncü taraf sınırı
Paket demosu doğrudan shipping sahne olmaz. Ham kaynak Assets/ThirdParty altında korunur; oyun prefab türevleri _Askida altında oluşturulur. Yeniden import bütün değişiklikleri ezmesin diye upstream mesh'i elle düzeltmek yerine project-owned türevi ve provenance kaydı kullanılır.

## Kabul
Temiz checkout farklı geliştirici cihazında belgeli editor ile açılır; missing GUID, beklenmeyen import farkı ve kayıp shader yoktur. Kurulum eksikleri README/setup görevi olarak kayda girer; “benim makinemde açıldı” yeterli değildir.

## Gereksinim ve test izleri
- REQ-002 → TC-002 — Temiz checkout tekrar açılmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-002 — Codex çalışma sözleşmesi](../../AGENTS.md)
- [ASK-037 — Modüller, assembly sınırları ve bağımlılıklar](037_module_boundaries.md)
- [ASK-039 — Veri şemaları ve içerik kimlikleri](039_data_schemas.md)
- [ASK-041 — Editör araçları ve asset kabul otomasyonu](041_editor_asset_validation.md)
- [ASK-042 — Build, CI ve bağımlılık kilitleme](042_build_ci_release.md)
