namespace OtelRezervasyon.Exceptions;

/// <summary>
/// Odanın belirtilen tarihlerde dolu olması veya kişi sayısının oda kapasitesini aşması durumunda fırlatılır.
/// </summary>
public class OdaDoluException : Exception
{
    public int OdaNo { get; }
    public DateTime? BaslangicTarihi { get; }
    public DateTime? BitisTarihi { get; }
    public int? IstenenKisiSayisi { get; }
    public int? MaksimumKapasite { get; }

    // 1. Senaryo: Tarih çakışması nedeniyle odanın dolu olması
    public OdaDoluException(int odaNo, DateTime baslangic, DateTime bitis)
        : base($"[OdaDoluException] {odaNo} numaralı oda {baslangic:yyyy-MM-dd} ile {bitis:yyyy-MM-dd} tarihleri arasında zaten rezerve edilmiştir.")
    {
        OdaNo = odaNo;
        BaslangicTarihi = baslangic;
        BitisTarihi = bitis;
    }

    // 2. Senaryo: Oda kişi kapasitesinin aşılması
    public OdaDoluException(int odaNo, int istenenKisiSayisi, int maksimumKapasite)
        : base($"[OdaDoluException] {odaNo} numaralı odanın kapasitesi ({maksimumKapasite} kişi) aşıldı! İstenen kişi sayısı: {istenenKisiSayisi}.")
    {
        OdaNo = odaNo;
        IstenenKisiSayisi = istenenKisiSayisi;
        MaksimumKapasite = maksimumKapasite;
    }

    // 3. Senaryo: Genel mesaj ve inner exception desteği
    public OdaDoluException(int odaNo, string mesaj, Exception? innerException = null)
        : base($"[OdaDoluException - Oda {odaNo}] {mesaj}", innerException)
    {
        OdaNo = odaNo;
    }
}