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
