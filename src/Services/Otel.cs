using OtelRezervasyon.Models;
using OtelRezervasyon.Strategies;

namespace OtelRezervasyon.Services;

/// <summary>
/// Oda ve rezervasyon kayıtlarını, sınıf değişmezlerini ve ceza stratejisini yöneten ana sınıf.
/// </summary>
public class Otel
{
    public const int MaksimumOtelKapasitesi = 45;

    private readonly int _kapasite;
    private readonly List<Oda> _odalar = new();
    private readonly List<Rezervasyon> _rezervasyonlar = new();
    private ICezaPolitikasi _cezaPolitikasi;

    public Otel(int kapasite, ICezaPolitikasi cezaPolitikasi)
    {
        // Sınıf Değişmezi: Otelin toplam oda sayısı 45 değerini aşamaz
        if (kapasite <= 0 || kapasite > MaksimumOtelKapasitesi)
        {
            throw new ArgumentOutOfRangeException(
                nameof(kapasite),
                $"Otel kapasitesi 1 ile {MaksimumOtelKapasitesi} arasında olmalıdır. Verilen değer: {kapasite}");
        }

        _kapasite = kapasite;
        _cezaPolitikasi = cezaPolitikasi ?? throw new ArgumentNullException(nameof(cezaPolitikasi), "Ceza politikası boş olamaz.");
    }

    public int Kapasite => _kapasite;
    public ICezaPolitikasi AktifCezaPolitikasi => _cezaPolitikasi;

    /// <summary>
    /// Çalışma sırasında (runtime) ceza hesaplama stratejisini değiştirir.
    /// </summary>
    public void CezaPolitikasiDegistir(ICezaPolitikasi yeniPolitika)
    {
        _cezaPolitikasi = yeniPolitika ?? throw new ArgumentNullException(nameof(yeniPolitika), "Yeni ceza politikası boş olamaz.");
    }
}