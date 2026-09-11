---
doc_id: ASK-039
title: "Veri şemaları ve içerik kimlikleri"
version: 0.1.0
status: proposed
owner_role: TechLead
last_reviewed: 2026-09-07
dependencies: ["ASK-010","ASK-016","ASK-023","ASK-043","ASK-061"]
---

# Veri şemaları ve içerik kimlikleri

## Tanım ve örnek ayrımı
ItemDefinition tasarım verisidir: id, version, displayNameKey, massKg, behaviorFamily, gripSockets, deliveryBounds, damageProfile, visualPrefab, collisionProfile. ItemInstance runtime state'tir; ScriptableObject asset'ine integrity veya holder yazılmaz. ContractDefinition ve ContractRun aynı ayrımı izler.

## Kimlik
Kararlı ASCII content ID kullanılır: item.piano.upright.a, level.courtyard.a, contract.courtyard.01. Unity GUID asset referansını yönetir; save/network kimliği olarak renderer adı veya array index kullanılmaz. Silinen content ID yeniden başka eşya için kullanılmaz. Registry build'de duplicate ID hatasında durur.

## Şema örneği
Item kaydı en az schemaVersion=1, definitionId, massKg>0, behaviorFamily enum, min/max bounds, damageProfileId ve provenanceId taşır. Bu plan içindeki JSON kayıtlar doküman/backlog içindir; henüz oyun runtime şemasının çalışan implementasyonu değildir. Oyun şeması kod ve validator ile birlikte üretilecektir.

## Doğrulama
Eksik localization key, kayıp prefab, negatif kütle, deliveryBounds olmaması, iki ayrı root Rigidbody, missing network component, desteklenmeyen collider ve lisanssız kaynak build öncesi raporlanır. Uyarı ile hata ayrımı açık olur; kritik gameplay referansı uyarıya düşürülmez.

## Denge sürümü
Kütle, hasar veya ödül verisi değişince contentVersion artar ve test etkisi değerlendirilir. Aktif oturum iki farklı manifest ile başlamaz. Save migration görsel prefab değişimi ile ekonomik anlam değişimini ayırır. Content hash deterministik sıralanmış kayıtlar üzerinden üretilir; JSON alan sırası tesadüfüne bırakılmaz.

## Kabul
Aynı tanımdan iki item spawn etmek bağımsız integrity üretir; editör play mode'dan çıkınca kaynak tanım kirlenmez. Registry bütün referansları çözebilir. Eksik item tanımında eski save sessizce yanlış tür eşya üretmez; migration veya açık hata gerekir.

## Gereksinim ve test izleri
- REQ-020 → TC-020 — Content registry hataları build'i engellemeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-010 — Sözlük, birimler ve başlangıç parametreleri](../00_foundation/010_glossary_parameters.md)
- [ASK-016 — Eşya semantiği ve değişmezler](../01_gameplay/016_item_state_model.md)
- [ASK-023 — Sözleşme tanımı, başarı ve bitirme](../01_gameplay/023_contract_rules.md)
- [ASK-043 — Save şeması, migration ve cloud çatışması](043_save_schema_migration.md)
- [ASK-061 — Varlık envanteri ve üretim kuyruğu](../05_content/061_asset_inventory.md)

