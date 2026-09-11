---
doc_id: ASK-010
title: "Sözlük, birimler ve başlangıç parametreleri"
version: 0.1.0
status: proposed
owner_role: GameDesign
last_reviewed: 2026-09-07
dependencies: ["ASK-016","ASK-018","ASK-022","ASK-030","ASK-065"]
---

# Sözlük, birimler ve başlangıç parametreleri

## Normatif terimler
ContractDefinition içerik tanımıdır; ContractRun tek oynanış denemesidir. ItemDefinition aynı türün tasarım verisi; ItemInstance dünyadaki tek örnektir. Authority sonucu belirleyen host sürecidir; Ownership ağ kütüphanesindeki nesne sahipliği olabilir ve oyun yetkisiyle eş anlamlı değildir. Holder eşyayı tutan oyuncudur; Driver vinç kontrol kiralamasını alan kişidir. Supported eşyanın platform tarafından taşındığı sınıflandırmadır. Secured geçerli sabitleme bağlantısı bulunan taşıma durumudur. Revision her kabul edilmiş durum değişiminin artan sürümüdür. SessionEpoch eski oturum mesajlarını ayırır.

## Merkezi başlangıç değerleri
Aşağıdaki rakamlar ölçülmüş sonuç veya motor garantisi değildir. data/parameters.json aynı kimlikleri taşır; uyuşmazlıkta değişiklik incelemesi gerekir.

| Parametre | Başlangıç önerisi | Birim / gerekçe |
|---|---:|---|
| P-PHYSICS-HZ | 50 | Hz; 0,02 s adım |
| P-SNAPSHOT-HZ | 20 | Hz; spike ile ayarlanır |
| P-RECONNECT-SEC | 60 | s; mevcut yuva rezervi |
| P-HOLDERS-MAX | 2 | oyuncu/eşya |
| P-PLAYER-MASS | 80 | kg; görünümden bağımsız |
| P-DECK-WIDTH | 2,4 | m; blockout |
| P-DECK-DEPTH | 1,8 | m; blockout |
| P-TILT-MAX | 12 | derece; kontrollü sınır |
| P-DELIVERY-DWELL | 1,0 | s; kesintisiz kararlılık |
| P-DELIVERY-SPEED | 0,15 | m/s; merkez doğrusal hız üstü |
| P-DELIVERY-ANGULAR | 5 | derece/s; açısal hız üstü |
| P-INTEGRITY-MIN | 20 | /100 teslimat alt sınırı |
| P-RECOVERY-MAX | 2 | eşya başına dünya dışı kurtarma |
| P-RECOVERY-PENALTY | 15 | s; kalan görev süresinden |
| P-WINCH-SECURE-SPEED | 0,05 | m/s; sabitleme için |
| P-TARGET-FPS | 60 | 1080p hedef hipotezi |

## Birim sözleşmesi
Unity içinde metre, kilogram, saniye kullanılır. Açısal hız API'sinin rad/s değeri sınır denetiminden önce birim dönüşümü yapar; tasarım tablosunda derece/s tutulur. Zamanlayıcılar istemci duvar saatinden değil host monotonik simülasyon zamanından hesaplanır. Dosya/şema kimlikleri ASCII; oyuncuya görünen metinler yerelleştirme anahtarıdır.

## Sayı değişikliği
Bir parametreyi değiştirince ilişkili fizik, ağ ve UX testinin tekrar koşulacağı kayda yazılır. “60 FPS” her cihaz sözü değildir; referans donanım ADR-HARDWARE ile seçilecektir. Fizik frekansı ile render kare hızı aynı şey değildir.

## İlgili kaynak belgeler
- [ASK-016 — Eşya semantiği ve değişmezler](../01_gameplay/016_item_state_model.md)
- [ASK-018 — Platform, destek kütlesi ve kontrollü eğim](../01_gameplay/018_platform_balance.md)
- [ASK-022 — Teslimat doğrulaması ve tek seferlik sonuç](../01_gameplay/022_delivery_scoring.md)
- [ASK-030 — Gecikme, öngörü ve düzeltme sınırları](../02_network/030_latency_prediction.md)
- [ASK-065 — Performans hedefi ve ölçüm disiplini](../06_quality/065_performance_targets.md)

