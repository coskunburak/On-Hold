---
doc_id: ASK-073
title: "Risk defteri ve azaltma planı"
version: 0.1.0
status: proposed
owner_role: Producer
last_reviewed: 2026-09-07
dependencies: ["ASK-026","ASK-046","ASK-065","ASK-068","ASK-071"]
---

# Risk defteri ve azaltma planı

## İlk risk kaydı
| Risk | Olasılık/etki başlangıcı | Erken sinyal | Azaltma / sahibi |
|---|---|---|---|
| R01 Moving-platform co-op jitter | Yüksek/yüksek | Uzak elde sürekli düzeltme | İlk spike / TechLead |
| R02 Denge kancası anlaşılmıyor | Orta/yüksek | Oyuncu rastgele sanıyor | Gri kutu test / Design |
| R03 Paket stil uyumsuzluğu | Yüksek/orta | Materyal değişse de siluet ayrı | Tek art sahnesi / ArtLead |
| R04 Scope büyümesi | Yüksek/yüksek | Yeni sistemli her level | Kesme listesi / Product |
| R05 Save/ödül çoğaltma | Orta/yüksek | Tekrar commit farkı | Transaction test / Backend |
| R06 Ekip kapasitesi yetersiz | Bilinmiyor/yüksek | Tahmin/saat sapması | Kapasite onayı / Producer |
| R07 Lisans/AI giriş belirsizliği | Orta/yüksek | Provenance eksik | Import kapısı / Producer |
| R08 Host cihaz darboğazı | Orta/yüksek | 4 oyuncuda fizik spike | Erken benchmark / TechLead |
| R09 Oyuncu grubu oluşturma | Orta/yüksek | Demo solo ziyaretçi terk ediyor | Açık co-op iletişimi / Product |
| R10 Yayın sonrası destek yükü | Orta/orta | Tekrarlayan bağlantı raporu | Tanı ve kapasite / QA |

Bunlar nicel olasılık ölçümü değil başlangıç sınıflandırmasıdır. Kullanıcı verisi ve teknik kanıtla güncellenir; renkli risk tablosu çözümün uygulandığı anlamına gelmez.

## Her riskin kaydı
Kimlik, tanım, tetikleyici, etki alanı, sahip, azaltma işi, fallback, son inceleme, kapanış kanıtı. “Dikkat edeceğiz” azaltma değildir. R01 fallback'i fiziksel halat eklemek değil kontrollü platform hareketini daha da sınırlandırmak olabilir.

## İnceleme
Her milestone ve yeni kritik bağımlılıkta ilk beş risk tekrar sıralanır. Kapanan riskin kanıtı saklanır; aynı sorun yeni sürümde dönerse yeniden açılır. Maliyet gerçekleşmişse risk değil issue olarak işlenir.

## Kapı
R01/R02 kapanmadan üç bina final art üretimi başlatılmaz. Lisansı belirsiz player-facing içerik public build'e girmez. Ekip kapasitesi bilinmeden yayın tarihi dışarıya verilmez.

## İlgili kaynak belgeler
- [ASK-026 — Ağ çözümü seçimi ve risk prototipi](../02_network/026_network_adr_spike.md)
- [ASK-046 — Sanat yönetimi ve paket uyumu](../04_art/046_art_direction.md)
- [ASK-065 — Performans hedefi ve ölçüm disiplini](../06_quality/065_performance_targets.md)
- [ASK-068 — Oyuncu testi, eğlence ve okunurluk](../06_quality/068_playtest_protocol.md)
- [ASK-071 — Üretim yol haritası ve kapasite senaryoları](071_roadmap_capacity.md)

