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
