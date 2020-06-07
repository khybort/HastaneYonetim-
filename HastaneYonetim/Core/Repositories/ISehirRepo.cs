using System.Collections.Generic;
using HastaneYonetim.Core.Models;

namespace HastaneYonetim.Core.Repositories
{
    public interface ISehirRepo
    {
        IEnumerable<Sehir> SehirleriGetir();
    }
}