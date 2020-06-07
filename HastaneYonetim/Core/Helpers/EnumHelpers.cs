using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;

namespace HastaneYonetim.Core.Helpers
{
    public class EnumHelpers
    {
        public static IEnumerable<SelectListItem> secimListesi(Type enumType)
        {
            var degerler = (from Enum e in Enum.GetValues(enumType)
                          select new SelectListItem
                          {
                              // Selected = e.Equals(enumValue),
                              Text = ToDescription(e),
                              Value = e.ToString()
                          });

            return degerler;
        }

        public static string ToDescription(Enum value)
        {
            var attributes =
                (DescriptionAttribute[])
                value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }

    }
}