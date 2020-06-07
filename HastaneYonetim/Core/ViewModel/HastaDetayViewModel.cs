using System.Collections.Generic;
using HastaneYonetim.Core.Models;

namespace HastaneYonetim.Core.ViewModel
{
    public class HastaDetayViewModel
    {
        public Hasta Hasta { get; set; }
        public IEnumerable<Randevu> Randevular { get; set; }
        public IEnumerable<Bakim> Bakimlar { get; set; }
        public int RandevulariSay { get; set; }
        public int BakimSay { get; set; }
    }
}