---
doc_id: ASK-008
title: "Tasarım ilkeleri ve anti-hedefler"
version: 0.1.0
status: proposed
owner_role: GameDesign
last_reviewed: 2026-09-07
dependencies: ["ASK-005","ASK-015","ASK-063","ASK-068"]
---

# Tasarım ilkeleri ve anti-hedefler

## Dört ilke
**Okunabilir fizik:** yükün ağırlığı, destek noktası ve kayma nedeni görsel/işitsel ipuçlarıyla anlaşılır. Gerçeğe birebir simülasyon yerine tutarlı, sınırlandırılmış davranış tercih edilir. Denge göstergesi, gerçek oyun kütle hesabıyla aynı veriyi kullanır.

**Birlikte kurtarma:** ekip üyelerinin küçük müdahaleleri anlamlıdır. Bırakma, vinci durdurma ve karşı tarafa geçme hata sonrası seçeneklerdir. Başarısızlığın tamamı tek oyuncunun düğmeye basmasına indirgenmez.

**Kısa deneme döngüsü:** yeniden deneme, uzun yükleme ve zorunlu yürüyüş cezasıyla ağırlaştırılmaz. Başarısız sözleşme kalıcı borç üretmez. Öğrenme kaybı vardır, oturumun anlamsızlaşması yoktur.

**Tutarlı görünüm:** ana set, karakter, mobilya, VFX ve UI birlikte değerlendirilir. Ücretsiz paket sayısı artırmak sanat yönü oluşturmaz.

## Anti-hedefler
Kontrolsüz rigidbody patlamasını komedi sanmak; oyuncuya açıklanmayan rastgele sabotaj; gerçeğe uygun ama okunmayan halat gerilimi; görev sırasında uzun envanter yönetimi; fizik nesnesi sayısını içerik zenginliğiyle karıştırmak; başka oyunun görsel kimliğine yaslanmak.

## Tasarım inceleme kartı
Her mekanik için dört soru yazılır: oyuncu ne görür, ne yapabilir, sonuç hangi kurala göre hesaplanır, başarısızlıktan nasıl döner? Örneğin eğim arttığında kenar şeridi ve yük sesi değişir; oyuncu karşı tarafa gider; platform destek kütlesi hedef açıyı etkiler; durdurma ve sabitleme yeni fırsat yaratır. Bu dört bağlantıdan biri yoksa mekanik hazır değildir.

## Çatışma çözümü
Fizik gerçekçiliği ile ağ kararlılığı çatışırsa özgün kancayı koruyan kontrollü çözüm prototiplenir. Görsel detay ile siluet okunurluğu çatışırsa etkileşim silueti korunur. Gerilim ile kamera konforu çatışırsa kamerayı zorlamak yerine ses, platform kenarı ve eşya hareketi kullanılır.

## Kabul
Her büyük mekanik değişikliğinde bir ilkeyi nasıl güçlendirdiği ve diğerlerine maliyeti not edilir. Oyuncu testinde iki ayrı kişinin aynı olayı farklı temel kurallarla açıklaması, belgede veya geri bildirimde belirsizlik işaretidir.

## İlgili kaynak belgeler
- [ASK-005 — Vizyon, özgün kanca ve deneyim](005_vision_hook.md)
- [ASK-015 — Etkileşim sorgusu ve komut sözleşmesi](../01_gameplay/015_interaction_contract.md)
- [ASK-063 — Erişilebilirlik ve kamera konforu](../05_content/063_accessibility.md)
- [ASK-068 — Oyuncu testi, eğlence ve okunurluk](../06_quality/068_playtest_protocol.md)

