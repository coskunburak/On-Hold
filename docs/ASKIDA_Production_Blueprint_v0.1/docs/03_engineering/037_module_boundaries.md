---
doc_id: ASK-037
title: "Modüller, assembly sınırları ve bağımlılıklar"
version: 0.1.0
status: proposed
owner_role: TechLead
last_reviewed: 2026-09-07
dependencies: ["ASK-027","ASK-036","ASK-038","ASK-039"]
---

# Modüller, assembly sınırları ve bağımlılıklar

## Önerilen modüller
Core kimlikler, sonuç türleri ve zaman soyutlamasını; Domain eşya/görev/ekonomi kurallarını; Gameplay Unity fizik ve hareket adaptörlerini; Networking framework entegrasyonunu; Presentation ses/VFX/UI'ı; Persistence kayıt uygulamasını; Editor doğrulama araçlarını taşır. Her birine sınırsız “Manager” eklenmez.

## Bağımlılık yönü
Domain motor veya Steam API'si bilmez. Gameplay ve Networking Domain sözleşmelerini kullanır. Presentation salt okunur state ve domain olaylarından görünüm türetir. Bootstrap somut adaptörleri bağlar. Editor kodu runtime assembly'ye sızmaz; test yardımcıları shipping binary'ye girmez.

## Dar arayüz örnekleri
ICommandGateway komut kabul sınırı; IWorldQuery erişim/geometri sorgusu; IPhysicsHandle motor etkisi; ICampaignStore atomik checkpoint; ISessionTransport bağlantı ve mesaj; IClock host monotonik zaman. Gerçek ikinci implementasyon veya test ihtiyacı olmayan her sınıf için interface yazılması şart değildir.

## Olay ve işlem farkı
Command yapılması istenen niyettir; Event kabul edilmiş sonuçtur. UI DeliveryCompleted event'ini tekrar göndererek para yaratamaz. Event bus global ve türsüz string kanalı olmamalı; yaşam süresi ve unsubscribe denetlenir. Snapshot değişimi ile tek seferlik sunum olayı ayrı taşınır.

## Ortak tuzaklar
NetworkBehaviour içinde bütün ekonomi; ScriptableObject üzerinde runtime oyuncu durumu; static service locator ile sahneler arası eski bağlantı; UnityEvent üzerinden gizli kayıt işlemi. Bunlar test ve state ownership'ünü belirsizleştirir. Domain ayrımı aşırı soyut değil, önemli kuralların motordan bağımsız test edilebilmesini amaçlar.

## Kabul
Domain testleri Unity sahnesi yüklemeden çalışabilir; Unity bağımlılıkları bilinen adaptör sınırındadır. asmdef referans döngüsü yoktur. Networking paketi kaldırıldığında compile sorunlarının hangi dar modülde toplanacağı öngörülebilir. Architecture validator ve code review ile sınır korunur.

## İlgili kaynak belgeler
- [ASK-027 — Otorite, sahiplik ve veri sınırları](../02_network/027_authority_matrix.md)
- [ASK-036 — Unity deposu ve dosya hiyerarşisi](036_unity_repository.md)
- [ASK-038 — Bootstrap, servis ömrü ve sahne geçişleri](038_bootstrap_lifecycle.md)
- [ASK-039 — Veri şemaları ve içerik kimlikleri](039_data_schemas.md)

