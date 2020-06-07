using System;
using System.Web.Mvc;
using HastaneYonetim.Core;
using HastaneYonetim.Core.ViewModel;

namespace HastaneYonetim.Controllers
{
    public class RaporlarController : Controller
    {
        private readonly IIsBirimi _isBirimi;

        public RaporlarController(IIsBirimi isBirimi)
        {
            _isBirimi = isBirimi;
        }

        //======================Bakım ========================//
        public ActionResult Attandences()
        {
            var bakimlar = _isBirimi.Bakimlar.BakimlariGetir();
            return View(bakimlar);
        }
        public ActionResult HastaBakim(string hastaNumarasi = null)
        {
            var hastaBakimlari = _isBirimi.Bakimlar.HastaBakimlariniGetir(hastaNumarasi);
            return View("_BakimKismi", hastaBakimlari);
        }

        // ===============Randevu ========================//
        public ActionResult Randevular()
        {
            var randevular = _isBirimi.Randevular.RandevulariGetir();
            return View(randevular);
        }

        [HttpPost]
        public ActionResult Randevular(RandevuAramaModeli viewModel)
        {
            var filtre = _isBirimi.Randevular.RandevulariFiltrele(viewModel);
            return View(filtre);
        }
        public ActionResult TestRandevu(RandevuAramaModeli viewModel)
        {
            var filtre = _isBirimi.Randevular.RandevulariFiltrele(viewModel);
            return PartialView("_Randevular", filtre);
        }
        //===============Randevu Sonu===================//

        //====================Günlük Randevular==============//

        public ActionResult GunluRandevular()
        {
            var gunluk = _isBirimi.Randevular.RandevulariGetir();
            return View(gunluk);
        }

        public ActionResult Gunluk(DateTime tarihGetir)
        {
            var gunlukRandevular = _isBirimi.Randevular.GunlukRandevulariGetir(tarihGetir);
            return View("_GunlukRandevular", gunlukRandevular);
        }
    }
}