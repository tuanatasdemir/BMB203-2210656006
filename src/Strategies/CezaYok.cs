using OtelRezervasyon.Common;
using OtelRezervasyon.Exceptions;

namespace OtelRezervasyon.Strategies;

/// <summary>
/// Gecikme durumunda herhangi bir ceza uygulamayan strateji (0 TL döner).
/// </summary>
public class CezaYok : ICezaPolitikasi
{
    public string PolitikaAdi => "Ceza Yok";

    public Para Ceza(int gecikme)
    {
        if (gecikme < 0)
        {
            throw new GecersizTarihException(gecikme, "Gecikme birimi negatif olamaz.");
        }

        return Para.Sifir("TL");
    }
}