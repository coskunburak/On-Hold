---
doc_id: ASK-017
title: "Ortak taşıma ve kuvvet modeli"
version: 0.1.0
status: proposed
owner_role: Gameplay
last_reviewed: 2026-09-07
dependencies: ["ASK-013","ASK-015","ASK-016","ASK-018","ASK-030","ASK-040"]
---

# Ortak taşıma ve kuvvet modeli

## Hedef his
Eşya ağırlığı hissedilir ancak oyuncuyu kontrol edilemez sarkaç haline getirmez. En fazla iki holder vardır. İkinci oyuncu ek kontrol ve sınırlı toplam kuvvet sağlar; her yeni elin limitsiz enerji eklemesi yasaktır. Kesin yay/sönüm sabitleri burada uydurulmaz; prefab kütlesi ve ağ koşullarına göre spike çıktısı olarak kilitlenir.

## Host uygulaması
Her holder için eşya yerel koordinatında bir grip noktası ve oyuncu önünde sınırlandırılmış hedef vardır. Host hedef mesafesini, görüş yolunu ve hızını doğrular. Servo kuvveti konum hatası ve bağıl hızdan hesaplanır, eşya ailesine göre clamp edilir. İki holder'ın kuvvetleri önce ayrı sınırlandırılır, sonra toplam kuvvet/tork limiti uygulanır. Aynı rigidbody'ye istemciler doğrudan force uygulamaz.

## Ağ hissi
Yerel el ve prompt hemen niyet gösterebilir. Otoriter eşya pozu sunucudan gelir; istemci pozunun keyfî kesinleştirilmesi kabul edilmez. Tam tahmin/geri sarma seçeneği yalnız risk çalışması ispatlarsa eklenir. İlk hedef fizik objelerinde görsel interpolasyon, karakterde sınırlandırılmış hareket öngörüsüdür; NGO'nun bunu otomatik sağladığı varsayılmaz.

## Geometri ve bırakma
Tutma noktası oyuncunun içinden veya duvar içinden geçemez. Eşya doorway'e sıkışınca force biriktirilmez; hedef mesafe sınırı ve kuvvet clamp devreye girer. Son holder bırakınca mevcut güvenli fizik hızı korunur, ek “fırlatma” darbesi verilmez. V1 ayrı atma özelliği içermez. Kopma holder'ı kaldırır; kalan oyuncunun grip noktası korunur.

## Platformla birleşim
Eşya platforma temas ediyorsa supported kütle hesabına bir kez girer. Havada tutulan eşyanın ağırlığı karakterin sabit 80 kg değerine ayrıca eklenmez; bu kontrollü arcade yaklaşımın açık sadeleştirmesidir. Gerçek yük aktarımı simülasyonu v1 hedefi değildir. Bu kural denge göstergesiyle tutarlı kalmalıdır.

## Kritik testler
İki oyuncu ters yönlere çektiğinde sonlu kuvvetler; iki elde 150 ms RTT altında kabul edilebilir kontrol; biri kopunca joint temizliği; düşük FPS istemcisinin diğerinin eşyasını hızlandırmaması; kapıda 60 saniye sıkışmada enerji patlamaması. Kabul eşikleri spike'ta ölçülüp dondurulacaktır.

## Gereksinim ve test izleri
- REQ-004 → TC-004 — İki holder sınırlı kuvvetle aynı eşyayı taşıyabilmeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-013 — Kontroller, kamera ve giriş niyetleri](013_controls_camera.md)
- [ASK-015 — Etkileşim sorgusu ve komut sözleşmesi](015_interaction_contract.md)
- [ASK-016 — Eşya semantiği ve değişmezler](016_item_state_model.md)
- [ASK-018 — Platform, destek kütlesi ve kontrollü eğim](018_platform_balance.md)
- [ASK-030 — Gecikme, öngörü ve düzeltme sınırları](../02_network/030_latency_prediction.md)
- [ASK-040 — Unity fizik uygulaması ve teknik sınırlar](../03_engineering/040_physics_integration.md)

