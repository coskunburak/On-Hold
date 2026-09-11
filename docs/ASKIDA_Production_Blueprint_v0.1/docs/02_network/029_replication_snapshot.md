---
doc_id: ASK-029
title: "Çoğaltma, snapshot ve ağ ilgisi"
version: 0.1.0
status: proposed
owner_role: Network
last_reviewed: 2026-09-07
dependencies: ["ASK-016","ASK-018","ASK-028","ASK-030","ASK-032"]
---

# Çoğaltma, snapshot ve ağ ilgisi

## Veri sınıfları
Düşük frekanslı güvenilir state: lifecycle, handling, holderIds, anchor, integrity değişimi, görev durumu. Yüksek frekanslı pose: item/platform poz ve hızları, server tick. Yerel kozmetik: el lerp'i, toz, metal gıcırtısı. Bir VFX spawn'ı için ayrı network rigidbody oluşturulmaz.

## Başlangıç snapshot'ı
Reconnect snapshot'ı sessionEpoch, ContractRun, görev saati, phase, item registry, platform state, active players, delivery ledger özeti ve son committed campaign revision taşır. Client registry kurulmadan pose uygulanmaz. Snapshot sırasında oluşan olaylar baseline tick sonrası tamponlanır veya yeni snapshot alınır; karışık iki zaman düzeyi yüklenmez.

## Hareketli referans çerçevesi
Platform üzerinde local-space render interpolation faydalı olabilir ama otoriter world state ile ilişki açık olmalıdır. Platform world pozu ve item world pozu farklı gecikme tamponlarından gösterilirse eşya kayıyormuş gibi görünür. Aynı render zamanına örnekleme, parent değiştirme sınırı ve unsecure world pose koruması ayrı test edilir.

## Ağ ilgisi
Küçük tek bina seviyesinde önce tüm gameplay nesnelerini açıkça çoğaltarak doğruluk sağlanır. Interest management yalnız trafik ölçümü ihtiyaç gösterirse eklenir. Oyuncudan uzak diye mandatory item semantiği unutulamaz. Uyuyan serbest eşyaların pose sıklığı azaltılabilir; uyandırma ve state geçişleri güvenilir biçimde görünür olmalıdır.

## Bant genişliği hesabı
N aktif obje × gönderim/saniye × payload byte × alıcı sayısı kaba host çıkışını verir; protokol başlıkları, güvenilir tekrarlar ve şifreleme ayrıca ölçülür. Örnek 30 × 20 × 48 × 3 = 86.400 byte/s ham pose verisidir; gerçek trafik veya garanti edilmiş paket boyutu değildir. Profiler/wire capture ile doğrulama gerekir.

## Kabul
Late snapshot alan oyuncu doğru anchor ve holder'ları görür; Delivered eşya yeniden spawn olmaz; uyuyan item hareket edince kalıcı donuk kalmaz. UI revision ile pose tick aynı kavram sayılmaz. Serialize schema değişiminde protokol uyumsuzluğu açıkça reddedilir.

## Gereksinim ve test izleri
- REQ-016 → TC-016 — Misafir reconnect temiz snapshot almalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-016 — Eşya semantiği ve değişmezler](../01_gameplay/016_item_state_model.md)
- [ASK-018 — Platform, destek kütlesi ve kontrollü eğim](../01_gameplay/018_platform_balance.md)
- [ASK-028 — Komut protokolü, tekrarlar ve sıralama](028_command_protocol.md)
- [ASK-030 — Gecikme, öngörü ve düzeltme sınırları](030_latency_prediction.md)
- [ASK-032 — Kopma, yeniden bağlanma ve host kaybı](032_disconnect_reconnect_hostloss.md)

