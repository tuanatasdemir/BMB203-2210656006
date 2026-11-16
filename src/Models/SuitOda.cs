using OtelRezervasyon.Common;

namespace OtelRezervasyon.Models;

/// <summary>
/// Suit oda modeli: 15-60 m² salon alanı aralığı ve 1.75 katsayı içerir.
/// </summary>
public class SuitOda : Oda
{
    private double _salonM2;

    public SuitOda(int odaNo, int kat, double salonM2) : base(odaNo, kat)
    {
        SalonM2 = salonM2;
    }

    public double SalonM2
    {
        get => _salonM2;
        private set
        {
            if (value < 15.0 || value > 60.0)
            {
                throw new ArgumentOutOfRangeException(nameof(SalonM2), "Suit oda salon alanı 15 ile 60 m² arasında olmalıdır.");
            }
            _salonM2 = value;
        }
    }

    public override decimal TurKatsayisi => 1.75m;
    public override int MaksimumKapasite => 4; // Suit odalar için sabit maksimum kapasite

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
        return $"{base.ToString()} - Tip: SuitOda | Salon Alanı: {SalonM2} m² | Katsayı: {TurKatsayisi:0.00}";
    }
}