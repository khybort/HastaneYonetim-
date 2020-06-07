using HastaneYonetim.Core;
using HastaneYonetim.Core.Repositories;
using HastaneYonetim.Persistence.Repositories;

namespace HastaneYonetim.Persistence
{
    public class IsBirimi : IIsBirimi
    {
        private readonly UygulamaDbContext _context;
        public IHastaRepo Hastalar { get; private set; }
        public IRandevuRepo Randevular { get; private set; }
        public IBakimRepo Bakimlar { get; private set; }
        public ISehirRepo Sehirler { get; private set; }
        public IDoktorRepo Doktorlar { get; private set; }
        public IUzmanlikRepo Uzmanliklar { get; private set; }
        public IUygulamaKullaniciRepo Kullanicilar { get; private set; }

        public IsBirimi(UygulamaDbContext context)
        {
            _context = context;
            Hastalar = new HastaRepo(context);
            Randevular = new RandevuRepo(context);
            Bakimlar = new BakimRepo(context);
            Sehirler = new SehirRepo(context);
            Doktorlar = new DoktorRepos(context);
            Uzmanliklar = new UzmanlikRepo(context);
            Kullanicilar = new UygulamaKullaniciRepo(context);

        }

        public void Tamamla()
        {
            _context.SaveChanges();
        }
    }
}