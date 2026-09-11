---
doc_id: ASK-059
title: "Seviye 2–3 — Kapsamlı varyasyon planı"
version: 0.1.0
status: proposed
owner_role: LevelDesign
last_reviewed: 2026-09-07
dependencies: ["ASK-007","ASK-056","ASK-058","ASK-060"]
---

# Seviye 2–3 — Kapsamlı varyasyon planı

## Seviye 2: Dar sokak
Amaç aynı mekaniği farklı yükleme yönü ve dar görüş çizgisiyle kullanmak. Kaynak odadan platforma 90 derece dönüş; iki güvenli durak; operatörün yük tarafını her zaman doğrudan görmemesi ping ihtiyacını artırır. Yeni winch sistemi veya araç sürüşü eklenmez. Görsel kimlik farklı cephe paleti ve sokak genişliğiyle kurulur.

## Seviye 3: Köşe teras
Amaç yük dağılımı ve hazırlık sırasını birleştirmek. İki kaynak grup aynı platformu paylaşır; hedefte geniş ama tek erişim hattı olan teras. Birden fazla eşya yükleme kararı verilir. Platformun serbest yatay uçuşu, ikinci vinç veya fiziksel halat inşası bu seviye için gizlice eklenmez.

## Ortak kit kullanımı
Çekirdek duvar, pencere, balkon ve ekipman aynı sanat ailesindedir. Yeni level için sınırsız benzersiz malzeme yoktur. Her seviye bir “hero silhouette” ve az sayıda özel modülle ayrışır. Yeni modül sayısı ve üretim saatleri backlog'a girer.

## Değişkenler
Dar sokak daha zor dönüş ve görünürlük; teras daha çok yük planlama. Rastgele rüzgâr, yağmurda kayganlık, hareketli trafik veya NPC sabotajı v1 zorunlu değildir. Bunlar fizik ve ağ test yüzeyini ciddi büyütür; ayrı teklif gerektirir.

## İçerik kapısı
Seviye 1 demo testinde başarı/konfor ve asset üretim hızı ölçülmeden 2–3'e final art bütçesi ayrılmaz. V1 önerisi her binada dört sözleşme, toplam 12'dir. Seviye 3 ilk kapsam kesme adayıdır; 8 sözleşmeli iki bina sürümü ürün sahibi kararıyla değerlendirilebilir.

## Kabul
Her level yeni mekanik kodu olmadan en az iki anlamlı görev varyasyonu sunar; iki kişi zorunlu rotayı bitirebilir. Görüş ve collision testleri dört oyuncuyla geçer. Hedef cihazda eş zamanlı görünür cephe miktarı performans bütçesine sığar.

## Gereksinim ve test izleri
- REQ-033 → TC-033 — Sözleşmeler geçerli ve farklı karar sunmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-007 — Kapsam, sürümler ve kesme sırası](../00_foundation/007_scope_matrix.md)
- [ASK-056 — Modüler dünya kiti ve apartman kimliği](056_modular_world_kit.md)
- [ASK-058 — Seviye 1 — Avlu apartmanı](058_level_one_courtyard.md)
- [ASK-060 — On iki sözleşmelik içerik taslağı](060_contract_catalog.md)

