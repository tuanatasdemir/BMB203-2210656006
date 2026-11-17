using OtelRezervasyon.Common;

namespace OtelRezervasyon.Models;

/// <summary>
/// Aile odası modeli: 1-3 arası çocuk yatağı alanı ve 1.75 tür katsayısı içerir.
/// </summary>
public class AileOdasi : Oda
{
    private int _cocukYatagi;

    public AileOdasi(int odaNo, int kat, int cocukYatagi) : base(odaNo, kat)
    {
        CocukYatagi = cocukYatagi;
    }

    public int CocukYatagi
    {
        get => _cocukYatagi;
        private set
        {
            if (value < 1 || value > 3)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(CocukYatagi), 
                    "Aile odası çocuk yatağı sayısı 1 ile 3 arasında olmalıdır.");
            }
            _cocukYatagi = value;
        }
    }

    public override decimal TurKatsayisi => 1.75m;

    // 2 yetişkin + çocuk yatağı sayısı kadar toplam kapasite
    public override int MaksimumKapasite => 2 + CocukYatagi;

    public override Para Hesapla(int gece)
    {
        if (gece <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(gece), "Konaklama süresi en az 1 gece olmalıdır.");
        }

        // Temel formül: birim fiyat (50 TL) x gece x tür katsayısı (1.75)
        decimal temelUcret = BirimFiyat * gece * TurKatsayisi;

        return new Para(temelUcret, "TL");
    }

    public override string ToString()
    {
        return $"{base.ToString()} - Tip: AileOdasi | Çocuk Yatağı: {CocukYatagi} | Katsayı: {TurKatsayisi:0.00}";
    }
}