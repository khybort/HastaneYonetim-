using System.Collections.Generic;
using System.Linq;
using HastaneYonetim.Core.Dto;
using HastaneYonetim.Core.Models;
using HastaneYonetim.Core.Repositories;

namespace HastaneYonetim.Persistence.Repositories
{
    public class SehirRepo : ISehirRepo
    {
        private readonly UygulamaDbContext _context;

        public SehirRepo(UygulamaDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Sehir> SehirleriGetir()
        {
            return _context.Sehirler.ToList();
        }
    }
}