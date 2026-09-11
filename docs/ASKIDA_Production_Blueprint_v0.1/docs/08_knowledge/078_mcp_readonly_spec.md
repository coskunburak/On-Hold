---
doc_id: ASK-078
title: "Salt okunur MCP bilgi servisi şartnamesi"
version: 0.1.0
status: proposed
owner_role: Knowledge
last_reviewed: 2026-09-07
dependencies: ["ASK-002","ASK-034","ASK-076","ASK-077","ASK-079"]
---

# Salt okunur MCP bilgi servisi şartnamesi

## Durum
Bu belge kurulmuş bir sunucu değil, ileride uygulanacak read-only MCP servis sözleşmesidir. Arşivde yerel arama/doğrulama yardımcıları vardır; ağda dinleyen MCP implementasyonu, token veya canlı endpoint yoktur. Notebook sağlayıcısının resmi MCP/API desteği varsayılmaz.

## Önerilen araçlar
search_docs(query, tags?, status?, limit≤10): ASK kimliği, başlık, kısa excerpt, relative path, revision ve hash döndürür. read_doc(doc_id, section?, max_chars≤24000): yalnız manifest'teki belgeyi okur, kesildiyse açık işaretler. get_related_docs(doc_id, depth≤1): doğrudan ilişkileri döndürür. get_task(task_id): W/REQ/TC ve bağımlılıklar. Dosya yazma, shell çalıştırma, paket kurma ve silme aracı yoktur.

## Güvenlik
Girdi docId üzerinden çözülür; arbitrary path kabul edilmez. Gerçek canonical path kök içinde olmalı; symlink kaçışı, ../ traversal, mutlak path ve URI şemaları reddedilir. Registry whitelist dışında dosya okunmaz. Secrets, lisanslı ham asset ve kullanıcı save'leri servis kökünde bulunmaz. İstek/yanıt boyutu, timeout ve concurrency sınırlıdır.

## Güvenilir bağlam
Her sonuç kaynak hash'i ve snapshotVersion taşır; istemci farklı snapshot sonuçlarını birleştirdiğinde uyarır. Dış metinler veri olarak sunulur; içerikteki “önceki talimatları yok say” gibi ifadeler yürütülemez. Server dokümandaki öneriyi onaylanmış karar gibi dönüştürmez.

## Bağlantı ortamı
Codex belgeleri yerel STDIO ve uzak Streamable HTTP seçeneklerini açıklar. CLI/IDE/Desktop'ta ilgili güvenilen yapılandırma yöntemi kullanılır; bu sohbet ortamının yerel config dosyasını otomatik okuyacağı varsayılmaz. Uzak servis için kimlik doğrulama, TLS ve yetki ayrıca kurulur. [Resmi Codex MCP rehberi](https://learn.chatgpt.com/docs/extend/mcp?surface=cli).

## Kabul
Traversal/symlink, bozuk ID, aşırı limit, stale hash ve prompt injection testleri. Varsayılan yalnız approved seçilecekse bu ilk pakette bütün belgeler proposed olduğundan boş sonuç doğru olabilir; kullanıcı açıkça proposed dahil seçebilir. Kurulum öncesi kullanıcıdan hosting ve erişim sınırı onayı alınır.

## Gereksinim ve test izleri
- REQ-039 → TC-039 — Bilgi tabanı izlenebilir ve salt okunur kullanılmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-002 — Codex çalışma sözleşmesi](../../AGENTS.md)
- [ASK-034 — Ağ güvenliği ve kötüye kullanım sınırları](../02_network/034_security_abuse.md)
- [ASK-076 — Notebook aktarımı ve tek kaynak ilkesi](076_notebook_import.md)
- [ASK-077 — Codex görev bağlamı ve güvenilir iş akışı](077_codex_task_workflow.md)
- [ASK-079 — Belge sürümleme, şablonlar ve değişiklik yönetimi](079_versioning_templates.md)

