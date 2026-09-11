---
doc_id: ASK-071
title: "Üretim yol haritası ve kapasite senaryoları"
version: 0.1.0
status: proposed
owner_role: Producer
last_reviewed: 2026-09-07
dependencies: ["ASK-004","ASK-007","ASK-009","ASK-069","ASK-073","ASK-074"]
---

# Üretim yol haritası ve kapasite senaryoları

## Tarih değil kapı planı
Bu çalışma ekip büyüklüğü, haftalık zaman ve bütçe bilinmeden kesin çıkış tarihi vermez. Önerilen sıra: karar/teknik spike → vertical slice → dış demo → içerik üretimi → alpha/content lock → beta/optimizasyon → release candidate → yayın sonrası destek. Bir kapı geçmeden paralel içerik büyütmek riski gizler.

## Kapasite hesabı
Bir tam zamanlı kişinin haftada 40 saatinin tamamı feature üretimi değildir. Toplantı, review, test, hata ve idari işleri hesaba katmak için başlangıç planında kişi başı 25 net üretim saati/hafta kullanılabilir. İki kişi × 25 × 2 hafta = 100 saatlik ilk risk sprinti kapasitesidir. Bu oran proje gerçeğiyle ilk iki sprintte düzeltilir.

## Senaryolar
İki deneyimli tam zamanlı geliştirici ve sınırlı dış sanat/QA desteği için 9–15 ay bir ön planlama aralığı olabilir; solo/yarı zamanlı üretimde 18–30 ay veya daha fazlası gerekebilir. Bunlar sektör benchmark'ı veya taahhüt değil, kapsamın büyüklüğünü anlatan kaba yargılardır. Gerçek tahmin W kayıtları, içerik üretim hızı ve belirsizlik kapanmasıyla güncellenir. Aynı süre içine part-time kapasiteyi full-time gibi yerleştirmeyin.

## Aşama çıktıları
Risk: çalışan ortak taşıma kararı. Slice: bir tam kalite örneği. Demo: dış oyuncu kanıtı. İçerik: onaylı kit ile tekrar üretim. Beta: yeni özellik değil kusur/perf. RC: yayın ve geri alma kanıtı. Her aşama bir testable build ve değişen risk listesi teslim eder.

## Paralel çalışmanın sınırı
Artist risk sırasında yalnız yeniden kullanılabilir referans/ölçek/tek örnek üzerinde çalışabilir; seçilmemiş platform tasarımının 40 varyantını üretmez. Gameplay ile networking aynı domain sözleşmesini paylaşır; iki farklı tutma sistemi paralel geliştirilmez.

## Bütçe belirsizliği
Editor/hizmet lisansları, test donanımı, asset source tier, ses/çeviri, mağaza görselleri, hukuki/muhasebe ve yayın sonrası destek ayrı kalemlerdir. Ücretsiz asset kullanımı bu maliyetleri sıfırlamaz. Nakit harcama üst sınırı kullanıcı onayı bekler.

## İlgili kaynak belgeler
- [ASK-004 — Proje bildirgesi ve başarı tanımı](../00_foundation/004_project_charter.md)
- [ASK-007 — Kapsam, sürümler ve kesme sırası](../00_foundation/007_scope_matrix.md)
- [ASK-009 — Karar kaydı ve onay bekleyenler](../00_foundation/009_decision_register.md)
- [ASK-069 — Milestone kalite kapıları ve bitmiş iş](../06_quality/069_quality_gates.md)
- [ASK-073 — Risk defteri ve azaltma planı](073_risk_register.md)
- [ASK-074 — Başlangıç backlog'u ve ilk iki sprint](074_backlog_first_sprints.md)

