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
