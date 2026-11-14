namespace OtelRezervasyon.Exceptions;

/// <summary>
/// Rezervasyon tarih aralığının veya konaklama gün sayısının geçersiz olması durumunda fırlatılır.
/// </summary>
/// 

public class GecersizTarihException : Exception
{
    public DateTime? GirisTarihi { get; }
    public DateTime? CikisTarihi { get; }
    public int? GecersizGunSayisi { get; }

    // 1. Senaryo: Çıkış tarihinin giriş tarihinden önce veya eşit olması
    public GecersizTarihException(DateTime girisTarihi, DateTime cikisTarihi)
        : base($"[GecersizTarihException] Çıkış tarihi ({cikisTarihi:yyyy-MM-dd}), giriş tarihinden ({girisTarihi:yyyy-MM-dd}) sonra olmalıdır.")
    {
        GirisTarihi = girisTarihi;
        CikisTarihi = cikisTarihi;
    }

    // 2. Senaryo: Gece veya gecikme süresinin negatif/sıfır verilmesi
    public GecersizTarihException(int gecersizGunSayisi, string aciklama)
        : base($"[GecersizTarihException] Geçersiz gün/gece değeri ({gecersizGunSayisi}): {aciklama}")
    {
        GecersizGunSayisi = gecersizGunSayisi;
    }
}