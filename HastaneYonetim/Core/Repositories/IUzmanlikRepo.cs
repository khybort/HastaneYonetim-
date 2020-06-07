using System.Collections.Generic;
using HastaneYonetim.Core.Models;

namespace HastaneYonetim.Core.Repositories
{
    public interface IUzmanlikRepo
    {
        IEnumerable<Uzmanlik> UzmanliklariGetir();
    }
}
