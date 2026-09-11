---
doc_id: ASK-045
title: "Hata sınıfları ve güvenli bozulma"
version: 0.1.0
status: proposed
owner_role: TechLead
last_reviewed: 2026-09-07
dependencies: ["ASK-012","ASK-032","ASK-033","ASK-038"]
---

# Hata sınıfları ve güvenli bozulma

## Hata sınıfları
UserAction: TooFar/Busy gibi beklenen retler. RecoverableSystem: bağlantı geçici hatası, content yükleme timeout'u. DataIntegrity: invalid save veya item invariant ihlali. FatalSession: host kaybı, uyumsuz protocol. Her sınıf farklı UI ve log seviyesi taşır; beklenen her ret exception değildir.

## Oyuncu mesajı
Ne oldu, ne korunuyor, ne kaybolabilir, şimdi hangi güvenli eylem var? Örnek: “Host bağlantısı kesildi. Bu turun kaydedilmemiş ilerlemesi korunamadı. Son kayıtlı kampanyadan yeni lobi açabilirsiniz.” Belirsiz “yeniden deneyin” aynı bozuk işlemi sonsuz döngüye sokmamalıdır.

## Yeniden deneme
Yalnız idempotent işlemler otomatik tekrar edilir; exponential backoff ve deneme tavanı gerekir. Delivery veya spend isteğini farklı requestId'lerle otomatik çoğaltmak yasaktır. Kaydetme tekrarında aynı transactionId korunur. İçerik yükleme iptal edildiyse eski callback yeni sahneye yazmaz.

## Invariant ihlali
Shipping build'de Lost + Held görülürse nesne karantinaya alınır, ilişkiler kontrollü temizlenir, session error kaydı üretilir; para eklenmez. Bu düzeltme normal oyun mekaniği haline getirilmez. Tekrar üreten hata P0/P1 triage'a girer.

## Dış bağımlılık
Steam veya seçilen relay erişilemiyorsa frontend ayarlar ve hata bilgisi çalışır; online oyunun çalıştığı iddia edilmez. Üçüncü taraf shader eksikse pembe materyalle public build çıkılmaz. Geliştirme fallback'i ile shipping fallback'i ayrıdır.

## Kabul
Kaydetme/davet/yükleme işlemlerine kontrollü hata enjeksiyonu yapılır. Spinner süreli ve iptal edilebilir; her hata durumundan ana menüye güvenli dönüş bulunur. Kalan async iş yeni oturum state'ini bozamaz.

## İlgili kaynak belgeler
- [ASK-012 — Oturum ve görev durum makinesi](../01_gameplay/012_session_state_machine.md)
- [ASK-032 — Kopma, yeniden bağlanma ve host kaybı](../02_network/032_disconnect_reconnect_hostloss.md)
- [ASK-033 — Kampanya işlemleri ve kayıt otoritesi](../02_network/033_campaign_transaction.md)
- [ASK-038 — Bootstrap, servis ömrü ve sahne geçişleri](038_bootstrap_lifecycle.md)

