using System;
using System.Collections.Generic;
using System.Linq;
using HastaneYonetim.Core.Models;
using HastaneYonetim.Core.ViewModel;

namespace HastaneYonetim.Core.Repositories
{
    public interface IRandevuRepo
    {
        IEnumerable<Randevu> RandevulariGetir();
        IEnumerable<Randevu> HastaylaRandevuGetir(int id);
        IEnumerable<Randevu> DoktordanRandevuGetir(int id);
        IEnumerable<Randevu> BugunRandevulariGetir(int id);
        IEnumerable<Randevu> YaklaşanRandevulariGetir(string kullaniciId);
        IEnumerable<Randevu> GunlukRandevulariGetir(DateTime tarihGetir);
        IQueryable<Randevu> RandevulariFiltrele(RandevuAramaModeli aramaModeli);
        bool RandevulariDogrula(DateTime randevuTarihi, int id);
        int RandevulariSay(int id);
        Randevu RandevuGetir(int id);
        void Ekle(Randevu randevu);

    }
}