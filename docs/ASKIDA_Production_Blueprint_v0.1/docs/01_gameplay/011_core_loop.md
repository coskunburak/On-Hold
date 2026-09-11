---
doc_id: ASK-011
title: "Temel oyun döngüsü ve zaman çizgisi"
version: 0.1.0
status: proposed
owner_role: GameDesign
last_reviewed: 2026-09-07
dependencies: ["ASK-012","ASK-018","ASK-022","ASK-023","ASK-024"]
---

# Temel oyun döngüsü ve zaman çizgisi

## Bir sözleşmenin akışı
Hazırlık → rota keşfi → eşyayı alma → platforma yerleştirme → dengeleme/sabitleme → kaldırma → boşaltma → teslimat → sonuç. Oyuncuların bu sırayı her eşya için aynı anda uygulaması gerekmez; bir kişi sonraki eşyayı hazırlarken diğerleri mevcut yükü taşıyabilir. İlerleme host tarafından yönetilen görev hedeflerine bağlıdır, animasyonun bitiş olayına değil.

## Hazırlık
Brifing binayı, zorunlu eşyaları, teslimat bölgesini ve süreyi gösterir. V1'de alınması zorunlu ücretli ekipman yoktur; temel platform ve kayışlar görevle birlikte gelir. Oyuncu hazır olmadan süre başlamaz. Yükleme tamamlandıktan sonra mevcut oyuncuların hazır işaretleri veya host'un açık başlatma kararı gerekir; yüklemesi tamamlanmamış istemci Active içine zorla sokulmaz.

## Aktif tur
10–15 dakikalık sözleşme süresi tasarım hipotezidir; ilk öğretici bundan daha kısa olabilir. Kalan süre host zamanından gösterilir. Eşyalar önceden yerleştirilmiş, bilinen ölçü ve kütle sınıfındadır. Oyuncu serbestçe rol değiştirir; tur ortasında görev tanımı, oyuncu sayısına göre gizlice değişmez.

## Bitiş
Süre dolunca veya geçerli erken bitirme oyu kabul edilince Settling başlar. Yeni tutma, sabitleme ve teslimat komutları kapatılır. Aynı host tick'inde zaman aşımı ile teslimat çakışırsa tick başında süre sıfıra ulaşmışsa zaman aşımı önceliklidir; önceki tick'te tamamlanan teslimat korunur. Sonuç bir kez hesaplanır ve kayıt katmanına tek işlem olarak sunulur.

## Eğlence ölçümü
Her 60–90 saniyede bir anlamlı koordinasyon fırsatı hedeflenebilir fakat zorunlu olay zamanlayıcısıyla sahte kaos yaratılmaz. Testte oyuncuların ne kadar süre aktif karar verdiği, boş beklediği, arayüz aradığı ve tekrar aynı yolu yürüdüğü ayrı işaretlenir. Vincin yanında bekleyen oyuncuya ikinci kontrol paneli eklemek yerine rol değişimini ve yük hazırlama alanını iyileştirmek önce denenir.

## Kabul senaryosu
İlk ekip brifingden sonuca destek almadan ulaşır; en az bir kurtarma davranışını kendisi keşfeder; host ve istemci aynı teslimat sayısını görür. Teknik testin geçmesi deneyim testinin geçtiği anlamına gelmez.

## Gereksinim ve test izleri
- REQ-013 → TC-013 — Bitiş sırası ve kısmi ödül tutarlı olmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-012 — Oturum ve görev durum makinesi](012_session_state_machine.md)
- [ASK-018 — Platform, destek kütlesi ve kontrollü eğim](018_platform_balance.md)
- [ASK-022 — Teslimat doğrulaması ve tek seferlik sonuç](022_delivery_scoring.md)
- [ASK-023 — Sözleşme tanımı, başarı ve bitirme](023_contract_rules.md)
- [ASK-024 — İlerleme ve küçük ölçekli ekonomi](024_progression_economy.md)

