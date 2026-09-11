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
