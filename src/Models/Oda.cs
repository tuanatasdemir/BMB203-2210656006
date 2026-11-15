using OtelRezervasyon.Common;

namespace OtelRezervasyon.Models;

/// <summary>
/// Oteldeki tüm oda türleri için ortak alanları, doğrulamaları ve soyut ücret hesaplama sözleşmesini barındıran taban sınıf.
/// </summary>
public abstract class Oda
{
    private readonly Guid _id;
    private int _odaNo;
    private int _kat;

    // Kişiye özel sabit parametre: Birim Fiyat = 50 TL
    public const decimal BirimFiyat = 50m;

    protected Oda(int odaNo, int kat)
    {
        Id = Guid.NewGuid();
        OdaNo = odaNo;
        Kat = kat;
    }

    public Guid Id
    {
        get => _id;
        private init
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("Oda benzersiz kimliği (Id) boş olamaz.", nameof(Id));
            }
            _id = value;
        }
    }

    public int OdaNo
    {
        get => _odaNo;
        protected set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(OdaNo), "Oda numarası 0'dan büyük pozitif bir tam sayı olmalıdır.");
            }
            _odaNo = value;
        }
    }

    public int Kat
    {
        get => _kat;
        protected set
        {
            if (value < 0 || value > 50)
            {
                throw new ArgumentOutOfRangeException(nameof(Kat), "Kat numarası 0 (zemin) ile 50 arasında olmalıdır.");
            }
            _kat = value;
        }
    }

    /// <summary>
    /// Alt oda türünün fiyat çarpan katsayısı.
    /// </summary>
    public abstract decimal TurKatsayisi { get; }

    /// <summary>
    /// Odada konaklayabilecek maksimum kişi kapasitesi.
    /// </summary>
    public abstract int MaksimumKapasite { get; }

    /// <summary>
    /// Temel formül: birim fiyat (50 TL) x gece x tür katsayısı.
    /// Alt sınıflarda kurallara göre override edilir.
    /// </summary>
    public abstract Para Hesapla(int gece);

    public override string ToString()
    {
        return $"[Id: {Id.ToString()[..8]} | Oda No: {OdaNo} | Kat: {Kat} | Tür: {GetType().Name}]";
    }
}