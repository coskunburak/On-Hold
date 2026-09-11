---
doc_id: ASK-025
title: "Düşme, sıkışma ve kurtarma kuralları"
version: 0.1.0
status: proposed
owner_role: Gameplay
last_reviewed: 2026-09-07
dependencies: ["ASK-016","ASK-021","ASK-023","ASK-032"]
---

# Düşme, sıkışma ve kurtarma kuralları

## Oyuncu düşmesi
Oyuncu kill volume'a girince gameplay incapacitated durumuna alınır; holder ve driver hakları temizlenir. Kısa görsel geçişten sonra host tarafından seçilen güvenli oyuncu noktasına döner. Kalıcı can/ölüm ekonomisi v1'de yoktur. Kamera düşüşü rahatsız edici hızda devam ettirilmez; hareket azaltma tercihi uygulanır. Taşıdığı eşya otomatik cebine ışınlanmaz.

## Eşya dünya dışı kurtarması
Available ve integrity sıfırdan büyük eşya kill volume'a girerse, recoveryCount sınırın altındaysa son doğrulanmış item recovery noktasına Free olarak alınır. Holder ve anchor temizlenir; doğrusal/açısal hız sıfırlanır; mevcut hasar korunur. RecoveryCount artar. Başlangıç önerisi her kurtarmada kalan görev süresinden 15 saniye düşmektir; bunun ekonomi değil zaman cezası olduğu UI'da görünür. Süre sıfıra iner ise normal Settling başlar.

İki kurtarma hakkı kullanılmış eşyada sonraki dünya dışı olay Lost üretir. Terminal Delivered/Lost item kurtarılamaz. Kurtarma alanı boş değilse sıradaki authored nokta denenir; hepsi doluysa kontrollü bekleme/kuyruk kullanılır, başka item'ın içine spawn yapılmaz.

## Sıkışma
Oyuncu “sıkıştım” kurtarma isteği için kısa bekleme ve cooldown kullanabilir; host güvenli nokta doğrular. Eşyaya sınırsız isteğe bağlı teleport v1'de verilmez; rota atlatma sömürüsü yaratır. Teknik olarak takılan eşya raporu tanı verisine girer. Geliştirme debug rescue'su yayın build'inde ekonomik oyun kuralı değildir.

## İletişim
Oyuncuya düşmenin maliyeti ve geri dönüş yeri önceden öğretilir. Ağ düzeltmesi veya sahne yükleme hatası normal oyuncu cezası olarak gizlenmez. Sistem kaynaklı reset ayrıca reasonCode taşır; shipping hatasını “oyunun kaosu” diye kabul etmeyin.

## Test
Aynı kill volume callback'i tekrarında sayaç bir artar; eşzamanlı hasar ve kurtarmada sıfır integrity canlanmaz; dolu kurtarma alanı penetrasyon üretmez; oyuncu koparken düşerse hayalet avatar kalmaz. Platform üzerinde oyuncu yeniden doğurtulmaz; güvenli sabit zemin tercih edilir.

## Gereksinim ve test izleri
- REQ-014 → TC-014 — Kurtarma terminal item'ı canlandırmamalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-016 — Eşya semantiği ve değişmezler](016_item_state_model.md)
- [ASK-021 — Hasar ve kontrollü kırılma](021_damage_breakage.md)
- [ASK-023 — Sözleşme tanımı, başarı ve bitirme](023_contract_rules.md)
- [ASK-032 — Kopma, yeniden bağlanma ve host kaybı](../02_network/032_disconnect_reconnect_hostloss.md)

