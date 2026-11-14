using OtelRezervasyon.Common;

namespace OtelRezervasyon.Strategies;

/// <summary>
/// Geç çıkış veya gecikme durumlarında uygulanacak ceza hesaplama stratejisi arayüzü.
/// </summary>
public interface ICezaPolitikasi
{
    string PolitikaAdi { get; }
    Para Ceza(int gecikme);
}