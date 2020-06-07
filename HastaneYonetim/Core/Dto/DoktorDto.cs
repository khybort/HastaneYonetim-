
using HastaneYonetim.Core.Models;
using System.Collections.Generic;


namespace HastaneYonetim.Core.Dto
{
    public class DoktorDto
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Telefon { get; set; }
        public bool musaitMi { get; set; }
        public string Adres { get; set; }
        public int UzmanlikId { get; set; }
        public UzmanlikDto Uzmanlik { get; set; }
        public ICollection<Randevu> Randevular { get; set; }



    }
}