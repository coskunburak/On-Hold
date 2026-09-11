# Kalite kapıları ve çıkış kanıtı

Önceki ASK-069 kimlikleri korunur. Hiçbir kapı bu teslimat sırasında geçmiş değildir. Kapı sahipleri rolü temsil eder; iki kişilik ekipte aynı kişi birden fazla rol alabilir ama kendi hatasının bağımsız tekrar testini mümkün olduğunda diğer kişiye yaptırır.

| Kapı | Örnek zaman | Zorunlu ürün kanıtı | Başarısızsa |
|---|---|---|---|
| G0 — çalışma tabanı | S00 sonu | Unity/URP kurulum, exact aday sürüm, kapasite/cihaz kaydı, temiz checkout ve yerel host/client smoke; çözüm uygunluğu hâlâ açık | Kurulum erişimi ve rol/saat planı kapanır |
| G1 — teknik risk | S03 sonu | 2/4 gerçek katılımcı, hareketli platformda ortak taşıma, gerçek internet, hedef ağ profili, holder/driver cleanup, ADR-NET | Yeni içerik yerine G1-R kurtarma |
| G2 — slice | S11 sonu | Bir rota/dört örnek, uçtan uca görev+kayıt, final hedefli görsel/ses/UI, dış grup gözlemi ve referans performans | Tek örnek kalite açığı kapanır |
| G3 — demo | S13 sonu | Avlu C01–C03, onboarding/TR/EN/input, 4–6 grup hedefli gözlem, maliyet hesabına bağlı v1 kapsamı | Öğretim/oynanış sorununa yeniden dilim |
| G4 — content complete | S17 sonu | Onaylı tüm bina/sözleşme/aile sayısı, localization, kaynak kaydı, kampanya softlock testi | Eksik içerik tamamlanır veya açıkça kesilir |
| G5 — release candidate | S21 sonu | P0/P1 sıfır, release build smoke, performans, soak, kayıt faultları, dağıtım/rollback ve yayın doğruluğu | RC ve çıkış tarihi kayar |

## Kapı klasörünün zorunlu içeriği

Her kapı için `Evidence/G1/<build-id>/` benzeri bir klasör kullan. Şunlar bulunmalı: karar tarihi/sahibi, commit ve build hash, exact Editor/paket sürümleri, cihaz ve ağ profili, çalıştırılan test kimlikleri, ham ölçüm veya rapor yolu, kısa video/görsel, açık kusurlar, kalan risk ve PASS/BLOCKED/FAIL sonucu. Büyük log/video Git LFS veya seçilen artifact saklama yoluna gidebilir; doküman yalnız kararlı bağlantısını taşır.

Sürümü değişen oyun buildinde önceki kanıtın hangi testler için geçerli kaldığı değerlendirilir. Fizik tick, transport, collider veya taşıma modeli değişince ilgili G1 testleri yeniden açılır. Kayıt şeması değişince atomiklik/migration; rig veya socket değişince carry animasyonu ve asset kabulü yeniden açılır. Bir renderer ayarı değişince bütün domain testlerini körlemesine büyütmek gerekmez; etkili yüzey seçilir.

## Şiddet ve düzeltme sırası

| Şiddet | Örnek | Geçiş kuralı |
|---|---|---|
| P0 | Kayıt kaybı/para çoğaltma, oturum crash, temel güvenlik blokajı | Release engeller; önce yeniden üretim ve minimal düzeltme |
| P1 | Zorunlu görev bitmiyor, holder kilidi, ciddi ağ ayrışması | G2 ve sonrası geçişi engeller |
| P2 | İşleyen alternatif yolu olan önemli UX/görsel kusur | Sahip, etki ve planlı düzeltme kaydı gerekir |
| P3 | Küçük kozmetik hata | Öncelik bütçesine göre |

Şiddet oyuncu etkisi, öncelik çalışma sırasıdır. Demo öncesi bir P2 görünürlük hatası P3 backend temizliğinden önce yapılabilir. Hiçbir sürümde kayıt kaybını sıradan bilinen hata diye normalleştirmeyiz.

## G2/G3 oyuncu gözlemi için önerilen başlangıç ölçütü

G2de bir iki kişilik, bir dört kişilik grup; G3te toplam 4–6 ayrı grup hedeflenir. G3 için öneri: ilk öğretici yükü grupların en az dörtte üçü doğrudan geliştirici yardımı olmadan teslim edebilsin; bütün gruplar tutma/sabitleme/denge farkını açıklayabilsin. Bunlar küçük örneklemde kullanılacak ürün kabul hipotezidir; sektör standardı veya ticari başarı oranı değildir. Gruplar farklı mekanik sorunlar yüzünden takılıyorsa yalnız ortalama başarı yüzdesiyle kapıyı açma. İlk gözlem öncesi eşikleri kaydet; veri geldikten sonra sessiz değiştirme.

G3 testinde oyuncuya oyunu yönlendirici şekilde anlatma. Tıkandığında nedenini kaydet; güvenilir gözlemi aldıktan sonra yardım ver ve yardım verildiğini işaretle. İkinci denemede öğrenme etkisini yeni oyuncu anlaşılırlığıyla karıştırma.
