using HastaneYonetim.Core.Dto;
using System;

namespace HastaneYonetim.Core.Dto
{
    public class HastaDto
    {
        public int Id { get; set; }
        public string HastaNumarasi { get; set; }
        public string Ad { get; set; }
        public DateTime DogumTarihi { get; set; }
        public string Telefon { get; set; }
        public string Adres { get; set; }
        public byte SehirId { get; set; }
        public SehirDto Sehirler { get; set; }
        public int DoktorId { get; set; }
        public DoktorDto Doktor { get; set; }

        public DateTime TarihSure { get; set; }
        public string Boy { get; set; }
        public string Kilo { get; set; }
    }
}