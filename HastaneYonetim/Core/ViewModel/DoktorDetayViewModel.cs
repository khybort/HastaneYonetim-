using System.Collections.Generic;
using HastaneYonetim.Core.Models;

namespace HastaneYonetim.Core.ViewModel
{
    public class DoktorDetayViewModel
    {
        public Doktor Doktor { get; set; }
        public IEnumerable<Randevu> YaklasanRandevular { get; set; }
        public IEnumerable<Randevu> Randevular { get; set; }
    }
}