using OtelRezervasyon.Common;
using OtelRezervasyon.Exceptions;
using OtelRezervasyon.Strategies;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("--- 2. Gün: Özel İstisnalar ve Strateji Testi ---");

ICezaPolitikasi cezaYok = new CezaYok();
ICezaPolitikasi sabitCeza = new GunlukSabitCeza();

Console.WriteLine($"{cezaYok.PolitikaAdi} (3 gün gecikme): {cezaYok.Ceza(3)}");
Console.WriteLine($"{sabitCeza.PolitikaAdi} (3 gün gecikme): {sabitCeza.Ceza(3)}");

try
{
    throw new OdaDoluException(101, new DateTime(2026, 11, 15), new DateTime(2026, 11, 18));
}
catch (OdaDoluException ex)
{
    Console.WriteLine($"Yakalanan İstisna 1: {ex.Message} (Oda No: {ex.OdaNo})");
}

try
{
    throw new GecersizTarihException(new DateTime(2026, 11, 20), new DateTime(2026, 11, 18));
}
catch (GecersizTarihException ex)
{
    Console.WriteLine($"Yakalanan İstisna 2: {ex.Message}");
}