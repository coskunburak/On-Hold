---
doc_id: ASK-022
title: "Teslimat doğrulaması ve tek seferlik sonuç"
version: 0.1.0
status: proposed
owner_role: Gameplay
last_reviewed: 2026-09-07
dependencies: ["ASK-010","ASK-016","ASK-023","ASK-033"]
---

# Teslimat doğrulaması ve tek seferlik sonuç

## Geçerli teslimat
Eşya Available + Free olmalıdır; integrity en az P-INTEGRITY-MIN; teslimat hacminin içinde tamamen bulunmalı; doğrusal hız P-DELIVERY-SPEED ve açısal hız P-DELIVERY-ANGULAR altında kesintisiz P-DELIVERY-DWELL süresi kalmalıdır. Held veya Secured eşya teslim edilmez. Timer herhangi bir koşul bozulursa sıfırlanır, duraklatılmaz.

## Tamamen içeride ne demektir?
Her prefab authored deliveryBounds köşe noktalarını yerel koordinatta taşır. Host bu noktaları zone koordinatına dönüştürür ve tümünü hacim içinde toleransla denetler. Renderer'ın dünya AABB'si tek başına doğruluk kaynağı değildir; eğik nesnelerde gereksiz büyük kutu oluşur. Tolerans zone profilinde açık tanımlanır; kamera görüntüsünden karar verilmez.

## Domain işlemi
Host, itemInstanceId'yi ContractRun teslimat defterinde kontrol eder. Yoksa lifecycle Delivered, holder/anchor boş, teslimat kaydı ve geçici sonuç artışı tek transaction içinde yapılır. EventId = contractRunId + itemInstanceId + deliveryRevision gibi çakışmasız alanlarla belirlenir. Aynı komut, trigger veya event tekrar geldiğinde yeni ödeme olmaz.

## Puan ve para
Geçici eşya değeri = definition baseValue × integrity/100; tam sayı yuvarlama yönü floor olarak tanımlanır. Sözleşme bonusu yalnız zorunlu öğeler ve kota sağlanınca eklenir. Süre bonusu v1 slice'ta yoktur; gizli formül eklenmez. Aktif turdaki toplam, kampanya cüzdanı değildir. Kalıcı cüzdan yalnız Settling'de ASK-033 işlemiyle değişir.

## Görünüm
Host kabulünden önce ilerleme halkası yerel tahmin olabilir; kesin tik ve ses authoritative event ile gelir. Delivered item kısa görsel geçişten sonra oyun fizik dünyasından çıkarılabilir. Event tekrarı sesi ve konfeti ikinci kez oynatmamalıdır. Sonuç ekranı aynı teslimat defterini kullanır.

## Kritik yarış
Süre aynı tick başında bittiyse teslimat işlenmez. Item aynı tick'te integrity sıfıra düştüyse Lost geçişi teslimat değerlendirmesinden önce uygulanır. Sıra: doğrulanmış komutlar → fizik/hasar → terminal cleanup → zone koşulları → sonuç değerlendirmesi. Uygulamadaki callback sırası buna uymuyorsa olaylar domain adımında kuyruklanır.

## Kabul
İki zone overlap'i aynı eşya için bir kayıt üretir; bağlantı tekrarında sonuca ikinci kez eklenmez; zone kenarında dönen nesnenin dwell'i sıfırlanır; 19 integrity reddedilir, 20 diğer koşullarla kabul edilir. Testler disk commit'ten bağımsız domain düzeyinde de çalışır.

## Gereksinim ve test izleri
- REQ-011 → TC-011 — Teslimat ve ödeme yinelenmemeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-010 — Sözlük, birimler ve başlangıç parametreleri](../00_foundation/010_glossary_parameters.md)
- [ASK-016 — Eşya semantiği ve değişmezler](016_item_state_model.md)
- [ASK-023 — Sözleşme tanımı, başarı ve bitirme](023_contract_rules.md)
- [ASK-033 — Kampanya işlemleri ve kayıt otoritesi](../02_network/033_campaign_transaction.md)

