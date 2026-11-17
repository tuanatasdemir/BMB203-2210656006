using OtelRezervasyon.Common;
using OtelRezervasyon.Exceptions;
using OtelRezervasyon.Models;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("--- 5. Gün: AileOdasi Polimorfizm ve Rezervasyon Değişmezleri Testi ---\n");

// 1. Polimorfik Oda Koleksiyonu Testi
List<Oda> odalar = new()
{
    new StandartOda(101, 1, 2),
    new SuitOda(201, 2, 40.0),
    new AileOdasi(301, 3, 2) // 2 yetişkin + 2 çocuk yatağı = 4 kişi kapasiteli
};

foreach (Oda oda in odalar)
{
    Console.WriteLine($"{oda} -> 3 Gece Ücreti: {oda.Hesapla(3)}");
}

// 2. Geçerli Rezervasyon ve Tarih Çakışması Kontrolü
Console.WriteLine("\n--- Rezervasyon ve Tarih Çakışması Testi ---");
var aileOdasi = odalar[2];
var rez1 = new Rezervasyon(aileOdasi, "Zeynep Şahin", new DateTime(2026, 12, 10), new DateTime(2026, 12, 15), 4);
Console.WriteLine(rez1);
Console.WriteLine($"Hesaplanan Ücret: {rez1.KonaklamaUcretiHesapla()}");
Console.WriteLine($"12-14 Aralık ile çakışıyor mu?: {rez1.TarihCakisiyorMu(new DateTime(2026, 12, 12), new DateTime(2026, 12, 14))}");
Console.WriteLine($"15-18 Aralık ile çakışıyor mu?: {rez1.TarihCakisiyorMu(new DateTime(2026, 12, 15), new DateTime(2026, 12, 18))}");

// 3. Kişi Sayısı Kapasite Aşımı Testi (OdaDoluException)
try
{
    Console.WriteLine("\nDeneniyor: 4 kişilik Aile Odasına 6 kişilik rezervasyon açma...");
    var hataliRez = new Rezervasyon(aileOdasi, "Kalabalık Aile", new DateTime(2026, 12, 20), new DateTime(2026, 12, 22), 6);
}
catch (OdaDoluException ex)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"[YAKALANAN İSTİSNA]: {ex.Message}");
    Console.ResetColor();
}