---
doc_id: ASK-007
title: "Kapsam, sürümler ve kesme sırası"
version: 0.1.0
status: proposed
owner_role: Producer
last_reviewed: 2026-09-07
dependencies: ["ASK-009","ASK-023","ASK-059","ASK-071"]
---

# Kapsam, sürümler ve kesme sırası

## Önerilen kapsam tablosu
| Aşama | Oynanabilir kapsam | Amaç |
|---|---|---|
| Risk prototipi | Gri kutu, bir platform, bir ağır eşya, 2 istemci | Fizik/ağ riski |
| Vertical slice | Bir apartman rotası, 1 sözleşme, 4 eşya prototipi; 2–4 oyuncu testi | Uçtan uca kalite |
| Dış demo | Bir bina, 3 sözleşme, öğretici, güvenli kayıt | İlk kullanıcı doğrulaması |
| v1 aday kapsam | 3 bina, toplam 12 sözleşme, 6 eşya davranış ailesi | Ticari sürüm adayı |

Aile sayısı model sayısı değildir. Dolap ve buzdolabı aynı ağır-dik aileyi paylaşabilir. V1 miktarları tahminî üst kapsamdır; slice ölçümlerinden sonra dondurulur. İki kişilik risk testi, dört kişilik sürüm desteğinin doğrulandığı anlamına gelmez.

## V1 dahil
Arkadaş davetiyle çevrimiçi oturum, yeniden bağlanma, ortak taşıma, sınırlı platform eğimi, sabitleme, sunucu doğrulamalı teslimat, host'a ait kampanya kaydı, ayarlanabilir kamera ve temel erişilebilirlik. Türkçe ve İngilizce arayüz önerilir; dış çeviri/QA maliyeti ayrıca planlanır.

## V1 hariç
Host migration, dedicated sunucu filosu, rekabetçi sıralama, çapraz platform, konsol, mod araçları, tam fiziksel halat, sürekli active ragdoll, sürülebilir kamyon, prosedürel şehir, açık dünya, kozmetik mağaza ve kullanıcı üretimi içerik. Yerel split-screen de mevcut kapsamda yoktur. Dış sesli sohbet kullanılabilir; oyun içi ses v1 zorunluluğu değildir, ping sistemi gereklidir.

## Kapsam kesme sırası
Önce üçüncü bina ve özel görsel varyantlar; sonra altı davranış ailesini dörde indirme; sonra pahalı çevresel olayları kaldırma. Kesilmeyecekler: iki/dört kişilik temel tutarlılık, kayıt güvenliği, anlaşılır etkileşim geri bildirimi, kritik erişilebilirlik ve lisans kaydı. Kapsam değişiminde içerik sayısı ASK-059/060/061 ve backlog'da birlikte güncellenir.

## Kabul
Yeni özellik teklifi hedef problemi, daha ucuz alternatifi, bağımlılıkları, ek test yüzeyini ve kesilecek eşdeğer işi belirtir. “Küçük ekleme” ayrı bütçe gerekçesi değildir. Ürün sahibi karar kaydı olmadan v1 dahil sütununa özellik eklenmez.

## İlgili kaynak belgeler
- [ASK-009 — Karar kaydı ve onay bekleyenler](009_decision_register.md)
- [ASK-023 — Sözleşme tanımı, başarı ve bitirme](../01_gameplay/023_contract_rules.md)
- [ASK-059 — Seviye 2–3 — Kapsamlı varyasyon planı](../05_content/059_levels_two_three.md)
- [ASK-071 — Üretim yol haritası ve kapasite senaryoları](../07_production/071_roadmap_capacity.md)

