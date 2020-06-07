using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace HastaneYonetim.Core.ViewModel
{
    public class GecerliSaat : ValidationAttribute
    {
        public override bool IsValid(object deger)
        {
            DateTime saat;
            var gecerliMi = DateTime.TryParseExact(Convert.ToString(deger),
                                                  "HH:mm",
                                                  CultureInfo.CurrentCulture,
                                                  DateTimeStyles.None,
                                                  out saat);
            return gecerliMi;
        }
    }
}