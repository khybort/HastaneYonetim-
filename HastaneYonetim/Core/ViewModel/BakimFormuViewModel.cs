using System;
using System.ComponentModel.DataAnnotations;

namespace HastaneYonetim.Core.ViewModel
{
    public class BakimFormuViewModel
    {
        public int Id { get; set; }

        [Required]
        public string KlinikBulgular { get; set; }

        [Required]
        [StringLength(255)]
        public string Teshis { get; set; }

        public string Teshis2 { get; set; }
        public string Teshis3 { get; set; }

        [Required]
        public string Terapi { get; set; }


        public DateTime Tarih { get; set; }

        public string Baslik { get; set; }

        [Required]
        public int Hasta { get; set; }


        [Required]
        public int Doktor { get; set; }


    }
}