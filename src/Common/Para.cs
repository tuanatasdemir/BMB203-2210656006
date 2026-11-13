namespace OtelRezervasyon.Common;

/// <summary>
/// Tüm parasal tutarları değişmez (immutable) olarak temsil eden değer nesnesi.
/// C# record yapısı sayesinde değer bazlı Equals ve GetHashCode otomatik sağlanır.
/// </summary>
public readonly record struct Para
{
    public decimal Tutar { get; }
    public string ParaBirimi { get; }

    public Para(decimal tutar, string paraBirimi = "TL")
    {
        if (tutar < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(tutar), "Para tutarı negatif olamaz.");
        }

        if (string.IsNullOrWhiteSpace(paraBirimi))
        {
            throw new ArgumentException("Para birimi boş olamaz.", nameof(paraBirimi));
        }

        Tutar = tutar;
        ParaBirimi = paraBirimi.Trim().ToUpperInvariant();
    }

    public static Para Sifir(string birim = "TL") => new(0m, birim);

    private static void BirimUyumuDogrula(Para a, Para b)
    {
        if (!string.Equals(a.ParaBirimi, b.ParaBirimi, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Farklı para birimleri ile işlem yapılamaz: '{a.ParaBirimi}' ve '{b.ParaBirimi}'.");
        }
    }

    public static Para operator +(Para a, Para b)
    {
        BirimUyumuDogrula(a, b);
        return new Para(a.Tutar + b.Tutar, a.ParaBirimi);
    }

    public static Para operator -(Para a, Para b)
    {
        BirimUyumuDogrula(a, b);
        if (a.Tutar < b.Tutar)
        {
            throw new InvalidOperationException("Çıkarma işlemi sonucu negatif para tutarı oluşamaz.");
        }
        return new Para(a.Tutar - b.Tutar, a.ParaBirimi);
    }

    public static Para operator *(Para a, decimal carpan)
    {
        if (carpan < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(carpan), "Çarpan negatif olamaz.");
        }
        return new Para(a.Tutar * carpan, a.ParaBirimi);
    }

    public override string ToString() => $"{Tutar:N2} {ParaBirimi}";
}