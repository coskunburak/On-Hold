---
doc_id: ASK-023
title: "Sözleşme tanımı, başarı ve bitirme"
version: 0.1.0
status: proposed
owner_role: GameDesign
last_reviewed: 2026-09-07
dependencies: ["ASK-011","ASK-022","ASK-024","ASK-060"]
---

# Sözleşme tanımı, başarı ve bitirme

## ContractDefinition alanları
Sabit definitionId, içerik sürümü, levelId, spawn seti, allowed player counts, başlangıç oyuncu sayısına göre süre/kota, mandatory item kimlikleri, isteğe bağlı eşyalar, base values, başarı bonusu, kurtarma cezası ve kilit açma bağımlılıkları. Rastgele seed alanı ileride dekor varyasyonu için ayrılabilir; v1 prosedürel şehir taahhüdü değildir.

## Başarı kuralı
Başarı = tüm mandatory eşyalar Delivered ve teslimat sayısı quota'ya en az eşit. Kota mandatory adetinden küçük olamaz, teslim edilebilir toplamdan büyük olamaz. Zorunlu eşya Lost olursa başarı imkânsız olabilir; oyuncuya hemen açıklanır. Ekip kalan eşyalarla kısmi gelir kazanmayı sürdürebilir veya abort edebilir.

## Süre sonu ve abort
Doğal süre sonunda teslim edilmiş eşyaların geçici değeri ödenir; başarılı değilse başarı bonusu yoktur. Açık Abort kazanç ödemez ve mevcut turu kapatır; onay ekranı bu farkı belirtir. Abort kalıcı kampanyayı silmez. “Başarısız olmak” ile “uygulama çöktüğü için commit olmamak” farklı sonuç nedenleridir.

## Erken bitirme oyu
Kota ve mandatory koşulları sağlandıysa aktif bağlı oyuncuların salt çoğunluğu, host dahil edilmek şartıyla, erken bitirmeyi onaylayabilir. Host'un oyu zorunludur; sadece host tek başına dört kişilik grubu bitiremez. Oy başlangıcındaki bağlı oyuncu listesi sabitlenir; kopma oyu otomatik kolaylaştırmaz, kısa süre sonunda iptal edilip yeni listeyle yeniden başlatılabilir. Tek bağlı host kalmışsa kendisi bitirebilir. Oylama süresi ve cooldown UI profilinde belirlenir.

## Zorluk tasarımı
Zorluk sadece ağırlık artırmak değildir: kapı açıklığı, dönüş açısı, yük merkezi, platform durakları ve aynı anda izlenecek işler değişir. Yeni davranış tanıtılan sözleşmede zaman baskısı düşürülür. Gizli rastgele başarısızlık veya tamamlanması imkânsız spawn yoktur.

## Veri kabulü
Editor validator bütün spawn referanslarını, hedeflerin ulaşılabilirliğine dair authored rota etiketlerini ve iki oyuncu desteğini denetler; gerçek oynanabilirlik yine playtest gerektirir. Zorunlu eşya hiçbir başlangıç varyantında kapıdan geometrik olarak imkânsız olmamalıdır.

## Gereksinim ve test izleri
- REQ-013 → TC-013 — Bitiş sırası ve kısmi ödül tutarlı olmalı
- REQ-033 → TC-033 — Sözleşmeler geçerli ve farklı karar sunmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-011 — Temel oyun döngüsü ve zaman çizgisi](011_core_loop.md)
- [ASK-022 — Teslimat doğrulaması ve tek seferlik sonuç](022_delivery_scoring.md)
- [ASK-024 — İlerleme ve küçük ölçekli ekonomi](024_progression_economy.md)
- [ASK-060 — On iki sözleşmelik içerik taslağı](../05_content/060_contract_catalog.md)

