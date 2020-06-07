using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HastaneYonetim.Core.Models;
using HastaneYonetim.Core.Repositories;
using HastaneYonetim.Core.ViewModel;

namespace HastaneYonetim.Persistence.Repositories
{
    public class RandevuRepo : IRandevuRepo
    {
        private readonly UygulamaDbContext _context;
        public RandevuRepo(UygulamaDbContext context)
        {
            _context = context;
        }

        IEnumerable<Randevu> IRandevuRepo.RandevulariGetir()
        {
            return _context.Randevular
                .Include(p => p.Hasta)
                .Include(d => d.Doktor)
                .ToList();
        }

        IEnumerable<Randevu> IRandevuRepo.HastaylaRandevuGetir(int id)
        {
            return _context.Randevular
               .Where(p => p.HastaId == id)
               .Include(p => p.Hasta)
               .Include(d => d.Doktor)
               .ToList();
        }

        IEnumerable<Randevu> IRandevuRepo.DoktordanRandevuGetir(int id)
        {
            return _context.Randevular
                .Where(d => d.DoktorId == id)
                .Include(p => p.Hasta)
                .ToList();
        }

        IEnumerable<Randevu> IRandevuRepo.BugunRandevulariGetir(int id)
        {
            DateTime bugün = DateTime.Now.Date;
            return _context.Randevular
                .Where(d => d.DoktorId == id && d.BaslangicTarihSure >= bugün)
                .Include(p => p.Hasta)
                .OrderBy(d => d.BaslangicTarihSure)
                .ToList();
        }

        IEnumerable<Randevu> IRandevuRepo.YaklaşanRandevulariGetir(string kullaniciId)
        {
            DateTime today = DateTime.Now.Date;
            return _context.Randevular
                .Where(d => d.Doktor.HekimId == kullaniciId && d.BaslangicTarihSure >= today && d.Durum == true)
                .Include(p => p.Hasta)
                .OrderBy(d => d.BaslangicTarihSure)
                .ToList();
        }

        IEnumerable<Randevu> IRandevuRepo.GunlukRandevulariGetir(DateTime tarihGetir)
        {
            return _context.Randevular.Where(a => DbFunctions.DiffDays(a.BaslangicTarihSure, tarihGetir) == 0)
               .Include(p => p.Hasta)
               .Include(d => d.Doktor)
               .ToList();
        }

        IQueryable<Randevu> IRandevuRepo.RandevulariFiltrele(RandevuAramaModeli aramaModeli)
        {
            var sonuc = _context.Randevular.Include(p => p.Hasta).Include(d => d.Doktor).AsQueryable();
            if (aramaModeli != null)
            {
                if (!string.IsNullOrWhiteSpace(aramaModeli.Ad))
                    sonuc = sonuc.Where(a => a.Doktor.Ad == aramaModeli.Ad);
                if (!string.IsNullOrWhiteSpace(aramaModeli.Secenek))
                {
                    if (aramaModeli.Secenek == "BuAy")
                    {
                        sonuc = sonuc.Where(x => x.BaslangicTarihSure.Year == DateTime.Now.Year && x.BaslangicTarihSure.Month == DateTime.Now.Month);
                    }
                    else if (aramaModeli.Secenek == "Bekliyor")
                    {
                        sonuc = sonuc.Where(x => x.Durum == false);
                    }
                    else if (aramaModeli.Secenek == "Onaylandı")
                    {
                        sonuc = sonuc.Where(x => x.Durum);
                    }
                }
            }

            return sonuc;
        }

        bool IRandevuRepo.RandevulariDogrula(DateTime randevuTarihi, int id)
        {
            return _context.Randevular.Any(a => a.BaslangicTarihSure == randevuTarihi && a.DoktorId == id);
        }

        int IRandevuRepo.RandevulariSay(int id)
        {
            return _context.Randevular.Count(a => a.HastaId == id);
        }

        Randevu IRandevuRepo.RandevuGetir(int id)
        {
            return _context.Randevular.Find(id);
        }

        void IRandevuRepo.Ekle(Randevu randevu)
        {
            _context.Randevular.Add(randevu);
        }
    }
}