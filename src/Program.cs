using OtelRezervasyon.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Para ucret1 = new(250m, "TL");
Para ucret2 = new(150m, "TL");
Console.WriteLine($"Toplam: {ucret1 + ucret2}");