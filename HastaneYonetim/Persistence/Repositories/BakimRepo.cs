using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HastaneYonetim.Core.Models;
using HastaneYonetim.Core.Repositories;

namespace HastaneYonetim.Persistence.Repositories
{
    public class BakimRepo : IBakimRepo
    {
        private readonly UygulamaDbContext _context;
        public BakimRepo(UygulamaDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Bakim> BakimlariGetir()
        {
            return _context.Bakimlar.ToList();
        }

        public IEnumerable<Bakim> BakimGetir(int id)
        {
            return _context.Bakimlar.Where(p => p.HastaId == id).ToList();
        }

        public IEnumerable<Bakim> HastaBakimlariniGetir(string terimAra = null)
        {
            var bakimlar = _context.Bakimlar.Include(p => p.Hasta);
            if (!string.IsNullOrWhiteSpace(terimAra))
            {
                bakimlar = bakimlar.Where(p => p.Hasta.HastaNumarasi.Contains(terimAra));
            }
            return bakimlar.ToList();
        }


        public int BakimSay(int id)
        {
            return _context.Bakimlar.Count(a => a.HastaId == id);
        }
        public void Ekle(Bakim bakim)
        {
            _context.Bakimlar.Add(bakim);
        }
    }
}