using System.Collections.Generic;
using System.Linq;
using HastaneYonetim.Core.Dto;
using HastaneYonetim.Core.Models;
using HastaneYonetim.Core.Repositories;
using HastaneYonetim.Core.ViewModel;
using HastaneYonetim.Models;

namespace HastaneYonetim.Persistence.Repositories
{
    public class UygulamaKullaniciRepo : IUygulamaKullaniciRepo
    {
        private readonly UygulamaDbContext _context;

        public UygulamaKullaniciRepo(UygulamaDbContext context)
        {
            _context = context;
        }

        public List<KullaniciViewModel> KullanicilariGetir()
        {
            return (from kullanici in _context.Users
                    from kullaniciRol in kullanici.Roles
                    join rol in _context.Roles
                        on kullaniciRol.RoleId equals rol.Id
                    select new KullaniciViewModel()
                    {
                        Id = kullanici.Id,
                        Eposta = kullanici.Email,
                        Rol = rol.Name,
                        aktifMi = kullanici.aktifMi
                    }).ToList();
        }

        public UygulamaKullanici KullaniciGetir(string id)
        {
            return _context.Users.Find(id);
        }

    }
}