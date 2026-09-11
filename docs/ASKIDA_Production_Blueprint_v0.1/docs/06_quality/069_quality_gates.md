---
doc_id: ASK-069
title: "Milestone kalite kapıları ve bitmiş iş"
version: 0.1.0
status: proposed
owner_role: QA
last_reviewed: 2026-09-07
dependencies: ["ASK-007","ASK-035","ASK-065","ASK-067","ASK-071"]
---

# Milestone kalite kapıları ve bitmiş iş

## Kapılar
G0 Tasarım tabanı: kritik ADR'ler, ekip ve risk spike hedefi onaylı. G1 Teknik risk: iki/dört istemci, platform/shared carry, gerçek internet yolu ve cleanup kanıtlı. G2 Slice: tek görev final hedefli art/UI/audio ile uçtan uca; commit güvenli. G3 Demo: üç görev, onboarding, erişim, oyuncu testi, destek toplama. G4 Content complete: onaylı v1 içerik ve localization. G5 Release candidate: P0/P1 kapalı, performans/soak/save/platform kontrolleri.

## Definition of Ready
İşin problemi, ilgili ASK/REQ, input-output, açık kararları, asset bağımlılığı, kabul testi ve yaklaşık kapasitesi bilinmelidir. “Multiplayer yap” Ready değildir. Belirsiz kritik teknoloji önce spike işidir; implementation tahmini gibi sunulmaz.

## Definition of Done
Kod/art teslimi + review + ilgili otomatik test + gerçek build kontrolü + belge/gereksinim güncellemesi + gerekli görsel/performans kanıtı + lisans/provenance. Sadece develop branch'e merge edilmek Done değildir. Plan belgesinin yazılması ilgili gameplay işini Done yapmaz.

## Bug önem sınıfları
P0 veri kaybı/çoğaltma, oturum crash'i, temel güvenlik/erişim blokajı. P1 temel görev tamamlanamıyor, holder kilidi, ciddi ağ ayrışması. P2 anlaşılır workaround'lu önemli kusur. P3 küçük kozmetik. Öncelik ve şiddet ayrı alanlar olabilir; release engelleyici kurallar açık yazılır.

## Kapıdan geçememe
Kapsam kesme, yeniden tasarım veya süre uzatma kararı Product/Tech/QA ile kayda girer. Test eşiğini sessiz düşürmek yoktur. İstisna yalnız etkisi, geçici çözümü ve son tarihi varsa değerlendirilir; veri kaybı “bilinen hata” diyerek normalleştirilmez.

## Kabul
Her kapı tek klasörde build, test raporu, art görüntüleri, risk güncellemesi ve onay kaydı taşır. Bu arşiv bu kapıları tanımlar; hiçbirini gerçekleşmiş olarak işaretlemez.

## Gereksinim ve test izleri
- REQ-008 → TC-008 — Risk kapısı kanıtla kapanmalı
- REQ-040 → TC-040 — Yayın iddiaları kanıta ve lisansa uymalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-007 — Kapsam, sürümler ve kesme sırası](../00_foundation/007_scope_matrix.md)
- [ASK-035 — Multiplayer test matrisi ve kanıt](../02_network/035_network_test_matrix.md)
- [ASK-065 — Performans hedefi ve ölçüm disiplini](065_performance_targets.md)
- [ASK-067 — Test stratejisi ve gereksinim izlenebilirliği](067_test_strategy_traceability.md)
- [ASK-071 — Üretim yol haritası ve kapasite senaryoları](../07_production/071_roadmap_capacity.md)

