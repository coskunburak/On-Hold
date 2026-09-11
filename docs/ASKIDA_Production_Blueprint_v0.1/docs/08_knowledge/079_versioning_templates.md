---
doc_id: ASK-079
title: "Belge sürümleme, şablonlar ve değişiklik yönetimi"
version: 0.1.0
status: proposed
owner_role: Knowledge
last_reviewed: 2026-09-07
dependencies: ["ASK-003","ASK-009","ASK-067","ASK-076","ASK-078"]
---

# Belge sürümleme, şablonlar ve değişiklik yönetimi

## Frontmatter sözleşmesi
doc_id, title, version, status, owner_role, last_reviewed, dependencies. Bu pakette bütün belgeler version 0.1.0 ve proposed durumundadır. Kullanıcı ihtiyacının doğrulanmış olduğu cümleler ürün bildirgesinde açıkça ayrılır. owner_role gerçek kişi atanmış demek değildir.

## Sürüm
Patch yazım/bağlantı düzeltmesi; minor anlamlı ama geriye uyumlu tasarım genişletmesi; major temel semantik veya tüketici şema kırılması için önerilir. Uygulama sürümü ve belge sürümü ayrı kavramdır. Hash herhangi bir byte farkını gösterir, semantik önemini göstermez.

## Değişiklik isteği şablonu
CR ID; problem/kanıt; eski kural; önerilen yeni kural; etkilenen ASK/REQ/TC/W; save/protocol/asset etkisi; maliyet; alternatif; onay sahibi; karar; uygulanma build'i; doğrulama sonucu. “AI böyle önerdi” tek başına gerekçe değildir.

## Yeni belge şablonu
Amaç/kapsam; öneri veya onay durumu; giriş/çıkış verileri; normatif kurallar; hata/eşzamanlılık; uygulama sınırı; kabul/kanıt; ilişkiler/kaynaklar. Her belge bütün başlıkları mekanik doldurmak zorunda değildir, ancak belirsiz davranışlar gizlenemez. Yeni ASK kimliği tekrar kullanılmaz.

## Türetilmiş dosyalar
Manifest hash'leri ve notebook ciltleri build_exports.py ile tekrar üretilir. data/requirements.json ve test_cases.json normatif plan kayıtlarıdır, elle gözden geçirilen değişiklik ister. validation_report.json yalnız en son yapısal kontrolü yansıtır; gerçek oyun test raporu değildir.

## Merge ve arşiv
İki çelişen karar sessizce birleştirilmez; ASK-009 kayıt sahibi çözer. Retired belge bir süre yönlendirme için kalabilir; 80 dosya sayısını korumak kalite hedefi değildir. Bu ilk teslimatın sayısı 80'dir, gelecekte ihtiyaçla değişebilir.

## Kabul
Hash ve dependency kontrolleri geçer; stale notebook cildi saptanır. Değişikliğin neden yapıldığı commit/CR kaydından bulunur. Onay alan bir belgeye bağımlı öneri otomatik onaylanmaz.

## Gereksinim ve test izleri
- REQ-039 → TC-039 — Bilgi tabanı izlenebilir ve salt okunur kullanılmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-003 — Belge haritası ve okuma rotaları](../00_foundation/003_document_map.md)
- [ASK-009 — Karar kaydı ve onay bekleyenler](../00_foundation/009_decision_register.md)
- [ASK-067 — Test stratejisi ve gereksinim izlenebilirliği](../06_quality/067_test_strategy_traceability.md)
- [ASK-076 — Notebook aktarımı ve tek kaynak ilkesi](076_notebook_import.md)
- [ASK-078 — Salt okunur MCP bilgi servisi şartnamesi](078_mcp_readonly_spec.md)

