---
doc_id: ASK-072
title: "Ekip rolleri, review ve çalışma ritmi"
version: 0.1.0
status: proposed
owner_role: Producer
last_reviewed: 2026-09-07
dependencies: ["ASK-002","ASK-009","ASK-036","ASK-069","ASK-074"]
---

# Ekip rolleri, review ve çalışma ritmi

## Rol sahipliği
Product kapsam ve öncelik; TechLead mimari ve sürüm; Gameplay semantik uygulama; Network co-op; ArtLead stil; TechnicalArtist import/performans; QA kanıt; Producer kapasite ve risk. Bir kişi birden çok rol olabilir; her işin yine tek accountable owner'ı bulunur. Bu plan belirli çalışanların var olduğunu varsaymaz.

## Haftalık ritim
Hafta başı hedef ve kapasite; kısa engel kontrolü; ortada oynanabilir build; hafta sonunda test kanıtı ve risk güncellemesi. Toplantı sayısını artırmak yerine yazılı iş kartı ve build linki kullanılır. “Bu hafta neler yaptım” yerine “hangi oyuncu davranışı artık doğrulandı” sorulur.

## İş kartı
W ID, problem, kapsam dışı, ASK/REQ, bağımlılık, tahmin aralığı, kabul testleri, owner, review rolü, build/commit, risk ve sonuç. AI'a verilecek görev de bu karttan türetilir. Büyük işi “oyunu yap” diye tek prompt'a dönüştürmek izlenebilirliği kaybettirir.

## Review
Network/ekonomi değişikliği domain ve QA gözü; art değişikliği stil ve teknik bütçe; level değişikliği oynanabilirlik ve collision; belge değişikliği bağımlılık ve karar durumu. Review yalnız kod biçimi değildir. Kullanıcı değişiklikleri AI tarafından üzerine yazılmaz.

## Ekip içi ortak dosyalar
Scene merge riski yüksekse iş alanları prefab/modül bazında ayrılır. Bir kişinin sahip olduğu art kaynaklarında lock iletişimi uygulanır. .meta ve GUID bütünlüğü kontrol edilir. Uzun branch ve büyük asset import PR'ları küçük anlamlı partilere bölünür.

## Kabul
Bir ekip üyesi yokken başka biri işin kararını ve son doğrulanan build'i bulabilir. Sözlü kararlar ASK-009 veya ilgili ADR'ye işlenir. Ekip kapasitesi dışında iş sessizce sprint'e eklenmez.

## İlgili kaynak belgeler
- [ASK-002 — Codex çalışma sözleşmesi](../../AGENTS.md)
- [ASK-009 — Karar kaydı ve onay bekleyenler](../00_foundation/009_decision_register.md)
- [ASK-036 — Unity deposu ve dosya hiyerarşisi](../03_engineering/036_unity_repository.md)
- [ASK-069 — Milestone kalite kapıları ve bitmiş iş](../06_quality/069_quality_gates.md)
- [ASK-074 — Başlangıç backlog'u ve ilk iki sprint](074_backlog_first_sprints.md)

