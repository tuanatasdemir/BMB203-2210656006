using OtelRezervasyon.Common;
using OtelRezervasyon.Exceptions;
using OtelRezervasyon.Models;
using OtelRezervasyon.Strategies;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("--- 3. Gün: ArtanCeza Stratejisi ve Oda Taban Sınıf Sabitleri Testi ---");
Console.WriteLine($"Oda Sabit Birim Fiyatı: {Oda.BirimFiyat} TL\n");

ICezaPolitikasi sabitCeza = new GunlukSabitCeza();
ICezaPolitikasi artanCeza = new ArtanCeza();

int[] testGecikmeler = { 0, 4, 6, 8 };

foreach (int gun in testGecikmeler)
{
    Console.WriteLine($"Gecikme: {gun} birim -> Sabit Ceza: {sabitCeza.Ceza(gun),-12} | Artan Ceza: {artanCeza.Ceza(gun)}");
}