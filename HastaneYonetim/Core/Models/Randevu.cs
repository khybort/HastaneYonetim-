using System;

namespace HastaneYonetim.Core.Models
{
    public class Randevu
    {

        public int Id { get; set; }
        public DateTime BaslangicTarihSure { get; set; }
        public string Detay { get; set; }
        public bool Durum { get; set; }
        public int HastaId { get; set; }
        public Hasta Hasta { get; set; }
        public int DoktorId { get; set; }
        public Doktor Doktor { get; set; }
    }

}
