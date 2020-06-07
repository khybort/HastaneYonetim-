

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace HastaneYonetim.Models
{
    public class UygulamaKullanici : IdentityUser
    {
        public string Ad { get; set; }

        public bool? aktifMi { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<UygulamaKullanici> manager)
        {

            var kullaniciKimligi = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return kullaniciKimligi;
        }
    }
}