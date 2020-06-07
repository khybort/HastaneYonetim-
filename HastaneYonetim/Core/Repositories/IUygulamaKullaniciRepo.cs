using System.Collections.Generic;
using HastaneYonetim.Core.Models;
using HastaneYonetim.Core.ViewModel;
using HastaneYonetim.Models;

namespace HastaneYonetim.Core.Repositories
{
    public interface IUygulamaKullaniciRepo
    {
        List<KullaniciViewModel> KullanicilariGetir();
        UygulamaKullanici KullaniciGetir(string id);
    }
}