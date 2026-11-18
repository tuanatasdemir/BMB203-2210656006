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

    // Ek Görev 2: İç listelerin kopyası değil, doğrudan salt-okunur (ReadOnly) sarmalayıcı görünümü
    public IReadOnlyList<Oda> Odalar => _odalar.AsReadOnly();
    public IReadOnlyList<Rezervasyon> Rezervasyonlar => _rezervasyonlar.AsReadOnly();

    // Ek Görev 2: Kayıtlara sıra numarasıyla erişim sağlayan Indexer ve sınır doğrulaması
    public Oda this[int index]
    {
        get
        {
            if (index < 0 || index >= _odalar.Count)
            {
                throw new IndexOutOfRangeException(
                    $"[Geçersiz Sıra Numarası] İstenen indeks ({index}) geçersizdir. Mevcut kayıtlı oda sayısı: {_odalar.Count}.");
            }
            return _odalar[index];
        }
    }

    public Rezervasyon RezervasyonGetir(int index)
    {
        if (index < 0 || index >= _rezervasyonlar.Count)
        {
            throw new IndexOutOfRangeException(
                $"[Geçersiz Sıra Numarası] İstenen rezervasyon indeksi ({index}) geçersizdir. Mevcut rezervasyon sayısı: {_rezervasyonlar.Count}.");
        }
        return _rezervasyonlar[index];
    }
}