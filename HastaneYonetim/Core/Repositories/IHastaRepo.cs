using System.Collections.Generic;
using System.Linq;
using HastaneYonetim.Core.Models;

namespace HastaneYonetim.Core.Repositories
{
    public interface IHastaRepo
    {
        IEnumerable<Hasta> HastalariGetir();
        IEnumerable<Hasta> SonHastalariGetir();

        Hasta HastaGetir(int id);

        void Ekle(Hasta hasta);
        void Kaldir(Hasta hasta);
    }
}
