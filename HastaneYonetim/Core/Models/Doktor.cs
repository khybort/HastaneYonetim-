using HastaneYonetim.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HastaneYonetim.Core.Models
{
    public class Doktor
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Telefon { get; set; }
        public bool musaitMi { get; set; }
        public string Adres { get; set; }
        public int UzmanlikId { get; set; }
        public Uzmanlik Uzmanlik { get; set; }
        public string HekimId { get; set; }
        public UygulamaKullanici Hekim { get; set; }
        public ICollection<Randevu> Randevular { get; set; }
        public Doktor()
        {
            Randevular = new Collection<Randevu>();
        }

    }
}