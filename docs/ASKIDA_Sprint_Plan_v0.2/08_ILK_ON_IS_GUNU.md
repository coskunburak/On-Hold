# İlk on iş günü — S00 uygulama sırası

Bu takvim iki kişilik örnek ekibin ilk iki haftasıdır. Kişi başı net günde yaklaşık 5 saat: yaklaşık 4 saat planlı üretim + 1 saat rezerv. Takım toplamı iki haftada 80 saat planlı + 20 saat rezervdir. Günlere yazılan işler hedef sırasıdır; S00 kartlarının saatleriyle birebir günlük sabit taahhüt değildir. Tek kişi bunların hepsini aynı iki haftaya sığdırmaya çalışmaz.

| Gün | A odağı | B odağı | Gün sonu somut çıktı |
|---|---|---|---|
| 1 | Unity/URP sürüm adayı, Hub kurulumu, Game projesi | Kapasite/cihaz listesi, depo/asset kuralı taslağı | Doğru şablonda açılan boş proje, karar taslağı |
| 2 | Bootstrap/Frontend/PhysicsLab ve Runtime sınırı | Version control/.meta/ignore ve temiz checkout denemesi | Çalışan ilk build ve kurulum notu |
| 3 | Input Actions, kamera ve oyuncu kapsülü | NGO/Transport uyumlu paket adayı, bağlantı iskeleti | Yerel host açılır; oyuncu hareket eder |
| 4 | Gri oda, kapı ölçüsü, tek item prefab | Client connect/spawn, remote kamera ayrımı | İki süreç; ortak test sahnesi |
| 5 | Basit root Rigidbody/collider temsilini doğrulama | Host otoritesinde tek küpün çoğaltılması | İlk hafta integrated build |
| 6 | UI/gameplay input ayrımı ve kontrol smoke | Ayrılma, yeniden bağlantı iskeleti, log reasonları | Menü açıkken oyun inputu kesilir |
| 7 | Physics layer/collision matrix, ölçek kontrolü | Windows hedef build ve backend araç zinciri smoke | Hedef platform build kanıtı veya görünür blokaj |
| 8 | Temiz makine/klasör kurulumunu yönergeyle tekrar | Hata temizleme, paket lock ve build kimliği | Eksik GUID/script olmadan açılan checkout |
| 9 | S01 carry kabul sahnesi hazırlığı | Host/client smoke tekrar ve kanıt düzenleme | S01 için hazır küp/kanape proxy ve test profili |
| 10 | G0 demo ve gerçek saatlerin değerlendirilmesi | Risk/engeller, S01 alt iş tahmini ve review | G0 kararı, temiz backlog, S01 Ready |

## Bugün yapılacak ilk beş adım

1. Unity Hubda Unity 6.3 LTS adayını seç.
2. New project ekranında **Universal 3D** seç.
3. Depo kökü ASKIDA, Unity klasörü Game olacak şekilde tam yolu kontrol et; Product Namei ASKIDA yap.
4. Proje açılınca Console, URP ve ilk buildi kontrol et.
5. İlk sahneye zemin, bir metre referans küpü ve kapsül koy; S00 kartlarının gerçek çalışma saatini kaydet.

İlk gün bütün Quaternius paketlerini ve nihai mobilyaları import etme gereği yok. S00–S03te fizik ölçüsünü değiştirebileceğimiz için proxy nesneler uygundur; tek görsel referans küçük sahnede tutulabilir. S08de onaylı örnek platform ve kanepeyle art üretim hattı tamamlanır.

## Solo ilk sprint daraltması

İki haftada 50 net saat ayırabiliyorsan 40 saat planlı iş seç: yaklaşık 6 saat karar/cihaz kaydı, 18 saat proje/depo, 10 saat input/test odası temeli, 6 saat ilk build/kanıt. Kalan 10 saat rezervdir. İkinci iki haftalık sprintte ilk host/client ve diğer S00 işleri tamamlanır; S01 etiketi ilerleme uğruna erken açılmaz. Bu örnek dağılımın totalini kapasiteye göre güncelle; ağ tecrüben yoksa ayrıca öğrenme/spike işi yaz.

S00 bittikten sonra kullanışlı soru “kaç script yazdık?” değil, “bu build başka makinede açılıyor mu, iki süreç bağlanıyor mu, sıradaki test için gerekli en küçük sahne var mı?” olmalıdır.
