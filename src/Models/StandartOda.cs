using OtelRezervasyon.Common;

namespace OtelRezervasyon.Models;

/// <summary>
/// Standart oda modeli: 1-3 arası yatak sayısı, 2.00 katsayı ve yatak > 2 için %25 ek ücret kuralı içerir.
/// </summary>
public class StandartOda : Oda
{
    private int _yatakSayisi;

    public StandartOda(int odaNo, int kat, int yatakSayisi) : base(odaNo, kat)
    {
        YatakSayisi = yatakSayisi;
    }

    public int YatakSayisi
    {
        get => _yatakSayisi;
        private set
        {
            if (value < 1 || value > 3)
            {
                throw new ArgumentOutOfRangeException(nameof(YatakSayisi), "Standart oda yatak sayısı 1 ile 3 arasında olmalıdır.");
            }
            _yatakSayisi = value;
        }
    }

    public override decimal TurKatsayisi => 2.00m;
    public override int MaksimumKapasite => YatakSayisi; // Yatak sayısı kadar kişi kalabilir

    public override Para Hesapla(int gece)
    {
        if (gece <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(gece), "Konaklama süresi en az 1 gece olmalıdır.");
        }

        // Temel formül: birim fiyat (50 TL) x gece x tür katsayısı (2.00)
        decimal temelUcret = BirimFiyat * gece * TurKatsayisi;

        // YatakSayisi > 2 ise sonuca %25 ek uygulanır
        if (YatakSayisi > 2)
        {
            temelUcret *= 1.25m;
        }

        return new Para(temelUcret, "TL");
    }

    public override string ToString()
    {
        return $"{base.ToString()} - Tip: StandartOda | Yatak Sayısı: {YatakSayisi} | Katsayı: {TurKatsayisi:0.00}";
    }
}