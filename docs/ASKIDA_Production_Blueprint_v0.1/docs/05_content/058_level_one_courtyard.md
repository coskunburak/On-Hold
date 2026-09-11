---
doc_id: ASK-058
title: "Seviye 1 — Avlu apartmanı"
version: 0.1.0
status: proposed
owner_role: LevelDesign
last_reviewed: 2026-09-07
dependencies: ["ASK-005","ASK-018","ASK-023","ASK-057","ASK-060"]
---

# Seviye 1 — Avlu apartmanı

## Tasarım görevi
Avlu apartmanı güvenli hazırlık ve tek net dikey yük hattıyla özgün kancayı öğretir. Birinci kat kaynak balkon, avlu park alanı ve bir üst hedef durak önerilir; final kat sayısı kamera/performans testine bağlıdır. Açık dünya sokakları yerine küçük çevre kapsülü vardır.

## Öğretim sırası
Önce tek kişinin taşıdığı sandık; sonra iki grip'li kanepe; sonra yüksek kütle merkezli buzdolabı; son olarak uzun/yoğun piano prototipi. Slice'ta bunlar dört model prototipidir; altı davranış ailesinin hepsi gerekmez. Her yeni item öncekinden bir yeni sorun ekler.

## Alan tasarımı
Kaynak odada eşyayı çevirmek için bir güvenli dönüş cebi bulunur. Balkon eşiği yükün yönünü seçtirir, milimetrik fizik takılması istemez. Platform alt durağında secure öğretimi için durağan alan; hedef balkonda delivery bekleme halkasının görüldüğü temiz zemin vardır. Avlu props'ları rota sınırını gösterir ama gereksiz ağ objesi olmaz.

## Anlatı
Kısa görev metni: “Yeni kiracı üst kata taşınıyor; piyano çizilmesin.” Diyalog sistemi, NPC kalabalığı veya sinematik gerektirmez. Apartman kişiliği renk, kapı numarası ve küçük nesne düzeniyle kurulur. Gerçek kişi, marka veya adres bilgisi kullanılmaz.

## Scriptlenmiş öğretim
İlk başarılı Grab prompt'u kapatır; ilk Secured onayı sonraki görevi açar. Bunlar host state'inden türetilir. Öğretici eşyayı gizlice teleport ederek başarılı göstermez. Deneyimli ekip öğretici metin yoğunluğunu azaltabilir; güvenlik kuralları değişmez.

## Kapı
Bir sözleşme hazırlıktan commit edilmiş sonuca kesintisiz çalışmalı; iki/dört oyuncu, controller, ping ve ağ stres kontrolü bulunmalı. Final art yalnız onaylı rota üzerine uygulanır. Oyuncu “neden eğildiğini” açıklayamıyorsa ikinci seviye tasarımına geçilmez.

## Gereksinim ve test izleri
- REQ-032 → TC-032 — Slice rotası iki/dört oyuncuyla geçilmeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-005 — Vizyon, özgün kanca ve deneyim](../00_foundation/005_vision_hook.md)
- [ASK-018 — Platform, destek kütlesi ve kontrollü eğim](../01_gameplay/018_platform_balance.md)
- [ASK-023 — Sözleşme tanımı, başarı ve bitirme](../01_gameplay/023_contract_rules.md)
- [ASK-057 — Blockout ve oynanabilir alan metrikleri](057_blockout_metrics.md)
- [ASK-060 — On iki sözleşmelik içerik taslağı](060_contract_catalog.md)

