using HastaneYonetim.Core.Repositories;

namespace HastaneYonetim.Core
{
    public interface IIsBirimi
    {
        IHastaRepo Hastalar { get; }
        IRandevuRepo Randevular { get; }
        IBakimRepo Bakimlar { get; }
        ISehirRepo Sehirler { get; }
        IDoktorRepo Doktorlar { get; }
        IUzmanlikRepo Uzmanliklar { get; }
        IUygulamaKullaniciRepo Kullanicilar { get; }

        void Tamamla();
    }
}