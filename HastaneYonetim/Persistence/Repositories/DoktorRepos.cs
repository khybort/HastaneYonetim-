using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HastaneYonetim.Core.Models;
using HastaneYonetim.Core.Repositories;

namespace HastaneYonetim.Persistence.Repositories
{
    public class DoktorRepos : IDoktorRepo
    {
        private readonly UygulamaDbContext _context;
        public DoktorRepos(UygulamaDbContext context)
        {
            _context = context;
        }


        public IEnumerable<Doktor> DoktorlariGetir()
        {
            return _context.Doktorlar
                .Include(s => s.Uzmanlik)
                .Include(u => u.Hekim)
                .ToList();
        }

        public IEnumerable<Doktor> MusaitDoktorlariGetir()
        {
            return _context.Doktorlar
                .Where(a => a.musaitMi == true)
                .Include(s => s.Uzmanlik)
                .Include(u => u.Hekim)
                .ToList();
        }

        public Doktor DoktorGetir(int id)
        {
            return _context.Doktorlar
                .Include(s => s.Uzmanlik)
                .Include(u => u.Hekim)
                .SingleOrDefault(d => d.Id == id);
        }

        public Doktor ProfilGetir(string kullaniciId)
        {
            return _context.Doktorlar
                .Include(s => s.Uzmanlik)
                .Include(u => u.Hekim)
                .SingleOrDefault(d => d.HekimId == kullaniciId);
        }
        public void Ekle(Doktor doktor)
        {
            _context.Doktorlar.Add(doktor);
        }
    }
}