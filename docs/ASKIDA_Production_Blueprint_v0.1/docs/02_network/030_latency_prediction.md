---
doc_id: ASK-030
title: "Gecikme, öngörü ve düzeltme sınırları"
version: 0.1.0
status: proposed
owner_role: Network
last_reviewed: 2026-09-07
dependencies: ["ASK-017","ASK-018","ASK-026","ASK-029","ASK-035"]
---

# Gecikme, öngörü ve düzeltme sınırları

## Üç ayrı gecikme
Input'un host'a ulaşması, host sonucunun gelmesi ve interpolation tamponunun gösterim gecikmesi birbirinden ayrılır. 150 ms RTT için her yönde sabit 75 ms varsayımı ölçüm yerine geçmez. Jitter ve paket kaybı, ortalama RTT kadar önemlidir.

## İlk yaklaşım
Paylaşılan rigidbody'ler host simülasyonu + istemci interpolasyonu kullanır. Yerel oyuncu hareketinde prediction/reconciliation ihtiyacı ayrı değerlendirilir. Yerel el ve etkileşim highlight'ı hızlı niyet gösterebilir; eşyanın kabul edilmemiş pozunu kesin gerçekmiş gibi otoriteye yazmaz. İlk prototip ağır objelerde tam fizik rollback içermez.

## Düzeltme politikası
Küçük görsel hata kısa süreli blend, büyük veya güvenlik açısından geçersiz poz hard correction gerektirebilir. Eşikler metre ve derece olarak profile'da açık tutulur; henüz sayısal kabul eşiği dondurulmadı. Collider authoritative temsil ile render child ayrımı düşünülür. Sonsuz smoothing item'ı duvar içinde bırakmamalıdır.

## Platform riski
Karakter platform hareketini hem base velocity hem transform parent ile iki kez alırsa kayma/fırlama oluşur. NetworkTransform ile custom servo aynı rigidbody'yi aynı anda sürerse jitter oluşur. Tek veri sahibi ve tek uygulama noktası şarttır. Platformun dönme noktasına göre noktasal hız, center velocity'den farklıdır; indirme/bırakma testinde bu fark görünür olur.

## Gerileme senaryoları
0/50/100/150/250 ms RTT; 0/15/40 ms jitter; %0/%1/%3/%5 kayıp ayrı profillerde sınanır. Her kombinasyonun tam çarpımını ilk sprintte çalıştırmak gerekmez; risk matrisi çiftleri seçer. 250 ms/%5 profili konfor garantisi değil kontrollü bozulma testidir: crash, sonsuz kilit veya para hatası olmamalıdır.

## Kabul kanıtı
Host ve istemci eşzamanlı video, server tick log'u, correction histogramı ve oyuncu gözlemi birlikte değerlendirilir. “Localhost'ta iyi” karar değildir. Eşiği geçmeyen shared carry varsa daha hızlı snapshot'a körlemesine geçmeden input/otorite ve moving-base hatası araştırılır.

## Gereksinim ve test izleri
- REQ-006 → TC-006 — Ağ stresinde state bozulmamalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-017 — Ortak taşıma ve kuvvet modeli](../01_gameplay/017_shared_carrying.md)
- [ASK-018 — Platform, destek kütlesi ve kontrollü eğim](../01_gameplay/018_platform_balance.md)
- [ASK-026 — Ağ çözümü seçimi ve risk prototipi](026_network_adr_spike.md)
- [ASK-029 — Çoğaltma, snapshot ve ağ ilgisi](029_replication_snapshot.md)
- [ASK-035 — Multiplayer test matrisi ve kanıt](035_network_test_matrix.md)

