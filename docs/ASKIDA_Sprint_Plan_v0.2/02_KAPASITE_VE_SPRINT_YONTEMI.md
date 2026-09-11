# Kapasite, sprint yöntemi ve tahmin sınırı

## Açık kapasite hesabı

Örnek ekip: iki teknik üretici, her biri haftada 40 takvim saatinden ortalama 25 net üretim saati çıkarır. Toplantı, iletişim, idari işler ve olağan bağlam değişimi bu 25in dışında varsayılır. Sprint içindeki özellik testleri/review, iş kartı saatlerine dahildir; plan dışı hata ve belirsizlik ise rezervdedir.

İki kişi × 25 net saat × iki hafta = **100 saat**. Bunun **80 saati iş paketi, 20 saati rezerv**. Ayrı QA/artist saatleri bedava sayılmaz: bu plandaki Art/QA rolleri mevcut toplam kapasiteden karşılanır. Dış destek eklenirse uzmanlık, saat ve bütçesi ayrıca yazılır. Codex ayrı bir tam zamanlı çalışan kabul edilmez; yardımının hız etkisi ancak gerçek sonuçlardan ölçülür.

24 paket × 80 = **1.920 planlı net saat**; 24 × 20 = **480 rezerv saat**; temel tahsis toplamı **2.400 saat**. İki kişi senaryosunda S00–S23 48 haftayı kaplar; yayın öncesi RC S21 sonu 44. hafta, çıkış penceresi S22 45–46, ilk destek S23 47–48dir. Bunlar kesintisiz örnek kapasite hesabıdır; başlama tarihi verilmediği için tarih uydurulmadı.

| Çalışma biçimi | Haftalık toplam net saat | İki haftada planlı iş / rezerv | 80 saatlik bir paketin kapasite karşılığı | 24 paketin teorik taban süresi |
|---|---:|---:|---:|---:|
| İki kişi, kişi başı 25 | 50 | 80 / 20 | 1 sprint / 2 hafta | 48 hafta |
| Solo, 25 | 25 | 40 / 10 | 2 sprint / 4 hafta | 96 hafta |
| Solo, 15 | 15 | 24 / 6 | 3,33 sprint / 6,67 hafta | 160 hafta |
| Solo, 10 | 10 | 16 / 4 | 5 sprint / 10 hafta | 240 hafta |

Solo çalışırken iki haftalık toplantı ritmini koru; S01 gibi 80 saatlik bir çalışma paketini S01-A/S01-B alt sprintlerine böl. İşi sığdırmak için her sprinti dört haftaya uzatman gerekmiyor. Kesirli sprintler yalnız kapasite hesabıdır; gerçek takvimde işler yeniden dilimlenir. Bu süreler sanat uzmanlığı eksikliği, izin, hastalık, harici onay ve kritik yol beklemesini içermeyen taban senaryolardır.

## Yakın ve uzak planın farklı güveni

- S00–S03: uygulamaya en yakın kartlar; sprint başında görevleri 2–8 saatlik alt işlere ayır ve tekrar tahmin et.
- S04–S13: amaç ve kabul kriteri belirli; saatler sprint bütçe tahsisidir. G1den sonra yeniden tahmin edilir.
- S14–S23: kapsam/kalite paketleri; üretim throughputu ve dış test bulguları bilinmediği için tarihler özellikle belirsizdir. Her bina bir sprintte tamamlanacak garantisi verilmez.

Altı iş kartının 6/18/18/16/12/10 saat dağılımı her sprint için ortak planlama zarfıdır; bütün işlerin aynı gerçek eforu gerektirdiği tahmini değildir. Özellikle final platform, animasyon, bina ve ağ sorunlarında kartın alt tahmini bu bütçeyi aşarsa sprint başlamadan böl. Fazla işi rezervin içine saklama; 20 saat rezerv tam özellik üretim bütçesi değildir.

İçerik üretim hesap örneği: onaylı bir hero propın gerçek maliyeti 20 saat çıkarsa, kalan 8 prop en az 160 saat ister; mevcut art tahsisi 54 saatse 106 saatlik açık vardır. Açık ancak iş azaltma, ek sprint veya gerçek dış destekle kapanır. AI var diye sıfır sayılmaz. Üç bina ve on iki görev üst hedefi G3te bu hesapla dondurulur.

## Sprint ritmi

1. Planning: amaç, kapı, kapasite, ilk üç risk, Done kanıtı ve test katılımcısını belirle. İki kişi için 45–60 dakika başlangıç önerisi.
2. Günlük kısa kontrol: dünün kanıtı, bugünün tek işi, blokaj. Solo için 5 dakikalık not yeterli.
3. İlk hafta içinde entegre build: sprint sonuna kadar dalları ayrı büyütme.
4. Ara kontrol: net saat ve kalan iş, risk kapısına göre daraltma.
5. Sprint sonu review: doğrudan playable build; ilgili negatif senaryo; kanıt dosyası.
6. Retrospektif: tahmin/gerçek saat, yeniden açılan hata, bekleme, sonraki küçük süreç düzeltmesi.

Kişi başına bir aktif üretim işi, takımda en fazla iki ana iş önerilir. Blocked kart WIPten görünmez silinmez. Tek kişinin ağ, sanat ve test rollerini aynı anda taşıdığı saatleri paralel sayma. Toplam 80 saatin sığması kritik yolun sığdığı anlamına gelmez; bağımlı işleri planningde günlere dağıt. Eşzamanlı ilerleyebilen sanat sadece kilitlenmiş ölçek/örnek üzerinde ilerler.

## Ready ve Done

Ready: problem, girdi/çıktı, owner, ilgili ASK/REQ/TC, bağımlılık, ölçülebilir kabul, tahmin aralığı ve gereken araç/build erişimi vardır. Kritik teknik bilinmezlik varsa önce zaman kutulu spike açılır.

Done: sınırlı değişiklik, review, ilgili anlamlı test, standalone build smoke, değişen oyun kuralı/asset kaydı ve gerçek kanıt. Kod yazıldı veya belge değişti olması yetmez. Sadece görsel/kolay geri alınabilir değişiklikte uygulamayı taklit eden birim test üretmek yerine sahne incelemesi uygundur. Domain işlemleri, ağ yarışları ve kayıt güvenliğinde test gereklidir.

Kanıt yoksa kart `IMPLEMENTED_NOT_VERIFIED`; erişim yoksa `BLOCKED`; kabul sağlanırsa `DONE`. Yüzde 90 Done sütunu yoktur. Bütün teslimat kartları başlangıçta PLANNEDdır.

## Kapı başarısızlığı

G1 başarısızsa S04 yerine G1-R1 kurtarma sprinti aç. R1 için tek hipotez, tek metrik ve kesilecek kapsam yaz. G2/G3te aynı yöntem geçerlidir. Kurtarma sprintleri S00–S23e eklenir ve sonraki haftalar kayar. İki başarısız kurtarma sonrasında aynı deneyi yinelemek yerine model/kapsam yeniden değerlendirilir. Bu karar otomatik iptal değil, görünür ürün kararıdır.

Daha gerçekçi takvim senaryosu olarak 48 haftalık tabana 4–8 hafta teknik kapı yeniden çalışma ve 2–4 hafta dış bağımlılık payı eklemek 54–60 hafta verir. Bu ayrı bir plan senaryosudur; 480 saatlik iç rezervin aynısını yeniden iş saati olarak toplamıyoruz. Dış bekleme sırasında başka iş yapılabiliyorsa takvim gecikmesinin tamamı eklenmez. Kalan iş / son 3 sprintin doğrulanmış iş hızı ile tahmin her kapıda yenilenir.
