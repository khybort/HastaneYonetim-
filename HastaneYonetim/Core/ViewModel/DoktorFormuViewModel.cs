using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HastaneYonetim.Core.Models;

namespace HastaneYonetim.Core.ViewModel
{
    public class DoktorFormuViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Ad { get; set; }

        [Required]
        public string Telefon { get; set; }
        [Required]
        public string Adres { get; set; }
        public bool MusaitMi { get; set; }


        [Required]
        public int Uzmanlik { get; set; }

        public IEnumerable<Uzmanlik> Uzmanliklar { get; set; }
        public IEnumerable<Doktor> Doktorlar { get; set; }

        public KayitViewModel KayitViewModel { get; set; }

    }
}