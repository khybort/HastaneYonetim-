using System;

namespace HastaneYonetim.Core.Models
{
    public class Bakim
    {
        public int Id { get; set; }
        public string KlinikBulgular { get; set; }
        public string Teshis { get; set; }
        public string Teshis2 { get; set; }
        public string Teshis3 { get; set; }
        public string Terapi { get; set; }
        public DateTime Tarih { get; set; }
        public int HastaId { get; set; }
        public Hasta Hasta { get; set; }
    }

}