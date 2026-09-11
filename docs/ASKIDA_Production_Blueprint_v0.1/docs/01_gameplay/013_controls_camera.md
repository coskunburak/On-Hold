---
doc_id: ASK-013
title: "Kontroller, kamera ve giriş niyetleri"
version: 0.1.0
status: proposed
owner_role: UX
last_reviewed: 2026-09-07
dependencies: ["ASK-015","ASK-017","ASK-019","ASK-052","ASK-063"]
---

# Kontroller, kamera ve giriş niyetleri

## Varsayılan eşleme önerisi
WASD hareket, fare bakış, E bağlamsal etkileşim, basılı sol tuş tutma, tuş bırakma bırakma, R sabitleme arayüzü, Q ping, Tab görev panosu, Esc menü. Bunlar dondurulmuş kontroller değildir. Gamepad eylemleri aynı semantik Action kimliklerine bağlanır; fizik kodu belirli klavye tuşu okumaz. Tutma için toggle seçeneği erişilebilirlik kapsamında desteklenir.

## Öncelik
Modal menü → arayüz odağı → vinç kontrol modu → eşya etkileşimi → boş el hareketi. Aynı giriş aynı karede hem UI onayı hem eşya bırakma üretmez. Menü açıldığında güvenli Release isteği gönderilir; istemci kaybolursa host ayrıca holder temizliği yapar. Menü çevrimiçi dünyayı durdurmaz; oyuncuya açık gösterilir.

## Kamera
İlk kişi kamera başı fiziksel rigidbody zincirine bağlanmaz. Platform pitch/roll bilgisi dünya ve ufuk ilişkisiyle görünür, kamera roll zorlaması kapalıdır. Görüş alanı, fare hassasiyeti, ters eksen, kamera sarsıntısı ve hareket azaltma ayrı ayarlanır. FOV aralığı ilk testte 70–100 dikey/yatay tanımı açık bir seçenek olarak belirlenmelidir; Unity kamera FOV birimiyle UI metni tutarlı olur, rastgele aynı rakam kopyalanmaz.

## Eşya tutma sırasında
Oyuncu elleriyle gerçekçi rig görünümü isterken kontrolü kaybetmemelidir. Tutma hedefi kamera önündeki sınırlı hacimde hesaplanır; duvar arkasına geçemez. Yük kamera içine sokulursa hedef mesafesi sınırlandırılır ve eşya saydamlaştırma yalnız yerel görsel çözüm olarak değerlendirilir; collider silinmez.

## Kontrol kabulü
Yeniden bağlanabilir tuşlar çakışma uyarısı verir; gamepad çıkarılması menüyü erişilemez bırakmaz; odak kaybı sonsuz tutma üretmez. İlk oyuncu ağ gecikmesini “tuş çalışmadı” sanmamalı: niyet bekliyor, kabul ve ret için farklı kısa geri bildirim vardır. Erişim reddi eşyayı yerel olarak kesin tutulmuş göstermez.

## Gereksinim ve test izleri
- REQ-035 → TC-035 — Erişim ayarları temel görevi desteklemeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-015 — Etkileşim sorgusu ve komut sözleşmesi](015_interaction_contract.md)
- [ASK-017 — Ortak taşıma ve kuvvet modeli](017_shared_carrying.md)
- [ASK-019 — Vinç kontrolü ve kontrol kiralaması](019_winch_control.md)
- [ASK-052 — Animasyon grafiği ve gerekli klipler](../04_art/052_animation_graph.md)
- [ASK-063 — Erişilebilirlik ve kamera konforu](../05_content/063_accessibility.md)

