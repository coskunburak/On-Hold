---
doc_id: ASK-064
title: "Yerelleştirme ve öğretici tasarımı"
version: 0.1.0
status: proposed
owner_role: UX
last_reviewed: 2026-09-07
dependencies: ["ASK-011","ASK-058","ASK-062","ASK-063"]
---

# Yerelleştirme ve öğretici tasarımı

## Dil kapsamı
Türkçe ve İngilizce ilk öneridir; kesin dil listesi maliyet ve test kapasitesiyle onaylanır. Runtime string birleştirmesiyle cümle oluşturmak yerine parametreli localization anahtarları kullanılır. Item/contract ID değişmez, görünen ad çevrilir.

## İçerik kuralları
Kısa, eylem odaklı metin; cihaz bağımsız action simgesi; çoğul ve sayı biçimleri; eksik font glyph testleri. “E'ye bas” sabit metni yerine yeniden atanmış etkileşim düğmesi gösterilir. Türkçe İ/ı ve büyük harfe çevirme locale kuralları test edilir.

## Öğretici akış
Bak/hareket → sandığı tut/bırak → ortak taşıma → platforma yerleştir → karşı ağırlık → secure → vinç → teslimat → sonuç/kayıt. Her adım gerçek authoritative davranışı gözler, sadece düğmeye basmayı değil. Oyuncu önceden adımı yaptıysa tekrar zorunlu değildir.

## Yardım dozu
İlk tur kısa bağlamsal yardım; tekrar turlarda azaltılabilir. Bir oyuncu öğrendi diye bütün grubun yardımını otomatik kapatma. Host'un başlattığı ortak görev ile yerel öğretici ilerleme ayrıdır; yardım kartı ağ state'ini değiştirmez.

## Çeviri süreci
Ana metin → bağlam/screenshot → çeviri → oyun içi kontrol → terminoloji review. Sadece tablo çevirisi line break ve dar arayüz sorununu göstermez. Sözlükte winch/vinç, secured/sabitlenmiş, holder/tutan oyuncu karşılıkları kararlı tutulur.

## Kabul
Pseudo-localization ile %30 uzamış metinlerde taşma kontrolü; Türkçe font ve sayı biçimi; controller prompt'u; eksik key için geliştirme alarmı. Öğretici oyuncuyu serbest bıraktığında sonraki hedefi anlayabilmeli; teknik ekip açıklaması gerektiren adım yeniden tasarlanır.

## Gereksinim ve test izleri
- REQ-036 → TC-036 — Yerelleştirme metin ve input'u korumalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-011 — Temel oyun döngüsü ve zaman çizgisi](../01_gameplay/011_core_loop.md)
- [ASK-058 — Seviye 1 — Avlu apartmanı](058_level_one_courtyard.md)
- [ASK-062 — Arayüz ve bilgi hiyerarşisi](062_ui_information.md)
- [ASK-063 — Erişilebilirlik ve kamera konforu](063_accessibility.md)

