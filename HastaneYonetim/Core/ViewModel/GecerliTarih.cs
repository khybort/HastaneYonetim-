using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace HastaneYonetim.Core.ViewModel
{
    public class GecerliTarih : ValidationAttribute
    {
        public override bool IsValid(object deger)
        {
            DateTime tarihSaat;
          var gecerliMi=  DateTime.TryParseExact(Convert.ToString(deger),
              "dd/MM/yyyy",
              CultureInfo.CurrentCulture,
              DateTimeStyles.None,
              out tarihSaat);
            return (gecerliMi);
        }
    }
}