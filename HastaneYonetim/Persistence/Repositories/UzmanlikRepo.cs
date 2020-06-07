using HastaneYonetim.Core.Models;
using HastaneYonetim.Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace HastaneYonetim.Persistence.Repositories
{
    public class UzmanlikRepo : IUzmanlikRepo
    {
        public readonly UygulamaDbContext Context;

        public UzmanlikRepo(UygulamaDbContext context)
        {
            Context = context;
        }

        public IEnumerable<Uzmanlik> UzmanliklariGetir()
        {
            return Context.Uzmanliklar.ToList();
        }
    }
}