using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HastaneYonetim.Core.Models;
using HastaneYonetim.Core.Repositories;

namespace HastaneYonetim.Persistence.Repositories
{
    public class HastaRepo : IHastaRepo
    {
        private readonly UygulamaDbContext _context;
        public HastaRepo(UygulamaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Hasta> HastalariGetir()
        {
            return _context.Hastalar.Include(c => c.Sehirler);
        }


        public Hasta HastaGetir(int id)
        {
            return _context.Hastalar
                .Include(c => c.Sehirler)
                .SingleOrDefault(p => p.Id == id);

        }

        public IEnumerable<Hasta> SonHastalariGetir()
        {
            return _context.Hastalar
                .Where(a => DbFunctions.DiffDays(a.TarihSure, DateTime.Now) == 0)
                .Include(c => c.Sehirler);
        }



        public void Ekle(Hasta hasta)
        {
            _context.Hastalar.Add(hasta);
        }

        public void Kaldir(Hasta hasta)
        {
            _context.Hastalar.Remove(hasta);
        }
    }
}