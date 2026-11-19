using OtelRezervasyon.Common;
using OtelRezervasyon.Exceptions;
using OtelRezervasyon.Models;
using OtelRezervasyon.Services;
using OtelRezervasyon.Strategies;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("================================================================================");
Console.WriteLine("  BMB203 NESNEYE YÖNELİK PROGRAMLAMA DÖNEM ÖDEVİ");
Console.WriteLine("  Ödev Kodu: NYP-OTL-656006 | Öğrenci: 2210656006 - Nimet Tuana Taşdemir");
Console.WriteLine("  Alan: Otel Rezervasyon Simülasyonu");
Console.WriteLine("================================================================================\n");

// --------------------------------------------------------------------------------
// ADIM 1: Otel nesnesini kapasite 45 ile ve GunlukSabitCeza stratejisiyle oluşturun.
// --------------------------------------------------------------------------------
Console.WriteLine(">>> [ADIM 1] Otel Nesnesi Başlatılıyor (Kapasite: 45, Strateji: GunlukSabitCeza)...");
ICezaPolitikasi baslangicStratejisi = new GunlukSabitCeza();
var otel = new Otel(kapasite: 45, cezaPolitikasi: baslangicStratejisi);
Console.WriteLine($"Otel oluşturuldu. Kapasite: {otel.Kapasite} | Aktif Strateji: {otel.AktifCezaPolitikasi.PolitikaAdi}\n");

// --------------------------------------------------------------------------------
// ADIM 2: Her türden en az iki olmak üzere toplam 6 Oda kaydı ekleyin (RezervasyonYap).
// --------------------------------------------------------------------------------
Console.WriteLine(">>> [ADIM 2] Her Türden 2 Olmak Üzere Toplam 6 Oda Rezervasyonu Ekleniyor...");

var oda1 = new StandartOda(odaNo: 101, kat: 1, yatakSayisi: 2);
var oda2 = new StandartOda(odaNo: 102, kat: 1, yatakSayisi: 3); // YatakSayisi = 3 (%25 ek ücret)
var oda3 = new SuitOda(odaNo: 201, kat: 2, salonM2: 35.0);
var oda4 = new SuitOda(odaNo: 202, kat: 2, salonM2: 50.0);
var oda5 = new AileOdasi(odaNo: 301, kat: 3, cocukYatagi: 1);
var oda6 = new AileOdasi(odaNo: 302, kat: 3, cocukYatagi: 2);

var rez1 = otel.RezervasyonYap(oda1, "Ahmet Yılmaz", new DateTime(2026, 11, 15), new DateTime(2026, 11, 18), 2);
var rez2 = otel.RezervasyonYap(oda2, "Mehmet Demir", new DateTime(2026, 11, 20), new DateTime(2026, 11, 27), 3);
var rez3 = otel.RezervasyonYap(oda3, "Ayşe Kaya", new DateTime(2026, 12, 01), new DateTime(2026, 12, 04), 2);
var rez4 = otel.RezervasyonYap(oda4, "Fatma Çelik", new DateTime(2026, 12, 05), new DateTime(2026, 12, 10), 3);
var rez5 = otel.RezervasyonYap(oda5, "Ali Öztürk", new DateTime(2026, 12, 12), new DateTime(2026, 12, 15), 3);
var rez6 = otel.RezervasyonYap(oda6, "Zeynep Şahin", new DateTime(2026, 12, 18), new DateTime(2026, 12, 22), 4);

foreach (var rez in otel.Rezervasyonlar)
{
    Console.WriteLine($"  + {rez}");
}
Console.WriteLine($"Toplam Eklenen Oda: {otel.Odalar.Count} | Toplam Rezervasyon: {otel.Rezervasyonlar.Count}\n");