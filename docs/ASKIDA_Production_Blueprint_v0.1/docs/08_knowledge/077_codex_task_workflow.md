---
doc_id: ASK-077
title: "Codex görev bağlamı ve güvenilir iş akışı"
version: 0.1.0
status: proposed
owner_role: Knowledge
last_reviewed: 2026-09-07
dependencies: ["ASK-002","ASK-009","ASK-067","ASK-074","ASK-078"]
---

# Codex görev bağlamı ve güvenilir iş akışı

## Bağlamı seç
Önce AGENTS.md ve manifest, sonra ilgili W/REQ/TC ile 3–6 temel ASK belgesi. Her görevde 80 belgenin tamamını talimat olarak yüklemek yerine gerekli domain bilgisi seçilir. AGENTS.md kısa yönlendirme kalır; uzun tasarım metinleri retrieval verisidir. [Codex AGENTS.md rehberi](https://learn.chatgpt.com/docs/agent-configuration/agents-md).

## Görev prompt şablonu
Amaç: hangi davranışı uygula veya incele. Yetki: sadece analiz / yerel kod değişikliği / test. Kapsam: ilgili W ve ASK kimlikleri. Değişmezler: özellikle bozulmayacak kurallar. Girdi: repo commit, engine lock, asset/protocol sürümü. Kabul: TC kimlikleri ve gerekli gerçek test. Yasaklar: paket yükseltme, yeni framework, harcama, dış yayın yok. Çıktı: fark, çalıştırılan test, kalan risk.

## Örnek
“W-011 kapsamında teslimat domain validator'ını uygula. ASK-016/022/023/033 oku. Held/Secured teslim edilemez; item başına bir ödül. Unity çalıştıramıyorsan bunu açıkça belirt, sahne testini geçti sayma. Save veya ağ framework'ünü değiştirme. Aynı tick timeout yarışını kapsayan test ekle.” Bu örnek hazır oyun implementasyonu değildir.

## Uygulama döngüsü
İncele → planla → küçük fark → domain test → gerekiyorsa Unity/build/network test → insan review → doküman ve kayıt güncelleme. AI'ın ürettiği test kendi yanlış varsayımını doğrulayabilir; kabul kriteri kaynak sözleşmeden bağımsız kontrol edilir.

## Araç sınırı
Codex'in Blender veya Unity çalıştırabilmesi o ortamda program, proje ve yetki bulunmasına bağlıdır. Model yeteneği uygulama erişimi anlamına gelmez. Bu teslimatta çalışan Unity editörü veya MCP server bağlantısı kurulmadı.

## Kabul
Yanıt “uygulandı”, “test edildi” ve “önerildi” durumlarını ayırır; gerçek build log'u olmadan başarı iddiası yoktur. Görev dışı dosya değişiklikleri açıklanır. Kullanıcı mevcut değişikliği ve gizli bilgiler korunur.

## Gereksinim ve test izleri
- REQ-039 → TC-039 — Bilgi tabanı izlenebilir ve salt okunur kullanılmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-002 — Codex çalışma sözleşmesi](../../AGENTS.md)
- [ASK-009 — Karar kaydı ve onay bekleyenler](../00_foundation/009_decision_register.md)
- [ASK-067 — Test stratejisi ve gereksinim izlenebilirliği](../06_quality/067_test_strategy_traceability.md)
- [ASK-074 — Başlangıç backlog'u ve ilk iki sprint](../07_production/074_backlog_first_sprints.md)
- [ASK-078 — Salt okunur MCP bilgi servisi şartnamesi](078_mcp_readonly_spec.md)

