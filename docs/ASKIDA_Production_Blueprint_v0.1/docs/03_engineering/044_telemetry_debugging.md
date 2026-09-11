---
doc_id: ASK-044
title: "Tanı, ölçüm ve gizlilik"
version: 0.1.0
status: proposed
owner_role: QA
last_reviewed: 2026-09-07
dependencies: ["ASK-034","ASK-035","ASK-065","ASK-068"]
---

# Tanı, ölçüm ve gizlilik

## Geliştirme tanısı
Debug overlay: build/epoch/host tick, phase, actor count, awake rigidbodies, active joints, command rejects, bytes/sec, correction büyüklüğü, delivery ledger count. Overlay shipping'te kullanıcıya zorunlu gösterilmez; destek modu bilinçli açılır. Tam obje listesini her kare string'e çevirmek profiler'ı bozabilir.

## Olay şeması
EventName, schemaVersion, monotonicTime, buildId, sessionRandomId, contractId, reasonCode ve sınırlı sayısal alanlar. Oyuncunun serbest yazdığı mesaj, ses kaydı, token ve dosya sistemindeki kişisel yol varsayılan telemetri değildir. SessionRandomId farklı oturumlarda kalıcı kimlik takibi yaratmaz.

## Oynanış olayları
contract_started, first_grab, first_secure, recovery_used, mandatory_lost, contract_settled, reconnect_result. Bu liste her tuşun kaydı anlamına gelmez. Event dedup anahtarı ve örnekleme kararı bulunur. Telemetri gönderilemezse oyun durmaz; kuyruk disk/bellek sınırı taşır.

## Performans ölçümü
Frame time dağılımı, fizik işleme, GC allocation, active object ve GPU pass maliyeti tutulur. Editor ile release player sonuçları ayrı etiketlenir. Logging açık profilde ölçülen performans son ürün performansı diye sunulmaz. Profiler overhead kontrolü yapılır.

## Kullanıcı araştırması
Video veya ses kaydı için açık bilgilendirme/onay gerekir; oyun telemetrisine gizlice ses eklenmez. Veri amacı, saklama süresi, erişebilen rol ve silme yöntemi yayın öncesi belirlenir. Bu plan herhangi bir hukuk bölgesinde otomatik uyumluluk sağlamaz.

## Kabul
Bağlantı bileti ve kişisel dosya yolu log taramasında görünmez; telemetry kapalıyken gameplay aynı çalışır; kuyruk sınırsız büyümez. Hata raporu kullanıcıya export önizlemesi sunar. Test sonucu ile oyuncu yorumu ayrı veri türleri olarak saklanır.

## İlgili kaynak belgeler
- [ASK-034 — Ağ güvenliği ve kötüye kullanım sınırları](../02_network/034_security_abuse.md)
- [ASK-035 — Multiplayer test matrisi ve kanıt](../02_network/035_network_test_matrix.md)
- [ASK-065 — Performans hedefi ve ölçüm disiplini](../06_quality/065_performance_targets.md)
- [ASK-068 — Oyuncu testi, eğlence ve okunurluk](../06_quality/068_playtest_protocol.md)

