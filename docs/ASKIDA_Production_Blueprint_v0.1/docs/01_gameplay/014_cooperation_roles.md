---
doc_id: ASK-014
title: "Co-op roller ve oyuncu sayısı ölçekleme"
version: 0.1.0
status: proposed
owner_role: GameDesign
last_reviewed: 2026-09-07
dependencies: ["ASK-017","ASK-018","ASK-019","ASK-023"]
---

# Co-op roller ve oyuncu sayısı ölçekleme

## Roller sınıf değildir
Taşıyıcı, dengeleyici, vinç operatörü ve rota gözcüsü anlık görevlerdir. Karakter seçimi kütle veya beceri avantajı vermez. Aynı oyuncu eşyayı bıraktıktan sonra vinci devralabilir. Zorunlu mikrofon yoktur; yön, durdur, yardıma gel ve eşya pingleri bütün çekirdek çağrıları karşılar.

## İki kişi
İki oyuncu ağır eşyayı birlikte yerleştirir; sabitler; biri vinci kullanırken diğeri denge/rota kontrolü yapar. Vince erişmek için yükü güvensiz biçimde bırakmak zorunlu olmamalıdır. Güvenli park ve sabitleme bu yüzden temel mekaniktir. Bir eşya mutlaka üç veya dört el gerektiremez; v1 bütün zorunlu görevler iki kişiyle tamamlanabilir.

## Üç ve dört kişi
Ek oyuncular hazırlık, denge ve sonraki yük görevleriyle paralellik sağlar. Bir eşyada en fazla iki holder bulunur; üçüncü kişi serbest itme veya denge işi yapar. Fazladan oyuncu otomatik üç kat kuvvet vermez. Dar geçitte dört kişinin birbirini sürekli kilitlemesi eğlence hedefi değildir; karakter-karakter çarpışması yumuşak çözüm veya uygun collision layer ile spike'ta denenir.

## Zorluk sabitleme
Başlangıç oyuncu sayısı ContractRun'a kaydedilir. Gerekli adet, süre ve isteğe bağlı hedefler bu sayıya göre tanımdan seçilebilir; Active sırasında kopmayla yeniden ölçeklenmez. Aksi durumda ayrılıp girerek ödül sömürüsü oluşabilir. Ancak iki kişilik uygulanabilir rota her konfigürasyonun temelidir.

## Toksik davranışa sınır
Host lobi erişimini ve oyuncu çıkarma yetkisini yönetir. Çıkarma holder/driver haklarını hemen temizler. V1 arkadaş odaklıdır; bu, istemcinin para veya teleport yetkisi olması anlamına gelmez. Oyuncuların birbirini sonsuz kilitlemesine izin veren tutma/sabitleme kilitleri süreli ve kurtarılabilir olmalıdır.

## Kabul
İki kişilik testte hiçbir anda üçüncü kişi zorunluluğu oluşmaz. Dört kişilik testte her oyuncu görev boyunca anlamlı iş bulur. Ortalama bekleme süresi ve tek oyuncunun baskın iş oranı gözlemle kaydedilir, test yapılmadan “dengeli” denmez.

## İlgili kaynak belgeler
- [ASK-017 — Ortak taşıma ve kuvvet modeli](017_shared_carrying.md)
- [ASK-018 — Platform, destek kütlesi ve kontrollü eğim](018_platform_balance.md)
- [ASK-019 — Vinç kontrolü ve kontrol kiralaması](019_winch_control.md)
- [ASK-023 — Sözleşme tanımı, başarı ve bitirme](023_contract_rules.md)

