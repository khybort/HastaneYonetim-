using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HastaneYonetim.Core.Models;

namespace HastaneYonetim.Core.ViewModel
{
    public class RandevuFormuViewModel
    {
        public int Id { get; set; }

        [Required]
        [GecerliTarih]
        public string Tarih { get; set; }

        [Required]
        [GecerliSaat]
        public string Saat { get; set; }


        [Required]
        public string Detay { get; set; }

        [Required]
        public bool Durum { get; set; }


        [Required]
        public int Hasta { get; set; }
        public IEnumerable<Hasta> Hastalar { get; set; }

        [Required]
        public int Doktor { get; set; }

        public IEnumerable<Doktor> Doktorlar { get; set; }
        public string Baslik { get; set; }

        public IEnumerable<Randevu> Randevular { get; set; }


        public DateTime BaslangicTarihiniGetir()
        {
            return DateTime.Parse(string.Format("{0} {1}", Tarih, Saat));
        }


    }
}