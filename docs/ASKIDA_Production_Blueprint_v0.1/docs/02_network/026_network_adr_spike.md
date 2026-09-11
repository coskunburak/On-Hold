---
doc_id: ASK-026
title: "Ağ çözümü seçimi ve risk prototipi"
version: 0.1.0
status: proposed
owner_role: TechLead
last_reviewed: 2026-09-07
dependencies: ["ASK-009","ASK-027","ASK-030","ASK-035","ASK-040"]
---

# Ağ çözümü seçimi ve risk prototipi

## Öneri ve açık karar
Host-authoritative listen-server önerilir; ağ framework'ü ve internet taşıma yolu henüz seçilmedi. Unity NGO bir adaydır, seçilmiş üretim bağımlılığı değildir. Steam lobi hizmeti, rigidbody replication veya NAT bağlantısının tamamı değildir. Lobi kimliği ile oyun bağlantısı ayrı katmanlardır.

## Karşılaştırma alanı
A adayı: NGO + doğrulanmış Unity transport/hizmet yolu. B adayı: NGO ile uyumluluğu kanıtlanmış SteamNetworkingSockets/SDR adapter yolu. B için bakımlı adapter, lisans, platform binary'leri, API uyumu ve test gerekir; resmi yerleşik NGO Steam transport varsayılmaz. Başka framework eklemek yalnız adaylar kritik ölçütleri karşılamazsa gerekçeli ADR değişikliğiyle olur. İki framework birlikte oyuna alınmaz.

## İki haftalık risk çalışması
Gün 1–2: sürüm matrisi, host/client build, tek shared item. Gün 3–5: iki holder, hareketli platform, uzaktan input ve tanı kayıtları. Gün 6–7: gerçek internet bağlantısı ve davet yolu, iki ayrı makine/ağ. Gün 8–9: 2/4 oyuncu, gecikme/kayıp, disconnect cleanup. Gün 10: test klipleri, ölçüm, aday karşılaştırması ve devam/daralt kararı. Bu bir zaman kutusudur; başarı garantisi değildir.

## Ölçüm
150 ms RTT ve %1 kayıpta ortak taşıma okunurluğu; platform üzerinde relative poz hatası; correction büyüklüğü/sıklığı; host fizik süresi; oyuncu başı trafik; bağlantı başarısızlık nedeni; release sonrası kalan holder/joint. Rakamlar seçili build, makine ve network profiliyle saklanır. Editor localhost testi tek kanıt olamaz.

## Resmi davranış ve sınır
NGO NetworkRigidbody belgeleri otorite dışında kinematik simülasyon ve NetworkTransform otorite ilişkisini açıklar. Bu mekanizma kendi başına yüksek kaliteli ortak taşıma tahmini sağlamaz. [Unity NetworkRigidbody](https://docs.unity3d.com/Packages/com.unity.netcode.gameobjects@2.9/manual/components/helper/networkrigidbody.html). Unreal alternatifi de gecikme ve fizik çarpışmalarını ücretsiz çözmez; [Networked Physics Overview](https://dev.epicgames.com/documentation/unreal-engine/networked-physics-overview).

## Karar kapısı
İki adaydan geçen seçilir ve exact version, license, sample commit, known issues ADR'ye yazılır. Hiçbiri geçmezse platformu daha kısıtlı hareket ettir veya eşya kuvvet modelini sadeleştir; kötü ağ hissinin üzerine üç bina üretme.

## Gereksinim ve test izleri
- REQ-001 → TC-001 — Motor ve sürüm kararı kanıtla seçilmeli
- REQ-007 → TC-007 — Gerçek internet bağlantısı doğrulanmalı
- REQ-008 → TC-008 — Risk kapısı kanıtla kapanmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-009 — Karar kaydı ve onay bekleyenler](../00_foundation/009_decision_register.md)
- [ASK-027 — Otorite, sahiplik ve veri sınırları](027_authority_matrix.md)
- [ASK-030 — Gecikme, öngörü ve düzeltme sınırları](030_latency_prediction.md)
- [ASK-035 — Multiplayer test matrisi ve kanıt](035_network_test_matrix.md)
- [ASK-040 — Unity fizik uygulaması ve teknik sınırlar](../03_engineering/040_physics_integration.md)

