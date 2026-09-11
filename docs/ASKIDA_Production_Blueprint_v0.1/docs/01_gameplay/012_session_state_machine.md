---
doc_id: ASK-012
title: "Oturum ve görev durum makinesi"
version: 0.1.0
status: proposed
owner_role: Gameplay
last_reviewed: 2026-09-07
dependencies: ["ASK-011","ASK-031","ASK-032","ASK-033"]
---

# Oturum ve görev durum makinesi

## Ayrı yaşam döngüleri
Uygulama bağlantısı, görev durumu ve eşya durumu farklı makineler olmalıdır. Bağlantının kopması ContractRun kimliğini otomatik değiştirmez. Bir item Lost olduğu için oturum Failed durumuna atlanmaz; görev hedefleri değerlendirilir.

| Durum | Giriş işi | Çıkış önkoşulu |
|---|---|---|
| Lobby | Yuvalar, build/content uyumu | Host başlatır; 2–4 uyumlu oyuncu |
| Loading | Sahne ve ağ nesneleri hazırlanır | Başlayacak tüm oyuncular hazır |
| Briefing | Manifest, rota ve kontroller | Hazır kontrolü ve host başlatma |
| Active | Host zamanlayıcı ve kurallar | Süre veya geçerli bitirme isteği |
| Settling | Komutları kapat, sonucu üret, kaydet | Kalıcı kayıt sonucu belli |
| Results | Özet ve hata varsa güvenli seçenek | Host devam eder |
| Hub | Yükseltme ve sonraki seçim | Yeni görev seçilir |

## Geçiş sahipliği
Yalnız SessionCoordinator host üzerinde geçiş yapar. UI “start” niyeti üretir; sahneyi doğrudan yüklemez. Her geçiş transitionId, epoch, önceki/yeni durum ve host tick taşır. Tekrar gelen bildirim aynı geçişi ikinci kez işletmez. Giriş/çıkış işlemleri iptal edilebilir ve idempotent tasarlanır.

## Yükleme hataları
Bir istemci zaman aşımına uğrarsa host'a o oyuncuyu lobide bırakma veya yüklemeyi iptal etme seçeneği verilir; sessizce eksik oyuncuyla Active başlatılmaz. Başlangıçta ikiden az uyumlu oyuncu varsa tur başlatılamaz. Active sırasında tek oyuncu kalması yeni başlangıç değildir; ASK-032 kuralı uygulanır.

## Settling güvenliği
Kaydetme başarısızsa sonuç RAM'de tutulur, aynı işlem kimliğiyle yeniden deneme yapılır. Kullanıcıya “kaydedildi” gösterilmez. Tekrar deneme yeni gelir hesabı yaratmaz. Uygulama kapanırsa önceki kalıcı checkpoint'e dönülebileceği açıkça belirtilir.

## Test
Her geçişte bağlantı kesme, sahne yükleme hatası ve yinelenen mesaj uygulanır. ContractRun başına en fazla bir sonuç ve en fazla bir commit beklenir. Tanı ekranında durum adı ve son geçiş nedeni bulunur; bunlar oyuncuya ham hata yığını olarak gösterilmez.

## Gereksinim ve test izleri
- REQ-015 → TC-015 — Lobi ve content handshake uyumlu olmalı
- REQ-021 → TC-021 — Sahne yaşam döngüsü kaynak sızdırmamalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-011 — Temel oyun döngüsü ve zaman çizgisi](011_core_loop.md)
- [ASK-031 — Lobi, Steam daveti ve bağlantı akışı](../02_network/031_lobby_steam_connectivity.md)
- [ASK-032 — Kopma, yeniden bağlanma ve host kaybı](../02_network/032_disconnect_reconnect_hostloss.md)
- [ASK-033 — Kampanya işlemleri ve kayıt otoritesi](../02_network/033_campaign_transaction.md)

