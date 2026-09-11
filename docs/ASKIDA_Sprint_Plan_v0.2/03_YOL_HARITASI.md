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
