---
doc_id: ASK-031
title: "Lobi, Steam daveti ve bağlantı akışı"
version: 0.1.0
status: proposed
owner_role: Network
last_reviewed: 2026-09-07
dependencies: ["ASK-012","ASK-026","ASK-032","ASK-075"]
---

# Lobi, Steam daveti ve bağlantı akışı

## Katmanları karıştırma
Steam lobby grup keşfi/daveti ve metadata taşımak için kullanılabilir. Oyun durumunu çoğaltan netcode ve gerçek paketleri taşıyan transport ayrı işlerdir. Başarılı Steam daveti, peer bağlantısının başarılı olduğu veya NAT sorununun çözüldüğü anlamına gelmez. [Steam multiplayer belgeleri](https://partner.steamgames.com/doc/features/multiplayer) ve [matchmaking](https://partner.steamgames.com/doc/features/multiplayer/matchmaking).

## Bağlantı sırası
Davet veya lobby seçimi → erişim uygunluğu → build/protocol/content manifest kontrolü → transport bağlantısı → oturum doğrulaması → oyuncu yuvası → lobi snapshot → hazır. Lobby metadata'sı güvenilir gameplay kararı değildir; host handshake tekrar doğrular. Maksimum dört oyuncu kontrolü hem lobby hem host giriş sınırında yapılır.

## V1 politikası
Arkadaş/davet odaklı erişim; rastgele public matchmaking zorunlu değil. Lobby/Hub'da yeni oyuncu alınır. Active içinde yeni kimlikli oyuncu spectate veya oynama hakkı almaz; açık mesajla sonraki turu bekler. Önceden başlamış oyuncunun rezervli reconnect'i ayrı akıştır.

## Transport seçimi
SteamNetworkingSockets veya SDR kullanımının gerektirdiği SDK/adapter, dağıtım dosyaları ve test hesapları karar kaydına girer. Unity hizmet yolu seçilirse servis maliyeti, hesap gereksinimi ve Steam davetini o bağlantı bilgisine eşleme yöntemi ayrıca belirtilir. [Steam Datagram Relay](https://partner.steamgames.com/doc/features/multiplayer/steamdatagramrelay). Bu arşiv etkin Steamworks hesabı veya çalışan adapter içermez.

## Hata deneyimi
BuildMismatch, ContentMismatch, LobbyFull, SessionInProgress, AuthFailed, ConnectTimeout ve HostUnavailable ayrı yerelleştirme anahtarlarıdır. “Bir hata oluştu” tek sonucu yeterli değildir. Yeniden deneme tekrar slot yaratmaz; iptal cleanup tamamlanmadan yeni bağlantı başlatılmaz.

## Kabul
İki fiziksel PC ve mümkünse farklı ağlarla davet, iptal, tam lobi ve sürüm uyuşmazlığı sınanır. Özel token ve bağlantı bileti loglanmaz. Steam servisinin erişilemez olduğu durumda kullanıcı sonsuz spinner'da kalmaz.

## Gereksinim ve test izleri
- REQ-007 → TC-007 — Gerçek internet bağlantısı doğrulanmalı
- REQ-015 → TC-015 — Lobi ve content handshake uyumlu olmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-012 — Oturum ve görev durum makinesi](../01_gameplay/012_session_state_machine.md)
- [ASK-026 — Ağ çözümü seçimi ve risk prototipi](026_network_adr_spike.md)
- [ASK-032 — Kopma, yeniden bağlanma ve host kaybı](032_disconnect_reconnect_hostloss.md)
- [ASK-075 — Steam hazırlığı, demo ve yayın kontrolü](../07_production/075_steam_launch_plan.md)

