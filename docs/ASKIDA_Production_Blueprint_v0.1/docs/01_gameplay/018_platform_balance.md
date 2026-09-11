---
doc_id: ASK-018
title: "Platform, destek kütlesi ve kontrollü eğim"
version: 0.1.0
status: proposed
owner_role: Gameplay
last_reviewed: 2026-09-07
dependencies: ["ASK-010","ASK-017","ASK-019","ASK-020","ASK-040","ASK-066"]
---

# Platform, destek kütlesi ve kontrollü eğim

## V1 simülasyon kararı
Platform halatlarla tamamen serbest asılı bir rigidbody zinciri değildir. Kaldırma rayı boyunca kontrollü yükseklik ve sınırlandırılmış pitch/roll kullanılır. Halatlar görseldir. Platform hareketi host'un tek otoriter adımında hesaplanır. Amaç arkadaşların karşı ağırlık hareketini okunabilir kılmak, gerçek kaldırma sistemi mühendisliğini taklit etmek değildir.

## Destek kümeleri
Her tick için platform local-space içinde SupportedSet oluşturulur: deck üzerindeki aktif oyuncular; deck tarafından desteklendiği sınıflandırılmış Available eşyalar; o platforma Secured eşyalar. Aynı item kimliği kümeye en fazla bir kere girer. Süs props'ları ve efekt parçaları girmez. Oyuncu görünümü ağırlığı değiştirmez.

Temas titremesini azaltmak için giriş/çıkış histerezisi gerekir: bir tek anlık collision callback ile kütle eklenip çıkarılmaz. Kesin süre ve temas eşiği spike'ta belirlenir. Held olup deck teması olmayan eşya kümeye girmez; taşıyıcının kütlesine dolaylı eklenmez. Bu sadeleştirme fiziken tam gerçekçi değildir, denetlenebilir oyun kuralıdır.

## Denge hesabı
Platform yerel yatay eksenleri x/z olsun. Toplam destek kütlesi M, ağırlıklı merkez C = Σ(m_i × p_i) / M olarak hesaplanır. Boş platformda C sıfır kabul edilir; platform öz kütlesi ve başlangıç dengeleyici terimi profile tanımlanır. C'nin deck merkezinden normalize edilmiş sapması hedef eğime çevrilir. Hedef açı ±P-TILT-MAX ile sınırlanır; motor yaklaşma hızı ve sönüm ayrıca sınırlandırılır. Oyuncu görsel göstergesi bu aynı C ve sınır hesabını okur.

## Hareket ve çarpışma
Kinematik/controlled platform ile karakter ve serbest rigidbody ilişkisi motor spike'ının ana riskidir. Transform'u render Update içinde teleport etmek yoktur. Kaldırma hızı, üstteki karakter taşıma hızı ve collision sonucu aynı physics zaman tabanına bağlanır. Penetrasyon, server/client divergence ve hareketli referans çerçevesi ölçülmeden içerik genişletilmez.

## Okunurluk
Deck üzerinde merkez işareti, kenarda denge yönü, işitsel gerilim katmanı ve yük konumu bulunur. Renk tek kanal değildir. Maksimum eğimde platform rastgele oyuncu fırlatmaz; yük kayabilir, vinç güvenlik kilidi devreye girebilir. Kilit eşiği ve tekrar açılma histerezisi ayrı profile alanıdır.

## Kabul
İki eşit kütle karşılıklı durunca merkez yaklaşık dengeli; tek kişi sağa yürüyünce doğru yönde eğim; secured eşya iki kez sayılmaz; oyuncu zıplaması tek karede aşırı eğim üretmez; boş platform kendiliğinden enerji kazanmaz. Fiziksel “approximately” toleransları test cihazında kayda geçirilir; evrensel deterministik PhysX sözü verilmez.

## Gereksinim ve test izleri
- REQ-005 → TC-005 — Platform destek kütlesini bir kere saymalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-010 — Sözlük, birimler ve başlangıç parametreleri](../00_foundation/010_glossary_parameters.md)
- [ASK-017 — Ortak taşıma ve kuvvet modeli](017_shared_carrying.md)
- [ASK-019 — Vinç kontrolü ve kontrol kiralaması](019_winch_control.md)
- [ASK-020 — Sabitleme, kayış ve anchor sözleşmesi](020_straps_anchors.md)
- [ASK-040 — Unity fizik uygulaması ve teknik sınırlar](../03_engineering/040_physics_integration.md)
- [ASK-066 — Başlangıç asset, render ve fizik bütçeleri](../06_quality/066_render_physics_asset_budgets.md)

