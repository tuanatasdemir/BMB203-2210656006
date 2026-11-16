using OtelRezervasyon.Common;
using OtelRezervasyon.Exceptions;
using OtelRezervasyon.Models;
using OtelRezervasyon.Strategies;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("--- 4. Gün: StandartOda ve SuitOda Modelleri Testi ---\n");

// 1. Test: StandartOda (Yatak sayısı 3 -> %25 ek ücret tetiklenmeli)
Oda standartOdaNormal = new StandartOda(101, 1, 2); // Normal yatak (2)
Oda standartOdaEkli = new StandartOda(102, 1, 3);   // Ekli yatak (>2)

int testGece = 4;
Console.WriteLine($"Oda 1 (2 Yatak, {testGece} Gece): {standartOdaNormal.Hesapla(testGece)}"); // 50 * 4 * 2.0 = 400 TL
Console.WriteLine($"Oda 2 (3 Yatak, {testGece} Gece): {standartOdaEkli.Hesapla(testGece)}");     // (50 * 4 * 2.0) * 1.25 = 500 TL

// 2. Test: SuitOda
Oda suitOda = new SuitOda(201, 2, 45.0);
Console.WriteLine($"Suit Oda (45 m², {testGece} Gece): {suitOda.Hesapla(testGece)}");       // 50 * 4 * 1.75 = 350 TL

// 3. Test: Geçersiz yatak sayısı denemesi (Hata yönetimi)
try
{
    Console.WriteLine("\nDeneniyor: 5 yataklı standart oda oluşturma...");
    Oda hataliOda = new StandartOda(103, 1, 5);
}
catch (ArgumentOutOfRangeException ex)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"[YAKALANAN İSTİSNA]: {ex.ParamName} -> {ex.Message}");
    Console.ResetColor();
}