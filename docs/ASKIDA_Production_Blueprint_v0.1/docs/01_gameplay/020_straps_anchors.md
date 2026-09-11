---
doc_id: ASK-020
title: "Sabitleme, kayış ve anchor sözleşmesi"
version: 0.1.0
status: proposed
owner_role: Gameplay
last_reviewed: 2026-09-07
dependencies: ["ASK-016","ASK-018","ASK-019","ASK-049"]
---

# Sabitleme, kayış ve anchor sözleşmesi

## Basitleştirme
Kayış sisteminin amacı taşıma sırasında eşyayı güvenli hale getirmektir. V1 kayışı serbest halat fiziği değildir; platform üzerinde önceden tanımlanmış anchor slotu, kısıtlı uygun eşya ve görsel bağlantı kullanır. Bir item aynı anda yalnız bir platforma Secured olabilir.

## Secure önkoşulları
Available + Free; holder sayısı sıfır; item deck üzerinde geçerli alan içinde; vinç hızı eşik altında; oyuncu erişim mesafesinde; anchor boş; eşya ailesi slot kapasitesine uygun. “R'ye bastım, herkesin elinden zorla aldım” davranışı yoktur. Geçersizse neyin düzeltilmesi gerektiği gösterilir.

## İşlem sınırı
Host anchor rezervasyonu, handling değişimi ve platform-local bağlı poz kaydını tek işlemde yapar. Fizik uygulaması güvenli bağlı temsil oluşturamazsa rezervasyon geri alınır. Görsel kayış sonradan yüklenemese bile oyun kilidi doğru kalır; görünmez kayış oyuncuya kabul edilebilir shipping sonuç değildir, art/QA hatasıdır.

Secured item serbest dinamik simülasyon ile ayrıca sürülmez. Kütlesi SupportedSet'e bir kere girer. Öneri bağlı kinematik temsil veya doğrulanmış constraint yaklaşımıdır; seçimin collision ve ağ etkisi spike'ta karşılaştırılır. Mesh'i parent etmek tek başına fizik çözümü sayılmaz.

## Unsecure
Available + Secured; mevcut platform ilişkisi; vinç güvenli duruşta; erişim geçerli. Bağ kaldırılırken world pose korunur. Yeni serbest rigidbody'ye platformun doğrulanmış noktasal hızı aktarılır, kopyalanmış render farkından rastgele hız üretilmez. Çok hızlı hareket veya penetrasyon varsa ret ve düzeltme mesajı gerekir.

## Kayış kapasitesi ve üretim
Anchor sayısı görev platform profilinde tanımlanır. Yeni anchor mesh'i eklemek gameplay kapasitesini otomatik artırmaz. Kayış uçları authored socket'lardan alınır; nesneye göre minimum iki görünür temas noktası stil kontrolünden geçer. Kayış renginin okunması tek doğrulama değildir; kilit simgesi ve kısa ses gerekir.

## Test
İki Secure isteği aynı slota gelir: bir kabul. Biri Unsecure yaparken host kopar: kalıcı mid-run fizik kaydı olmadığı için bir sonraki açılış eski checkpoint'e döner. Eşya Lost olursa anchor temizlenir; slot sonsuza kadar dolu kalmaz. Kayış görseli yeniden bağlanmada snapshot'tan üretilebilir olmalıdır.

## Gereksinim ve test izleri
- REQ-010 → TC-010 — Anchor işlemi tek ve atomik olmalı

Durum ve ayrıntılı adımlar: [gereksinim kayıtları](../../data/requirements.json) ve [test kayıtları](../../data/test_cases.json). Oyun testleri henüz çalıştırılmadı.

## İlgili kaynak belgeler
- [ASK-016 — Eşya semantiği ve değişmezler](016_item_state_model.md)
- [ASK-018 — Platform, destek kütlesi ve kontrollü eğim](018_platform_balance.md)
- [ASK-019 — Vinç kontrolü ve kontrol kiralaması](019_winch_control.md)
- [ASK-049 — Her nesne için üretim şartnamesi](../04_art/049_prop_asset_spec.md)

