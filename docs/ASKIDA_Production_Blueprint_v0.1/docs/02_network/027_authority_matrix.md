---
doc_id: ASK-027
title: "Otorite, sahiplik ve veri sınırları"
version: 0.1.0
status: proposed
owner_role: Network
last_reviewed: 2026-09-07
dependencies: ["ASK-015","ASK-016","ASK-028","ASK-033"]
---

# Otorite, sahiplik ve veri sınırları

## Otorite matrisi
| Veri/iş | Host | İstemci |
|---|---|---|
| Shared item poz/hız | Simüle eder | Görsel çoğaltma |
| Tutma/sabitleme kabulü | Doğrular ve değiştirir | Niyet yollar |
| Platform denge/vinç | Hesaplar | Input ve gösterim |
| Hasar/teslimat/para | Tek kaynak | Salt okunur |
| Kamera/yerel UI | Gerekmez | Yönetir |
| Kozmetik parçacık | Event kimliği üretir | Havuzdan gösterir |
| Kampanya kaydı | Kalıcı yazar | Host değilse yazmaz |

Host oyuncusu da mümkün olduğunca aynı komut doğrulama yolundan geçer. Sadece “yerel olduğu için” mesafe, bakiye veya handling koşulunu atlamaz. Ağ framework ownership'ünü istemciye vermek domain otoritesini devretme kararı değildir.

## Bileşen sınırı
NetworkAdapter giriş paketini DomainCommand'a çevirir. DomainService önkoşulları ve state değişimini yönetir. PhysicsAdapter güvenli motor etkisini uygular. Presentation domain olaylarını gösterir. UI'dan Rigidbody veya CampaignStore'a gizli doğrudan çağrı yapılamaz. Ancak her küçük sınıf için gereksiz servis zinciri de kurulmaz; sınır gerçek güvenlik veya test ihtiyacına dayanır.

## Host'un güven sınırı
Listen host teknik olarak kendi oyununun belleğini değiştirebilir. V1 bu konuda güçlü anti-cheat veya rekabet adaleti iddiası taşımaz. Buna rağmen uzak istemcinin yetkisiz komutu, yanlış item kimliği veya kasten aşırı paket trafiği sınırlandırılır. Host hilesini engellemek ayrı sunucu mimarisi ve işletim maliyetidir.

## Yayınlanabilir veri
İstemciye gönderilen snapshot minimum gerekli görev ve item durumunu içerir. Yerel dosya yolu, token, kullanıcı e-postası veya gereksiz Steam profil verisi gönderilmez. Görünüm kimlikleri whitelist içeriğe referans verir; istemcinin arbitrary asset path yükletmesi mümkün olmamalıdır.

## Kabul
Bir istemci script'i kendi transform'unu veya local cüzdan UI değerini değiştirince authoritative teslimat/para etkilenmez. Host-local ile remote aynı geçersiz komutu aynı reasonCode ile reddeder. Authority geçişi v1'de gizli bir eşya sahipliği devriyle yapılmaz.

## Gereksinim ve test izleri
- REQ-003 → TC-003 — İstemci paylaşılan state'i doğrudan değiştirememeli

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-015 — Etkileşim sorgusu ve komut sözleşmesi](../01_gameplay/015_interaction_contract.md)
- [ASK-016 — Eşya semantiği ve değişmezler](../01_gameplay/016_item_state_model.md)
- [ASK-028 — Komut protokolü, tekrarlar ve sıralama](028_command_protocol.md)
- [ASK-033 — Kampanya işlemleri ve kayıt otoritesi](033_campaign_transaction.md)

