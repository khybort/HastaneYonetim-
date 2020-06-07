using System.Collections.Generic;
using HastaneYonetim.Core.Models;

namespace HastaneYonetim.Core.Repositories
{
    public interface IDoktorRepo
    {
        IEnumerable<Doktor> DoktorlariGetir();
        IEnumerable<Doktor> MusaitDoktorlariGetir();
        Doktor DoktorGetir(int id);
        Doktor ProfilGetir(string kullaniciId);
        void Ekle(Doktor doktor);
    }
}