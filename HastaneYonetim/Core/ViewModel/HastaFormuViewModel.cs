using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;
using HastaneYonetim.Controllers;
using HastaneYonetim.Core.Helpers;
using HastaneYonetim.Core.Models;

namespace HastaneYonetim.Core.ViewModel
{
    public class HastaFormuViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Ad { get; set; }

        public Cinsiyet Cinsiyet { get; set; }
        [Required]
        [GecerliTarih]
        public string DogumTarihi { get; set; }


        [Required]
        public string TelefonNumarasi { get; set; }
        [Required]
        public string Adres { get; set; }
        public string Boy { get; set; }
        public string Kilo { get; set; }

        public byte Sehir { get; set; }

        public DateTime Tarih { get; set; }

        public string Baslik { get; set; }

        public DateTime DogumTarihiniGetir()
        {
            //TODO: Validate BirthDate 

            return DateTime.Parse(string.Format("{0}", DogumTarihi));
            //return DateTime.ParseExact(BirthDate, "dd/MM/yyyy", CultureInfo.CurrentCulture);
        }

        public IEnumerable<Sehir> Sehirler { get; set; }



        public string Action
        {
            get
            {
                Expression<Func<HastalarController, ActionResult>> update = (c => c.Update(this));
                Expression<Func<HastalarController, ActionResult>> create = (c => c.Olustur(this));

                var hareket = (Id != 0) ? update : create;
                return (hareket.Body as MethodCallExpression).Method.Name;

            }
        }

        #region for dropdownlist

        public IEnumerable<SelectListItem> CinsiyetlerListesi
        {
            get { return HastaneMgtHelpers.CinsiyetSecimListesi(); }
            set { }
        }

        #endregion
    }
}