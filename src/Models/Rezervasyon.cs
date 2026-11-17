using OtelRezervasyon.Common;
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

    /// <summary>
    /// Giriş ve çıkış tarihleri arasındaki toplam gece sayısı.
    /// </summary>
    public int ToplamGece => (_cikisTarihi - _girisTarihi).Days;

    /// <summary>
    /// İlgili odanın polimorfik Hesapla() metodunu kullanarak toplam konaklama ücretini döndürür.
    /// </summary>
    public Para KonaklamaUcretiHesapla() => _rezerveOda.Hesapla(ToplamGece);

    /// <summary>
    /// Verilen tarih aralığının bu rezervasyon ile çakışıp çakışmadığını denetler.
    /// Kapalı (çıkışı yapılmış) rezervasyonlar çakışma yaratmaz.
    /// </summary>
    public bool TarihCakisiyorMu(DateTime yeniGiris, DateTime yeniCikis)
    {
        if (!_aktifMi)
        {
            return false;
        }

        return _girisTarihi < yeniCikis.Date && yeniGiris.Date < _cikisTarihi;
    }

    /// <summary>
    /// Müşteri otelden ayrıldığında rezervasyon kaydını kapatır.
    /// </summary>
    public void CikisKapat()
    {
        if (!_aktifMi)
        {
            throw new InvalidOperationException($"#{_id.ToString()[..8]} kodlu rezervasyonun çıkışı zaten yapılmıştır.");
        }
        _aktifMi = false;
    }

    public override string ToString()
    {
        string durum = _aktifMi ? "Aktif" : "Kapandı (Çıkış Yapıldı)";
        return $"[Rez #{_id.ToString()[..8]} | Müşteri: {_musteriAdi} | Kişi: {_kisiSayisi} | " +
               $"{_girisTarihi:yyyy-MM-dd} -> {_cikisTarihi:yyyy-MM-dd} ({ToplamGece} Gece) | Durum: {durum}] -> {_rezerveOda}";
    }
}