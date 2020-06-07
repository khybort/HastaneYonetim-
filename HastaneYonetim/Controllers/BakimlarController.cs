using System;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using HastaneYonetim.Core;
using HastaneYonetim.Core.Models;
using HastaneYonetim.Core.ViewModel;

namespace HastaneYonetim.Controllers
{
    [Authorize]
    public class BakimlarController : Controller
    {
        private readonly IIsBirimi _isBirimi;
        public BakimlarController(IIsBirimi isBirimi)
        {
            _isBirimi = isBirimi;
        }

        public ActionResult Detaylar(int id)
        {
            var bakim = _isBirimi.Bakimlar.BakimGetir(id);
            return View("_bakimKismi", bakim);
        }

        public ActionResult Olustur(int id)
        {
            var viewModel = new BakimFormuViewModel
            {
                Hasta= id,
                Baslik = "Bakım Ekle"
            };
            return View("BakimFormu", viewModel);
        }

        [HttpPost]
        public ActionResult Olustur(BakimFormuViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("BakimFormu", viewModel);

            var bakim = new Bakim
            {
                Id = viewModel.Id,
                KlinikBulgular = viewModel.KlinikBulgular,
                Teshis = viewModel.Teshis,
                Teshis2= viewModel.Teshis2,
                Teshis3= viewModel.Teshis3,
                Terapi = viewModel.Terapi,
                Tarih = DateTime.Now,
                Hasta = _isBirimi.Hastalar.HastaGetir(viewModel.Hasta)
            };
            _isBirimi.Bakimlar.Ekle(bakim);
            _isBirimi.Tamamla();
            return RedirectToAction("Detaylar", "Hastalar", new { id = viewModel.Hasta});
        }



    }
}