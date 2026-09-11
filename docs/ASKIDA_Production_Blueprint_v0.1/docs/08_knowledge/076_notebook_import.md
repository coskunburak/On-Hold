---
doc_id: ASK-076
title: "Notebook aktarımı ve tek kaynak ilkesi"
version: 0.1.0
status: proposed
owner_role: Knowledge
last_reviewed: 2026-09-07
dependencies: ["ASK-001","ASK-003","ASK-077","ASK-079"]
---

# Notebook aktarımı ve tek kaynak ilkesi

## Kaynak düzeni
Kanonik bilgi Git'te yönetilecek Markdown belgeleridir. Notebook aracı sorgulama/özetleme katmanıdır; otomatik ürün kararı veya depo yazma kaynağı değildir. Bu arşivde bir Git deposu veya notebook hesabı oluşturulmadı. Kullanıcı kendi ortamında zip'i açıp kaynakları içe alır.

## Aktarım dosyaları
notebook_exports/volume_01.txt … volume_10.txt her biri sekiz kaynak belge içerir. Her bölümün başında ASK kimliği, yol, sürüm, durum ve SHA-256 bulunur. TXT ciltleri kaynak Markdown'dan türetilmiştir, elle düzenlenmez. ZIP dosyasının kendisinin tüm belgeleri otomatik içe alacağı varsayılmaz.

## Ürün desteği
Google'ın güncel yardımında Markdown ve TXT kaynakları destekleniyor; plan başına kaynak sınırı değişebildiği için on cilt pratik bir aktarım seçeneğidir. Yerel dosya yüklemelerini Git ile otomatik senkron saymayın. [Kaynak ekleme ve desteklenen türler](https://support.google.com/notebooklm/answer/16215270?hl=en). Ürün adı/arayüzü ve plan sınırları değişebilir; işlem günü kontrol edin.

## Kullanım adımları
1. Yeni notebook oluştur; README ve gerekli ciltleri seç.
2. İlk soruda “Öneri ile onaylı kararı ayır; ASK kimliği ve sürümle cevapla” de.
3. Her yanıtın işaret ettiği kanonik dosyayı doğrula.
4. Değişiklik önerisini kullanıcı kararı olarak değil taslak fark olarak kaydet.
5. Onaylı değişikliği depodaki Markdown'a uygula; validator ve export builder çalıştır.
6. Eski yerel upload sürümünü yeni ciltle kontrollü değiştir; iki sürümü aynı notebook'ta açık bırakma.

## Sorgu örnekleri
“ASK-016 ve ASK-022 arasında terminal durum çelişkisi var mı?”; “W-004 için önkoşullar ve TC kimliklerini çıkar”; “ASK-047'de ücretsiz olduğu doğrulanan tier ile henüz arşivi incelenmemiş özellikleri ayır.” Notebook cevabı yeni kaynak değildir; gözden geçirilmemiş cevabı manifest'e eklemeyin.

## Kabul
Rastgele seçilen üç ASK için araç doğru sürüm ve dosya yolunu gösterebilmeli. Eski export hash'i fark edilirse yeniden üretim gerekir. Notebook sağlayıcısına özel veya lisanslı ham kaynak yüklemek otomatik yetki kapsamı değildir.

## Gereksinim ve test izleri
- REQ-039 → TC-039 — Bilgi tabanı izlenebilir ve salt okunur kullanılmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-001 — ASKIDA — Üretim bilgi tabanı başlangıcı](../../README.md)
- [ASK-003 — Belge haritası ve okuma rotaları](../00_foundation/003_document_map.md)
- [ASK-077 — Codex görev bağlamı ve güvenilir iş akışı](077_codex_task_workflow.md)
- [ASK-079 — Belge sürümleme, şablonlar ve değişiklik yönetimi](079_versioning_templates.md)

