using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace HastaneYonetim.Core.Helpers
{
    public static class HastaneMgtHelpers
    {
        public static IEnumerable<SelectListItem> CinsiyetSecimListesi()
        {
            var cinsiyetOgeleri = EnumHelpers.secimListesi(typeof(Gender)).ToList();
            cinsiyetOgeleri.Insert(0, new SelectListItem { Text = "Select", Value = "" });
            return cinsiyetOgeleri;
        }

    }
}