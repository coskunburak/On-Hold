---
doc_id: ASK-060
title: "On iki sözleşmelik içerik taslağı"
version: 0.1.0
status: proposed
owner_role: GameDesign
last_reviewed: 2026-09-07
dependencies: ["ASK-007","ASK-023","ASK-058","ASK-059","ASK-061"]
---

# On iki sözleşmelik içerik taslağı

## Öneri kataloğu
| ID | Bina | Odak | Özel doğrulama |
|---|---|---|---|
| C01 İlk Yük | Avlu | Sandık ve temel kontrol | Öğreticisiz ikinci tekrar |
| C02 İki El Bir Kanepe | Avlu | İki holder, dönüş | İki kişi tamamlanabilir |
| C03 Denge Meselesi | Avlu | Dik/ağır yük | Karşı ağırlık okunur |
| C04 Son Kat Piyanosu | Avlu | Birleşik beceri | Hasar/teslimat dengesi |
| C05 Dar Kapı | Sokak | Yön değiştirme | Geometrik geçiş mümkün |
| C06 Kör Nokta | Sokak | Ping ve operatör iletişimi | Sessiz iletişim yeterli |
| C07 Çift Sefer | Sokak | Yük sırası | Tek sefer sömürüsü yok |
| C08 Kırılgan Komşu | Sokak | Hasar yönetimi | Görsel hasar okunur |
| C09 Teras Düzeni | Teras | Yeni rota okuma | Yol bulma süresi |
| C10 Ağırlık Dağılımı | Teras | Birden çok destek yükü | Kütle bir kez sayılır |
| C11 Önce Hangisi | Teras | İş paralelleştirme | Dört kişi boş beklemez |
| C12 Büyük Taşınma | Teras | Final birleşim | Soak ve kayıt güvenliği |

Bu adlar çalışma adıdır, yerelleştirme metni değildir. Slice C01'in birleşik teknik varyantını, dış demo C01–C03'ü içerir. V1'de dört Avlu sözleşmesi bulunur; demo sayısı ile tam sürüm sayısı karıştırılmaz.

## Her sözleşmenin doldurulacak kartı
Definition ID ve sürüm; tam spawn listesi; mandatory item'lar; 2/3/4 oyuncu için kota/süre; eşya değerleri; başarı bonusu; açık/kapalı öğretim adımları; kaynak/hedef alanlar; recovery noktaları; kabul testleri; ölçülen ortalama tamamlama süresi. Bu sürüm nihai denge değerlerini içermez çünkü oynanış verisi yoktur.

## Altı davranış ailesi
Kompakt-kutu, uzun-geniş, dik-ağır, hassas-kırılgan, yuvarlanmaya eğilimli, dengesiz-kütle-merkezli. Aile özelliği yeni network actor türü olmak zorunda değildir; aynı item sistemi profile üzerinden çalışır. İlk dört modelin hepsi benzersiz mekanik kod gerektirmez.

## Çeşitlilik sınırı
Yalnız süreyi kısaltıp aynı görevi dört kez satmak yeterli içerik değildir. Her sözleşme en az bir farklı planlama sorusu sorar. Bunun oyuncularca algılanıp algılanmadığı ASK-068 testinde sınanır.

## Gereksinim ve test izleri
- REQ-033 → TC-033 — Sözleşmeler geçerli ve farklı karar sunmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-007 — Kapsam, sürümler ve kesme sırası](../00_foundation/007_scope_matrix.md)
- [ASK-023 — Sözleşme tanımı, başarı ve bitirme](../01_gameplay/023_contract_rules.md)
- [ASK-058 — Seviye 1 — Avlu apartmanı](058_level_one_courtyard.md)
- [ASK-059 — Seviye 2–3 — Kapsamlı varyasyon planı](059_levels_two_three.md)
- [ASK-061 — Varlık envanteri ve üretim kuyruğu](061_asset_inventory.md)

