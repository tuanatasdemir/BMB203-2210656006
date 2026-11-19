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

// --------------------------------------------------------------------------------
// ADIM 3: StandartOda (YatakSayisi = 3) için 7 gece hesabı ve %25 ek ücret gösterimi.
// --------------------------------------------------------------------------------
Console.WriteLine(">>> [ADIM 3] StandartOda (YatakSayisi = 3) 7 Gece Hesaplama ve %25 Ek Ücret Kontrolü...");
int hesapGece = 7;
Para standartOdaUcreti = oda2.Hesapla(hesapGece);

decimal tabanTutar = Oda.BirimFiyat * hesapGece * oda2.TurKatsayisi; // 50 * 7 * 2.00 = 700 TL
decimal ekUcret = tabanTutar * 0.25m;                                // 700 * 0.25 = 175 TL

Console.WriteLine($"  İncelenen Kayıt : {oda2}");
Console.WriteLine($"  Temel Formül    : {Oda.BirimFiyat:N2} TL x {hesapGece} Gece x {oda2.TurKatsayisi:0.00} = {tabanTutar:N2} TL");
Console.WriteLine($"  Ek Ücret Kuralı : YatakSayisi ({oda2.YatakSayisi}) > 2 olduğu için +%25 ek uygulandı (+{ekUcret:N2} TL)");
Console.WriteLine($"  Nihai Ücret     : {standartOdaUcreti}\n");

// --------------------------------------------------------------------------------
// ADIM 4: Kural ihlali denemesi - Aynı oda için tarihleri çakışan iki rezervasyon olamaz.
// --------------------------------------------------------------------------------
Console.WriteLine(">>> [ADIM 4] Kural İhlali Denemesi: Aynı Odaya Çakışan Tarihte Rezervasyon...");
try
{
    Console.WriteLine("  Deneniyor: Oda 101 (mevcut: 2026-11-15 / 2026-11-18) için 2026-11-16 / 2026-11-19 rezervasyonu...");
    otel.RezervasyonYap(oda1, "Çakışan Misafir", new DateTime(2026, 11, 16), new DateTime(2026, 11, 19), 2);
}
catch (OdaDoluException ex)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"  [YAKALANAN İSTİSNA]: {ex.Message}");
    Console.ResetColor();
}
Console.WriteLine();

// --------------------------------------------------------------------------------
// ADIM 5: OdaDoluException tetikleme senaryosu (program çökmeden devam eder).
// --------------------------------------------------------------------------------
Console.WriteLine(">>> [ADIM 5] OdaDoluException Tetikleme (Kapasite Aşımı) ve Güvenli Devam...");
try
{
    Console.WriteLine($"  Deneniyor: Maksimum {oda1.MaksimumKapasite} kişilik Oda 101 için 5 kişilik rezervasyon...");
    otel.RezervasyonYap(oda1, "Kalabalık Grup", new DateTime(2026, 12, 25), new DateTime(2026, 12, 28), 5);
}
catch (OdaDoluException ex)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"  [YAKALANAN İSTİSNA]: {ex.Message}");
    Console.ResetColor();
}

// Ek Doğrulama: İkinci zorunlu istisna (GecersizTarihException) kontrolü
try
{
    Console.WriteLine("  Ek Kontrol (GecersizTarihException): Çıkış tarihi giriş tarihinden önce veriliyor...");
    otel.RezervasyonYap(oda1, "Hatalı Tarih Misafiri", new DateTime(2026, 12, 28), new DateTime(2026, 12, 25), 1);
}
catch (GecersizTarihException ex)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"  [YAKALANAN İSTİSNA]: {ex.Message}");
    Console.ResetColor();
}
Console.WriteLine("  -> İstisnalar yakalandı, program çökmeden normal akışına devam ediyor.\n");