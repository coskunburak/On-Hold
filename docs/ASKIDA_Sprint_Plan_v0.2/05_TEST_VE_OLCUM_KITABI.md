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
