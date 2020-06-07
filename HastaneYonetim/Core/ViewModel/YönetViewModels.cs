using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http.Authentication;

namespace HastaneYonetim.Core.ViewModel
{
    public class IndexViewModel
    {
        public bool SifresiVarMi { get; set; }
        public IList<UserLoginInfo> Girisler { get; set; }
        public string TelefonNumarasi { get; set; }
        public bool IkiAsama { get; set; }
        public bool TarayiciHatirladiMi { get; set; }
    }

    public class GirisleriYonetViewModel
    {
        public IList<UserLoginInfo> SimdikiGirisler { get; set; }
        public IList<AuthenticationDescription> DigerGirisler { get; set; }
    }

    public class AsamaViewModel
    {
        public string Amac { get; set; }
    }

    public class SifreYerlestirViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string YeniSifre { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string SifreOnayla { get; set; }
    }

    public class SifreDegistirViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string EskiSifre { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string YeniSifre { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string SifreOnayla { get; set; }
    }

    public class TelefonNumarasiEkleViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Numara { get; set; }
    }

    public class TelefonNumarasiDogrulaViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Kod { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string TelefonNumarasi { get; set; }
    }

    public class IkiAsamaliYapilandirViewModel
    {
        public string SecilenSaglayici { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Saglayicilar { get; set; }
    }
}