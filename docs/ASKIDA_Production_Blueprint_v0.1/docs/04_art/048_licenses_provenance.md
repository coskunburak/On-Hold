---
doc_id: ASK-048
title: "Lisans, kaynak kaydı ve AI kullanımı"
version: 0.1.0
status: proposed
owner_role: Producer
last_reviewed: 2026-09-07
dependencies: ["ASK-047","ASK-050","ASK-061","ASK-075","ASK-080"]
---

# Lisans, kaynak kaydı ve AI kullanımı

## Kaynak defteri
Her varlık için provenanceId, yazar, ürün URL'si, indirme tarihi, dosya hash'i, tier, lisans adı/metni snapshot'ı, satın alma belgesi gerekiyorsa konumu, yapılan türevler, dağıtım/attribution ve AI giriş izni durumu tutulur. “İnternette ücretsiz” lisans değildir. Kaynak paket kullanıcıya bu ZIP içinde yeniden dağıtılmıyor; sadece bağlantılar ve üretim planı bulunur.

## Lisans sınıfları
CC0 kaynaklarda bile ürünün gerçekten o lisansla dağıtılan dosyası ve üçüncü taraf içerik istisnaları kontrol edilir. CC-BY attribution gerektirir; NC işaretli içerik ticari oyuna izinsiz alınmaz. Mağaza EULA'sı standalone asset yeniden satışını veya dağıtımını yasaklayabilir. Nihai oyun içinde kullanma izni, ham kaynakları GitHub'a koyma izni değildir.

## AI ayrı izin sorusudur
Paket mesh/texture/animasyonunu modele yüklemek, ticari oyunda kullanmaktan farklıdır. [Unity Asset Store şartları](https://unity.com/legal/as-terms) AI kullanımına ilişkin kısıtlar içerir; varlığı model girdisi yapmadan geçerli metin ve hak sahibinin izni incelenmelidir. AI'a yalnız izinli kendi kaynaklarımızı veya doğrulanmış uygun lisanslı içeriği göndeririz. Bu belge hukuki danışmanlık değildir.

## Ses özel durumları
[Sonniss GDC koleksiyonu](https://sonniss.com/gameaudiogdc/) ticari kullanım koşulları yanında AI/ML eğitimi yasağı bildirir; kullanım senaryosu ayrıca değerlendirilir. [Freesound](https://freesound.org/help/faq/) her dosyada aynı lisansı kullanmaz. Mixamo kullanımı [Adobe FAQ](https://helpx.adobe.com/creative-cloud/faq/mixamo-faq.html) üzerinden kontrol edilir; kaynak animasyonları ayrı asset paketi olarak yeniden dağıtma hakkı varsayılmaz.

## Onay kapısı
Provenance eksikse placeholder etiketlenir ve public build'e girmez. Lisans değişirse daha önce edinilen sürüm ve koşulların kaydı korunur; güncel sayfa geçmiş lisans kanıtı yerine geçmez. Steam AI beyanı için oyuncunun gördüğü AI içerik envanteri ayrıca tutulur.

## Kabul
Build'e giren her üçüncü taraf varlık manifest'ten kaynak kaydına izlenebilir. Attribution dosyası gerçek kullanılan içeriği kapsar. Gizli erişim bilgileri, fatura kişisel verisi veya lisanslı ham paket kaynakları notebook aktarımına eklenmez.

## Gereksinim ve test izleri
- REQ-024 → TC-024 — Shipping varlığı lisansa izlenebilir olmalı
- REQ-026 → TC-026 — AI üretimi gerçek araç ve insan QA ile doğrulanmalı
- REQ-040 → TC-040 — Yayın iddiaları kanıta ve lisansa uymalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-047 — Seçilecek paketler ve kullanım sınırları](047_package_selection.md)
- [ASK-050 — Astra/Codex ile Blender üretimi ve export](050_ai_blender_export.md)
- [ASK-061 — Varlık envanteri ve üretim kuyruğu](../05_content/061_asset_inventory.md)
- [ASK-075 — Steam hazırlığı, demo ve yayın kontrolü](../07_production/075_steam_launch_plan.md)
- [ASK-080 — Kaynaklar, doğrulama sınırları ve açık kanıtlar](../08_knowledge/080_sources_evidence_limits.md)

