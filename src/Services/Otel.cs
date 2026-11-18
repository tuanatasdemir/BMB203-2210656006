using OtelRezervasyon.Common;
using OtelRezervasyon.Exceptions;
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

    /// <summary>
    /// Otele yeni bir oda kaydeder. Otel kapasitesi (45) aşılırsa istisna fırlatır.
    /// </summary>
    public void OdaKaydet(Oda oda)
    {
        if (oda == null)
        {
            throw new ArgumentNullException(nameof(oda), "Eklenecek oda nesnesi boş olamaz.");
        }

        if (_odalar.Any(o => o.OdaNo == oda.OdaNo))
        {
            return; // Oda zaten kayıtlıysa tekrar ekleme
        }

        // Sınıf Değişmezi: Otelin toplam oda sayısı 45 (_kapasite) değerini aşamaz
        if (_odalar.Count >= _kapasite)
        {
            throw new OdaDoluException(oda.OdaNo, $"Otelin maksimum oda kapasitesi ({_kapasite}) dolmuştur, yeni oda eklenemez.");
        }

        _odalar.Add(oda);
    }

    /// <summary>
    /// Belirtilen oda ve tarih aralığı için çakışma ve kapasite kontrollerini yaparak rezervasyon oluşturur.
    /// </summary>
    public Rezervasyon RezervasyonYap(Oda oda, string musteriAdi, DateTime girisTarihi, DateTime cikisTarihi, int kisiSayisi)
    {
        if (oda == null)
        {
            throw new ArgumentNullException(nameof(oda));
        }

        // Oda henüz otel envanterinde yoksa kapasite kontrolüyle kaydet
        if (!_odalar.Any(o => o.OdaNo == oda.OdaNo))
        {
            OdaKaydet(oda);
        }

        // Sınıf Değişmezi: Aynı oda için tarihleri çakışan iki rezervasyon olamaz
        bool tarihCakisiyor = _rezervasyonlar.Any(r =>
            r.RezerveOda.OdaNo == oda.OdaNo &&
            r.TarihCakisiyorMu(girisTarihi, cikisTarihi));

        if (tarihCakisiyor)
        {
            throw new OdaDoluException(oda.OdaNo, girisTarihi, cikisTarihi);
        }

        // Rezervasyon constructor'ı tarih sırasını (GecersizTarihException) ve kişi kapasitesini (OdaDoluException) doğrular
        var yeniRezervasyon = new Rezervasyon(oda, musteriAdi, girisTarihi, cikisTarihi, kisiSayisi);
        _rezervasyonlar.Add(yeniRezervasyon);

        return yeniRezervasyon;
    }

    /// <summary>
    /// Rezervasyon kaydını kapatır, konaklama ücretini ve varsa gecikme cezasını hesaplayarak döndürür.
    /// </summary>
    public Para CikisYap(Guid rezervasyonId, int gecikmeBirimi = 0)
    {
        var rezervasyon = _rezervasyonlar.FirstOrDefault(r => r.Id == rezervasyonId);
        if (rezervasyon == null)
        {
            throw new KeyNotFoundException($"[Hata] #{rezervasyonId} kimlikli rezervasyon kaydı bulunamadı.");
        }

        rezervasyon.CikisKapat();

        Para konaklamaUcreti = rezervasyon.KonaklamaUcretiHesapla();
        Para gecikmeCezasi = _cezaPolitikasi.Ceza(gecikmeBirimi);

        return konaklamaUcreti + gecikmeCezasi;
    }

    /// <summary>
    /// Tüm rezervasyonların toplam konaklama ücretini Para değer nesnesi olarak hesaplar.
    /// </summary>
    public Para ToplamKonaklamaUcretiHesapla()
    {
        Para toplam = Para.Sifir("TL");
        foreach (var rez in _rezervasyonlar)
        {
            toplam += rez.KonaklamaUcretiHesapla();
        }
        return toplam;
    }
}