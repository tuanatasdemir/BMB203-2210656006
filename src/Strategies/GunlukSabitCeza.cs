using OtelRezervasyon.Common;
using OtelRezervasyon.Exceptions;

namespace OtelRezervasyon.Strategies;

/// <summary>
/// Her gecikme birimi için sabit 40 TL ceza ekleyen strateji.
/// </summary>
public class GunlukSabitCeza : ICezaPolitikasi
{
    public const decimal GunlukCezaTutari = 40m;

    public string PolitikaAdi => "Günlük Sabit Ceza (40 TL/Birim)";

    public Para Ceza(int gecikme)
    {
        if (gecikme < 0)
        {
            throw new GecersizTarihException(gecikme, "Gecikme birimi negatif olamaz.");
        }

        if (gecikme == 0)
        {
            return Para.Sifir("TL");
        }

        return new Para(gecikme * GunlukCezaTutari, "TL");
    }
}