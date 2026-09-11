---
doc_id: ASK-035
title: "Multiplayer test matrisi ve kanıt"
version: 0.1.0
status: proposed
owner_role: QA
last_reviewed: 2026-09-07
dependencies: ["ASK-026","ASK-030","ASK-032","ASK-067"]
---

# Multiplayer test matrisi ve kanıt

## Temel matris
| Profil | RTT | Jitter | Kayıp | Beklenti |
|---|---:|---:|---:|---|
| N0 | Yerel ölçüm | 0 | 0 | İşlevsel referans |
| N1 | 50 ms | 15 ms | %0 | Normal his |
| N2 | 100 ms | 15 ms | %1 | Normal kullanım |
| N3 | 150 ms | 40 ms | %1 | Hedef stres |
| N4 | 250 ms | 40 ms | %5 | Güvenli bozulma |

Bu değerler emülasyon girdisidir; gerçek ölçülen RTT ayrıca raporlanır. N4'te akıcı deneyim vaat edilmez; state corruption, çoğaltılmış para ve cleanup hatası kabul edilmez.

## Her turda temel senaryolar
2/3/4 oyuncu; host güçlü/zayıf cihaz; iki holder aynı eşya; dolu platform yükselmesi; sürücünün kopması; sağ/sol kenara eşzamanlı yürüyüş; secure/unsecure; timer ile delivery yarışı; Results commit sırasında bağlantı kaybı. Her kontrol tüm kombinasyonlarda her commit'te koşmaz: PR smoke, gece matrisi, milestone soak olarak ayrılır.

## Ölçümler
p50/p95/p99 correction mesafesi, platform relative hata, authoritative fizik ms, istemci frame time, byte/s ve packet/s, retransmit oranı, komut ret nedeni, holder/joint sayısı. Ortalama ping tek başına rapor değildir. Host/client saat eşlemesi server tick üzerinden yapılır; duvar saatlerinin senkron olduğu varsayılmaz.

## Süre
PR smoke 5–10 dakikalık sabit rota; milestone 30 dakikalık yinelenen görev; release adayı en az 2 saatlik 4 oyuncu soak önerisi. Bunlar henüz koşulmuş testler değildir. Soak sırasında memory drift, ağ obje sayısı ve pool kapasitesi takip edilir.

## Hata raporu
Build hash, dependency lock hash, map/contract ID, roster, emülasyon profili, actual RTT, reproduction adımları, beklenen/gerçek sonuç, log bundle ve eşzamanlı videolar. “Bazen lag oluyor” yerine tick aralığı ve etkilenen nesne kimliği gerekir.

## Kapı
Kritik state mismatch veya kalıcı holder kilidi açıkken içerik milestone'u teknik olarak geçmiş sayılmaz. Hissiyat toleransları ilk spike'ta belirlenir ve sonradan geçmek için sessizce gevşetilmez.

## Gereksinim ve test izleri
- REQ-006 → TC-006 — Ağ stresinde state bozulmamalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-026 — Ağ çözümü seçimi ve risk prototipi](026_network_adr_spike.md)
- [ASK-030 — Gecikme, öngörü ve düzeltme sınırları](030_latency_prediction.md)
- [ASK-032 — Kopma, yeniden bağlanma ve host kaybı](032_disconnect_reconnect_hostloss.md)
- [ASK-067 — Test stratejisi ve gereksinim izlenebilirliği](../06_quality/067_test_strategy_traceability.md)

