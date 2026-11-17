using OtelRezervasyon.Exceptions;

namespace OtelRezervasyon.Models;

/// <summary>
/// Bir odaya ait konaklama rezervasyon kaydını ve rezervasyon değişmezlerini temsil eder.
/// </summary>
public class Rezervasyon
{
    private readonly Guid _id;
    private readonly Oda _rezerveOda;
    private string _musteriAdi = string.Empty;
    private DateTime _girisTarihi;
    private DateTime _cikisTarihi;
    private int _kisiSayisi;
    private bool _aktifMi;

    public Rezervasyon(Oda rezerveOda, string musteriAdi, DateTime girisTarihi, DateTime cikisTarihi, int kisiSayisi)
    {
        _rezerveOda = rezerveOda ?? throw new ArgumentNullException(nameof(rezerveOda), "Rezervasyon için geçerli bir oda seçilmelidir.");

        // Sınıf Değişmezi 1: Çıkış tarihi giriş tarihinden sonra olmalıdır
        if (cikisTarihi.Date <= girisTarihi.Date)
        {
            throw new GecersizTarihException(girisTarihi.Date, cikisTarihi.Date);
        }

        // Sınıf Değişmezi 2: Kişi sayısı pozitif olmalı ve oda kapasitesini aşmamalıdır
        if (kisiSayisi <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(kisiSayisi), "Rezervasyon kişi sayısı en az 1 olmalıdır.");
        }

        if (kisiSayisi > _rezerveOda.MaksimumKapasite)
        {
            throw new OdaDoluException(_rezerveOda.OdaNo, kisiSayisi, _rezerveOda.MaksimumKapasite);
        }

        _id = Guid.NewGuid();
        MusteriAdi = musteriAdi;
        _girisTarihi = girisTarihi.Date;
        _cikisTarihi = cikisTarihi.Date;
        _kisiSayisi = kisiSayisi;
        _aktifMi = true;
    }

    public Guid Id => _id;
    public Oda RezerveOda => _rezerveOda;

    public string MusteriAdi
    {
        get => _musteriAdi;
        private set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Müşteri adı boş olamaz.", nameof(MusteriAdi));
            }
            _musteriAdi = value.Trim();
        }
    }

    public DateTime GirisTarihi => _girisTarihi;
    public DateTime CikisTarihi => _cikisTarihi;
    public int KisiSayisi => _kisiSayisi;
    public bool AktifMi => _aktifMi;
}