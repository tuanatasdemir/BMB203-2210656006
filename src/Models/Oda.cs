namespace OtelRezervasyon.Models;

/// <summary>
/// Oteldeki tüm oda türleri için ortak alanları ve doğrulamaları barındıran soyut taban sınıf.
/// </summary>
public abstract class Oda
{
    private readonly Guid _id;
    private int _odaNo;
    private int _kat;

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
}