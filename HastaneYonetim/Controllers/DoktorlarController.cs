using System.Web.Mvc;
using HastaneYonetim.Core;
using HastaneYonetim.Core.ViewModel;
using Microsoft.AspNet.Identity;

namespace HastaneYonetim.Controllers
{
    [Authorize]
    public class DoktorlarController : Controller
    {
        private readonly IIsBirimi _isBirimi;

        public DoktorlarController(IIsBirimi isBirimi)
        {
            _isBirimi = isBirimi;
        }

        public ActionResult Index()
        {
            var doktorlar = _isBirimi.Doktorlar.DoktorlariGetir();
            return View(doktorlar);
        }

        //Admin Detay Sayfası
        public ActionResult Detaylar(int id)
        {
            var viewModel = new DoktorDetayViewModel
            {
                Doktor = _isBirimi.Doktorlar.DoktorGetir(id),
                YaklasanRandevular = _isBirimi.Randevular.BugunRandevulariGetir(id),
                Randevular = _isBirimi.Randevular.DoktordanRandevuGetir(id),
            };
            return View(viewModel);
        }

        public ActionResult DoktorProfili()
        {
            var kullaniciId = User.Identity.GetUserId();
            var viewModel = new DoktorDetayViewModel
            {
                Doktor= _isBirimi.Doktorlar.ProfilGetir(kullaniciId),
                Randevular = _isBirimi.Randevular.YaklaşanRandevulariGetir(kullaniciId),
            };
            return View(viewModel);
        }
        public ActionResult Duzenle(int id)
        {
            var doktor = _isBirimi.Doktorlar.DoktorGetir(id);
            if (doktor == null) return HttpNotFound();
            var viewModel = new DoktorFormuViewModel()
            {

                Id = doktor.Id,
                Ad = doktor.Ad,
                Telefon = doktor.Telefon,
                Adres = doktor.Adres,
                MusaitMi = doktor.musaitMi,
                Uzmanlik = doktor.UzmanlikId,
                Uzmanliklar = _isBirimi.Uzmanliklar.UzmanliklariGetir()

            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Duzenle(DoktorFormuViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Uzmanliklar = _isBirimi.Uzmanliklar.UzmanliklariGetir();
                return View(viewModel);
            }

            var doktorInDb = _isBirimi.Doktorlar.DoktorGetir(viewModel.Id);
            doktorInDb.Id = viewModel.Id;
            doktorInDb.Ad = viewModel.Ad;
            doktorInDb.Telefon = viewModel.Telefon;
            doktorInDb.Adres = viewModel.Adres;
            doktorInDb.musaitMi = viewModel.MusaitMi;
            doktorInDb.UzmanlikId = viewModel.Uzmanlik;

            _isBirimi.Tamamla();

            return RedirectToAction("Detaylar", new { id = viewModel.Id });
        }


    }

}

