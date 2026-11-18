using OtelRezervasyon.Common;
using OtelRezervasyon.Exceptions;
using OtelRezervasyon.Models;
using OtelRezervasyon.Services;
using OtelRezervasyon.Strategies;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("--- 6. Gün: Otel Servisi, DI, Indexer ve Rezervasyon Akışı Testi ---\n");

// 1. DI ile Otel oluşturma (Kapasite: 45, Strateji: GunlukSabitCeza)
var otel = new Otel(45, new GunlukSabitCeza());
Console.WriteLine($"Otel oluşturuldu. Kapasite: {otel.Kapasite}, Strateji: {otel.AktifCezaPolitikasi.PolitikaAdi}");

// 2. RezervasyonYap testi
var oda101 = new StandartOda(101, 1, 2);
var oda201 = new SuitOda(201, 2, 35.0);

var rez1 = otel.RezervasyonYap(oda101, "Ahmet Yılmaz", new DateTime(2026, 11, 18), new DateTime(2026, 11, 21), 2);
var rez2 = otel.RezervasyonYap(oda201, "Ayşe Kaya", new DateTime(2026, 11, 20), new DateTime(2026, 11, 23), 2);

// 3. Indexer erişim testi
Console.WriteLine($"\nIndexer Testi -> otel[0]: {otel[0]}");
Console.WriteLine($"Indexer Testi -> otel[1]: {otel[1]}");

// 4. Tarih çakışması testi (Oda 101 için çakışan tarih)
try
{
    Console.WriteLine("\nDeneniyor: Oda 101 için çakışan tarih (19-22 Kasım) rezervasyonu...");
    otel.RezervasyonYap(oda101, "Mehmet Demir", new DateTime(2026, 11, 19), new DateTime(2026, 11, 22), 1);
}
catch (OdaDoluException ex)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"[YAKALANAN İSTİSNA]: {ex.Message}");
    Console.ResetColor();
}

// 5. Strateji değiştirme ve CikisYap testi
otel.CezaPolitikasiDegistir(new ArtanCeza());
Console.WriteLine($"\nYeni Strateji: {otel.AktifCezaPolitikasi.PolitikaAdi}");

Para odenecekTutar = otel.CikisYap(rez1.Id, gecikmeBirimi: 2);
Console.WriteLine($"Rezervasyon 1 Çıkış Yapıldı (2 birim gecikmeli toplam): {odenecekTutar}");
Console.WriteLine($"Otel Toplam Konaklama Geliri: {otel.ToplamKonaklamaUcretiHesapla()}");