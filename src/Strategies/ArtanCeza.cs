using OtelRezervasyon.Common;
using OtelRezervasyon.Exceptions;

namespace OtelRezervasyon.Strategies;

/// <summary>
/// İlk 6 gecikme birimi için 40 TL, 6 birimden sonraki her birim için iki katı (80 TL) ceza uygulayan zorunlu strateji.
/// </summary>
public class ArtanCeza : ICezaPolitikasi
{
    public const int EsikBirim = 6;
    public const decimal TemelBirimCeza = 40m;
    public const decimal ArtanBirimCeza = TemelBirimCeza * 2m; // 80 TL

    public string PolitikaAdi => "Artan Ceza (İlk 6 birim 40 TL, sonrası 80 TL)";

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

        if (gecikme <= EsikBirim)
        {
            return new Para(gecikme * TemelBirimCeza, "TL");
        }

        decimal ilkKisim = EsikBirim * TemelBirimCeza; // 6 * 40 = 240 TL
        int asanBirim = gecikme - EsikBirim;
        decimal asanKisim = asanBirim * ArtanBirimCeza; // Aşan * 80 TL

        return new Para(ilkKisim + asanKisim, "TL");
    }
}