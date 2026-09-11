# Riskler, kritik yol ve kapsam azaltma

## Kritik yol

Kurulum ve ortak eşya → iki holder → hareketli platform → gerçek internet G1 → güvenilir state/anchor/teslimat/kayıt → tek görev kalitesi G2 → dış demo G3 → ölçülen içerik üretimi → alpha G4 → performans ve beta → RC G5. Bu sıra özellik bağımlılığıdır; sadece tarih satırları değildir.

## Risk defteri

| Risk | Erken işaret | Sorumlu rol / kapanış | Müdahale |
|---|---|---|---|
| Ortak taşıma kötü ağ hissi | N2/N3te sürekli büyük correction, tutma gecikmesi | Network / G1 | Servo/hız/eğim sadeleştirme, zaman uyumlu snapshot; gerekçeli aday değişimi |
| Hareketli platform kararsız | Oyuncu kayması, yük fırlaması, double velocity | Gameplay / S02–03 | Tek physics sahibi, noktasal hız kontrolü, kontrollü hareket |
| Eşya state yarışı | Duplicate holder/anchor, terminal item canlanması | Tech / S04–06 | Tek işlem sınırı, revision/sequence, invariant testleri |
| Kayıt/ödül çoğaltma | Aynı run ikinci bakiye değişimi | Tech / S06–G5 | İdempotent commit, fault injection, checkpoint sınırı |
| Görsel paket uyumsuzluğu | Aynı ışıkta farklı oran/kenar/roughness | Art / S08 | Tek kabul sahnesi, project-owned türev, daha az model |
| Art tahmini aşımı | Hero propın gerçek eforu tahsisin üstünde | Producer / G2–G3 | Model sayısını kes, üretim sprintini böl, gerçek dış destek bütçesi |
| Dört oyuncu host CPU yükü | Tek oyuncu iyi, dört oyuncu p95 bütçe dışı | Performance / G1/G2/S19 | Body/contact/joint/command profil, dekor sınırı |
| Framework/adapter bakımı | Hedef Editorla uyumsuz API/binary | Network / S03 | Exact sürüm ve lisans/maintenance kaydı; tek aday üretim tabanı |
| Katılımcı ve test donanımı yok | Dört süreç yalnız aynı makinede deneniyor | Producer / S00/S03 | Makine/grup erişimini erken ayır; kanıtı BLOCKED bırak |
| Windows build yolu geç keşfedilir | Yalnız Mac Editor smoke var | Build / S00 | Windows backend/plugin smoke ilk sprint |
| Scope creep | Her görev benzersiz sistem istiyor | Burak / her planning | Yeni iş için problem, test maliyeti, yerine kesilecek iş |
| Steam/hizmet dış beklemesi | Erişim/inceleme tamamlanmamış | Producer / en geç S13, tekrar S21 | Hazırlığı erken başlat, güncel şartı doğrula, beklemeyi kapasiteden ayır |
| Bilgi tabanı sapması | Kodda başka oyun kuralı, belgede eski kabul | Tech / her review | ASK/REQ/TC güncellemesi; kanonik ve türetilmiş ayrımı |
| Solo yük ve yorgunluk | İki sprint art arda rezerv tüketimi | Burak / her review | Gerçek net saati azalt, WIP sınırı, sprint kapsamını böl |

## Dış bağımlılıkları son haftaya bırakmama

S00da test cihazı/işletim sistemi/build erişimi planlanır. S03te gerçek bağlantı hizmeti veya Steam adapterı için gereken erişim/maliyet doğrulanır. S11de dış oyuncu grupları hazırlanır. S13te mağaza/yayın hesabı ve güncel platform süreçleri için ihtiyaç listesi çıkarılır; varsa gerekli hazırlık işleri backlogda görünür. S21 bu hazırlıkları ilk kez keşfetme sprinti değildir; son doğrulama ve provadır. Katılımcıya mesaj gönderme, hizmet harcaması veya yayın yapma bu plan dosyasının hazırlanmasıyla gerçekleşmiş sayılmaz.

## Kapsam kesme sırası

1. Üçüncü bina ve ona bağlı C09–C12; miktarlar 2 bina/8 sözleşme olarak bütün belgelerde değişir.
2. Aynı davranış ailesindeki özel model varyantları ve yoğun çevre ayrıntısı.
3. Altı aileyi dört aileye indirme; yeni davranışın oynanış değeri ile ağ/test maliyetini karşılaştır.
4. Pahalı çevre olayları, kozmetik animasyonlar ve ilave ses varyasyonları.

İki/dört oyuncu doğruluğu, kayıt güvenliği, anlaşılır ret/bağlantı mesajı, temel erişilebilirlik, lisans kaydı ve mandatory eşya geçişleri kesilmez. Farklı hedefe geçmek gerekiyorsa bunun adı yeni ürün kararıdır; test başlığını değiştirerek geçmiş gösterilmez.

## Saat aşımı için somut karar kuralı

Bir iş üç gün boyunca aynı bilinmezlikte kalıyorsa küçük bir teşhis kartı aç ve tek hipotez test et. Sprint ortasında kalan zorunlu iş kullanılabilir üretim saatini aşıyorsa kozmetik işi kapsamdan çıkar. İki sprint art arda rezervin tamamı tüketiliyorsa geçmiş tahmin iyimserdir; üçüncü sprintte 80 saat yerine daha düşük taahhüt ver. Kapı işini sonraki sprintin içerik işiyle üst üste yığıp nominal takvimi koruma.

G1, G2 ve G3ten sonra yeni takvim çıkarılması planın başarısızlığı değil, belirsizliğin ölçüme dönüşmesidir. Ticari kalite hedefi kapı kanıtıyla değerlendirilir; 24 sprint yazılmış olması oyunun 24 sprintte biteceğinin kanıtı değildir.
