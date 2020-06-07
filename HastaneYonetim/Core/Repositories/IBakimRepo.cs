using System.Collections.Generic;
using HastaneYonetim.Core.Models;

namespace HastaneYonetim.Core.Repositories
{
    public interface IBakimRepo
    {
        IEnumerable<Bakim> BakimlariGetir();
        IEnumerable<Bakim> BakimGetir(int id);
        IEnumerable<Bakim> HastaBakimlariniGetir(string terimAra = null);
        int BakimSay(int id);
        void Ekle(Bakim bakim);
    }
}
