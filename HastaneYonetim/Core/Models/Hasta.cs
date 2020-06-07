using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HastaneYonetim.Core.Models
{
    public class Hasta
    {
        public int Id { get; set; }
        public string HastaNumarasi { get; set; }
        public string Ad { get; set; }
        public Cinsiyet Cinsiyet { get; set; }
        public DateTime DogumTarihi { get; set; }
        public string Telefon { get; set; }
        public string Adres { get; set; }
        public byte SehirId { get; set; }
        public Sehir Sehirler { get; set; }
        public DateTime TarihSure { get; set; }
        public string Boy { get; set; }
        public string Kilo { get; set; }

        public int Yas
        {
            get
            {
                var simdi = DateTime.Today;
                var yas = simdi.Year - DogumTarihi.Year;
                if (DogumTarihi > simdi.AddYears(-yas)) yas--;
                return yas;
            }

        }
        public ICollection<Randevu> Randevular { get; set; }
        public ICollection<Bakim> Bakimlar { get; set; }

        public Hasta()
        {
            Randevular = new Collection<Randevu>();
            Bakimlar = new Collection<Bakim>();
        }
    }
}