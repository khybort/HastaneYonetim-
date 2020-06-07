using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HastaneYonetim.Core.ViewModel
{
    public class HariciGirisOnaylamaViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Eposta { get; set; }
    }

    public class HariciGirisListesiViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class KoduGonderViewModel
    {
        public string SecilenSaglayici { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Saglayicilar { get; set; }
        public string ReturnUrl { get; set; }
        public bool HatirlaBeni { get; set; }
    }

    public class KoduDogrulaViewModel
    {
        [Required]
        public string Saglayici { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Kod { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool TarayiciHatirla { get; set; }

        public bool HatirlaBeni { get; set; }
    }

    public class UnuttumViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Eposta { get; set; }
    }

    public class GirisViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Eposta { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Sifre { get; set; }

        [Display(Name = "Remember me?")]
        public bool HatirlaBeni { get; set; }
    }


    public class SifreSifirlaViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Eposta { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Sifre { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string SifreOnay { get; set; }

        public string Kod { get; set; }
    }

    public class SifreUnuttumViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Eposta { get; set; }
    }
}
