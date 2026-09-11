---
doc_id: ASK-009
title: "Karar kaydı ve onay bekleyenler"
version: 0.1.0
status: proposed
owner_role: Producer
last_reviewed: 2026-09-07
dependencies: ["ASK-004","ASK-026","ASK-046","ASK-071","ASK-080"]
---

# Karar kaydı ve onay bekleyenler

## Durum dili
Kullanıcı ihtiyacı doğrulanmış olabilir; çözümün onaylandığı anlamına gelmez. Bu sürümde aşağıdaki ADR'lerin tamamı PROPOSED durumundadır. “Önerilen temel” üzerinden belgeler tutarlı yazılmıştır. Onay sahibi, tarih ve kanıt girilmeden APPROVED yapılmaz.

| ADR | Önerilen temel | Onay için kanıt |
|---|---|---|
| ADR-ENGINE | Unity 6 + URP; tam sürüm kilidi bekliyor | Host/client fizik spike ve araç becerisi |
| ADR-NET | Listen-server, host otoritesi; framework açık | NGO/transport entegrasyon testi |
| ADR-CAMERA | İlk kişi, kamera roll kapalı | Konfor ve yük okuma testi |
| ADR-ART | Quaternius şehir çekirdeği + özel araçlar | Ortak stil doğrulama sahnesi |
| ADR-SESSION | 2–4 arkadaş, tur içi yeni oyuncu yok | Kopma/yükleme akışı |
| ADR-SAVE | Host kampanyası, tur sonu atomik kayıt | Çökme/tekrar ödeme testleri |
| ADR-PLATFORM | Kontrollü eğim, görsel halat | Uzak istemcide ortak taşıma |
| ADR-SCOPE | 3 bina / 12 sözleşme üst hedef | Slice içerik üretim hızı |
| ADR-BUDGET | Ekip ve nakit bütçesi belirlenmedi | Kullanıcı girdisi |
| ADR-HARDWARE | 1080p/60 hedef hipotezi; cihaz açık | Referans cihaz listesi |
| ADR-AI | Kod destekli Blender üretimi, insan QA | Lisanslı örnek varlığın kabulü |

## Birlikte cevaplayacağımız sorular
Kaç kişi hangi rollerde haftada kaç saat çalışacak? Unity ve Unreal deneyimin hangisinde daha yüksek? İlk altı ay nakit üst sınırı nedir? Referans minimum PC hangi gerçek cihaz olacak? Sanat için düşük bütçeli özel üretim ayrılabilir mi? İlk dil ve dağıtım bölgeleri nedir? Bu cevaplar alınmadan tarih ve harcama kesinleştirilmez.

## ADR şablonu
Kimlik; problem; bağlam; seçenekler; seçilen öneri; reddedilen alternatiflerin gerekçesi; geçiş maliyeti; ölçülebilir kapı; karar sahibi; onay tarihi; etkilenmiş ASK/REQ/TC/W kimlikleri; yeniden değerlendirme koşulu.

## Değişiklik örneği
Unreal seçilirse oyun kuralları büyük ölçüde korunur, ancak ASK-026–045'in Unity uygulama ayrıntıları yeniden yazılır. Bu arşiv aynı anda iki motorun üretim planı değildir. Sadece isimleri değiştirerek tam destek iddia edilemez.

## Kabul
Bir Codex görevi karar durumu okuyarak “uygula” ile “karşılaştır ve öner” işlerini ayırabilmelidir. Onaylanmamış varsayımlar kullanıcıya kesin karar olarak aktarılmamalıdır.

## Gereksinim ve test izleri
- REQ-001 → TC-001 — Motor ve sürüm kararı kanıtla seçilmeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-004 — Proje bildirgesi ve başarı tanımı](004_project_charter.md)
- [ASK-026 — Ağ çözümü seçimi ve risk prototipi](../02_network/026_network_adr_spike.md)
- [ASK-046 — Sanat yönetimi ve paket uyumu](../04_art/046_art_direction.md)
- [ASK-071 — Üretim yol haritası ve kapasite senaryoları](../07_production/071_roadmap_capacity.md)
- [ASK-080 — Kaynaklar, doğrulama sınırları ve açık kanıtlar](../08_knowledge/080_sources_evidence_limits.md)

