---
doc_id: ASK-055
title: "Ses tasarımı, miks ve sesli iletişim"
version: 0.1.0
status: proposed
owner_role: Audio
last_reviewed: 2026-09-07
dependencies: ["ASK-019","ASK-021","ASK-048","ASK-063"]
---

# Ses tasarımı, miks ve sesli iletişim

## Ses dili
Ahşap vurma, metal titreşim, kumaş sürtünme ve vinç motoru farklı yüzey aileleri oluşturur. Ağır eşyanın sesi yalnız ses yüksekliğiyle değil düşük frekans ve kısa gövde karakteriyle ayrışır. Sürekli gıcırtı yorucu olmamalı; denge gerilimi sınırlı katman ve histerezisle yükselir.

## Kaynak/üretim
Kenney Impact Sounds prototip katmanıdır; tek sesin her temasta tekrar etmesi engellenir. Sonniss veya Freesound ek seslerinde dosya bazlı lisans doğrulanır. Kendi kayıtlarında kaydın içindeki müzik, konuşma ve özel alan izinleri ayrıca değerlendirilir. AI ses kullanımı kaynak/izin ve oyuncuya sunulan içerik envanterine girer.

## Mixer
Master, SFX, Ambience, Music, UI ayrı gruplardır. V1 oyun içi voice chat şart değildir; harici sohbet kullanan ekip için müzik ve vinç gürültüsü konuşmayı bastırmamalıdır. Dahili voice eklenirse codec, servis/transport, mute/report ve gizlilik yeni kapsamdır; mevcut plan bunu uygulamış saymaz.

## Teknik
Yakın fizik olayları spatial, UI doğrulaması uygun 2D kanal olabilir. Collision çiftleri ses cooldown'ı taşır; mikro temaslar her tick ses üretmez. Eşzamanlı ses öncelikleri ve voice limit belirlenir. Motor loop'u gerçek winch state/hızından türetilir, client input tuşundan değil.

## Erişilebilirlik
Kritik bilgi yalnız sesle verilmez: vinç kilidi, kritik hasar ve delivery ret nedeni UI/ping karşılığı taşır. Mono mix ve ayrı volume ayarları düşünülür. Altyazı/olay metni yalnız gerçekten kullanılan konuşma/önemli ses kapsamına göre yazılır.

## Kabul
Kulaklık ve hoparlör testinde dört oyuncunun darbe trafiği clipping üretmez; motor durunca loop kalmaz; bağlantı tekrarında iki motor loop'u üst üste binmez. Sessiz modda temel görev tamamlanabilir.

## Gereksinim ve test izleri
- REQ-031 → TC-031 — Ses state ile uyumlu ve erişilebilir olmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-019 — Vinç kontrolü ve kontrol kiralaması](../01_gameplay/019_winch_control.md)
- [ASK-021 — Hasar ve kontrollü kırılma](../01_gameplay/021_damage_breakage.md)
- [ASK-048 — Lisans, kaynak kaydı ve AI kullanımı](048_licenses_provenance.md)
- [ASK-063 — Erişilebilirlik ve kamera konforu](../05_content/063_accessibility.md)

