namespace OtelRezervasyon.Common;

/// <summary>
/// Tüm parasal tutarları değişmez (immutable) olarak temsil eden değer nesnesi.
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

    public override string ToString() => $"{Tutar:N2} {ParaBirimi}";
}