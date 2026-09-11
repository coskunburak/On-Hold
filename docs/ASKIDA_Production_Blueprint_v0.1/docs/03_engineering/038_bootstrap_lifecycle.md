---
doc_id: ASK-038
title: "Bootstrap, servis ömrü ve sahne geçişleri"
version: 0.1.0
status: proposed
owner_role: Gameplay
last_reviewed: 2026-09-07
dependencies: ["ASK-012","ASK-036","ASK-037","ASK-045"]
---

# Bootstrap, servis ömrü ve sahne geçişleri

## Tek başlangıç
Bootstrap scene yapılandırmayı okur, yerel ayarları yükler, platform servislerini başlatır ve Frontend'e geçer. Üretim build'i rastgele gameplay sahnesinden başlamaz. Editor hızlı oynatma aracı eksik bootstrap'ı görünür biçimde oluşturabilir; gizli singleton zinciriyle farklı davranış üretmez.

## Ömürler
Application lifetime: ayarlar, platform adapter, log yöneticisi. Session lifetime: transport, roster, epoch, command gateway. Contract lifetime: item registry, platform, delivery ledger, timer. Scene presentation lifetime: kamera, HUD, ses emitters. Contract kapanınca application servisi yanlışlıkla silinmez; session kapanınca eski item referansları kalmaz.

## Başlatma sırası
Config validation → platform init → frontend. Host session: transport → handshake registry → level load → spawn registry → snapshot ready → Briefing. Her adım cancellation token veya eşdeğer kontrollü iptal yolu taşır. Bir sonraki adım hata alırsa kurulan önceki kaynaklar ters sırada temizlenir.

## Teardown
Yeni input'u kapat; domain phase'i bitir; holder/driver haklarını kaldır; network spawn'ları kontrollü kaldır; event aboneliklerini bırak; sahneyi boşalt; session cache'i sıfırla. Teardown ikinci çağrıda crash olmamalıdır. DontDestroyOnLoad nesneleri adına bakarak çoğaltılmaz.

## İçerik yükleme
V1 küçük içerikte standart scene/prefab referansları yeterliyse Addressables zorunlu değildir. Ölçüm ve paketleme gereksinimi gösterirse ek ADR ile seçilir. Asenkron yükleme sırasında progress kullanıcıya gerçek aşamayı anlatır; sahte yüzde son aşamada sonsuza dek takılmaz.

## Test
Arka arkaya 20 lobi → görev → sonuç → lobi geçişinde servis/abonelik sayısı artmaz. İptal edilen yükleme UI'ı kilitlemez. Offline frontend servis başlatma hatasını açıklayabilir. Testte hangi sahnenin yüklü olduğu ve active session epoch debug panelinde görünür.

## Gereksinim ve test izleri
- REQ-021 → TC-021 — Sahne yaşam döngüsü kaynak sızdırmamalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-012 — Oturum ve görev durum makinesi](../01_gameplay/012_session_state_machine.md)
- [ASK-036 — Unity deposu ve dosya hiyerarşisi](036_unity_repository.md)
- [ASK-037 — Modüller, assembly sınırları ve bağımlılıklar](037_module_boundaries.md)
- [ASK-045 — Hata sınıfları ve güvenli bozulma](045_errors_resilience.md)

