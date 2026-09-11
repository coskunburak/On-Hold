---
doc_id: ASK-056
title: "Modüler dünya kiti ve apartman kimliği"
version: 0.1.0
status: proposed
owner_role: LevelDesign
last_reviewed: 2026-09-07
dependencies: ["ASK-046","ASK-047","ASK-053","ASK-057"]
---

# Modüler dünya kiti ve apartman kimliği

## Kit rolleri
Görsel cephe modülleri, oynanabilir collision kit'i ve set dressing ayrı katmanlardır. Bir pencere texture'ı gerçek geçiş değildir. Dışarıdan görünen ama erişilmeyen alanlar basit arka plan kit'i kullanır; oyuncunun girdiği balkon/oda gerçek geometri ve collision gerektirir.

## Ölçü standardı
Grid ve kat yüksekliği ilk blockout'ta seçilir; örneğin 0,25 m alt grid ve 3 m kat yüksekliği başlangıç adayıdır. Paket kaynak ölçülerine göre kör snap yapılmaz. Kapı açıklığı en büyük zorunlu eşyanın döndürülerek geçebildiği ölçüyle doğrulanır. Karakter collision capsule'ı art model boyundan bağımsız kayda girer.

## Özel tamamlayıcılar
Balkon korkuluğu düz/köşe, klima kutusu ve braketi, tente kapalı/açık görsel varyant, basit kablo hattı, kapı çerçevesi, apartman giriş panosu, korkuluk köşe başlığı, taşıma alanı zemini. Markalı logo ve gerçek kişi/telefon bilgisi kullanılmaz. Yerel kimlik klişe bir çöp/eskilik yığınına indirgenmez.

## Kit bağlantıları
Socket adları, pivot, grid ölçüsü, trim devamı, UV yönü ve collision seam'leri belgeye girer. İki modül birleşince oyuncu capsule'ı takılmamalıdır. Süs kablo ve küçük vidalar fizik engeli olmaz. Balkon açıklığı hem göze hem collider'a aynı sınırı sunar.

## Üretim sırası
Önce bir tam oynanabilir duvar/balkon kesiti, sonra köşe ve kat varyantları, sonra arka plan. Hedef oyun ışığında test etmeden bütün şehir kit'ini materyal dönüşümüne sokma. Kullanılmayan paket parçalarını shipping asset manifest'ine ekleme.

## Kabul
Bir level designer yalnız onaylı kit ile birinci rotayı kurabilir; yeni bespoke mesh ihtiyacı kayıtlıdır. Seams, lightmap/probe geçişi, silhouette ve collision ayrı kontrol edilir. Aynı modülün tekrarını dağıtmak için rastgele ölçekle kapı ölçüsünü bozma.

## Gereksinim ve test izleri
- REQ-032 → TC-032 — Slice rotası iki/dört oyuncuyla geçilmeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-046 — Sanat yönetimi ve paket uyumu](../04_art/046_art_direction.md)
- [ASK-047 — Seçilecek paketler ve kullanım sınırları](../04_art/047_package_selection.md)
- [ASK-053 — Malzeme, texture ve aydınlatma standardı](../04_art/053_materials_textures_lighting.md)
- [ASK-057 — Blockout ve oynanabilir alan metrikleri](057_blockout_metrics.md)

