# ASKIDA — Birleşik ayrıntılı sprint planı

Sürüm 0.2.0 · 8 Eylül 2026 · Önerilen plan.

Bu dosya kanonik rehber ve sprint belgelerinin türetilmiş kopyasıdır. Kaynaklarını ayrıca yüklersen tekrar oluşur. Oyun testleri çalıştırılmış değildir.


---

Kaynak: `00_README.md`

# ASKIDA — Ayrıntılı sprint planı v0.2

Tarih: 8 Eylül 2026. Durum: uygulanabilir plan önerisi; görevlerin tamamı PLANNED. Hazırlanan önceki ASKIDA Production Blueprint v0.1 belgeleriyle birlikte kullanılır. Bu teslimat bir Unity projesi oluşturmaz ve hiçbir oyun testinin geçtiğini iddia etmez.

Başlangıç tercihi: **Unity Hub → New project → Universal 3D; Unity 6.3 LTS ailesinin kurulumda doğrulanmış güncel kararlı yaması.** Şablon URP ile önceden yapılandırılmış boş 3D projedir. Resmî ad ve davranış [Unity 6.3 URP kurulum belgesinde](https://docs.unity3d.com/6000.3/Documentation/Manual/urp/creating-a-new-project-with-urp.html) doğrulandı.

Bu planda 24 çalışma paketi S00–S23 bulunur. İki kişi, kişi başına haftada 25 net saat ve iki haftalık sprint varsayımında her sprint 100 net saat kapasiteye sahiptir: 80 saat planlı iş, 20 saat hata/belirsizlik rezervi. S00–S03 yakın dönem ayrıntılı plan; S04–S13 yeniden tahmin edilecek plan; S14–S23 uzak dönem kapasite taslağıdır. Kart sayısı kesin takvim güveni anlamına gelmez.

## Dosyaları hangi sırayla okuyacaksın?

1. [Unity kurulumu](01_UNITY_KURULUMU.md): doğru şablon, paketler, ayarlar ve klasör düzeni.
2. [Kapasite ve çalışma yöntemi](02_KAPASITE_VE_SPRINT_YONTEMI.md): solo/iki kişi zaman hesabı ve iş bitirme kuralları.
3. [Yol haritası](03_YOL_HARITASI.md): 24 sprint ve kapılar.
4. [İlk on iş günü](08_ILK_ON_IS_GUNU.md): ilk sprintte uygulanacak sıra.
5. [S00](sprints/S00.md) ile başla; her sprintin kendi ayrıntılı dosyası var.
6. [Kalite kapıları](04_KALITE_KAPILARI.md) ve [test kitabı](05_TEST_VE_OLCUM_KITABI.md): bir sonraki aşamaya geçme koşulları.
7. [Kararlar ve değişiklikler](06_KARARLAR_VE_BASELINE_FARKLARI.md), [bilgi tabanı kullanımı](07_BILGI_TABANI_VE_GOREV_SABLONU.md), [riskler](09_RISKLER_VE_KAPSAM_KESME.md).

Tek dosya isteyen NotebookLM benzeri araçlar için [ASKIDA_TAM_SPRINT_PLANI.md](ASKIDA_TAM_SPRINT_PLANI.md) aynı belgelerin birleşik kopyasıdır. Kaynak dokümanlarla birleşik kopyayı aynı notebooka birlikte yüklemek tekrar yaratır; birini seç. JSON kayıtları data/ altında; araca aktarılmış, uzakta kurulmuş bir board veya MCP sunucusu değildir.

## Planlanan ürün sınırı

2–4 arkadaş; ilk kişi kamera önerisi; kontrollü yük platformu; aynı eşyayı en fazla iki oyuncuyla tutma; sabitleme; host doğrulamalı teslimat/hasar/para; hosta ait tur sonu kampanya kaydı. Önce bir rota/dört eşyalı slice, sonra Avlu C01–C03 demo. V1 üst kapsamı üç bina, on iki sözleşme ve altı davranış ailesidir; G3te ölçülen üretim hızına göre daraltılabilir.

Tam halat simülasyonu, host migration, dedicated sunucu filosu, split-screen, açık dünya, aktif ragdoll, oyun içi voice chat, konsol ve cross-play mevcut plana dahil değildir. Bunlar otomatik olarak küçük ek özellik sayılmaz. Geliştiricinin Mac kullanması macOS shipping taahhüdü değildir; dağıtım varsayımı Windows PC/Steamdir.

## Teslimatın sınırı

Bu paket önceki 80 kaynak belgenin yerine geçen bir v0.2 blueprint değildir; üretim eki ve ayrıntılı backlogdur. ASK-071/074teki sıkıştırılmış ilk sprintin yerine burada önerilen rezervli S00–S03 dizisi kullanılacaksa bu karar bilgi tabanına kaydedilir. Eski arşiv değiştirilmedi. Oyun sistemi seçimleri onaylanmış gibi etiketlenmedi; mevcut talebin Unity yönü temel alındı.


---

Kaynak: `01_UNITY_KURULUMU.md`

# Unity projesini doğru kurmak

## Doğrudan seçim

| Alan | ASKIDA için başlangıç kararı |
|---|---|
| Editor ailesi | Unity 6.3 LTS / 6000.3; tam patch S00da doğrulanıp kilitlenecek |
| Hub şablonu | **Universal 3D** |
| Render pipeline | URP; seçilen Editorun uyumlu paket seti |
| Oyun yaklaşımı | 3D GameObject + MonoBehaviour + Rigidbody; test edilebilir C# domain katmanı |
| Oyun adı | ASKIDA |
| Yerel klasör | Depo içinde Game; Hub Project Name Game seçilip Product Name ASKIDA yapılabilir |
| Dağıtım varsayımı | Windows x86-64 / Steam; Mac geliştirme ayrı, Mac ürün desteği ayrı karar |
| Ağ önerisi | Listen-server; host bütün paylaşılan fizik ve oyun sonucunun otoritesi |
| İlk ağ adayı | NGO + uyumlu Unity Transport; gerçek internet yolu S03te doğrulanacak |
| Kamera | İlk kişi, varsayılan kamera roll kapalı; konfor testinde değerlendirilecek |
| İlk hedef | İki oyuncu, bir ağır eşya, bir platform; S03te dört oyuncu |

Universal 3D şablonu URP ve 3D renderer ile boş proje açar. Universal 3D sample ise örnek ortamlar içerir; ASKIDAnın üretim tabanına ihtiyaç olmayan örnek sahneleri getirmemek için boş şablon uygundur. [Unity resmî şablon açıklaması](https://docs.unity3d.com/6000.3/Documentation/Manual/urp/creating-a-new-project-with-urp.html).

8 Eylül 2026 kontrolünde Unity 6.3 LTS desteği Aralık 2027ye kadar listeleniyor. Unity, yeni/orta aşamadaki projeler için güncel Update sürümlerini de öneriyor; bunlar da production-ready olarak tanımlanıyor. Burada 6.3 LTS seçimi küçük ekibin sürüm değişimini sınırlama tercihidir; LTSnin her koşulda daha iyi olduğu iddiası değildir. Tam patch, URP ve ağ paketi aynı makinede smoke yapılmadan kilitlenmiş sayılmaz. [Unity sürüm ve destek politikası](https://unity.com/releases/unity-6/support).

## Hub üzerinden adımlar

1. Unity Hubda Installs üzerinden Unity 6.3 LTS ailesinin kurulum günündeki uygun kararlı yamasını seç. Beta/alpha kullanma; sürümün bilinen sorunlarını oku.
2. Geliştirme işletim sisteminle uyumlu Editor ve gerekli build modüllerini kur. Windows dağıtım buildi ve özellikle hedef IL2CPP yolunu erken doğrulayacak Windows makinesi/runner planla.
3. Bilgisayarda ASKIDA adlı depo kökünü oluştur. Hubın eklediği proje alt klasörünü dikkate al: Project Name `Game`, Location `ASKIDA` seçilirse beklenen yol `ASKIDA/Game` olur. Hubda gösterilen tam yolu kontrol et; yanlışlıkla `Game/Game` üretme.
4. Projects → New project → Universal 3D → Create project. Açıldığında Console temizliğini ve URP asset atamasını kontrol et.
5. Player ayarlarında Product Name `ASKIDA`, Company Name gerçek seçtiğin stüdyo/kişi adı olsun. Uydurma yayınevi adı kullanma.
6. Version Control / meta görünürlüğü ve Asset Serialization / Force Text ayarlarını doğrula. Assets dosyalarıyla .meta dosyalarını birlikte sürümle.
7. S00ın minimal klasörlerini ve `SCN_Bootstrap`, `SCN_Frontend`, `SCN_PhysicsLab` sahnelerini oluştur. Tek bir başlangıç sahnesinden servis yaşam döngüsü başlasın.
8. Input Actions oluştur, UI ve gameplay action maplerini ayır. Klavye/fare ilk test; gamepad temeli kaybolmayacak şekilde soyutlanır.
9. İlk yerel buildi al; gerçek Windows dağıtım makinesinde de ilk smoke kaydını üret. Editorda Play tuşuna basılması build kapısını kapatmaz.
10. Tam Editor sürümünü ProjectVersion.txt, bağımlılıkları manifest ve lock ile kaydet. İlk temiz checkout denemesinden sonra S01e geç.

Windows IL2CPP yolunun C++ araç zinciri ve Windows SDK gereksinimleri vardır; kurulumda seçtiğin Editor sürümünün Windows gereksinimlerini doğrula. Mac geliştirme denemesi Windows dağıtım doğrulaması yerine geçmez. [Unity Windows gereksinimleri](https://docs.unity3d.com/6000.3/Documentation/Manual/windows-requirements-and-compatibility.html). Bu bağlantı 6000.3 ailesinin Windows koşullarını gösterir; tam yama ve yerel araç zinciri S00da birlikte kontrol edilir.

## Paket ekleme sırası

| Paket/araç | Ne zaman? | Gerekçe ve sınır |
|---|---|---|
| URP | Şablonla | Render tabanı; elle rastgele daha yeni sürüme yükseltme |
| Input System | S00 | Eylem eşleme ve remap; seçilen Editora uyumlu Released sürüm |
| Unity Test Framework | S00 | Domain ve PlayMode doğrulaması; proje kurulumunda mevcut mu kontrol et |
| NGO + Unity Transport adayı | S00–S03 | Yerel ortak eşya, sonra internet; henüz onaylı prod çözümü değil |
| Multiplayer test/emülasyon araçları | S01–S03 | Ayrı süreçler ve gecikme tanısı; uygun paket sürümünü doğrula |
| Animation Rigging | S09 öncesi küçük spike | El IK ihtiyacı; API/rig uyumu doğrulandıktan sonra ekle |
| Localization | En geç S10 | Başta string key sistemi; S12de TR/EN tam akış |
| Cinemachine | İhtiyaç kanıtlanırsa | Basit ilk kişi kamera için zorunlu değil |
| Addressables | Yükleme/bellek ölçümü gerektirirse | Baştan tüm oyunu bunun üzerine taşımak gerekmiyor |
| UGS Multiplayer Services/Relay veya Steam adapter | S03 adayına göre | Hizmet, kimlik, maliyet ve gerçek internet deneyiyle seçilecek |

Input Systemin kurulum ve aktif backend adımları resmî kılavuzda açıklanıyor. Paket sürümünü bu tablodan tahmin etme; Package Managerdaki uyumluluğu dene ve tam sürümü kilitle. [Input System kurulum kılavuzu](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.17/manual/Installation.html).

NetworkRigidbody tek başına kaliteli co-op taşıma çözümü değildir: NGOda otorite dışındaki örneklerin kinematik oluşu ve NetworkTransform otoritesiyle ilişkisi dikkate alınmalıdır. İki kişinin ortak kuvveti, hareketli referans sistemi ve görsel gecikme sunumu bizim test edeceğimiz oyun davranışıdır. [Unity NetworkRigidbody](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@2.9/manual/components/helper/networkrigidbody.html).

## Başlangıç ayarları — hepsi ölçümle gözden geçirilecek

| Ayar | İlk profil | Karar verme yöntemi |
|---|---|---|
| Ölçek | 1 unit = 1 metre | Referans küp/kapı/oyuncu/eşya aynı sahnede kontrol |
| Fixed Timestep | 0.02 s / 50 Hz | ASK-040la tutarlı; 60 FPS render ile aynı olmak zorunda değil |
| Physics catch-up | Editor başlangıcı kayıt altına alınır | Host stres ölçümünden önce körlemesine düşürme/yükseltme yok |
| Ağ tick/snapshot | İlk spike profili; exact değer kayıtlı | Physics tickten ayrı karar; gecikme, correction ve trafikle seç |
| URP renderer | Şablonun mevcut yolu önce kaydedilir | İlk sade sahnede gereksiz renderer değişimi yapma |
| Işık/gölge | Basit gün ışığı + kontrollü dolgu | Gerçek zamanlı ışık ve uzak gölgeyi ölçüme göre sınırla |
| Post-process | Sınırlı; motion blur başlangıçta kapalı | İlk kişi etkileşim okunurluğu ve konfor |
| Kalite | İlk Low/High profili | Gameplay collider ve zorunlu içerik kaliteyle değişmez |
| Dinamik eşya | Bir root Rigidbody, compound primitive collider | CCD/interpolation seçenekleri eşya ve ağ temsiline göre ölçülür |
| Katmanlar | WorldStatic, Player, Carryable, Platform, Trigger, Cosmetic | Collision matrix S00da yazılı; player-player S01/02 test kararı |

Fixed timestep fizik adım aralığıdır; render kare süresine eşitlenmesi zorunlu değildir. Timestep ve maksimum catch-up maliyeti ölçülür. [Unity Time ayarları](https://docs.unity3d.com/6000.3/Documentation/Manual/class-TimeManager.html).

## Dosya hiyerarşisi

| Depo yolu | İçerik/sorumluluk |
|---|---|
| `Game/Assets/_Askida/Runtime/Core` | Saf durum/kimlik/işlem altyapısı |
| `Game/Assets/_Askida/Runtime/Gameplay` | Carrying, Platform, Delivery, Contracts, Recovery |
| `Game/Assets/_Askida/Runtime/Network` | Otorite, komut doğrulama, snapshot, session adapters |
| `Game/Assets/_Askida/Runtime/UI` | Menüler ve host stateinden okuyan sunum |
| `Game/Assets/_Askida/Runtime/Audio` | Event sesleri ve loop yaşam döngüsü |
| `Game/Assets/_Askida/Runtime/Presentation` | Görsel el, animasyon, kozmetik durum |
| `Game/Assets/_Askida/Editor` | Validation, Import, Build |
| `Game/Assets/_Askida/Tests` | EditMode, PlayMode, Integration |
| `Game/Assets/_Askida/Art` | Meshes, Materials, Textures, Animations, VFX |
| `Game/Assets/_Askida/Prefabs` | Players, Items, Platform, Environment, UI |
| `Game/Assets/_Askida/Data` | Items, Contracts, Levels, Profiles, Localization |
| `Game/Assets/_Askida/Scenes` | Bootstrap, Frontend, Hub, Levels, Validation |
| `Game/Assets/ThirdParty` | Üretici/paket/sürüm bazında ham paket; project-owned türev ayrı |
| `Game/Packages` | manifest.json, packages-lock.json |
| `Game/ProjectSettings` | Paylaşılan Editor/proje ayarları |
| `SourceArt` | Blender/Textures/Audio/Exports/Provenance; doğrudan oyun importu değil |
| `Docs/ASKIDA` | Önceki 80 belgelik kanonik bilgi tabanı |
| `Docs/ASKIDA_Sprints` | Bu ek planın kanonik dosyaları |
| `Tools` | AssetBuild, Validation, Release yardımcıları |
| `Builds` | Üretilmiş buildler; normal kaynak Git takibinin dışında |

`Game/Library`, `Temp`, `Obj`, `Logs`, `UserSettings` ve üretilmiş buildler commit edilmez. `.meta` dosyaları silinip yeniden üretilmez. Büyük binary kaynaklar için LFS kota/erişim planı değerlendirilir; metin belgeler LFSye alınmaz. Asset Store paketlerini ham kaynakta düzenlemek yerine _Askida altında türev prefab kullan.

S00da her alan için boş sınıf yazmak gerekmiyor. Core, Gameplay, Network, Presentation, Editor ve test assembly sınırları ihtiyaç kadar kurulmalı. Domain ağ kütüphanesine bağımlı olmayacak; görsel elin animasyonu Grab sonucuna karar vermeyecek. Bu sınırlar production kalitesini destekler; klasör sayısı tek başına kalite ölçütü değildir.


---

Kaynak: `02_KAPASITE_VE_SPRINT_YONTEMI.md`

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


---

Kaynak: `03_YOL_HARITASI.md`

# Sprint yol haritası

İki kişilik örnek kapasite: her sprint iki hafta, 80 planlı + 20 rezerv net saat. Haftalar başlangıca göredir. Bağımlılık veya kapı başarısızsa sonraki haftalar kayar; S14 sonrası tarihler özellikle yeniden tahmin edilecektir.

| Sprint | Örnek hafta | Amaç | Kapı |
|---|---|---|---|
| [S00](sprints/S00.md) | 1–2 | Unity kurulumu ve tekrar üretilebilir temel | G0 — planlama tabanı; ağ uygunluğu henüz açık |
| [S01](sprints/S01.md) | 3–4 | İki oyuncunun aynı eşyayı taşıması | Sprint kabulü |
| [S02](sprints/S02.md) | 5–6 | Hareketli platform, denge ve vinç | Sprint kabulü |
| [S03](sprints/S03.md) | 7–8 | Gerçek internet ve teknik risk kapısı | G1 — teknik risk |
| [S04](sprints/S04.md) | 9–10 | Oyun semantiği ve üretim mimarisi | Sprint kabulü |
| [S05](sprints/S05.md) | 11–12 | Sabitleme, hasar ve kurtarma | Sprint kabulü |
| [S06](sprints/S06.md) | 13–14 | Teslimat, tur sonucu ve güvenli kayıt | Sprint kabulü |
| [S07](sprints/S07.md) | 15–16 | Lobi ve oturum dayanıklılığı | Sprint kabulü |
| [S08](sprints/S08.md) | 17–18 | Ortak görsel dil ve örnek asset üretim hattı | Sprint kabulü |
| [S09](sprints/S09.md) | 19–20 | Karakter, el teması ve sesli geri bildirim | Sprint kabulü |
| [S10](sprints/S10.md) | 21–22 | Avlu rotası, dört eşya ve temel arayüz | Sprint kabulü |
| [S11](sprints/S11.md) | 23–24 | Vertical slice kalite kapısı | G2 — vertical slice |
| [S12](sprints/S12.md) | 25–26 | Öğretici ve üç sözleşmelik demo | Sprint kabulü |
| [S13](sprints/S13.md) | 27–28 | Dış demo testi ve kapsam kararı | G3 — dış demo |
| [S14](sprints/S14.md) | 29–30 | İçerik üretim araçları ve eşya aileleri | Sprint kabulü |
| [S15](sprints/S15.md) | 31–32 | Sokak binası ve C05–C08 | Sprint kabulü |
| [S16](sprints/S16.md) | 33–34 | Teras binası ve C09–C12 | Sprint kabulü |
| [S17](sprints/S17.md) | 35–36 | İlerleme dengesi ve alpha içerik kilidi | G4 — content complete / alpha |
| [S18](sprints/S18.md) | 37–38 | Erişilebilirlik ve son kullanıcı deneyimi | Sprint kabulü |
| [S19](sprints/S19.md) | 39–40 | Ölçüme dayalı optimizasyon | Sprint kabulü |
| [S20](sprints/S20.md) | 41–42 | Beta: ağ, kayıt ve tam regresyon | Sprint kabulü |
| [S21](sprints/S21.md) | 43–44 | Release candidate ve dağıtım provası | G5 — release candidate |
| [S22](sprints/S22.md) | 45–46 | Yayın hazırlığı ve kontrollü çıkış | Sprint kabulü |
| [S23](sprints/S23.md) | 47–48 | Yayın sonrası istikrar ve sonraki plan | Sprint kabulü |

## Aşamaların ürün çıktıları

| Paketler | Teslim edilen ürün seviyesi | Yeni içerik için koşul |
|---|---|---|
| S00–S03 | Gri ortamda gerçek internet üzerinden iki/dört kişilik platform ve ortak taşıma | G1 |
| S04–S07 | Tutarlı oyun statei, kayış/hasar/recovery, kayıt ve dayanıklı oturum | İlgili semantik/kayıt testleri |
| S08–S11 | Tek Avlu rotası, dört eşya örneği, final hedefli art/animasyon/UI/ses | G2 |
| S12–S13 | Avlu C01–C03 demo, onboarding ve dış gözlem | G3 ve ölçülen kapsam |
| S14–S17 | Onaylı içerik envanteri, diğer binalar/görevler, ilerleme | G4 |
| S18–S21 | UX, optimizasyon, beta, release candidate | G5 |
| S22–S23 | Karara bağlı yayın ve ilk destek | Gerçek yayın yetkisi ve artifact smoke |

G1 geçmeden sanat sadece yeniden kullanılabilir ölçü/stil örnekleri düzeyinde kalır. G2den önce geniş model koleksiyonu üretilmez. G3te v1 üst kapsamı dondurulur. G4ten sonra yeni özellik yerine kalite işi yapılır.

## Yakın iş ile uzak tahmini ayırma

S00–S03te kartlar doğrudan alt işlere bölünebilir. S04–S13 sprint girişinde yeniden tahmin edilir. S14–S23te önceki kapılardaki gerçek içerik hızı görülmeden bina/görev adetleri taahhüt edilmez. Her altı kartlık paket bir kapasite zarfıdır; özellikle bina üretimi iki veya daha fazla sprinte bölünebilir.

Orijinal W-001–W-032 işleri bu pakette iz olarak tutulmuştur. W-031 canlı MCP prototipi isteğe bağlı ayrı çalışma olup kritik oyun yolundan saat çalmaz. Yeni SP kartlarıyla eski W tahminlerini toplamayın.


---

Kaynak: `04_KALITE_KAPILARI.md`

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


---

Kaynak: `05_TEST_VE_OLCUM_KITABI.md`

# Test kitabı ve ölçüm protokolü

Bu dosyadaki tüm senaryolar planlıdır. Blueprintte REQ-001–040 ile TC-001–040 ilişkisi korunur. Yeni SP iş kartları ilgili REQlara bağlanır; var olan TC adımları orijinal data/test_cases.jsondan okunur. Aşağıdaki PTEST kimlikleri ek üretim senaryolarıdır; eski TC kimliklerini yeniden numaralandırmaz.

## Çalıştırılabilir senaryolar

| Kimlik / ilk sprint | Kurulum ve işlem | Beklenen sonuç | Kanıt |
|---|---|---|---|
| PTEST-01 / S00 | Temiz checkout; belgeli Editor; bağımlılık çözümü; standalone build | Eksik GUID/script/shader yok; build açılır | Commit, Editor, build log |
| PTEST-02 / S01 | Aynı eşyaya iki eşzamanlı Grab, üçüncü Grab, duplicate Release; 50 döngü | En fazla iki holder; birinin releasei diğerinin hakkını silmez; döngü sonu sıfır joint | State/joint sayacı + video |
| PTEST-03 / S02 | Eşit karşı ağırlık, çok collider eşya, zıplayan oyuncu ve deckten ayrılan yük | Kütle bir kez; eşit yük dengeli; histerezis titremeyi sınırlar | SupportedSet listesi + denge eğrisi |
| PTEST-04 / S03 | 2 ve 4 oyuncu, farklı ağlar; platformda taşıma; 150 ms RTT/%1 loss | Tur tamamlanır, state corruption olmaz; görsel hata hedefiyle karşılaştırılır | Ağ profili + host/client capture |
| PTEST-05 / S03/S07 | Holder veya driver clientı kapat; grace içinde aynı kimlikle dön | Haklar hostun kopmayı saptaması sonrası temiz; güvenli spawn/snapshot; eski hak geri verilmez | Zaman damgalı event izi |
| PTEST-06 / S04 | Yasak lifecycle/handling kombinasyonları; joint create başarısızlığı | İşlem reddi veya rollback; yarım durum yok | EditMode sonuçları |
| PTEST-07 / S05 | Aynı anchora iki secure; hareketliyken unsecure; 100 bağla/çöz | Bir rezervasyon; güvensiz çözme ret; sızıntısız son state | Anchor sayacı ve PlayMode kayıt |
| PTEST-08 / S05 | Aynı darbe çok colliderla; sonra integrity zero; ardından recovery | Darbe çoğalmaz; Lost terminal; bağlantı temiz; recovery reddi | Beklenen/gerçek integrity |
| PTEST-09 / S06 | Aynı itemi teslim sınırında sallandır; aynı run sonucunu 10 kez uygula | Tek ledger kaydı/tek ödül; koşulsuz erken teslim yok | Delivery ledger ve bakiye |
| PTEST-10 / S06/S20 | Kayıt öncesi/ortası/sonrasında fault; bozuk primary/geçerli backup; eski şema | Son sağlam checkpoint veya açık hata; desteklenen migration; çift kazanç yok | Fixture + beklenen state |
| PTEST-11 / S07 | Mismatch build/content; dolu lobi; late packet; 20 join/load/leave döngüsü | Açık ret/timeout; çift avatar/manager yok | ReasonCode ve kaynak sayacı |
| PTEST-12 / S08 | Balkon/platform/kanepe/karakter tek ışıkta; yakın ve uzak; Low/High | Ortak biçim/renk/ölçek, tutma okunur, collider doğru | Dört sabit kamera screenshotu |
| PTEST-13 / S09 | IK/Animator kapalı açık, release/disconnect; dört karakter | Gameplay aynı; hayalet el/ses yok; maliyet kayıtlı | Video + CPU capture |
| PTEST-14 / S10/S15–17 | Her mandatory eşyayı her görevde 2/3/4 oyuncuyla taşı | Geometrik geçiş ve iki kişi tamamlanabilirlik | Görev/roster/sonuç tablosu |
| PTEST-15 / S12/S18 | TR/EN, büyük metin, remap, klavye/gamepad, ses kapalı | Menüden çıkış ve görev bilgisi erişilir; metin taşmaz | Ekran ve input matrisi |
| PTEST-16 / S11/S19 | Referans cihazda ısınma sonrası aynı ağır sahnede 10 dk capture | Dondurulmuş CPU/GPU p95/p99 bütçesine kıyas | Ham capture, ayar ve özet |
| PTEST-17 / S19/S20 | Dört oyuncu 2 saat, 20 sahne/tur döngüsü; kopma ekle | Sürekli bellek/joint/listener artışı, crash veya corruption yok | Başlangıç/son RAM ve sayaç |
| PTEST-18 / S21 | Temiz RC install, önceki kayıt, update, rollback provası | Doğru artifact; kayıt uyumluluğu veya güvenli forward-fix kararı | Artifact hash + release smoke |

## Ağ testi profilleri

| Profil | RTT hedefi | Jitter | Paket kaybı | Kullanım |
|---|---:|---:|---:|---|
| N0 | Ölçülen yerel taban | Ölçülen | 0 ek kayıp | Hızlı teşhis; internet kanıtı değil |
| N1 | 50 ms | 10 ms | %0 | İyi bağlantı referansı |
| N2 | 100 ms | 10 ms | %1 | Olağan co-op hedef örneği |
| N3 | 150 ms | 30 ms | %1 | G1 hedef zorlayıcı profil |
| N4 | 250 ms | 50 ms | %3 | Güvenli bozulma/stres; akıcılık vaadi değil |

Emülatör tek yön gecikme istiyorsa gidiş ve dönüş toplamını RTTye dönüştür. Jitterın aracın hangi dağılım/genişlik tanımı olduğunu kaydet. Gerçek ağın doğal RTTsine eklenen gecikmeyi toplam hedefle karıştırma. Paket kaybı rastgele mi burst mü belirtilir; N4te en az bir kısa burst/bağlantı kesme ayrıca denenir. Bu profiller internet hizmet garantisi değildir.

2 oyuncu = 1 host + 1 client. 4 oyuncu = 1 host + 3 client. S03 kapı testinde dört oyuncunun ayrı süreç ve makinelerde, en az iki bağımsız internet bağlantısında çalışması hedeflenir. Tek makinedeki dört süreç yalnız kapasite/ön doğrulama sağlar. Katılımcı veya hizmet erişimi yoksa ilgili matris satırı NOT_RUN/BLOCKED kalır.

## Ölçülebilir başlangıç eşikleri

Aşağıdaki sayılar ürün hipotezidir; ölçülmüş sonuç veya evrensel fizik standardı değildir. S01/S02de ilk ölçümlerden sonra, G1 testinden önce ADRye yazılarak dondurulur. Uygunsuz eşik seçilmişse yeni sürüm/tarih ve gerekçe ile değiştirilir; test geçmişi silinmez.

| Metrik | Önerilen ilk kabul | Ölçüm tanımı |
|---|---|---|
| State değişmezi ihlali | 0 | Yasak lifecycle/handling/holder/anchor kombinasyonu |
| Çift teslimat/ödül | 0 | Aynı item/run işlem kimliğinde ikinci kalıcı yan etki |
| NaN / sınırsız hız / kalıcı joint | 0 | Planlı 10 dk risk senaryosu içinde |
| Kopma sonrası cleanup | Hostun kopmayı saptamasından sonra en geç 2 authoritative tick | Bağlantı timeout süresi ayrı ölçülür; kablo çekme anından 2 tick vaat edilmez |
| Platform üzerinde görsel göreli konum hatası | N3te p95 ≤ 0,15 m önerisi | Aynı render snapshot zamanına örneklenmiş authoritative local pose ile görüntülenen platform-local pose farkı; intentional input hareketi ayrı |
| Büyük görsel düzeltme | >0,30 m düzeltme en fazla 2/dk önerisi | Respawn/recovery gibi bilinçli yer değiştirme ayrı olay; ham video ile konfor denetimi de gerekir |
| 1080p/60 frame bütçesi | CPU ve GPU p95 ayrı ayrı ≤16,67 ms; p99 ≤33,3 ms ilk öneri | Belirlenen cihaz/kalite ve 10 dk steady gameplay; yükleme ayrı tutulur; CPU/GPU toplanmaz |
| Uzun oturum bellek | Isınma sonrası açıklanamayan sürekli artış olmamalı | 20 turda unload/cleanup sonrası eşdeğer sahne karşılaştırması; cache plato yapabilir |
| Mutlak RAM/VRAM, trafik, yükleme | S00 cihaz ve S03/S11 capture sonrası dondurulur | Ölçüm olmadan evrensel MB/kbps limiti uydurulmaz |

Görsel poz hatasını ölçerken ham host now ile geciktirilmiş render poseu kıyaslama; bu, bilinçli interpolation gecikmesini hata diye sayar. Görsel kalite için zaman uyumlu hata yanında inputtan ilk görsel tepkiye kadar süre ve oyuncu konforu ayrıca ölçülür. İyi interpolation sayısı kötü hissetmeyi tek başına aklamaz.

## Performans protokolü

Releasee yakın standalone build kullan. Development/Profiler capture ile darboğazı bul; profiler bağlı olmayan buildde frame sonucu ayrıca doğrula. Donanım, OS, GPU driver, güç modu, çözünürlük, kalite, VSync/FPS limit, backend ve oyuncu sayısını kaydet. Host shared fizik/ağ/render taşıdığı için host ve remote istemciyi ayrı raporla. Bir dakikalık ısınmanın ardından 10 dakikalık sabit senaryo başlangıç önerisidir; termal durum stabil değilse bunu kaydet.

CPU main/render thread, GPU, fizik adımı, aktif body/contact/joint sayısı, GC, peak RAM/VRAM ve trafik örneklenir. Ağ trafiğinde uygulama payloadı ile protokol/relay overheadını karıştırma; ölçüm hangi katmansa yaz. Görsel kalite azaltmak oyun kuralını veya colliderı değiştiremez.

## Kanıt kaydı şablonu

```json
{
  "test_id": "PTEST-04",
  "sprint_id": "S03",
  "status": "NOT_RUN",
  "build_id": null,
  "commit": null,
  "editor_version": null,
  "package_lock_hash": null,
  "hardware": null,
  "network_profile": "N3",
  "roster_count": 4,
  "duration_seconds": null,
  "expected": "state invariants preserved; run completes",
  "actual": null,
  "metrics_path": null,
  "video_path": null,
  "issues": [],
  "reviewer": null
}
```

Testin otomatik yapılabilmesi kaliteli olduğu anlamına gelmez. Domain ve kayıt testleri otomatize edilir; gerçek internet, oyun hissi, görsel uyum ve kullanıcı anlama testi gerçek oynanış kanıtı ister. Bu paketin JSON/bağlantı doğrulaması PTEST senaryolarını çalıştırmış sayılmaz.


---

Kaynak: `06_KARARLAR_VE_BASELINE_FARKLARI.md`

# Kararlar ve önceki planla farklar

## v0.1 belgeleriyle ilişki

ASK-071 genel kapasite senaryolarını, ASK-074 ilk iki sprint taslağını, ASK-026 iki haftalık network risk spikeını tanımlıyordu. Bu yeni plan o deneyi **kurulumdan başlayarak S00–S03e yayılan dört çalışma paketi** olarak ayrıntılandırıyor. Önceki iki kişilik 100 saatlik spike bütün rezervi tüketiyordu. Yeni dizi 320 saat planlı üretim + 80 saat rezerv içeriyor; kapsamına tekrar üretilebilir build, temel input/karakter, taşıma, platform ve gerçek internet karar kanıtı giriyor. İki ayrı tahmini aynı anda yürürlükte sayma.

S00–S03ün daha geniş tutulması zorunlu olarak sekiz hafta dolmasını beklemek anlamına gelmez. Hazır altyapı varsa kabul kanıtıyla birleşebilir; hız varsayımı işi Done yapmaz. Bu ek plan uygulanacaksa ASK-026/071/074e v0.2 planına yönlendirme ve tahmin değişikliği kaydı eklenmesi önerilir. Bu teslimatta eski dosyaların kimliği veya içeriği değiştirilmedi.

ASK-069 kapı adları G0–G5 korunuyor; kanıtların hangi sprintte toplanacağı netleşti. ASK-007deki 3 bina/12 sözleşme/6 aile üst hedefi korunuyor ama S13 G3 kararıyla dondurulacak. ASK-040ın 50 Hz fizik önerisi aynen korundu. Eşya statei, destek kümesi, sabitleme ve host-loss semantiği değiştirilmedi.

## Karar durumu

| Karar | Bu planın durumu | Kapanış kanıtı |
|---|---|---|
| Unity motor yönü | Kullanıcının mevcut talebine göre çalışma temeli | Yeni motor karşılaştırması gerekmiyor |
| Universal 3D / URP | Önerilen doğrudan proje şablonu | S00 import/build |
| 6.3 LTS exact patch | Aile önerildi; tam patch açık | S00 paket/build matrisi; G1de teknik uygunluk |
| NGO/transport | Aday; onaylı production bağımlılığı değil | S03 gerçek internet ve ortak fizik |
| İlk kişi kamera | Önceki taslak korunuyor | S09/S11 konfor ve yük okunurluğu |
| Kontrollü platform | Önceki taslak korunuyor | S02/S03 fizik ve ağ |
| Quaternius tabanı | Önceki aday korunuyor | S08 ortak sahne kabulü |
| Ekip/haftalık saat | İki kişi örnek senaryo | Burakın gerçek kapasitesi; plan bölünür |
| Windows/1080p60/minimum cihaz | Hedef hipotezi; cihaz açık | S00 gerçek cihaz, S11/S19 ölçüm |
| 3 bina/12 sözleşme/6 aile | Üst kapsam | S13 throughput + test, S17 registry |
| Harcama, hizmet, Steam yayını | Bu plan harcama/yayın yapmıyor | İlgili işlem zamanında somut ürün kararı |
| Canlı MCP | Temel sprint yolunun dışında | Ayrı bounded görev; okuma kapsamı/yetki testi |

Bu dosya kullanıcıdan bütün kararları önceden onaylamasını isteyen bir blokaj listesi değildir. Mevcut bilgiyle plan hazırlanmıştır. S00da gerçek kapasite ve cihazı yazıp sonraki iki haftayı boyutlandırmak yeterli ilk adımdır. Paket ve bağlantı kararları kendi deney kapısında kapanır.

## İncelenen kaynaklar

8 Eylül 2026da önceki arşivin README/AGENTS, kapsam ve karar belgeleri; oyun state/platform/anchor/reconnect sözleşmeleri; network spike, Unity repo/fizik, kalite/kapasite/backlog ve gereksinim kayıtları okundu. Dış teknik bilgiler için aşağıdaki birincil kaynaklar kullanıldı; paketlerin Unityde çalıştığı bu oturumda test edilmedi.

| Kaynak | Doğrulanan konu |
|---|---|
| [Unity sürüm desteği](https://unity.com/releases/unity-6/support) | LTS/Update politikası, 6.3 destek aralığı |
| [Unity 6.3 URP kurulumu](https://docs.unity3d.com/6000.3/Documentation/Manual/urp/creating-a-new-project-with-urp.html) | Universal 3D boş URP şablonu |
| [NetworkRigidbody](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@2.9/manual/components/helper/networkrigidbody.html) | Otorite ve kinematik örnek sınırı |
| [Time ayarları](https://docs.unity3d.com/6000.3/Documentation/Manual/class-TimeManager.html) | Fizik timestep/catch-up ayarları |
| [Input System kurulumu](https://docs.unity3d.com/Packages/com.unity.inputsystem@1.17/manual/Installation.html) | Kurulum/aktivasyon; exact sürüm önerisi değildir |
| [Windows gereksinimleri](https://docs.unity3d.com/6000.3/Documentation/Manual/windows-requirements-and-compatibility.html) | 6000.3 Windows/IL2CPP araç zinciri; yerel smoke gerekli |

Planın saatleri, sayısal kabul eşikleri, takvimi ve ürün tercihleri bu kaynakların vaatleri değildir; ASKIDA için önerilen üretim kararlarıdır. Asset fiyatları, Steam bekleme süreleri veya hizmet kotaları güncelmiş gibi yeniden aktarılmadı; ilgili sprintte doğrulanacak.


---

Kaynak: `07_BILGI_TABANI_VE_GOREV_SABLONU.md`

# Bilgi tabanı, sprint kartları ve Codex görev akışı

## Tek kaynak ve kimlikler

Önceki 80 belge `Docs/ASKIDA` altında, bu paket `Docs/ASKIDA_Sprints` altında tutulabilir. Eski ASK/REQ/TC/W kimlikleri değişmez. Yeni iş kimlikleri `SP-00-01` biçimindedir; S00ın birinci kartıdır. `W-003` ile `SP-01-02` aynı işin birebir yeniden adlandırılması değildir. Eski W işleri üst kapsam izi, SP kartları yeni çalışma dilimleridir. Eski ve yeni tahminleri toplayarak iki kez efor sayma.

`data/sprint_backlog.json` bütün sprintleri, kartları, saat tahsislerini, bağımlılıkları ve gereksinim referanslarını içerir. Her kartın status/actual_hours/evidence alanı vardır. `data/baseline_reference_index.json` önceki belgelerin kimlik/yol ve gereksinim başlıklarını taşır; kaynak belgelerin yerine geçen özet değildir. `data/manifest.json` paketteki kanonik dosyaların SHA-256 kayıtlarını içerir. `data/validation_report.json` yalnız bu planın yapısal kontrolüdür.

Kartların source_docs/requirements/test_case_refs alanları sprintten miras alınır; bunlar inceleme kümesidir. Gerçek bir geliştirme görevini başlatırken o kartla doğrudan ilişkili alt kümeyi seç. Gerekli yeni davranış için eski REQ yoksa yeni kimlik ve test ekle; ilgisiz mevcut gereksinime zorla bağlama.

## Notebook kullanımı

Tek uzun kaynak isteyen araca `ASKIDA_TAM_SPRINT_PLANI.md` yüklenebilir. Daha küçük kaynak isteyen araca ilk on rehber belgesi ve o anki sprint dosyaları yüklenebilir. Kanonik sprint dosyası ve birleşik dosyayı aynı koleksiyona birlikte yüklemek alıntıyı tekrar ettirir. Asıl oyun semantiği için önceki Blueprint belgeleri de gereklidir; sprint planı onların yerine geçmez.

Sürüm güncellenince türetilmiş birleşik kopyayı yeniden oluştur; eski notebook kaynağını sürüm etiketiyle değiştir. Bir araçta bu dosyaların gerçekten import edildiğini bu teslimat iddia etmez. Burada yerel dosyalar ve makine tarafından okunabilir kayıtlar hazırlanmıştır.

## İleride salt okunur bilgi erişimi

Önceki ASK-078 ve W-031 ayrı iş olarak kalır. Canlı MCP sunucusu bu oyun roadmapinin kritik yolunda değildir. Amaç gerektiğinde `doc_id`, sprint, REQ veya terimle ilgili kaynağı okuyabilmek; oyun kodu deposuna ve hesaplara sınırsız erişim açmak değildir. Sunucu yapılırsa kanonik kaynak sürümü, path sınırı, arama/read ve hata sözleşmeleri ayrıca test edilir. Bu planda kurulu sunucu veya bağlanmış NotebookLM/Codex entegrasyonu yoktur.

## Tek iş için görev örneği

```text
Proje: ASKIDA
Aktif sprint: S01
İş: SP-01-03 — İkinci holder ve eşzamanlı talepler

Önce oku:
- Depo kökündeki çalışma kuralları
- Docs/ASKIDA/README.md ve görevle ilgili AGENTS.md
- ASK-015, ASK-016, ASK-017, ASK-027, ASK-028, ASK-040
- REQ-003, REQ-004, REQ-009 ve ilgili TC kayıtları
- Docs/ASKIDA_Sprints/sprints/S01.md

Amaç:
İki oyuncunun aynı eşyaya tutma niyetini host doğrulasın;
aynı eşya en fazla iki holder taşısın; üçüncü istek reddedilsin.
Birinin bırakması diğerinin tutma hakkını silmesin.

Bağımlılıklar:
SP-01-01 kural tabanı ve SP-01-02 tek holder servo davranışı hazır olmalı.

Kabul:
Eşzamanlı iki Grab kabul; üçüncü ret.
Duplicate/stale Release yeni veya başka holderı etkilemez.
Tutarken kopma sonrası kalan holder devam eder.
50 tut/bırak döngüsünde kalan joint yok.

İstenen çıktı:
İlgili kod değişikliği, anlamlı state/PlayMode doğrulaması,
çalıştırılan build/testin gerçek sonucu, değişen belge ve açık risk.
Unity çalıştırılamıyorsa NOT_RUN yaz; test geçmiş gibi raporlama.
```

Bu bir görev şablonudur; mevcut oturumda oyunun yazılmasını veya uzak servis kurulmasını istemez. Burak uygulamaya geçtiğinde gerçek depo, sürüm ve kanıt yollarıyla doldurulmalıdır.

## Sprint review kaydı

Her reviewda sprint amacı; planlı/gerçek saat; Done kartlar; tamamlanmayan neden; build kimliği; test sonuçları; önemli hata; görsel kalite kararı; ölçüm değişimi; kapı kararı; sonraki kapasite yazılır. Mevcut belgelerle çelişen davranış varsa etkilenen ASK kimliklerini açıkça göster. Yeni planlamayı onaylı eski oyun kuralını sessizce değiştirmenin aracı yapma.


---

Kaynak: `08_ILK_ON_IS_GUNU.md`

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


---

Kaynak: `09_RISKLER_VE_KAPSAM_KESME.md`

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


---

Kaynak: `sprints/S00.md`

# S00 — Unity kurulumu ve tekrar üretilebilir temel

Durum: **PLANNED**. Örnek takvim: hafta 1–2. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yakın dönem plan; planningde alt iş tahmini yapılacak.

**Sprint amacı:** Boş URP projesinden iki ayrı süreçte açılan host/client testine ve temiz build yoluna ulaşmak.

**Giriş bağımlılığı:** Başlangıç; ekip/araç erişimi kontrolü. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** Temiz checkout açılır; Bootstrap → Frontend → Validation geçişi gösterilir; host ve bir client bağlanır.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-00-01 | Burak/Product | 6 | Kapasite ve teknik karar kaydı |
| SP-00-02 | Tech | 18 | Universal 3D projesi ve depo |
| SP-00-03 | Network | 18 | İlk host ve client iskeleti |
| SP-00-04 | Gameplay | 16 | Gri test odası ve input |
| SP-00-05 | Build/QA | 12 | Development ve hedef backend smoke |
| SP-00-06 | QA/Producer | 10 | Temel kanıt ve sonraki sprint hazırlığı |

### SP-00-01 — Kapasite ve teknik karar kaydı

**Sorumlu:** Burak/Product. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** Haftalık gerçek saat, geliştirme işletim sistemi, iki test makinesi, minimum cihaz adayı ve Windows dağıtım hedefini kaydet. Unity 6.3 LTS yama adayını ve paket listesini ADR-ENGINE taslağına yaz.

**Kartın kabulü:** Ekip/cihaz/sürüm alanları dolu; bilinmeyenlerin sahibi ve kapanış sprinti var.

**Kart bağımlılığı:** Sprint giriş koşulları; önceki kart yok..

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-00-02 — Universal 3D projesi ve depo

**Sorumlu:** Tech. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Game altında Universal 3D oluştur. Assets/ProjectSettings/Packages ve .meta dosyalarını sürümle; üretilen klasörleri dışla. Runtime, Editor, Tests ayrımını ve ilk Bootstrap sahnesini kur.

**Kartın kabulü:** Başka klasöre temiz checkout aynı Editor ile açılır; eksik script/GUID veya pembe materyal yok.

**Kart bağımlılığı:** SP-00-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-00-03 — İlk host ve client iskeleti

**Sorumlu:** Network. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** NGO + uyumlu Unity Transport adayını yerel deneyde kur; bağlantı, spawn ve ayrılma ekranını ekle. Paylaşılan tek küpü yalnız host fizik adımı sürsün.

**Kartın kabulü:** İki bağımsız süreç aynı küp durumunu görür; client doğrudan authoritative state yazamaz.

**Kart bağımlılığı:** SP-00-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-00-04 — Gri test odası ve input

**Sorumlu:** Gameplay. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Bir metre ölçü referansı, zemin, duvar, kapı açıklığı, oyuncu kapsülü ve kamera ekle. Input Actions ile Move/Look/Interact/Release/Menu ayır.

**Kartın kabulü:** Oyuncu yürür ve bakar; UI açıkken gameplay input kapalıdır; remote kamera yerel oyuncuya bağlanmaz.

**Kart bağımlılığı:** SP-00-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-00-05 — Development ve hedef backend smoke

**Sorumlu:** Build/QA. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** İlk Development build al; sürüm/commit logla. Hedef Windows build yolunu dene; IL2CPP seçilecekse uygun Windows derleme makinesinde boş build smoke yap.

**Kartın kabulü:** Editor dışında build açılır; backend veya Windows makinesi engeli kayıtlıdır; macOS testi Windows kanıtı diye sunulmaz.

**Kart bağımlılığı:** SP-00-02, SP-00-03, SP-00-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-00-06 — Temel kanıt ve sonraki sprint hazırlığı

**Sorumlu:** QA/Producer. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Setup adımlarını ikinci kez uygula. Kısa video, Console çıktısı, paket lock ve açık riskleri sakla. S01 kartlarını gerçek süreyle yeniden boyutlandır.

**Kartın kabulü:** G0 kararı ve temel smoke kaydı var; S01 için iki süreç ve ortak eşya hazır.

**Kart bağımlılığı:** SP-00-01, SP-00-02, SP-00-03, SP-00-04, SP-00-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- Bir temiz checkout ve bir standalone build gerçekten denenmiş olmalı.
- Host/client bağlantısı iki ayrı süreçte kurulmalı; bu yerel bağlantı testi internet kanıtı sayılmaz.
- Tam Editor yaması ve paket sürümleri dosyaya yazılmalı; latest ifadesi bağımlılık olamaz.

## Risk ve kapsam kararı

**En önemli risk:** Editor veya paket uyumsuzluğu yüzünden ilk fizik denemesine ulaşamamak.

**Kapasite yetmezse:** Özel shader, bütün asset paketleri, tam CI sistemi ve nihai karakter modeli bu sprintin kapsamına girmez.

**Kapı:** G0 — planlama tabanı; ağ uygunluğu henüz açık

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-009, ASK-026, ASK-036, ASK-037, ASK-038, ASK-042, ASK-065, ASK-074, ASK-076, ASK-077.

**Gereksinimler:** REQ-001, REQ-002, REQ-003, REQ-038, REQ-039.

**Var olan testler:** TC-001, TC-002, TC-003, TC-038, TC-039.

**Eski backlog kapsam izi:** W-001, W-002, W-003, W-030. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S01.md`

# S01 — İki oyuncunun aynı eşyayı taşıması

Durum: **PLANNED**. Örnek takvim: hafta 3–4. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yakın dönem plan; planningde alt iş tahmini yapılacak.

**Sprint amacı:** İki oyuncunun tek bir ağır eşyayı sınırlı kuvvetle, aynı host otoritesi altında tutup bırakması.

**Giriş bağımlılığı:** S00. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** Host ve client kanepenin iki ucunu tutar; dar kapıdan geçer; sırayla bırakır ve client çıkınca kalan oyuncu devam eder.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-01-01 | Design/Tech | 6 | Tutma sözleşmesini daralt |
| SP-01-02 | Gameplay | 18 | Sınırlı servo ile tek oyuncu taşıma |
| SP-01-03 | Gameplay/Network | 18 | İkinci holder ve eşzamanlı talepler |
| SP-01-04 | Network | 16 | Komut ve görsel çoğaltma |
| SP-01-05 | QA | 12 | Dar kapı ve kopma matrisi |
| SP-01-06 | Tech/Producer | 10 | Ölçüm ve model seçimi |

### SP-01-01 — Tutma sözleşmesini daralt

**Sorumlu:** Design/Tech. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** Tek uzun eşya, iki authored tutma noktası ve holder üst sınırı belirle. Minimal lifecycle/handling/integrity ayrımını şimdi kur; S04 üretimleştirecek.

**Kartın kabulü:** Free/Held geçişleri ve ret nedenleri yazılı; hasar ayrı eksen olarak kalır.

**Kart bağımlılığı:** SP-00-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-01-02 — Sınırlı servo ile tek oyuncu taşıma

**Sorumlu:** Gameplay. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Kamera hedefinden tutma hedefi üret; host üzerinde mesafe, kuvvet ve tork sınırları uygula. Aynı Rigidbody birden fazla sistemce sürülmesin.

**Kartın kabulü:** Duvara bastırma sınırsız hız üretmez; bırakma ile bütün bağlantılar temizlenir.

**Kart bağımlılığı:** SP-01-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-01-03 — İkinci holder ve eşzamanlı talepler

**Sorumlu:** Gameplay/Network. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** İki tutma hedefini tek authoritative kuvvet modelinde birleştir. Üçüncü holder talebini reddet; release yalnız kendi hakkını sonlandırsın.

**Kartın kabulü:** İki tutma isteği aynı anda gelse de en fazla iki holder olur; biri bırakınca diğeri sürdürür.

**Kart bağımlılığı:** SP-01-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-01-04 — Komut ve görsel çoğaltma

**Sorumlu:** Network. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Grab/Release komutlarına sequence/revision bağlamı ekle; client yalnız niyet gönderir. Yerel el hissini görsel katmanda tut; world state hosttan gelir.

**Kartın kabulü:** Tekrarlanan komut ek joint oluşturmaz; stale release başka oyuncunun hakkını silemez.

**Kart bağımlılığı:** SP-01-01, SP-01-02, SP-01-03.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-01-05 — Dar kapı ve kopma matrisi

**Sorumlu:** QA. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** 10 dakikalık taşıma; 50 tut/bırak; duvara sıkıştırma; tutarken istemci kapatma uygula. Başlangıçta 100 ve 150 ms RTT profilleriyle farkı kaydet.

**Kartın kabulü:** Crash, NaN veya kalıcı joint yok; gecikme kaynaklı hata büyüklüğü ve videosu mevcut.

**Kart bağımlılığı:** SP-01-02, SP-01-03, SP-01-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-01-06 — Ölçüm ve model seçimi

**Sorumlu:** Tech/Producer. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Tepki gecikmesi, correction, holder sayısı ve host fizik süresini görünür kıl. Ağ sorununu gizleyen görsel yumuşatmayı ayrı ölç.

**Kartın kabulü:** S02 için tek kuvvet modeli seçilmiş; çözülmeyen taşıma sorunu varsa S02 kapsamı küçültülmüş.

**Kart bağımlılığı:** SP-01-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- Aynı eşya iki oyuncuyla döndürülebilmeli; üçüncü holder reddedilmeli.
- İstemci koptuğunda host kopmayı saptadıktan sonraki cleanup penceresinde joint/holder temizlenmeli.
- 50 tutma-bırakma döngüsü sonrası holder/joint sayacı başlangıca dönmeli.

## Risk ve kapsam kararı

**En önemli risk:** Eşyayı kameraya parent ederek hızlı bir demo yapmak ve ağ/çarpışma sorununu ertelemek.

**Kapasite yetmezse:** Final el modeli, animasyon IK ve hasar efektleri ertelenir; tutma doğruluğu ertelenmez.

**Kapı:** Bu sprintte ayrı milestone kapısı yok; sprint kabulü ve önceki açık kapılar geçerli.

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-015, ASK-016, ASK-017, ASK-027, ASK-028, ASK-030, ASK-040.

**Gereksinimler:** REQ-003, REQ-004, REQ-009.

**Var olan testler:** TC-003, TC-004, TC-009.

**Eski backlog kapsam izi:** W-003, W-004, W-009. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S02.md`

# S02 — Hareketli platform, denge ve vinç

Durum: **PLANNED**. Örnek takvim: hafta 5–6. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yakın dönem plan; planningde alt iş tahmini yapılacak.

**Sprint amacı:** Ortak taşımayı kontrollü yükselen/eğilen platform üzerinde çalıştırmak.

**Giriş bağımlılığı:** S01. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** İki kişi yükle platforma çıkar, karşı ağırlık olur, vinci kaldırır ve üst kattaki gri hedefe yanaşır.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-02-01 | Tech/Design | 6 | Platform parametre profili |
| SP-02-02 | Gameplay | 18 | Kontrollü deck hareketi |
| SP-02-03 | Gameplay | 18 | Destek kümesi ve denge |
| SP-02-04 | Network | 16 | Platform referansı ve vinç kontrol hakkı |
| SP-02-05 | QA | 12 | Platform stres sahnesi |
| SP-02-06 | Tech/Producer | 10 | Hareket modeli incelemesi |

### SP-02-01 — Platform parametre profili

**Sorumlu:** Tech/Design. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** Yükseklik sınırı, hız, eğim sınırı, histerezis ve boş platform davranışını veri profilinde tanımla. Halatı görsel kabul et.

**Kartın kabulü:** Her ayarın birimi ve başlangıç değeri kaydedilmiş; simülasyonun tek sahibi belli.

**Kart bağımlılığı:** SP-01-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-02-02 — Kontrollü deck hareketi

**Sorumlu:** Gameplay. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Platformu fizik adımında sür; karakterin platform noktasal hızını bir kez almasını sağla. Serbest eşya-platform teması ve iniş çıkışı kur.

**Kartın kabulü:** Duran oyuncu platformdan sistematik kaymaz; inişte önceki platform hızı iki kez uygulanmaz.

**Kart bağımlılığı:** SP-02-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-02-03 — Destek kümesi ve denge

**Sorumlu:** Gameplay. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Oyuncu ve eşya kimliklerini deduplicate et. Temas giriş/çıkış histerezisini ve weighted center hesabını uygula. Held fakat desteksiz eşyaya belge kuralını uygula.

**Kartın kabulü:** Karşılıklı eşit kütle dengelenir; tek eşya birden çok collider ile ağırlığını çoğaltmaz.

**Kart bağımlılığı:** SP-02-01, SP-02-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-02-04 — Platform referansı ve vinç kontrol hakkı

**Sorumlu:** Network. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Host doğrulamalı driver lease kur; Up/Down/Stop komutlarını süre sınırıyla işle. Platform ve yük snapshot zamanlarını uyumlu sun.

**Kartın kabulü:** İki operatör aynı anda motoru sürmez; driver kopunca motor güvenli duruma geçer.

**Kart bağımlılığı:** SP-02-01, SP-02-02, SP-02-03.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-02-05 — Platform stres sahnesi

**Sorumlu:** QA. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** İki ve mümkünse dört süreç; kenarda yük, eşzamanlı zıplama, platforma geçiş, kapı sıkışması ve acil durdurma dene.

**Kartın kabulü:** 10 dakikada enerji birikmesi veya sonsuz correction döngüsü yok; bütün hatalar build ile kayıtlı.

**Kart bağımlılığı:** SP-02-02, SP-02-03, SP-02-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-02-06 — Hareket modeli incelemesi

**Sorumlu:** Tech/Producer. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Host/client göreli kayma kayıtlarını karşılaştır; gerekirse eğim/hız aralığını azalt ve parametre değişikliğini ADR-PLATFORM taslağına geçir.

**Kartın kabulü:** S03 internet testi için tekrarlanabilir bir platform senaryosu ve sabit profil var.

**Kart bağımlılığı:** SP-02-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- SupportedSet aynı itemInstanceId değerini tick başına en fazla bir kez saymalı.
- Driver koptuğunda vinç kendi kendine yükselmeye devam etmemeli.
- Platform üzerinde birlikte taşıma 10 dakika sürdürülebilmeli; hatalı fırlatma varsa kapı kapanmaz.

## Risk ve kapsam kararı

**En önemli risk:** Platform hızı karaktere iki kez eklenebilir; destek kütlesi temas titreşiminde sıçrayabilir.

**Kapasite yetmezse:** Tam halat, fiziksel makara zinciri ve görsel vinç detayları kesilir; kontrollü eğim temel özellik olarak korunur.

**Kapı:** Bu sprintte ayrı milestone kapısı yok; sprint kabulü ve önceki açık kapılar geçerli.

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-018, ASK-019, ASK-020, ASK-040, ASK-066.

**Gereksinimler:** REQ-004, REQ-005, REQ-006.

**Var olan testler:** TC-004, TC-005, TC-006.

**Eski backlog kapsam izi:** W-004, W-005, W-006. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S03.md`

# S03 — Gerçek internet ve teknik risk kapısı

Durum: **PLANNED**. Örnek takvim: hafta 7–8. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yakın dönem plan; planningde alt iş tahmini yapılacak.

**Sprint amacı:** 2–4 oyuncuyla gerçek ağda taşıma/platform uygunluğunu kanıtlayıp tek ağ çözümünü seçmek.

**Giriş bağımlılığı:** S02. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** Farklı internet bağlantılarındaki dört oyuncu yükü taşır; biri ayrılıp yeniden girer; ağ profili değişir ve oturum temiz kapanır.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-03-01 | Tech | 6 | Karşılaştırma ölçütlerini dondur |
| SP-03-02 | Network | 18 | İnternet transport entegrasyonu |
| SP-03-03 | Network | 18 | Dört oyuncu ve temel reconnect |
| SP-03-04 | QA/Network | 16 | Kontrollü gecikme ve kayıp |
| SP-03-05 | QA | 12 | G1 yeniden üretim paketi |
| SP-03-06 | Burak/Tech/QA | 10 | ADR-NET go/no-go |

### SP-03-01 — Karşılaştırma ölçütlerini dondur

**Sorumlu:** Tech. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** Adayları, gerçek bağlantı yolunu, RTT/jitter/loss profillerini ve düzeltme eşiği önerilerini testten önce kaydet. Sadece başarısızlık gerekçesi varsa ikinci adaya zaman ayır.

**Kartın kabulü:** Karşılaştırma aynı sahne, item, cihaz ve senaryoyu kullanır; eşikler sonuca göre oynanmaz.

**Kart bağımlılığı:** SP-02-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-03-02 — İnternet transport entegrasyonu

**Sorumlu:** Network. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Doğrulanmış Unity bağlantı yolu veya uyumlu Steam adapter adayını kur. Lobi keşfi, kimlik ve oyun taşımasını ayrı ele al. Hizmet erişimi yoksa gerçek internet kanıtını BLOCKED bırak.

**Kartın kabulü:** Farklı ağlardan iki makine bağlantı kurar; kullanılan transport sürümü ve bağlantı logları kayıtlı.

**Kart bağımlılığı:** SP-03-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-03-03 — Dört oyuncu ve temel reconnect

**Sorumlu:** Network. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Dört katılımcı kapasitesini, eski bağlantı komutlarının reddini, holder/driver cleanup ve aynı kimlikle snapshot dönüşünü dene.

**Kartın kabulü:** Tur içi yeni oyuncu kabul edilmez; dönen oyuncu eski eşyayı otomatik tutmaz.

**Kart bağımlılığı:** SP-03-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-03-04 — Kontrollü gecikme ve kayıp

**Sorumlu:** QA/Network. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** 50/100/150 ms RTT, 0/10/30 ms jitter, 0/%1 loss örnekleri; 250 ms/%3 loss stres profili uygula. Tek yön gecikmeyi RTT diye etiketleme.

**Kartın kabulü:** Her profil için süre, sonuç, correction ve trafik ölçümü var; stres güvenli başarısızlıkla ayrılmış.

**Kart bağımlılığı:** SP-03-02, SP-03-03.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-03-05 — G1 yeniden üretim paketi

**Sorumlu:** QA. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** İki oyuncu iki farklı ağda; dört oyuncu ayrı süreç/makinelerde ve en az iki ağda test et. Tutarken kopma, host kaybı ve tekrar oturum açma dene.

**Kartın kabulü:** Dört süreç tek bilgisayarda test yalnız ön kanıttır; gerçek makine/bağlantı matrisi teslim edilmiş.

**Kart bağımlılığı:** SP-03-03, SP-03-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-03-06 — ADR-NET go/no-go

**Sorumlu:** Burak/Tech/QA. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Exact framework/transport sürümü, lisans ve açık hataları yaz. Başarısızsa bir kurtarma sprinti veya kontrollü hareket sadeleştirmesi seç.

**Kartın kabulü:** G1 PASS/BLOCKED/FAIL kararı kanıtlı; PASS olmadan pahalı içerik üretimine geçilmiyor.

**Kart bağımlılığı:** SP-03-02, SP-03-03, SP-03-04, SP-03-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- Hedef 150 ms RTT/%1 loss profilinde temel tur tamamlanabilir; aşağıdaki test kitabındaki doğruluk koşulları ihlal edilmez.
- Host/client oyun durumu ayrışması, kalıcı tutma hakkı veya sonsuz hata düzeltme döngüsü yok.
- Host kaybı kontrollü menü dönüşü üretir; host migration vaat edilmez.
- Ağ çözümü, transport, Editor ve paket sürümleri karar kaydında birlikte kilitlenir.

## Risk ve kapsam kararı

**En önemli risk:** Yerel ağ başarısını üretim bağlantısı ve gecikme kalitesi sanmak.

**Kapasite yetmezse:** Hiçbir aday geçmezse içerik sprintleri kaydırılır; platform hızı/eğimi ve eşya servo karmaşıklığı azaltılır. İkinci framework üretime birlikte eklenmez.

**Kapı:** G1 — teknik risk

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-026, ASK-029, ASK-030, ASK-031, ASK-032, ASK-035, ASK-044, ASK-069.

**Gereksinimler:** REQ-006, REQ-007, REQ-008, REQ-016, REQ-022.

**Var olan testler:** TC-006, TC-007, TC-008, TC-016, TC-022.

**Eski backlog kapsam izi:** W-006, W-007, W-008, W-013, W-016. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S04.md`

# S04 — Oyun semantiği ve üretim mimarisi

Durum: **PLANNED**. Örnek takvim: hafta 9–10. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yeniden tahmin edilecek çalışma paketi; bu saatler kesin efor tahmini değil.

**Sprint amacı:** Prototip kurallarını test edilebilir domain katmanına geçirmek; ağ, fizik ve görünümü açık sınırlara ayırmak.

**Giriş bağımlılığı:** S03. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** Aynı komut tekrar gönderilir; geçersiz durum reddedilir; üç tur arka arkaya açılıp kapanır ve kaynak sayacı sıfırlanır.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-04-01 | Tech | 6 | Modül ve işlem sınırı |
| SP-04-02 | Gameplay | 18 | Eşya değişmezleri |
| SP-04-03 | Network | 18 | Komut güvenilirliği |
| SP-04-04 | Tech | 16 | Registry ve sahne yaşam döngüsü |
| SP-04-05 | QA | 12 | Domain ve lifecycle testleri |
| SP-04-06 | Tech/Producer | 10 | Prototip borcu kapatma |

### SP-04-01 — Modül ve işlem sınırı

**Sorumlu:** Tech. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** Core/Gameplay/Network/Presentation bağımlılıklarını belirle. StateStore yazma noktası, command validation ve domain event sözleşmesini kaydet.

**Kartın kabulü:** Domain testleri ağ bağlantısı ve görsel mesh gerektirmiyor.

**Kart bağımlılığı:** SP-03-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-04-02 — Eşya değişmezleri

**Sorumlu:** Gameplay. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Available/Delivered/Lost ile Free/Held/Secured eksenlerini ve integrity alanını ayır. Joint oluşturma başarısızlığında rollback uygula.

**Kartın kabulü:** Delivered+Held, Lost+anchor, integrity=0+Available ve üç holder durumları üretilemiyor.

**Kart bağımlılığı:** SP-04-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-04-03 — Komut güvenilirliği

**Sorumlu:** Network. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Epoch/sequence/revision doğrulaması, erişim mesafesi, holder yetkisi ve rate limit ekle. Hatalı payload için sınırlı ret yolu kur.

**Kartın kabulü:** Duplicate komut yan etkiyi tekrar üretmiyor; flood kaynak kullanımını kontrolsüz artırmıyor.

**Kart bağımlılığı:** SP-04-01, SP-04-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-04-04 — Registry ve sahne yaşam döngüsü

**Sorumlu:** Tech. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** ItemDefinition/ContractDefinition kimlikleri ve veri doğrulayıcılarını kur. Bootstrap servis ömrünü ve sahne unload temizliğini tek yoldan geçir.

**Kartın kabulü:** Duplicate ID, eksik zorunlu prefab ve null profile build öncesi hata; tekrar açılan sahnede çift manager yok.

**Kart bağımlılığı:** SP-04-01, SP-04-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-04-05 — Domain ve lifecycle testleri

**Sorumlu:** QA. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** Geçerli/yasak geçişler, yarım grab rollback, terminal cleanup ve 20 sahne geçişini kapsa.

**Kartın kabulü:** Anlamlı state testleri geçti; listener, spawned object ve joint sayaçlarında sürekli artış yok.

**Kart bağımlılığı:** SP-04-02, SP-04-03, SP-04-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-04-06 — Prototip borcu kapatma

**Sorumlu:** Tech/Producer. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Deney kodunu kaldır veya test sahnesine ayır; üretim koduyla aynı objeyi süren debug script bırakma. REQ/TC izlerini güncelle.

**Kartın kabulü:** Tek state sahibi ve tek fizik sürücüsü var; açık teknik borçların sahibi belirli.

**Kart bağımlılığı:** SP-04-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- Her işlem ya bütünüyle uygulanır ya state ve fizik bağlantıları önceki tutarlı duruma döner.
- 20 sahne yükleme/boşaltmada servis ve event subscription sayısı büyümez.
- Tüm kritik state değişmezleri saf domain testinde doğrulanır.

## Risk ve kapsam kararı

**En önemli risk:** Her durumu MonoBehaviour boolean alanlarıyla saklayıp eşya geçişlerini birden çok yerde yürütmek.

**Kapasite yetmezse:** Genel amaçlı framework, ECS dönüşümü ve her sınıf için soyutlama üretimi yapılmaz; mevcut oyunun işlem sınırları yeterlidir.

**Kapı:** Bu sprintte ayrı milestone kapısı yok; sprint kabulü ve önceki açık kapılar geçerli.

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-012, ASK-015, ASK-016, ASK-027, ASK-028, ASK-037, ASK-038, ASK-039, ASK-041.

**Gereksinimler:** REQ-009, REQ-020, REQ-021, REQ-022.

**Var olan testler:** TC-009, TC-020, TC-021, TC-022.

**Eski backlog kapsam izi:** W-009, W-015, W-016. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S05.md`

# S05 — Sabitleme, hasar ve kurtarma

Durum: **PLANNED**. Örnek takvim: hafta 11–12. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yeniden tahmin edilecek çalışma paketi; bu saatler kesin efor tahmini değil.

**Sprint amacı:** Taşımanın risk/önlem ilişkisini ve çıkmaza girmeden geri dönme yollarını kurmak.

**Giriş bağımlılığı:** S04. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** Bir yük bağlanır, vinçle taşınır, güvenli duruşta çözülür; başka yük darbe alır ve sıkışan eşya izinli kurtarılır.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-05-01 | Design | 6 | Davranış ve eşik tablosu |
| SP-05-02 | Gameplay | 18 | Atomik anchor işlemi |
| SP-05-03 | Gameplay | 18 | Hasar olayı ve terminal cleanup |
| SP-05-04 | Gameplay/UX | 16 | Recovery ve kaybolma |
| SP-05-05 | QA | 12 | Yarış ve hata testleri |
| SP-05-06 | UX/Design | 10 | Geri bildirim ve oyun ayarı |

### SP-05-01 — Davranış ve eşik tablosu

**Sorumlu:** Design. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** Secure/unsecure önkoşulu, darbe gruplama penceresi, recovery hakkı ve ret metinlerini yaz. İlk eşikler ayarlanabilir profil olsun.

**Kartın kabulü:** Aynı olayın hangi işlemi tetiklediği ve hangisini reddettiği belirsiz değil.

**Kart bağımlılığı:** SP-04-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-05-02 — Atomik anchor işlemi

**Sorumlu:** Gameplay. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Slot rezervasyonu, state geçişi ve bağlı fizik temsilini tek işlemde kur. Held eşya bağlanamasın; unsecure world pose ve platform noktasal hızını korusun.

**Kartın kabulü:** Aynı slota iki istekten biri kazanır; başarısız işlem slot bırakır; ağırlık tek sayılır.

**Kart bağımlılığı:** SP-05-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-05-03 — Hasar olayı ve terminal cleanup

**Sorumlu:** Gameplay. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Impulse/relative hız temelli ayarlanabilir model dene. Aynı çarpışmayı collider sayısından bağımsız grupla. IntegrityZero Lost üretip joint ve anchor temizlesin.

**Kartın kabulü:** Çok collider eşya aynı darbeden katlı hasar almıyor; kırıntılar yeni authoritative yük olmuyor.

**Kart bağımlılığı:** SP-05-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-05-04 — Recovery ve kaybolma

**Sorumlu:** Gameplay/UX. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Sıkışma/düşme için güvenli authored noktalar ve sınırlandırılmış recovery komutu ekle. Host geçerli lifecycle ve hakkı denetlesin.

**Kartın kabulü:** Delivered/Lost eşya canlanamaz; spawn başka eşyayla penetrasyon yaratırsa güvenli ret var.

**Kart bağımlılığı:** SP-05-02, SP-05-03.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-05-05 — Yarış ve hata testleri

**Sorumlu:** QA. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** Secure/Secure, Secure/Grab, Unsecure/Disconnect, Damage/Deliver yarışlarını ve slot temizliğini test et.

**Kartın kabulü:** Kabul sırası host tarafından tek belirleniyor; kalıcı dolu slot veya görünmez tutma yok.

**Kart bağımlılığı:** SP-05-02, SP-05-03, SP-05-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-05-06 — Geri bildirim ve oyun ayarı

**Sorumlu:** UX/Design. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Kilit ikonu, ret nedeni, hasar göstergesi ve recovery maliyetini gri UI ile göster; iki kişilik turda anlaşılırlığı izle.

**Kartın kabulü:** Oyuncu neden bağlayamadığını açıklayabiliyor; renk dışı kilit göstergesi var.

**Kart bağımlılığı:** SP-05-02, SP-05-03, SP-05-04, SP-05-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- 100 secure/unsecure denemesinde anchor sayısı sızmamalı.
- Aynı temas örneğinde collider sayısı değişince hasar orantısız çoğalmamalı.
- Kurtarma terminal item kimliğini tekrar Available yapmamalı.

## Risk ve kapsam kararı

**En önemli risk:** Kayışı görsel parent işlemi sanmak; çok collider temasında hasarı çoğaltmak.

**Kapasite yetmezse:** Parça bazlı fiziksel kırılma ve düğüm/halat çözme sistemi yerine kontrollü görsel hasar kullanılır.

**Kapı:** Bu sprintte ayrı milestone kapısı yok; sprint kabulü ve önceki açık kapılar geçerli.

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-016, ASK-019, ASK-020, ASK-021, ASK-025, ASK-049.

**Gereksinimler:** REQ-010, REQ-012, REQ-014.

**Var olan testler:** TC-010, TC-012, TC-014.

**Eski backlog kapsam izi:** W-010, W-012. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S06.md`

# S06 — Teslimat, tur sonucu ve güvenli kayıt

Durum: **PLANNED**. Örnek takvim: hafta 13–14. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yeniden tahmin edilecek çalışma paketi; bu saatler kesin efor tahmini değil.

**Sprint amacı:** Yükü teslim etmekten kampanya kaydına kadar tek sonuç üreten işlem zincirini tamamlamak.

**Giriş bağımlılığı:** S05. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** Tur tamamlanır, sonuç iki kez tetiklenmeye çalışılır, oyun kayıt ortasında kapatılır ve yeniden açılır.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-06-01 | Design/Tech | 6 | Tur ve kayıt sınırı |
| SP-06-02 | Gameplay | 18 | Teslimat doğrulaması |
| SP-06-03 | Tech | 18 | Atomik kampanya kaydı |
| SP-06-04 | Gameplay/UX | 16 | Bitiş ve asgari ekonomi |
| SP-06-05 | QA | 12 | Çökme ve migration matrisi |
| SP-06-06 | QA/Producer | 10 | Kayıt kanıt paketi |

### SP-06-01 — Tur ve kayıt sınırı

**Sorumlu:** Design/Tech. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** ContractRunId, delivery ledger, finish reason, sonuç özeti ve checkpoint sınırını tanımla. Tur içi fizik devam kaydını kapsam dışı tut.

**Kartın kabulü:** Hangi anda kalıcı kazanç oluştuğu UI ve domain için aynı.

**Kart bağımlılığı:** SP-05-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-06-02 — Teslimat doğrulaması

**Sorumlu:** Gameplay. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Available+Free, hedef bölgesi, hız ve bekleme koşullarını host doğrulasın. Item kimliğini tek ledger işleminde terminal yap.

**Kartın kabulü:** Sınırda sallanan eşya erken teslim olmaz; aynı item yeniden teslim edilemez.

**Kart bağımlılığı:** SP-06-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-06-03 — Atomik kampanya kaydı

**Sorumlu:** Tech. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Şema sürümü, geçici dosya, doğrulanmış yazma/değiştirme yolu ve yedek geri dönüşü uygula. Commit kimliğiyle tekrar uygulamayı engelle.

**Kartın kabulü:** Kesilen yazma son sağlam checkpointi yok etmez; aynı tur sonucu iki kez para eklemez.

**Kart bağımlılığı:** SP-06-01, SP-06-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-06-04 — Bitiş ve asgari ekonomi

**Sorumlu:** Gameplay/UX. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Tamamlama/süre bitimi/abort sırasını tanımla. Basit kampanya ilerlemesi ve satın alma işlem sınırını kur; denge içeriği S17de genişler.

**Kartın kabulü:** Abort önceki kampanyayı silmez; bir satın alma iki kez uygulanmaz; kaynak yetersizliği görünür.

**Kart bağımlılığı:** SP-06-02, SP-06-03.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-06-05 — Çökme ve migration matrisi

**Sorumlu:** QA. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** Yazma öncesi/ortası/sonrası fault injection, bozuk ana kayıt, geçerli yedek, desteklenen eski şema ve aynı sonuç tekrarını test et.

**Kartın kabulü:** Beklenen checkpoint ve bakiye her senaryoda doğrulanmış; sessiz yeni oyun sıfırlaması yok.

**Kart bağımlılığı:** SP-06-02, SP-06-03, SP-06-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-06-06 — Kayıt kanıt paketi

**Sorumlu:** QA/Producer. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Örnek save fixtureları, beklenen bakiye, migration sonucu ve known limitation kaydet. Kullanıcı dosya yolu tanılama yönergesi hazırla.

**Kartın kabulü:** Testler kontrollü fixture kullanıyor; gerçek oyuncu kaydı test için ezilmiyor.

**Kart bağımlılığı:** SP-06-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- Aynı ContractRunId sonucu 10 kez uygulandığında bakiye bir kez değişmeli.
- Kayıt kesintileri tanımlı son sağlam checkpoint veya açık hata üretmeli.
- Tur sonucu ve kalıcı kayıt başarısı UIda birbirinden anlaşılır olmalı.

## Risk ve kapsam kararı

**En önemli risk:** Teslimat RPC tekrarında çift para vermek veya oyun kapanırken son sağlam kaydı bozmak.

**Kapasite yetmezse:** Ayrıntılı yükseltme ağacı, bulut kayıt ve tur ortası resume ertelenir; kayıt güvenliği ertelenmez.

**Kapı:** Bu sprintte ayrı milestone kapısı yok; sprint kabulü ve önceki açık kapılar geçerli.

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-012, ASK-022, ASK-023, ASK-024, ASK-033, ASK-043, ASK-045.

**Gereksinimler:** REQ-011, REQ-013, REQ-017, REQ-018, REQ-019.

**Var olan testler:** TC-011, TC-013, TC-017, TC-018, TC-019.

**Eski backlog kapsam izi:** W-011, W-014. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S07.md`

# S07 — Lobi ve oturum dayanıklılığı

Durum: **PLANNED**. Örnek takvim: hafta 15–16. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yeniden tahmin edilecek çalışma paketi; bu saatler kesin efor tahmini değil.

**Sprint amacı:** Oyuncunun davet, yükleme, kopma ve yeniden bağlanma akışlarını üretim kalitesine yaklaştırmak.

**Giriş bağımlılığı:** S06. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** Lobide uyumsuz build reddedilir; yüklemede biri ayrılır; aktif turda bir misafir kopup güvenli noktaya döner.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-07-01 | UX/Network | 6 | Oturum geçiş tablosu |
| SP-07-02 | Network | 18 | Roster ve sürüm handshake |
| SP-07-03 | Network | 18 | Reconnect ve epoch temizliği |
| SP-07-04 | UI/Tech | 16 | Host kaybı ve hata ekranları |
| SP-07-05 | QA | 12 | 20 bağlantı döngüsü |
| SP-07-06 | Build/Producer | 10 | Test build dağıtım yönergesi |

### SP-07-01 — Oturum geçiş tablosu

**Sorumlu:** UX/Network. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** Frontend/Lobby/Loading/Active/Results/Abort geçişlerine timeout, reasonCode ve geri dönüş düğmesi ata.

**Kartın kabulü:** Her bekleme ekranının çıkışı var; oyuncu sonsuz spinnerda kalmıyor.

**Kart bağımlılığı:** SP-06-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-07-02 — Roster ve sürüm handshake

**Sorumlu:** Network. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Build/content hash, kimlik ve hazır olma durumunu doğrula. Aktif tur için rosterı kilitle; oyuncu sayısıyla hedefler tur ortasında oynamasın.

**Kartın kabulü:** Uyumsuz içerikle maç başlamaz; yeni kimlik aktif tura katılamaz.

**Kart bağımlılığı:** SP-07-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-07-03 — Reconnect ve epoch temizliği

**Sorumlu:** Network. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Aynı kimlik rezervasyonu, yeni sequence bağlamı ve snapshot apply sınırı ekle. Avatar güvenli sabit noktada dönsün.

**Kartın kabulü:** Eski komutlar reddedilir; dönüşte önceki holder/driver hakkı kendiliğinden verilmez.

**Kart bağımlılığı:** SP-07-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-07-04 — Host kaybı ve hata ekranları

**Sorumlu:** UI/Tech. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Host loss, dolu lobi, timeout, bağlantı reddi ve yeniden denemeyi yerelleştirme anahtarlarıyla bağla.

**Kartın kabulü:** Host kapanınca checkpoint sınırı doğru açıklanır; kullanıcı menüye döner.

**Kart bağımlılığı:** SP-07-01, SP-07-02, SP-07-03.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-07-05 — 20 bağlantı döngüsü

**Sorumlu:** QA. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** Join/leave/reconnect/load-cancel döngülerini iki/dört oyuncuyla uygula; geç snapshot ve content mismatch ekle.

**Kartın kabulü:** Kalıcı boş/dolu slot hatası, çift avatar veya çift network manager yok.

**Kart bağımlılığı:** SP-07-02, SP-07-03, SP-07-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-07-06 — Test build dağıtım yönergesi

**Sorumlu:** Build/Producer. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Aynı buildin katılımcılara doğrulanabilir ulaştırılmasını ve log toplama formatını yaz. Servis maliyetini tahmin yerine gözlenen trafikten güncelle.

**Kartın kabulü:** Test ekibinin sürümleri eşleşiyor; bağlantı başarısızlıkları reasonCode ile ayrılıyor.

**Kart bağımlılığı:** SP-07-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- 20 oturum aç/kapat döngüsünde roster ve kaynak sayaçları temizlenmeli.
- Grace süresi dolunca aktif tur yeni oyuncuya açılmamalı.
- Host loss migration başlatmamalı; son checkpoint davranışı kullanıcıya doğru gösterilmeli.

## Risk ve kapsam kararı

**En önemli risk:** Bağlantıyı yalnız başarılı senaryoda ele almak; eski paketlerle yeni oyuncunun hakkını karıştırmak.

**Kapasite yetmezse:** Oyun içi sesli sohbet ve genel matchmaking bu akışların yerine konmaz; arkadaş oturumu önceliklidir.

**Kapı:** Bu sprintte ayrı milestone kapısı yok; sprint kabulü ve önceki açık kapılar geçerli.

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-012, ASK-029, ASK-031, ASK-032, ASK-034, ASK-038, ASK-045, ASK-062.

**Gereksinimler:** REQ-015, REQ-016, REQ-017, REQ-021, REQ-022, REQ-034.

**Var olan testler:** TC-015, TC-016, TC-017, TC-021, TC-022, TC-034.

**Eski backlog kapsam izi:** W-013, W-016, W-026. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S08.md`

# S08 — Ortak görsel dil ve örnek asset üretim hattı

Durum: **PLANNED**. Örnek takvim: hafta 17–18. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yeniden tahmin edilecek çalışma paketi; bu saatler kesin efor tahmini değil.

**Sprint amacı:** Quaternius tabanlı dünyanın, özel platformun ve yakın plan eşyaların aynı oyuna ait görünmesini sağlamak.

**Giriş bağımlılığı:** S07. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** Tek ışık altında balkon, karakter referansı, platform, kanepe ve buzdolabı aynı sahnede yakın/uzak gösterilir.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-08-01 | Burak/Art | 6 | Görsel kabul panosu |
| SP-08-02 | Art | 18 | Final aday platform |
| SP-08-03 | Art | 18 | Final aday kanepe ve aktarım |
| SP-08-04 | TechArt | 16 | Malzeme ve import kuralları |
| SP-08-05 | QA/Art | 12 | Aynı sahnede kabul |
| SP-08-06 | Art/Producer | 10 | Asset kartı ve üretim süresi |

### SP-08-01 — Görsel kabul panosu

**Sorumlu:** Burak/Art. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** Siluet, kenar, doygunluk, roughness, yıpranma ve ölçek referanslarını dondur. Downtown ana aday; mobilya geçici kaynak olarak işaretlenir.

**Kartın kabulü:** Örnek ekran görüntüsünün onay/ret gerekçesi var; ortak shader tek kabul ölçütü değil.

**Kart bağımlılığı:** SP-07-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-08-02 — Final aday platform

**Sorumlu:** Art. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Kontrol kutusu, deck, kapak, korkuluk ve anchor socketlerini oyun ölçülerine göre hazırla. Kaynak ve export ayrımını koru.

**Kartın kabulü:** Mesh parçaları doğru pivot/ölçekte; oynanış colliderı görsel çubuk ayrıntısına bağımlı değil.

**Kart bağımlılığı:** SP-08-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-08-03 — Final aday kanepe ve aktarım

**Sorumlu:** Art. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Kanepeyi onaylı stile uyarla; Blender kaynak, kontrollü FBX, materyal eşleme ve compound collider prefabı üret.

**Kartın kabulü:** Kapı/platform/tutma testleri geçti; kaynak lisansı ve yapılan düzenleme kayıtlı.

**Kart bağımlılığı:** SP-08-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-08-04 — Malzeme ve import kuralları

**Sorumlu:** TechArt. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Ortak malzeme paleti, gölge/LOD örneği ve texture import presetleri kur. Renderer/material maliyetini Frame Debugger ve profiler ile ölç.

**Kartın kabulü:** Yakın planda yeterli görünüm ve kaydedilmiş maliyet var; varyantların ayrı material instance etkisi biliniyor.

**Kart bağımlılığı:** SP-08-01, SP-08-02, SP-08-03.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-08-05 — Aynı sahnede kabul

**Sorumlu:** QA/Art. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** Nötr ve oyun ışığı, birinci şahıs yakın plan, iki mesafe ve düşük kalite profilinde karşılaştır. Collider gizmos ile kapı açıklığını denetle.

**Kartın kabulü:** Pembe shader yok; tutma noktaları okunur; uyumsuz varlık final etiketi almıyor.

**Kart bağımlılığı:** SP-08-02, SP-08-03, SP-08-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-08-06 — Asset kartı ve üretim süresi

**Sorumlu:** Art/Producer. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Her örnek için model/UV/import/collider/oyun testi sürelerini kaydet. Sonraki varlık sayısını bu throughput ile hesapla.

**Kartın kabulü:** En az bir uçtan uca kabul edilmiş platform ve kanepe kartı var; toplu üretim tahmini ölçüme bağlı.

**Kart bağımlılığı:** SP-08-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- Platform ve kanepe gerçek Unity test sahnesinde yan yana değerlendirilmiş olmalı.
- Aynı materyal kullanmak yeterli değildir; oran/kenar/detay/renk kriterleri tek tek kapanmalı.
- Asset kimliği kaynak, lisans, export ve oyun prefabına kadar izlenebilmeli.

## Risk ve kapsam kararı

**En önemli risk:** Aynı üreticiden geldiği için paketleri uyumlu saymak veya ortak shaderı otomatik batching sanmak.

**Kapasite yetmezse:** Bütün mobilyaların final üretimi yerine tek referans kanepe bitirilir; özel model emeği 18 saatlik kartı aşıyorsa sonraki sprintten kapasite alınır.

**Kapı:** Bu sprintte ayrı milestone kapısı yok; sprint kabulü ve önceki açık kapılar geçerli.

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-046, ASK-047, ASK-048, ASK-049, ASK-050, ASK-053, ASK-061, ASK-066.

**Gereksinimler:** REQ-023, REQ-024, REQ-025, REQ-026, REQ-029.

**Var olan testler:** TC-023, TC-024, TC-025, TC-026, TC-029.

**Eski backlog kapsam izi:** W-017, W-018, W-019, W-020, W-022. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S09.md`

# S09 — Karakter, el teması ve sesli geri bildirim

Durum: **PLANNED**. Örnek takvim: hafta 19–20. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yeniden tahmin edilecek çalışma paketi; bu saatler kesin efor tahmini değil.

**Sprint amacı:** Co-op taşımanın diğer oyuncularda anlaşılmasını ve yerel kamerada konforlu hissedilmesini sağlamak.

**Giriş bağımlılığı:** S08. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** İki karakter aynı kanepeyi tutar, döndürür ve bırakır; kamera roll olmadan eğimli platformda oynanır.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-09-01 | Art/Design | 6 | Rig ve animasyon kapsamı |
| SP-09-02 | Animation | 18 | Rig ve hareket entegrasyonu |
| SP-09-03 | Animation/Gameplay | 18 | El IK ve carry sunumu |
| SP-09-04 | Audio/VFX | 16 | Darbe ve motor geri bildirimi |
| SP-09-05 | QA/UX | 12 | Konfor ve okunurluk turu |
| SP-09-06 | Art/Tech | 10 | Animasyon maliyet kontrolü |

### SP-09-01 — Rig ve animasyon kapsamı

**Sorumlu:** Art/Design. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** Tek rig, locomotion, carry, release ve kontrol kutusu pozlarını seç. İlk kişi elleri ile remote beden görünürlüğü kuralını yaz.

**Kartın kabulü:** Gereksiz varyasyon yok; temel hareket setinin kaynak ve retarget planı açık.

**Kart bağımlılığı:** SP-08-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-09-02 — Rig ve hareket entegrasyonu

**Sorumlu:** Animation. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Universal Base Character adayını doğrula, animasyonları retarget et, root motion ve controller sorumluluğunu ayır.

**Kartın kabulü:** Görsel kök hareketi oyuncu fizik kökünü ikinci kez sürmüyor; yürüyüş ölçüsü tutarlı.

**Kart bağımlılığı:** SP-09-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-09-03 — El IK ve carry sunumu

**Sorumlu:** Animation/Gameplay. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Tutma socketlerine görsel el hedefi üret; kabul edilen holder state ile blend kur. Release/disconnectte IK ağırlığını güvenle çöz.

**Kartın kabulü:** El animasyonu komut otoritesi değil; bırakılan eşya görünmez ele bağlı kalmıyor.

**Kart bağımlılığı:** SP-09-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-09-04 — Darbe ve motor geri bildirimi

**Sorumlu:** Audio/VFX. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Malzeme ailelerine darbe varyasyonu, şiddet eşiği, sürtünme ve yük gerilimi bağla. Kozmetik olayı event kimliğiyle tek oynat.

**Kartın kabulü:** Bir çarpışma tekrar RPC ile çift ses/VFX üretmiyor; küçük efektler ağ objesi olmuyor.

**Kart bağımlılığı:** SP-09-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-09-05 — Konfor ve okunurluk turu

**Sorumlu:** QA/UX. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** FOV, hassasiyet, kamera shake kapatma ve roll kapalı seçimini iki oyuncuyla dene. Eller/beden clipping ve IK kopma klipleri al.

**Kartın kabulü:** Zorunlu headbob yok; karakter tutuluyor/bırakıyor ayrımı sessiz videoda okunuyor.

**Kart bağımlılığı:** SP-09-02, SP-09-03, SP-09-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-09-06 — Animasyon maliyet kontrolü

**Sorumlu:** Art/Tech. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Dört karakter IK/Animator CPU ölç; uzaktaki görünümün güncelleme sınırını dene ve işitsel karışımı düzelt.

**Kartın kabulü:** Profil kaydı var; kalite azaltımı gameplay holder state değiştirmiyor.

**Kart bağımlılığı:** SP-09-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- Aynı rig seçilen oyuncu varyantlarında doğrulanmalı.
- IK/Animator kapatılınca gameplay kuralları çalışmaya devam etmeli.
- Kopma ve sahne unload sonrası ses loop veya IK hedefi kalmamalı.

## Risk ve kapsam kararı

**En önemli risk:** Animasyon eventlerinin fizik tutmayı yönetmesi; yakın plandaki ellerin ağ hatalarını gizlemesi.

**Kapasite yetmezse:** Emote koleksiyonu ve gelişmiş ragdoll ertelenir; taşıma ve bırakma görselleri tamamlanır.

**Kapı:** Bu sprintte ayrı milestone kapısı yok; sprint kabulü ve önceki açık kapılar geçerli.

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-013, ASK-014, ASK-017, ASK-051, ASK-052, ASK-054, ASK-055.

**Gereksinimler:** REQ-027, REQ-028, REQ-030, REQ-031, REQ-035.

**Var olan testler:** TC-027, TC-028, TC-030, TC-031, TC-035.

**Eski backlog kapsam izi:** W-021, W-023, W-027. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S10.md`

# S10 — Avlu rotası, dört eşya ve temel arayüz

Durum: **PLANNED**. Örnek takvim: hafta 21–22. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yeniden tahmin edilecek çalışma paketi; bu saatler kesin efor tahmini değil.

**Sprint amacı:** Bir tam görev rotasında temel ürünün bütün katmanlarını bir araya getirmek.

**Giriş bağımlılığı:** S09. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** Oyuncular menüden göreve girer, dört farklı eşya örneğini taşır, teslim eder ve sonuç ekranından tekrar oynar.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-10-01 | Level/Design | 6 | Rota ve görev kartı |
| SP-10-02 | Level | 18 | Oynanabilir Avlu düzeni |
| SP-10-03 | Art/Gameplay | 18 | Dört eşya örneği |
| SP-10-04 | UI/UX | 16 | HUD ve sonuç akışı |
| SP-10-05 | QA | 12 | Rota ve erişim doğrulaması |
| SP-10-06 | Producer/Design | 10 | Slice adayını birleştir |

### SP-10-01 — Rota ve görev kartı

**Sorumlu:** Level/Design. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** Avlu için kaynak oda, kapı, balkon, platform hattı, hedef ve recovery noktalarını ölç. C01 birleşik slice varyantını yaz.

**Kartın kabulü:** İki kişinin bütün mandatory eşyalara ulaşabileceği rota var; süre/kota profil alanı.

**Kart bağımlılığı:** SP-09-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-10-02 — Oynanabilir Avlu düzeni

**Sorumlu:** Level. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Onaylı modüler kit ile gri geometriyi değiştir; kapı/balkon geçişlerini collider ile birlikte koru.

**Kartın kabulü:** Görsel geometri ilerledikçe oynanış açıklıkları kapanmıyor; her kritik rotanın ölçüsü kayıtlı.

**Kart bağımlılığı:** SP-10-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-10-03 — Dört eşya örneği

**Sorumlu:** Art/Gameplay. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Kompakt kutu, uzun kanepe, dik ağır eşya ve kırılgan eşya profil/prefablarını tamamla. Ortak sistem üzerinden farklı ağırlık/şekil/hasar davranışı ver.

**Kartın kabulü:** Dört örnek farklı karar gerektiriyor; her model için ayrı ağ kodu gerekmiyor.

**Kart bağımlılığı:** SP-10-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-10-04 — HUD ve sonuç akışı

**Sorumlu:** UI/UX. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Ret nedeni, holder, denge, bağ, görev sayacı ve kayıt durumu göster. Input ikonları ve TR/EN anahtarlarını UI temeline bağla.

**Kartın kabulü:** UI host stateini doğru okuyor; para kayıt başarılı olmadan kaydedildi diye gösterilmiyor.

**Kart bağımlılığı:** SP-10-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-10-05 — Rota ve erişim doğrulaması

**Sorumlu:** QA. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** İki/dört oyuncuyla her eşya kapıdan geçsin; yüksek kontrast/gri ton ve büyük metinle kritik bilgi denensin.

**Kartın kabulü:** Hiçbir mandatory eşya geometrik softlock yaratmıyor; renk tek bilgi kanalı değil.

**Kart bağımlılığı:** SP-10-02, SP-10-03, SP-10-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-10-06 — Slice adayını birleştir

**Sorumlu:** Producer/Design. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Tek test buildini dondur, eksik final varlıkları açık listele ve S11 kusur kuyruğunu hazırla.

**Kartın kabulü:** Tek uçtan uca görev var; demo üç görevmiş gibi sayılmıyor.

**Kart bağımlılığı:** SP-10-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- İki oyuncu da dört oyuncu da aynı rotayı tamamlayabilmeli.
- Dört örneğin collider/tutma/hasar/teslimat testleri var.
- Görev tekrar başlatıldığında önceki run kimliği ve sayaçları kalmamalı.

## Risk ve kapsam kararı

**En önemli risk:** Güzel görünen ama kanepenin fiziksel olarak geçemediği bir apartman üretmek.

**Kapasite yetmezse:** Ek odalar, geniş sokak ve ikinci bina kesilir; tek Avlu rotası final kaliteye yaklaşır.

**Kapı:** Bu sprintte ayrı milestone kapısı yok; sprint kabulü ve önceki açık kapılar geçerli.

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-011, ASK-023, ASK-049, ASK-056, ASK-057, ASK-058, ASK-060, ASK-062, ASK-063, ASK-064.

**Gereksinimler:** REQ-025, REQ-032, REQ-033, REQ-034, REQ-035, REQ-036.

**Var olan testler:** TC-025, TC-032, TC-033, TC-034, TC-035, TC-036.

**Eski backlog kapsam izi:** W-019, W-024, W-025, W-026, W-027. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S11.md`

# S11 — Vertical slice kalite kapısı

Durum: **PLANNED**. Örnek takvim: hafta 23–24. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yeniden tahmin edilecek çalışma paketi; bu saatler kesin efor tahmini değil.

**Sprint amacı:** Tek görevin oynanış, görsel, ses, ağ ve kayıt kalitesini birlikte doğrulamak.

**Giriş bağımlılığı:** S10. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** Tek build üzerinde dışarıdan iki grup sessizce izlenir; oturumun başından kayıt sonrası yeniden açılışa kadar oynarlar.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-11-01 | QA/Producer | 6 | G2 test kapsamı |
| SP-11-02 | Gameplay/Network | 18 | En yüksek riskli kusurlar |
| SP-11-03 | Art/UX | 18 | Tutarlılık ve okunurluk kusurları |
| SP-11-04 | Performance | 16 | Temsili host/client profil |
| SP-11-05 | QA/Design | 12 | İki dış grup testi |
| SP-11-06 | Burak/Producer | 10 | G2 karar ve yeniden tahmin |

### SP-11-01 — G2 test kapsamı

**Sorumlu:** QA/Producer. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** Dondurulmuş build, cihaz ve ağ profillerini seç; P0/P1 listesi ve bütün kabul kanıtlarını kontrol et.

**Kartın kabulü:** Test öncesi boş kalan kriterlerin sahibi var; bilinmeyenler PASS sayılmıyor.

**Kart bağımlılığı:** SP-10-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-11-02 — En yüksek riskli kusurlar

**Sorumlu:** Gameplay/Network. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Taşıma hissi, platform jitter, penetration veya state yarışı kaynaklı ilk engelleyici kusurları gider.

**Kartın kabulü:** Her düzeltme önceki fail senaryosunda yeniden denenmiş; rastgele parametre artırımı yok.

**Kart bağımlılığı:** SP-11-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-11-03 — Tutarlılık ve okunurluk kusurları

**Sorumlu:** Art/UX. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Yakın plan eşya, el teması, kapı/anchor okunurluğu ve UI ret metinlerini ortak sahnede düzelt.

**Kartın kabulü:** Burak aynı kalite panosuyla onay verir; istisnalar asset kartında açık.

**Kart bağımlılığı:** SP-11-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-11-04 — Temsili host/client profil

**Sorumlu:** Performance. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Dört oyuncu ve en ağır slice yükünde CPU/GPU, fizik, allocation, trafik ve bellek ölç.

**Kartın kabulü:** Referans cihaz ve p95/p99 sonuçları kaydedilmiş; darboğaz için en az bir doğrulanmış çözüm var.

**Kart bağımlılığı:** SP-11-02, SP-11-03.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-11-05 — İki dış grup testi

**Sorumlu:** QA/Design. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** Bir iki kişilik ve bir dört kişilik grupla 30–45 dakikalık gözlem yap; açıklama yapmadan ilk denemeyi izle, tıkanma/başarı nedenlerini sor.

**Kartın kabulü:** Davet edilecek kişilere iletişim ayrı yürütülür; tamamlanma ve tıkanma kayıtları anonim tutulur.

**Kart bağımlılığı:** SP-11-02, SP-11-03, SP-11-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-11-06 — G2 karar ve yeniden tahmin

**Sorumlu:** Burak/Producer. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Eğlence geri bildirimini ve gerçek üretim saatlerini değerlendir. Demo ve v1 içerik üst sınırını güncelle.

**Kartın kabulü:** G2 kararı kanıtlı; G2 başarısızsa demo yerine aynı kalite açığına sprint ayrılır.

**Kart bağımlılığı:** SP-11-04, SP-11-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- Tek görev uçtan uca oynanır, sonuç kaydedilir ve yeniden açılışta korunur.
- Açık P0/P1 yok; iki/dört oyuncu testi ve ortak stil sahnesi onaylı.
- Performans hedefleri belirlenmiş cihazla kıyaslanır; yalnız ortalama FPS raporu kabul edilmez.

## Risk ve kapsam kararı

**En önemli risk:** Sistemleri ayrı ayrı bitmiş sayıp birlikte oynandıklarında ortaya çıkan sorunu kaçırmak.

**Kapasite yetmezse:** Kapsam yetişmezse yeni içerik başlamaz; dilim içindeki görsel varyantlar ve çevre detayı azaltılır.

**Kapı:** G2 — vertical slice

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-026, ASK-046, ASK-065, ASK-067, ASK-068, ASK-069, ASK-071, ASK-073.

**Gereksinimler:** REQ-008, REQ-023, REQ-032, REQ-034, REQ-037.

**Var olan testler:** TC-008, TC-023, TC-032, TC-034, TC-037.

**Eski backlog kapsam izi:** W-008, W-017, W-024, W-028. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S12.md`

# S12 — Öğretici ve üç sözleşmelik demo

Durum: **PLANNED**. Örnek takvim: hafta 25–26. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yeniden tahmin edilecek çalışma paketi; bu saatler kesin efor tahmini değil.

**Sprint amacı:** İlk kez oynayan bir grubun C01–C03 görevlerini dış anlatım olmadan öğrenebilmesi.

**Giriş bağımlılığı:** S11. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** Yeni grup C01de tutmayı, C02de iki kişi dönmeyi, C03te karşı ağırlığı öğrenir ve görev seçimine döner.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-12-01 | Design | 6 | Öğrenme basamakları |
| SP-12-02 | Design/Gameplay | 18 | C01–C03 veri kartları |
| SP-12-03 | UX | 18 | Bağlamsal öğretici ve ping |
| SP-12-04 | UI | 16 | Ayarlar ve giriş tamamlama |
| SP-12-05 | QA/Localization | 12 | TR/EN ve kontrol testi |
| SP-12-06 | Design/Producer | 10 | Demo adayı ve gözlem soruları |

### SP-12-01 — Öğrenme basamakları

**Sorumlu:** Design. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** Tut/bırak, birlikte çevir, platforma koy, sabitle, vinç, teslim et adımlarını küçük bağlamsal hedeflere böl.

**Kartın kabulü:** Her adım tek yeni davranış öğretir; ilerleme host stateinden doğrulanır.

**Kart bağımlılığı:** SP-11-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-12-02 — C01–C03 veri kartları

**Sorumlu:** Design/Gameplay. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Üç görevin spawn, hedef, süre/kota ve 2/3/4 oyuncu ayarlarını doldur. Slice birleşik varyantını shipping C01 ile karıştırma.

**Kartın kabulü:** Üç görevin farklı karar sorusu ve geçerli item listesi var.

**Kart bağımlılığı:** SP-12-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-12-03 — Bağlamsal öğretici ve ping

**Sorumlu:** UX. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Eylem sonrası kısa açıklama, hatadan dönüş ipucu ve bakılan nesne/konum için ping kur.

**Kartın kabulü:** İki kişi sesli sohbet olmadan kritik hedefi ve ihtiyacı iletebiliyor.

**Kart bağımlılığı:** SP-12-01, SP-12-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-12-04 — Ayarlar ve giriş tamamlama

**Sorumlu:** UI. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Klavye/gamepad menü odağı, yeniden bağlama, hassasiyet, FOV, metin boyutu ve ses kanallarını bağla.

**Kartın kabulü:** Ayarlar oturum değişiminde korunur; input değişince ikonlar güncellenir.

**Kart bağımlılığı:** SP-12-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-12-05 — TR/EN ve kontrol testi

**Sorumlu:** QA/Localization. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** Türkçe karakter, uzun İngilizce metin, düğme remap ve gamepad ile tüm demo akışını dolaş.

**Kartın kabulü:** Kesilmiş zorunlu metin, tuzak menü odağı ve eksik eylem göstergesi yok.

**Kart bağımlılığı:** SP-12-02, SP-12-03, SP-12-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-12-06 — Demo adayı ve gözlem soruları

**Sorumlu:** Design/Producer. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** İlk anlama süresi, yardımsız teslimat, yanlış sabitleme ve görev terk nedenlerini ölçen form hazırla.

**Kartın kabulü:** S13 için sabit demo buildi ve yönlendirmesiz test yönergesi var.

**Kart bağımlılığı:** SP-12-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- Üç sözleşme çalışır; C01–C03ün her biri iki oyuncuyla tamamlanabilir.
- Ping ile temel koordinasyon mümkündür; renk ve ses tek bilgi kaynağı değildir.
- TR/EN ve hedef kontrol cihazları bütün menü akışında denendi.

## Risk ve kapsam kararı

**En önemli risk:** Öğreticiyi uzun metinle çözmek; gamepad odağını ve sessiz iletişimi unutmak.

**Kapasite yetmezse:** Uzun sinematik, tam seslendirme ve ayrı öğretici haritası yerine Avlu içi bağlamsal öğretim kullanılır.

**Kapı:** Bu sprintte ayrı milestone kapısı yok; sprint kabulü ve önceki açık kapılar geçerli.

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-014, ASK-023, ASK-058, ASK-060, ASK-062, ASK-063, ASK-064, ASK-068.

**Gereksinimler:** REQ-031, REQ-033, REQ-034, REQ-035, REQ-036.

**Var olan testler:** TC-031, TC-033, TC-034, TC-035, TC-036.

**Eski backlog kapsam izi:** W-025, W-026, W-027, W-032. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S13.md`

# S13 — Dış demo testi ve kapsam kararı

Durum: **PLANNED**. Örnek takvim: hafta 27–28. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yeniden tahmin edilecek çalışma paketi; bu saatler kesin efor tahmini değil.

**Sprint amacı:** Ürünün anlaşılması, eğlencesi ve teknik sorunlarına dış oyuncu kanıtı toplamak.

**Giriş bağımlılığı:** S12. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** 4–6 bağımsız grubun anonim oturum bulguları, en önemli üç sorun ve bir sonraki builddeki değişim gösterilir.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-13-01 | Producer/QA | 6 | Dış test protokolü |
| SP-13-02 | QA/Design | 18 | Gözlemli demo oturumları |
| SP-13-03 | Gameplay/UX | 18 | En yüksek etkili üç sorun |
| SP-13-04 | QA/Design | 16 | Hedefli tekrar test |
| SP-13-05 | Art/Producer | 12 | İçerik üretim tahmini |
| SP-13-06 | Burak/Producer | 10 | G3 ve ürün kapsamı dondurma |

### SP-13-01 — Dış test protokolü

**Sorumlu:** Producer/QA. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** 4–6 grup hedefle; en az iki grup iki, iki grup dört oyuncu olsun. Build, cihaz, ağ, başlangıç bilgisi ve kayıt iznini netleştir.

**Kartın kabulü:** Katılımcı bulma ve bekleme takvimi ayrı; örneklem pazar tahmini olarak sunulmuyor.

**Kart bağımlılığı:** SP-12-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-13-02 — Gözlemli demo oturumları

**Sorumlu:** QA/Design. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** 30–60 dakikalık turlarda ilk teslimat, yardım ihtiyacı, görev başarısı, yanlış anlama ve birlikte güldükleri anları kaydet.

**Kartın kabulü:** Her grubun ölçümü aynı tanımla tutuluyor; teknik başarısızlık oyun zorluğundan ayrılıyor.

**Kart bağımlılığı:** SP-13-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-13-03 — En yüksek etkili üç sorun

**Sorumlu:** Gameplay/UX. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Gözlemlerin ortak nedenlerine göre üç düzeltme seç; tek gruba özgü istekleri doğrudan özellik yapma.

**Kartın kabulü:** Her işin önceki gözlem ve beklenen davranışla bağlantısı var.

**Kart bağımlılığı:** SP-13-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-13-04 — Hedefli tekrar test

**Sorumlu:** QA/Design. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Düzeltmeleri en az iki grupla yeniden dene; öğrenme etkisini not et. Yeni grup mümkünse birini ona ayır.

**Kartın kabulü:** Değişimin iyileştirdiği ve bozduğu noktalar kayıtlı; kesin istatistik iddiası yok.

**Kart bağımlılığı:** SP-13-03.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-13-05 — İçerik üretim tahmini

**Sorumlu:** Art/Producer. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** Kabul edilmiş bir eşya ve bir modüler rota maliyetini ölç. 3 bina/12 sözleşme için kalan iş ve dış destek ihtiyacını güncelle.

**Kartın kabulü:** Gelecek sprint saatleri içerik throughputuyla karşılaştırılmış; sığmayan kapsam görünür.

**Kart bağımlılığı:** SP-13-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-13-06 — G3 ve ürün kapsamı dondurma

**Sorumlu:** Burak/Producer. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Demo bulgularıyla 3/12 üst hedefi koru veya 2 bina/8 sözleşmeye kes. Mağaza materyali için doğrulanmış ürün cümleleri çıkar.

**Kartın kabulü:** Onaylı kapsam, kesilenler ve yeniden tahmin kaydı var; yayın tarihi bu kapıda da garanti değil.

**Kart bağımlılığı:** SP-13-04, SP-13-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- Planlanan test örneklemi tamamlanır veya katılımcı eksikliği açık BLOCKED kalır.
- Yeni oyuncu taşıma/sabitleme/denge ayrımını açıklayabilir; tekrar eden tıkanıklıklar sahiplenilmiş.
- G3te kapsam sayısı ve üretim kapasitesi birlikte onaylanmış olmalı.

## Risk ve kapsam kararı

**En önemli risk:** Yakın arkadaş övgüsünü ticari talep veya retention ölçümü saymak.

**Kapasite yetmezse:** Testten önce geniş içerik üretimi başlatılmaz; demo zayıfsa G3 kurtarma sprinti eklenir.

**Kapı:** G3 — dış demo

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-006, ASK-007, ASK-060, ASK-065, ASK-068, ASK-069, ASK-071, ASK-073, ASK-075.

**Gereksinimler:** REQ-032, REQ-033, REQ-035, REQ-037, REQ-040.

**Var olan testler:** TC-032, TC-033, TC-035, TC-037, TC-040.

**Eski backlog kapsam izi:** W-028, W-032. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S14.md`

# S14 — İçerik üretim araçları ve eşya aileleri

Durum: **PLANNED**. Örnek takvim: hafta 29–30. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yeniden tahmin edilecek çalışma paketi; bu saatler kesin efor tahmini değil.

**Sprint amacı:** Onaylı görsel ve teknik örnekleri tekrar üretilebilir içerik sürecine çevirmek.

**Giriş bağımlılığı:** S13. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** Yeni bir eşya profil/prefab üzerinden eklenir; validator eksik tutma noktası ve duplicate IDyi yakalar.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-14-01 | Producer/Art | 6 | İçerik envanteri ve sıra |
| SP-14-02 | Tools | 18 | İçerik validatorları |
| SP-14-03 | Gameplay/Art | 18 | Kalan davranış aileleri |
| SP-14-04 | Level/Design | 16 | Modüler rota kitini bitirme |
| SP-14-05 | QA/Art | 12 | Varlık başına teknik kabul |
| SP-14-06 | Producer | 10 | Üretim hızını yeniden ölçme |

### SP-14-01 — İçerik envanteri ve sıra

**Sorumlu:** Producer/Art. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** G3 kapsamına göre gerekli aile, model, prefab, materyal ve sözleşmeleri say. C04 ve final Avlu açıklarını aynı envantere al.

**Kartın kabulü:** Her zorunlu assetin sahibi, reused/custom durumu ve kabul tarihi hedefi var.

**Kart bağımlılığı:** SP-13-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-14-02 — İçerik validatorları

**Sorumlu:** Tools. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Ölçek, collider, Rigidbody, socket, ID, profil ve lisans kaydı kontrollerini build öncesine bağla. Yanlış fizik temsilini hata say.

**Kartın kabulü:** Eksik zorunlu socket ve duplicate ID test fixturelarında build engelleniyor.

**Kart bağımlılığı:** SP-14-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-14-03 — Kalan davranış aileleri

**Sorumlu:** Gameplay/Art. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Onaylıysa yuvarlanmaya eğilimli ve dengesiz kütle merkezli örnekleri veri profiliyle ekle. İlk dört ailede shared pipelineı koru.

**Kartın kabulü:** Yeni davranış iki/dört kişilik platform testini geçiyor; ailenin model sayısıyla aynı olmadığı açık.

**Kart bağımlılığı:** SP-14-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-14-04 — Modüler rota kitini bitirme

**Sorumlu:** Level/Design. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Kapı, pencere, balkon, zemin ve teslim bölgesi modüllerini ölçü/pivot standardıyla paketle; Avlu C04ü veriye ekle.

**Kartın kabulü:** Yeni rota aynı parçalardan geometrik olarak geçerli oluşturulabiliyor; C04 mandatory listesi doğru.

**Kart bağımlılığı:** SP-14-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-14-05 — Varlık başına teknik kabul

**Sorumlu:** QA/Art. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** Yeni örnekleri kapı/stack/drop/secure/iki holder ve düşük kalite ışık sahnesinden geçir.

**Kartın kabulü:** Onaysız asset shipping registryye giremiyor; collider performans sonucu kayıtlı.

**Kart bağımlılığı:** SP-14-02, SP-14-03, SP-14-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-14-06 — Üretim hızını yeniden ölçme

**Sorumlu:** Producer. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Yeni örneklerin gerçek saatlerini ve kusur yeniden açılma oranını kaydet; S15–S17yi bu veriye göre böl.

**Kartın kabulü:** 80 saatlik sprint kapsamına sığmayan modeller için ek sprint veya görünür kesinti var.

**Kart bağımlılığı:** SP-14-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- Onaylı her davranış ailesinin en az bir teknik kabul edilmiş örneği bulunmalı.
- C04 ve bina 2/3 için modüler kit hazır olmalı.
- Art throughput kanıtı olmadan sonraki iki bina bitişi taahhüt edilmez.

## Risk ve kapsam kararı

**En önemli risk:** Yeni her model için özel script üretip içerik maliyetini katlamak.

**Kapasite yetmezse:** Önce model varyasyonları azaltılır; aile sayısı altıdan dörde düşürülecekse kapsam belgeleri beraber değişir.

**Kapı:** Bu sprintte ayrı milestone kapısı yok; sprint kabulü ve önceki açık kapılar geçerli.

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-039, ASK-041, ASK-049, ASK-050, ASK-057, ASK-060, ASK-061, ASK-066.

**Gereksinimler:** REQ-020, REQ-024, REQ-025, REQ-026, REQ-033.

**Var olan testler:** TC-020, TC-024, TC-025, TC-026, TC-033.

**Eski backlog kapsam izi:** W-015, W-019, W-020, W-025. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S15.md`

# S15 — Sokak binası ve C05–C08

Durum: **PLANNED**. Örnek takvim: hafta 31–32. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yeniden tahmin edilecek çalışma paketi; bu saatler kesin efor tahmini değil.

**Sprint amacı:** Aynı kurallarla farklı taşıma kararları yaratan ikinci oynanabilir bina üretmek.

**Giriş bağımlılığı:** S14. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** C05 dönüş, C06 ping/kör nokta, C07 sefer sırası ve C08 kırılgan yük kararları gösterilir.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-15-01 | Level/Design | 6 | Bina 2 ölçü ve sözleşme planı |
| SP-15-02 | Level | 18 | Sokak oynanabilir geometri |
| SP-15-03 | Design/Gameplay | 18 | Dört görev verisi |
| SP-15-04 | Art/Audio | 16 | Bina kimliği ve çevre |
| SP-15-05 | QA | 12 | 2/3/4 oyuncu rota taraması |
| SP-15-06 | Performance/Producer | 10 | İçerik maliyet ve kusur kapanışı |

### SP-15-01 — Bina 2 ölçü ve sözleşme planı

**Sorumlu:** Level/Design. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** Kaynak/hedef rotalarını, görüş sınırlarını ve dört görevdeki farklı planlama sorusunu çiz.

**Kartın kabulü:** Her mandatory yükün geometrik geçişi gri sahnede kanıtlı.

**Kart bağımlılığı:** SP-14-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-15-02 — Sokak oynanabilir geometri

**Sorumlu:** Level. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Onaylı modüllerle cephe/oda/balkon/iniş bölgelerini kur; kapı collisionını oyun ölçüsüyle koru.

**Kartın kabulü:** C05–C08 rotaları yeni fizik kodu gerektirmeden çalışıyor.

**Kart bağımlılığı:** SP-15-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-15-03 — Dört görev verisi

**Sorumlu:** Design/Gameplay. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Spawn listesi, süre/kota, recovery ve oyuncu sayısı profillerini yaz. C07de sefer koşulunu açık kuralla doğrula.

**Kartın kabulü:** Her görevin otomatik veri kontrolü geçiyor; tek sefer sömürüsü test edilmiş.

**Kart bağımlılığı:** SP-15-01, SP-15-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-15-04 — Bina kimliği ve çevre

**Sorumlu:** Art/Audio. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Sınırlı Türkiye apartman detayları, tabela ve ortam sesini ortak palette ekle.

**Kartın kabulü:** Görsel farklılık yeni shader ailesi veya gereksiz dinamik obje yığını yaratmıyor.

**Kart bağımlılığı:** SP-15-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-15-05 — 2/3/4 oyuncu rota taraması

**Sorumlu:** QA. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** Dört görevi farklı oyuncu sayılarıyla tamamla; kör noktada sesli sohbet olmadan ping kullanımını test et.

**Kartın kabulü:** Zorunlu eşya rotası softlock yaratmıyor; oyuncu sayısı yüzünden imkânsız görev yok.

**Kart bağımlılığı:** SP-15-02, SP-15-03, SP-15-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-15-06 — İçerik maliyet ve kusur kapanışı

**Sorumlu:** Performance/Producer. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Hostta bina 1 ve 2yi aynı protokolle ölç; yeni dekor/mesh collider sorunlarını gider veya azalt.

**Kartın kabulü:** Yeni sahne yükü profil raporunda; S16 başlamadan temel rota engelleri kapalı.

**Kart bağımlılığı:** SP-15-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- C05–C08 ayrı karar sunmalı ve 2/3/4 oyuncuda tamamlanmalı.
- Oynanışla ilgisiz çevre nesneleri ağ üzerinden çoğaltılmamalı.
- Bina 2 kaynak/lisans ve perf kayıtları envantere bağlı olmalı.

## Risk ve kapsam kararı

**En önemli risk:** İkinci binayı yalnız farklı renkli birinci bina yapmak veya dar kapıyı imkânsızlaştırmak.

**Kapasite yetmezse:** Özgün model miktarı önce azaltılır. 80 saat yetmezse bu sprint geometri+veri ve sanat+kabul olarak ikiye bölünür.

**Kapı:** Bu sprintte ayrı milestone kapısı yok; sprint kabulü ve önceki açık kapılar geçerli.

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-056, ASK-057, ASK-059, ASK-060, ASK-061, ASK-065.

**Gereksinimler:** REQ-025, REQ-032, REQ-033, REQ-037.

**Var olan testler:** TC-025, TC-032, TC-033, TC-037.

**Eski backlog kapsam izi:** W-024, W-025, W-028. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S16.md`

# S16 — Teras binası ve C09–C12

Durum: **PLANNED**. Örnek takvim: hafta 33–34. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yeniden tahmin edilecek çalışma paketi; bu saatler kesin efor tahmini değil.

**Sprint amacı:** Karşı ağırlık, yük sırası ve ekip iş paylaşımını birleştiren son içerik dilimini tamamlamak.

**Giriş bağımlılığı:** S15. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** C09 rota, C10 yük dağılımı, C11 iş sırası ve C12 birleşik taşıma görevleri oynanır.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-16-01 | Level/Design | 6 | Bina 3 kapsam kontrolü |
| SP-16-02 | Level | 18 | Teras geometri ve güvenli alanlar |
| SP-16-03 | Design/Gameplay | 18 | C09–C12 görev kartları |
| SP-16-04 | Art/Audio | 16 | Teras görsel bitiriş |
| SP-16-05 | QA | 12 | Final görev stres turu |
| SP-16-06 | Producer/QA | 10 | İçerik envanteri kapanış taslağı |

### SP-16-01 — Bina 3 kapsam kontrolü

**Sorumlu:** Level/Design. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** G3te üçüncü bina korunmuşsa başlat; kesilmişse sprinti mevcut içerik kalitesine ata. Oyuncu sayısına göre iş yükünü veriyle düzenle.

**Kartın kabulü:** Bina kapsamı kullanıcı kararıyla uyumlu; iki kişi için zorunlu eşzamanlı üç eylem yok.

**Kart bağımlılığı:** SP-15-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-16-02 — Teras geometri ve güvenli alanlar

**Sorumlu:** Level. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Açık rota, platform yanaşma, kat sınırları ve kurtarma noktalarını modüler kitle kur.

**Kartın kabulü:** Platform rotası ve her büyük eşyanın dönüş hacmi doğrulanmış.

**Kart bağımlılığı:** SP-16-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-16-03 — C09–C12 görev kartları

**Sorumlu:** Design/Gameplay. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Birden çok destek yükü, görev sırası ve final birleşimini mevcut sistemlerle tanımla.

**Kartın kabulü:** Yeni görevler ayrı ağ actor veya fizik çözümü gerektirmiyor.

**Kart bağımlılığı:** SP-16-01, SP-16-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-16-04 — Teras görsel bitiriş

**Sorumlu:** Art/Audio. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Kontrollü uzak plan, sınırlı çevre detayı ve ses atmosferini ekle; gölge/LOD mesafelerini sahneye göre ayarla.

**Kartın kabulü:** Uzak manzara stil ve perf bütçesi içinde; dekor gameplay collisionını engellemiyor.

**Kart bağımlılığı:** SP-16-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-16-05 — Final görev stres turu

**Sorumlu:** QA. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** C12yi 2/3/4 oyuncuyla; bir misafir kopması, recovery ve hasarlı yükle bitir. C10da destek kütlesini denetle.

**Kartın kabulü:** Teslimat/ödül doğru; yük bir kez sayılıyor; kopma sonrası tur kuralları değişmiyor.

**Kart bağımlılığı:** SP-16-02, SP-16-03, SP-16-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-16-06 — İçerik envanteri kapanış taslağı

**Sorumlu:** Producer/QA. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** 12 görevin veya onaylı azaltılmış listenin eksiklerini çıkar; placeholder ve geçici sesleri isimleriyle listele.

**Kartın kabulü:** S17 alpha kapanışına kalan işler gerçek liste; görünmez içerik borcu yok.

**Kart bağımlılığı:** SP-16-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- C09–C12 veya onaylı azaltılmış kapsam doğrulanmış olmalı.
- Final görev iki kişiyle temel co-op yetenekleri kullanılarak bitirilebilir.
- Açık placeholder ve asset borçları alpha kararından önce görünürdür.

## Risk ve kapsam kararı

**En önemli risk:** Dört oyunculuk finali iki oyuncuyla imkânsız kılmak veya gereksiz yeni mekaniğe dayandırmak.

**Kapasite yetmezse:** İlk kesilecek tam içerik üçüncü binadır; kesilirse toplam sözleşme 12den 8e güncellenir ve mağaza metnine yansır.

**Kapı:** Bu sprintte ayrı milestone kapısı yok; sprint kabulü ve önceki açık kapılar geçerli.

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-018, ASK-023, ASK-059, ASK-060, ASK-061, ASK-065.

**Gereksinimler:** REQ-005, REQ-025, REQ-032, REQ-033, REQ-037.

**Var olan testler:** TC-005, TC-025, TC-032, TC-033, TC-037.

**Eski backlog kapsam izi:** W-024, W-025, W-028. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S17.md`

# S17 — İlerleme dengesi ve alpha içerik kilidi

Durum: **PLANNED**. Örnek takvim: hafta 35–36. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yeniden tahmin edilecek çalışma paketi; bu saatler kesin efor tahmini değil.

**Sprint amacı:** Onaylı bütün görevleri, basit ekipman ilerlemesini ve içeriğin tamamlanma durumunu birleştirmek.

**Giriş bağımlılığı:** S16. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** Yeni kampanyadan son göreve kadar ilerleme simüle edilir; başarısız/az kazançlı turlarla da softlock oluşmadığı gösterilir.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-17-01 | Design | 6 | Ekonomi ve unlock tablosu |
| SP-17-02 | Gameplay | 18 | İlerleme ve satın alma tamamlama |
| SP-17-03 | Design/QA | 18 | Bütün görev dengeleme |
| SP-17-04 | Art/Localization | 16 | Placeholder ve içerik kapanışı |
| SP-17-05 | QA | 12 | Kampanya ve migration regresyonu |
| SP-17-06 | Burak/Producer | 10 | G4 karar ve feature freeze |

### SP-17-01 — Ekonomi ve unlock tablosu

**Sorumlu:** Design. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** Başlangıç bakiye, görev ödülü, hasar etkisi ve mevcut ekipman yükseltmelerini tabloya dök. Yeni yükseltme ağacı icat etme.

**Kartın kabulü:** Her kilidin önkoşulu ve geriye dönüş yolu var; gerekli ekipman alınamaz durumda kalınmıyor.

**Kart bağımlılığı:** SP-16-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-17-02 — İlerleme ve satın alma tamamlama

**Sorumlu:** Gameplay. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Host doğrulamalı satın alma, bakiye, unlock ve save işlemini mevcut atomik sınırla bitir.

**Kartın kabulü:** Tekrarlanan satın alma tek sonuç; misafir host kampanyasını yetkisiz değiştiremiyor.

**Kart bağımlılığı:** SP-17-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-17-03 — Bütün görev dengeleme

**Sorumlu:** Design/QA. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** 2/3/4 oyuncu tamamlama süreleri, hasar ve başarısızlık nedenlerinden kota/süreleri ayarla.

**Kartın kabulü:** Aynı görevin farkı salt süre azaltımı değil; iki kişi için kapasite yeterli.

**Kart bağımlılığı:** SP-17-01, SP-17-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-17-04 — Placeholder ve içerik kapanışı

**Sorumlu:** Art/Localization. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Shipping varlık, TR/EN metin, ses ve ikon envanterini tarayıp zorunlu eksikleri kapat.

**Kartın kabulü:** Açık placeholder yok veya kapsamdan çıkarılmış; her asset kaynak kaydına bağlı.

**Kart bağımlılığı:** SP-17-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-17-05 — Kampanya ve migration regresyonu

**Sorumlu:** QA. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** Eski demodan desteklenen kayıt geçişi, başarısız tur, tekrar ödül ve tam kampanya turunu test et.

**Kartın kabulü:** Kampanya ilerlemesi ve checkpoint doğru; test fixturelarına yeni içerik kimlikleri ekli.

**Kart bağımlılığı:** SP-17-02, SP-17-03, SP-17-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-17-06 — G4 karar ve feature freeze

**Sorumlu:** Burak/Producer. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Onaylı bina/görev/aile sayılarını gerçek build registrysiyle karşılaştır; beta kusur kuyruğunu sırala.

**Kartın kabulü:** G4 PASS ise yeni özellik ve görev eklenmez; sonraki iş doğruluk/konfor/performans olur.

**Kart bağımlılığı:** SP-17-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- Onaylı tüm içerik oynanabilir ve zorunlu TR/EN metinleri mevcut.
- Yeni kampanya herhangi bir zorunlu satın alma nedeniyle softlocka girmiyor.
- G4 kararı registryden sayılarla doğrulanır; planned asset completed sayılmaz.

## Risk ve kapsam kararı

**En önemli risk:** Ekonomiyle zorunlu ekipmanı erişilemez kılmak veya content lockta placeholder saklamak.

**Kapasite yetmezse:** Üçüncü bina, özel varyant ve pahalı olaylar kesme sırasını izler; kayıt güvenliği ve temel erişim korunur.

**Kapı:** G4 — content complete / alpha

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-007, ASK-023, ASK-024, ASK-033, ASK-043, ASK-060, ASK-061, ASK-069.

**Gereksinimler:** REQ-013, REQ-018, REQ-019, REQ-024, REQ-033, REQ-036.

**Var olan testler:** TC-013, TC-018, TC-019, TC-024, TC-033, TC-036.

**Eski backlog kapsam izi:** W-014, W-018, W-025. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S18.md`

# S18 — Erişilebilirlik ve son kullanıcı deneyimi

Durum: **PLANNED**. Örnek takvim: hafta 37–38. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yeniden tahmin edilecek çalışma paketi; bu saatler kesin efor tahmini değil.

**Sprint amacı:** İçeriği dondurulmuş oyunun bütün akışlarını anlaşılır, okunabilir ve kontrol edilebilir yapmak.

**Giriş bağımlılığı:** S17. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** Oyun klavye/fare ve gamepad ile; büyük metin, azaltılmış kamera hareketi ve sessiz iletişim koşullarında tamamlanır.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-18-01 | UX/QA | 6 | Akış ve erişim denetimi |
| SP-18-02 | UX | 18 | Kontrol ve konfor kusurları |
| SP-18-03 | UI/Localization | 18 | Metin ve hata açıklamaları |
| SP-18-04 | Audio/UX | 16 | İşitsel ve görsel karşılıklar |
| SP-18-05 | QA | 12 | Cihaz ve çözünürlük matrisi |
| SP-18-06 | Producer/QA | 10 | UX kapanış kaydı |

### SP-18-01 — Akış ve erişim denetimi

**Sorumlu:** UX/QA. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** Menü, lobi, aktif görev, sonuç, kayıt hatası ve host loss için aynı kontrol listesini uygula.

**Kartın kabulü:** Her zorunlu eylemin klavye ve hedef gamepad karşılığı var.

**Kart bağımlılığı:** SP-17-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-18-02 — Kontrol ve konfor kusurları

**Sorumlu:** UX. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Rebind çakışması, odağın kaybolması, toggle/hold seçenekleri, kamera shake ve hassasiyet kusurlarını kapat.

**Kartın kabulü:** Ayar değişikliği tutma/menü durumunu kilitlemiyor; varsayılana dönüş çalışıyor.

**Kart bağımlılığı:** SP-18-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-18-03 — Metin ve hata açıklamaları

**Sorumlu:** UI/Localization. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** TR/EN son metinleri, sayı biçimi, uzun metin taşması ve dinamik input ikonlarını düzelt.

**Kartın kabulü:** Ekranda ham anahtar yok; bütün retler kullanıcıya yapabileceği eylemi açıklıyor.

**Kart bağımlılığı:** SP-18-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-18-04 — İşitsel ve görsel karşılıklar

**Sorumlu:** Audio/UX. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Kritik seslerin görsel karşılığını, master/music/SFX denetimini, motor loop şiddetini ve pingi tamamla.

**Kartın kabulü:** Ses kapalıyken teslim/tehlike/kilit okunabilir; renk farkı tek uyarı değil.

**Kart bağımlılığı:** SP-18-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-18-05 — Cihaz ve çözünürlük matrisi

**Sorumlu:** QA. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** 1080p hedef yanında yaygın en-boy oranları, UI ölçeği ve focus kaybını test et. Steam Deck desteğini ancak seçilmiş cihazda doğrulanırsa ayrı hedef yap.

**Kartın kabulü:** Kesilmiş düğme ve ulaşılmaz menü yok; test edilmemiş cihaz desteği iddia edilmiyor.

**Kart bağımlılığı:** SP-18-02, SP-18-03, SP-18-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-18-06 — UX kapanış kaydı

**Sorumlu:** Producer/QA. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Önceki demo sorunlarının tekrarını karşılaştır, P1 erişim engellerini kapat ve P2leri sahiplen.

**Kartın kabulü:** Feature freeze korunmuş; UX değişikliklerinin regresyon etkileri test edilmiş.

**Kart bağımlılığı:** SP-18-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- TR/EN, hedef input cihazı ve kritik ekran matrisi tamamlanır.
- Temel görevi engelleyen erişim kusuru yok.
- Düşük görsel kalite ayarı gameplay collider veya zorunlu objeleri değiştirmez.

## Risk ve kapsam kararı

**En önemli risk:** Erişilebilirliği sonradan eklenen bir ayar ekranıyla sınırlamak.

**Kapasite yetmezse:** Yeni diller ve desteklenmemiş platform vaatleri eklenmez; mevcut TR/EN kalitesi tamamlanır.

**Kapı:** Bu sprintte ayrı milestone kapısı yok; sprint kabulü ve önceki açık kapılar geçerli.

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-013, ASK-055, ASK-062, ASK-063, ASK-064, ASK-070.

**Gereksinimler:** REQ-031, REQ-034, REQ-035, REQ-036.

**Var olan testler:** TC-031, TC-034, TC-035, TC-036.

**Eski backlog kapsam izi:** W-026, W-027. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S19.md`

# S19 — Ölçüme dayalı optimizasyon

Durum: **PLANNED**. Örnek takvim: hafta 39–40. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yeniden tahmin edilecek çalışma paketi; bu saatler kesin efor tahmini değil.

**Sprint amacı:** En ağır gerçek oyun sahnesinde host ve client için dondurulmuş performans bütçelerini sağlamak.

**Giriş bağımlılığı:** S18. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** Aynı cihaz, build ayarı, kamera ve yükte önce/sonra CPU/GPU, p95/p99, bellek ve trafik grafikleri karşılaştırılır.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-19-01 | Performance | 6 | Benchmark koşullarını dondur |
| SP-19-02 | Tech/Gameplay | 18 | CPU ve fizik darboğazı |
| SP-19-03 | TechArt | 18 | GPU ve render darboğazı |
| SP-19-04 | Tech | 16 | Bellek, yükleme ve hot path |
| SP-19-05 | QA/Performance | 12 | 2 saatlik soak ve kıyas |
| SP-19-06 | Producer/Tech | 10 | Donanım hedefi kararı |

### SP-19-01 — Benchmark koşullarını dondur

**Sorumlu:** Performance. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** Referans min ve önerilen cihaz; kalite, çözünürlük, build backend, ısınma süresi ve en ağır yük listesini sabitle.

**Kartın kabulü:** Önce/sonra ölçümler aynı koşulu kullanır; VSync ve profiler maliyeti kayıtlı.

**Kart bağımlılığı:** SP-18-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-19-02 — CPU ve fizik darboğazı

**Sorumlu:** Tech/Gameplay. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Aktif Rigidbody, temas çifti, joint, destek kümesi ve komut işleme maliyetini profilden sırala; en pahalı doğrulanmış sorunu düzelt.

**Kartın kabulü:** Fizik doğruluğu matrisi hâlâ geçiyor; daha hızlı ama farklı oyun kuralı oluşmuyor.

**Kart bağımlılığı:** SP-19-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-19-03 — GPU ve render darboğazı

**Sorumlu:** TechArt. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Gölge mesafesi, renderer/material çeşitliliği, şeffaflık, LOD ve ışık maliyetini ölçüp düzenle.

**Kartın kabulü:** Aynı sahnede GPU süresi düşüyor; önemli tutma/tehlike bilgisi kaybolmuyor.

**Kart bağımlılığı:** SP-19-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-19-04 — Bellek, yükleme ve hot path

**Sorumlu:** Tech. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Allocation kaynakları, gereksiz instantiate, cache ve sahne kaynak ömrünü düzelt. Havuz boyutu yük testine bağlı olsun.

**Kartın kabulü:** Tekrarlanan turlarda bellek sınırsız artmıyor; loading bütçesi ölçülmüş.

**Kart bağımlılığı:** SP-19-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-19-05 — 2 saatlik soak ve kıyas

**Sorumlu:** QA/Performance. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** Dört oyuncu host ve remote istemcide uzun oturum; tekrarlanan görev yüklemesi ve ağır yük stresini uygula.

**Kartın kabulü:** Açık sızıntı yok; p95/p99 ve peak memory hedefle kıyaslanmış.

**Kart bağımlılığı:** SP-19-02, SP-19-03, SP-19-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-19-06 — Donanım hedefi kararı

**Sorumlu:** Producer/Tech. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Bütçe tutmuyorsa önce dekor/kalite ölçeğini azalt; minimum cihaz veya FPS hedefi değişikliği için açık ürün kararı yaz.

**Kartın kabulü:** Minimum gereksinim yalnız ölçülen cihazlarla ilişkilendirilmiş; gizli hedef düşürme yok.

**Kart bağımlılığı:** SP-19-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- Dondurulmuş referans cihazda 1080p/60 hedefi için kabul ölçütleri sağlanır veya G5 blokajı yazılır.
- p95/p99, peak RAM/VRAM ve host fizik/ağ maliyeti raporlu.
- Optimizasyon sonrası ortak taşıma, hasar, anchor ve teslimat doğruluğu değişmez.

## Risk ve kapsam kararı

**En önemli risk:** Poligon sayısını azaltıp asıl fizik/ağ/GC darboğazını kaçırmak.

**Kapasite yetmezse:** Uzak dekor, gölge ve kozmetik efekt yoğunluğu önce azaltılır; zorunlu fizik nesnesi veya ağ doğruluğu kesilmez.

**Kapı:** Bu sprintte ayrı milestone kapısı yok; sprint kabulü ve önceki açık kapılar geçerli.

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-040, ASK-044, ASK-053, ASK-054, ASK-065, ASK-066.

**Gereksinimler:** REQ-005, REQ-029, REQ-030, REQ-037.

**Var olan testler:** TC-005, TC-029, TC-030, TC-037.

**Eski backlog kapsam izi:** W-022, W-023, W-028. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S20.md`

# S20 — Beta: ağ, kayıt ve tam regresyon

Durum: **PLANNED**. Örnek takvim: hafta 41–42. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yeniden tahmin edilecek çalışma paketi; bu saatler kesin efor tahmini değil.

**Sprint amacı:** İçerik tamamlandıktan sonra çapraz sistem hatalarını ve uzun oturum kusurlarını kapatmak.

**Giriş bağımlılığı:** S19. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** Seçilmiş bütün görevlerde ağ profili, reconnect, duplicate sonuç, kayıt kesintisi ve yeniden açılış sonuçları izlenebilir raporda gösterilir.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-20-01 | QA | 6 | Risk temelli beta matrisi |
| SP-20-02 | QA/Network | 18 | Ağ regresyonu |
| SP-20-03 | QA/Tech | 18 | Kayıt ve protokol fault testleri |
| SP-20-04 | Gameplay/Network | 16 | Engelleyici hata kapanışı |
| SP-20-05 | QA | 12 | Release backend ve uzun oturum |
| SP-20-06 | Producer/QA | 10 | RC giriş kararı |

### SP-20-01 — Risk temelli beta matrisi

**Sorumlu:** QA. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** Bütün görevlerde normal 2/3/4 smoke; en ağır görevlerde geniş ağ/fault matrisi seç. Her olası kombinasyonu körlemesine çoğaltma.

**Kartın kabulü:** Her P0/P1 mekanizmanın hangi görev/cihazda kapsandığı belli.

**Kart bağımlılığı:** SP-19-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-20-02 — Ağ regresyonu

**Sorumlu:** QA/Network. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** 150 ms/%1 hedef ve 250 ms/%3 stres; reconnect, stale komut, roster kilidi ve driver kopmasını shipping topolojide test et.

**Kartın kabulü:** Hedefte görev tamamlanır; stres koşulunda güvenli bozulma var, state corruption yok.

**Kart bağımlılığı:** SP-20-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-20-03 — Kayıt ve protokol fault testleri

**Sorumlu:** QA/Tech. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Desteklenen şemalar, duplicate commit, yedek geri dönüş, bozuk payload ve rate limit testlerini son içerikle uygula.

**Kartın kabulü:** Bakiye veya terminal state çoğalmıyor; bozuk kayıt açık hata/sağlam yedek yoluna gidiyor.

**Kart bağımlılığı:** SP-20-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-20-04 — Engelleyici hata kapanışı

**Sorumlu:** Gameplay/Network. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Bulunan P0/P1leri kök nedeniyle düzelt; repro fixture veya senaryo kaydı ekle.

**Kartın kabulü:** Her kapanan kusur ilgili son buildde yeniden doğrulanmış.

**Kart bağımlılığı:** SP-20-02, SP-20-03.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-20-05 — Release backend ve uzun oturum

**Sorumlu:** QA. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** Hedef release backend/stripping ile test et; geliştirme buildine özgü davranışa güvenme. 2 saatlik dört oyuncu soak yap.

**Kartın kabulü:** AOT/stripping veya plugin binary hatası yok; uzun oturum kanıtı mevcut.

**Kart bağımlılığı:** SP-20-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-20-06 — RC giriş kararı

**Sorumlu:** Producer/QA. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Açık kusurları şiddet/öncelik/çözüm sahibiyle triage et. P0/P1 sıfırlanana kadar RCyi engelle.

**Kartın kabulü:** S21 giriş için bilinen sorunlar ve regresyon raporu imzalı.

**Kart bağımlılığı:** SP-20-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- P0/P1 açık değil; kapatılan kusurlar yeniden üretimle doğrulanmış.
- Release backendde ağ/kayıt/kimlik yolu çalışıyor.
- Bütün görevlerin 2/3/4 smoke kaydı ve kritik görevlerin geniş stres kanıtı var.

## Risk ve kapsam kararı

**En önemli risk:** Yeni içerikte beliren eski bir ortak taşıma veya kayıt hatasını yalnız eski sahnede test etmek.

**Kapasite yetmezse:** Yeni içerik/özellik eklenmez; kritik kusur varsa RC kayar, test eşiği düşmez.

**Kapı:** Bu sprintte ayrı milestone kapısı yok; sprint kabulü ve önceki açık kapılar geçerli.

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-028, ASK-032, ASK-033, ASK-034, ASK-035, ASK-043, ASK-067, ASK-070.

**Gereksinimler:** REQ-006, REQ-011, REQ-016, REQ-017, REQ-018, REQ-022, REQ-038.

**Var olan testler:** TC-006, TC-011, TC-016, TC-017, TC-018, TC-022, TC-038.

**Eski backlog kapsam izi:** W-013, W-014, W-016, W-029. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S21.md`

# S21 — Release candidate ve dağıtım provası

Durum: **PLANNED**. Örnek takvim: hafta 43–44. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yeniden tahmin edilecek çalışma paketi; bu saatler kesin efor tahmini değil.

**Sprint amacı:** Satılacak build, paketleme, mağaza iddiaları ve geri dönüş sürecini doğrulamak.

**Giriş bağımlılığı:** S20. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** Temiz makineye RC kurulur; ağ ve kayıt smoke yapılır; önceki build ve uyumlu kayıt politikasıyla geri dönüş prova edilir.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-21-01 | Producer | 6 | RC ve dış bağımlılık kontrolü |
| SP-21-02 | Build | 18 | RC paketleme ve semboller |
| SP-21-03 | QA/Release | 18 | Kurulum ve rollback provası |
| SP-21-04 | Producer/Art | 16 | Mağaza ve içerik doğruluğu |
| SP-21-05 | QA | 12 | Temiz makine son smoke |
| SP-21-06 | Burak/Producer/QA | 10 | G5 imza ve yayın kararı paketi |

### SP-21-01 — RC ve dış bağımlılık kontrolü

**Sorumlu:** Producer. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** Dağıtım hesabı, hedef kanal, mağaza varlıkları, güncel inceleme/bekleme koşulları ve yayın yetkilisini kontrol et. Süreleri güncel resmî kaynaktan o gün doğrula.

**Kartın kabulü:** Dış bekleme süresi plan üzerinde; bu dosyada sabit Steam inceleme süresi uydurulmuyor.

**Kart bağımlılığı:** SP-20-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-21-02 — RC paketleme ve semboller

**Sorumlu:** Build. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Exact commit, paket lock, content hash, release backend, log seviyesi ve debug symbol arşivini üret.

**Kartın kabulü:** Aynı kaynaktan yeniden build alınabiliyor; test aracı/anahtar shipping pakette yok.

**Kart bağımlılığı:** SP-21-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-21-03 — Kurulum ve rollback provası

**Sorumlu:** QA/Release. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Temiz install, update, önceki kayıt ve önceki build dönüş senaryolarını test et. Yeni save şeması eski buildle uyumsuzsa backup/forward fix politikası uygula.

**Kartın kabulü:** Sadece binary rollback yapılınca kayıt kaybı olmayacağı kanıtlı veya rollback bloke ve güvenli alternatif yazılı.

**Kart bağımlılığı:** SP-21-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-21-04 — Mağaza ve içerik doğruluğu

**Sorumlu:** Producer/Art. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Gerçek buildden görüntü, açıklama, kontrol ve sistem gereksinimlerini hazırla. Lisans ve oyunda görülen AI içerik kayıtlarını mevcut platform formuna göre gözden geçir.

**Kartın kabulü:** 2–4 oyuncu, içerik sayısı ve cihaz iddiaları test kanıtıyla eşleşiyor.

**Kart bağımlılığı:** SP-21-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-21-05 — Temiz makine son smoke

**Sorumlu:** QA. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** Seçilen minimum/önerilen cihazlarda başlat, katıl, taşı, bağla, teslim et, kaydet, yeniden aç ve host loss uygula.

**Kartın kabulü:** RC artifact kimliği ile test edilen artifact aynı; P0/P1 yok.

**Kart bağımlılığı:** SP-21-02, SP-21-03.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-21-06 — G5 imza ve yayın kararı paketi

**Sorumlu:** Burak/Producer/QA. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Build, kusur listesi, performans, lisans, rollback ve destek planını tek karar kaydında topla.

**Kartın kabulü:** Teknik olarak release ready durumu kanıtlı; gerçek yayın ayrı açık ürün kararıyla yapılacak.

**Kart bağımlılığı:** SP-21-03, SP-21-04, SP-21-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- G5 dosyasında gerçek RC kimliği ve tüm zorunlu kanıt bağlantıları var.
- Release build test edilmiş; mağaza açıklaması vaat edilmeyen özellik içermiyor.
- Dağıtım ve geri dönüş prosedürü kayıt uyumluluğunu birlikte ele alıyor.

## Risk ve kapsam kararı

**En önemli risk:** Build hazır olmasını mağaza incelemesi, yayın yetkisi ve dağıtım sürecinin hazır olmasıyla eşitlemek.

**Kapasite yetmezse:** Mağaza incelemesi/erişim gecikirse yayın tarihi kayar; buildi onaysız kanala göndererek telafi edilmez.

**Kapı:** G5 — release candidate

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-042, ASK-048, ASK-067, ASK-069, ASK-070, ASK-075.

**Gereksinimler:** REQ-024, REQ-038, REQ-040.

**Var olan testler:** TC-024, TC-038, TC-040.

**Eski backlog kapsam izi:** W-018, W-029, W-032. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S22.md`

# S22 — Yayın hazırlığı ve kontrollü çıkış

Durum: **PLANNED**. Örnek takvim: hafta 45–46. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yeniden tahmin edilecek çalışma paketi; bu saatler kesin efor tahmini değil.

**Sprint amacı:** Yayın kararı alınmışsa hazır sürümü kontrollü dağıtmak; ilk sorunları hızlı sınıflandırmak.

**Giriş bağımlılığı:** S21. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** Go/no-go kaydı, doğru kanaldaki buildin temiz kurulum smoke sonucu ve ilk olay takip panosu gösterilir.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-22-01 | Burak/Producer | 6 | Go/no-go toplantısı |
| SP-22-02 | Release | 18 | Dağıtım uygulaması |
| SP-22-03 | QA | 18 | Canlı kanal smoke |
| SP-22-04 | Support/Tech | 16 | İlk olay toplama |
| SP-22-05 | Tech/QA | 12 | Acil düzeltme provası veya uygulama |
| SP-22-06 | Producer | 10 | Çıkış raporu |

### SP-22-01 — Go/no-go toplantısı

**Sorumlu:** Burak/Producer. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** G5in hâlâ geçerli olduğunu, açık dış blokajları, destek kapasitesini ve yayın yetkisini doğrula.

**Kartın kabulü:** Yayın kararı kişi/tarih/build ile kayıtlı; karar yoksa sprint hazırlık olarak sürer.

**Kart bağımlılığı:** SP-21-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-22-02 — Dağıtım uygulaması

**Sorumlu:** Release. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Açık yetki ve hesap erişimi varsa onaylı kanala RCyi yayınla; yoksa yüklemeye hazır paketi ve adımları tamamla, engeli bildir.

**Kartın kabulü:** Yayınlandı beyanı gerçek kanal/build gözlemine dayanır; plan yazılması yayın sayılmaz.

**Kart bağımlılığı:** SP-22-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-22-03 — Canlı kanal smoke

**Sorumlu:** QA. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Dağıtılan buildi temiz kurulumla doğrula; iki farklı ağdan bağlantı, görev ve kayıt senaryolarını uygula.

**Kartın kabulü:** Canlı artifact sürümü beklenenle eşleşiyor; kritikte rollout durdurma kuralı uygulanıyor.

**Kart bağımlılığı:** SP-22-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-22-04 — İlk olay toplama

**Sorumlu:** Support/Tech. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** Crash, bağlantı nedeni, kayıt problemi ve oyuncu bildirimlerini build/platform/şiddetle sınıflandır. Gereksiz kişisel veri toplama.

**Kartın kabulü:** Her P0/P1in sahibi ve yeniden üretim adımı var; destek saatleri gerçek ekip kapasitesine uygun.

**Kart bağımlılığı:** SP-22-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-22-05 — Acil düzeltme provası veya uygulama

**Sorumlu:** Tech/QA. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** P0/P1 varsa minimal hotfix ve ilgili regresyonu çalıştır; yoksa hotfix yolunu stagingde prova et.

**Kartın kabulü:** Hotfix yeni özellik içermiyor; save ve ağ uyumluluğu değerlendirilmiş.

**Kart bağımlılığı:** SP-22-03, SP-22-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-22-06 — Çıkış raporu

**Sorumlu:** Producer. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** İlk sorun kategorileri, gerçek bağlantı/crash gözlemleri ve açık belirsizlikleri özetle. Veri azsa oranlardan kesin sonuç çıkarma.

**Kartın kabulü:** S23 sırası oyuncu etkisine göre; kayıtsız başarı iddiası yok.

**Kart bağımlılığı:** SP-22-03, SP-22-04, SP-22-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- Yayın yalnız gerçek ürün kararı ve yetkili erişimle gerçekleşir.
- Canlı veya yayın öncesi staging artifact smoke sonucu saklanır.
- Kritik olay için sahibi, durdurma/hotfix kararı ve kayıt koruma yolu vardır.

## Risk ve kapsam kararı

**En önemli risk:** Yayın gününe son dakika özellik eklemek veya izleme/sorumlu olmadan dağıtmak.

**Kapasite yetmezse:** G5 sonrası özellik eklenmez; dış yayın beklerse sprintin kalan kapasitesi QA/destek hazırlığına gider.

**Kapı:** Bu sprintte ayrı milestone kapısı yok; sprint kabulü ve önceki açık kapılar geçerli.

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-042, ASK-044, ASK-070, ASK-075.

**Gereksinimler:** REQ-038, REQ-040.

**Var olan testler:** TC-038, TC-040.

**Eski backlog kapsam izi:** W-029, W-032. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)


---

Kaynak: `sprints/S23.md`

# S23 — Yayın sonrası istikrar ve sonraki plan

Durum: **PLANNED**. Örnek takvim: hafta 47–48. Bütçe: 80 saat planlı iş + 20 saat rezerv. Yeniden tahmin edilecek çalışma paketi; bu saatler kesin efor tahmini değil.

**Sprint amacı:** İlk sürüm kusurlarını düzeltmek ve sonraki kapsamı gerçek oyuncu verisiyle belirlemek.

**Giriş bağımlılığı:** S22. Önceki sprint Done ve ilgili kapı PASS olmadan zorunlu üretim işi açılmaz.

**Sprint sonunda gösterilecek:** Bir hotfixin sorundan doğrulamaya kadar izi ve sonraki dört sprintin kanıta dayalı önerisi gösterilir.

## İş paketleri

| Kart | Sorumlu rol | Planlı saat | İş |
|---|---|---:|---|
| SP-23-01 | Producer/Support | 6 | Olay ve geri bildirim ayrımı |
| SP-23-02 | Tech | 18 | Kritik düzeltme paketi |
| SP-23-03 | QA | 18 | Hotfix ve eski kayıt regresyonu |
| SP-23-04 | Performance/Network | 16 | Gerçek cihaz örneklerini inceleme |
| SP-23-05 | Producer/Design | 12 | Sonraki sürüm seçenekleri |
| SP-23-06 | Burak/Producer | 10 | Retrospektif ve bakım planı |

### SP-23-01 — Olay ve geri bildirim ayrımı

**Sorumlu:** Producer/Support. **Bütçe tahsisi:** 6 net saat.

**Yapılacak iş:** Crash/kayıt/bağlantı/konfor/içerik isteğini ayrı sınıflandır; tekrarları birleştir.

**Kartın kabulü:** En yüksek oyuncu etkili sorunlar ve bilinmeyen sıklıklar açık.

**Kart bağımlılığı:** SP-22-06.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-23-02 — Kritik düzeltme paketi

**Sorumlu:** Tech. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Önce P0 veri/oturum, sonra P1 görev/holder sorunlarını minimal değişiklikle çöz.

**Kartın kabulü:** Repro artık geçiyor; düzeltme yeni kayıt veya bağlantı uyumsuzluğu yaratmıyor.

**Kart bağımlılığı:** SP-23-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-23-03 — Hotfix ve eski kayıt regresyonu

**Sorumlu:** QA. **Bütçe tahsisi:** 18 net saat.

**Yapılacak iş:** Desteklenen önceki build/kayıt kombinasyonlarını ve etkilenen görevleri test et.

**Kartın kabulü:** Yama artifacti kendi kanıtına sahip; önceki RC raporu kopyalanmıyor.

**Kart bağımlılığı:** SP-23-02.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-23-04 — Gerçek cihaz örneklerini inceleme

**Sorumlu:** Performance/Network. **Bütçe tahsisi:** 16 net saat.

**Yapılacak iş:** İzinli log ve kullanıcı cihaz tariflerinden temsilî kötü durumu yeniden üret; gerekirse kalite önerisi veya hedefli düzeltme yap.

**Kartın kabulü:** Ölçülmemiş yaygınlık iddiası yok; sorun bir cihazda tekrar üretilebiliyor veya açık kalıyor.

**Kart bağımlılığı:** SP-23-01.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-23-05 — Sonraki sürüm seçenekleri

**Sorumlu:** Producer/Design. **Bütçe tahsisi:** 12 net saat.

**Yapılacak iş:** Kalite borcu, istenen içerik ve teknik maliyeti karşılaştır; dört sprintlik öneri çıkar. Host migration gibi kapsam dışı işi yeni karar olarak ele al.

**Kartın kabulü:** Yeni roadmap geçmiş v1 vaadi gibi sunulmuyor; kapasite ve alternatifler açık.

**Kart bağımlılığı:** SP-23-01, SP-23-03, SP-23-04.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

### SP-23-06 — Retrospektif ve bakım planı

**Sorumlu:** Burak/Producer. **Bütçe tahsisi:** 10 net saat.

**Yapılacak iş:** Tahmin/gerçek saat, kusur kaynakları, art throughput ve dış bağımlılık beklemelerini değerlendir. Destek rotasyonu ve bilgi tabanı sürümünü güncelle.

**Kartın kabulü:** Çalışan süreç korunmuş; sonraki plan gerçek hızla boyutlandırılmış.

**Kart bağımlılığı:** SP-23-03, SP-23-04, SP-23-05.

**Kapanış kaydı:** Yapılan dosya/asset değişikliği, çalıştırılan ilgili test, gerçek build/kanıt yolu ve harcanan saat kartın evidence/actual_hours alanına yazılır. Uygulama yapılmadan durum PLANNED kalır.

## Uygulama sırası ve kapasite

Önce sözleşme/ölçüt kartını tamamla; uygulama kartlarını ortak girdi hazır olduğunda ayır. Bir kart diğerinin ürettiği scene/prefab/statei kullanıyorsa onu bekler veya yalnız sözleşme seviyesinde çalışır. En geç ilk hafta sonunda entegre build al; ikinci haftada negatif senaryo, düzeltme ve kanıt üret. Sorumlu rol bir kişi sayısı değildir: Art, Network ve QA saatleri toplam 100 saatlik takım kapasitesinin içindedir.

20 saat rezerv plan dışı kusur ve teknik belirsizliğe ayrılır. Altı kartın planlı doğrulama/review işleri zaten 80 saate dahildir. Uzak sprintlerde bir iş bütçesini aşıyorsa küçük işlere bölüp ek sprint aç; aynı bütçeyle tamamlanacakmış gibi bırakılamaz.

## Sprint kabul kriterleri

- Kritik hotfixler ilgili release ve kayıt regresyonunu geçer.
- Bilinen sorunlar doğru sürüm ve sahiplerle kayıtlıdır.
- Sonraki içerik kapsamı temel oyun istikrarını bozacak kapasite varsayımına dayanmaz.

## Risk ve kapsam kararı

**En önemli risk:** Çıkış sonrası her isteği roadmap taahhüdüne çevirmek ve ana kusurları ertelemek.

**Kapasite yetmezse:** Yeni içerik duyurusu yerine kritik kalite borcu önceliklenir; haftalarca kesintisiz destek varsayılmaz.

**Kapı:** Bu sprintte ayrı milestone kapısı yok; sprint kabulü ve önceki açık kapılar geçerli.

Kapı/temel kabul başarısızsa sonraki kapsam otomatik başlamaz. FAIL/BLOCKED nedeni, kurtarma hipotezi, owner ve yeni çalışma paketi kayda girer. Kanıt eksikliği başarı sayılmaz.

## Bilgi tabanı ve test izi

**Kaynak belgeler:** ASK-044, ASK-067, ASK-070, ASK-071, ASK-073, ASK-075.

**Gereksinimler:** REQ-016, REQ-018, REQ-037, REQ-038, REQ-040, REQ-039.

**Var olan testler:** TC-016, TC-018, TC-037, TC-038, TC-040, TC-039.

**Eski backlog kapsam izi:** W-028, W-029, W-032. Eski saatlerle bu saatleri toplama; birebir görev yeniden adlandırması değildir.

[Yol haritası](03_YOL_HARITASI.md) · [Test kitabı](05_TEST_VE_OLCUM_KITABI.md) · [Kalite kapıları](04_KALITE_KAPILARI.md) · [Görev şablonu](07_BILGI_TABANI_VE_GOREV_SABLONU.md)
