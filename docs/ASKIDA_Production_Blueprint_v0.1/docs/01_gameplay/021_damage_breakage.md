---
doc_id: ASK-021
title: "Hasar ve kontrollü kırılma"
version: 0.1.0
status: proposed
owner_role: Gameplay
last_reviewed: 2026-09-07
dependencies: ["ASK-016","ASK-022","ASK-025","ASK-040","ASK-054"]
---

# Hasar ve kontrollü kırılma

## Hasar otoritesi
Integrity yalnız host tarafından değişir. Çarpışma sesi hasar verildiği anlamına gelmez; istemci kozmetik çarpışma gözleyebilir fakat integrity yazamaz. Hasar için bağıl darbe büyüklüğü, temas türü, eşya ailesi ve cooldown profili değerlendirilir. Gerçek fizik enerji hesabı ile arcade hasar eğrisi ayrı kavramlardır.

## Hesap tasarımı
Her eşya ailesinin hasarsız darbe eşiği, artış eğrisi ve tek olay tavanı bulunur. Aynı rigidbody çiftinin aynı çarpışma bölümündeki çoklu contact noktaları bir olayda birleştirilir. Sürekli zeminde duran eşyanın her fixed tick'te hasar alması yasaktır. Pair cooldown ve yeniden çarpışma ayrımı gerekir; kesin eşikler graybox sonuçlarıyla belirlenecek.

## Davranış
Integrity 20'nin altına inen eşya teslim edilemez ancak sıfıra kadar Available kalabilir; UI düşük değer gerekçesini gösterir. Integrity sıfırda Lost olur: holder ve anchor ilişkileri temizlenir, görev uygunluğu yeniden değerlendirilir. Kırılma görsel parçaları yerel, süreli ve değersizdir; ekonomik item kimliği veya etkileşim collider'ı edinmez. Runtime mesh fracture v1 kapsamı değildir; önceden hazırlanmış hasar varyantları kullanılır.

## Bilgi tasarımı
Üç okunur aşama önerilir: sağlam, çizilmiş/hasarlı, kritik. Bunlar tek bir integrity değerinden türetilir; kendi bağımsız HP havuzları yoktur. Çatlak, ses ve simge birlikte kullanılır. Gizli rastgele kritik hasar yoktur. Sağlam görünen ama teslim edilemeyen item kabul edilmez.

## Sömürü ve hata
İstemciden gelen “collisionDamage” isteği doğrudan çalışmaz. Yerel FPS düşmesi hasarı çoğaltmaz. Güvenli kurtarma teleport'u hasar darbesi yaratmaz; recovery ile çarpışma hasarı aynı tick'te çakışırsa recovery sonrası yapay temas hasarı bastırılır, gerçek önceki hasar silinmez.

## Kabul
Aynı yükseklikten kontrollü düşüşler tolerans içinde benzer hasar verir; zeminde 60 saniye duran eşya hasar biriktirmez; kopan holder hasar olayı üretmez; Lost eşya tekrar teslim edilemez. Sayısal toleranslar fizik benchmark kaydına işlenir.

## Gereksinim ve test izleri
- REQ-012 → TC-012 — Hasar temas sayısıyla çoğalmamalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-016 — Eşya semantiği ve değişmezler](016_item_state_model.md)
- [ASK-022 — Teslimat doğrulaması ve tek seferlik sonuç](022_delivery_scoring.md)
- [ASK-025 — Düşme, sıkışma ve kurtarma kuralları](025_recovery_failures.md)
- [ASK-040 — Unity fizik uygulaması ve teknik sınırlar](../03_engineering/040_physics_integration.md)
- [ASK-054 — VFX, geri bildirim ve havuzlama](../04_art/054_vfx_feedback.md)

