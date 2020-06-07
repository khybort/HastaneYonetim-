using System;
using System.Linq;
using System.Web.Mvc;
using HastaneYonetim.Core;
using HastaneYonetim.Core.Models;
using HastaneYonetim.Core.ViewModel;

namespace HastaneYonetim.Controllers
{
    [Authorize(Roles = RolAdi.DoktorRolAdi + "," + RolAdi.AdminRolAdi)]
    public class HastalarController : Controller
    {
        private readonly IIsBirimi _isBirimi;

        public HastalarController(IIsBirimi isBirimi)
        {
            _isBirimi = isBirimi;
        }
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Detaylar(int id)
        {
            var viewModel = new HastaDetayViewModel()
            {
                Hasta = _isBirimi.Hastalar.HastaGetir(id),
                Randevular = _isBirimi.Randevular.HastaylaRandevuGetir(id),
                Bakimlar = _isBirimi.Bakimlar.BakimGetir(id),
                RandevulariSay = _isBirimi.Randevular.RandevulariSay(id),
                BakimSay = _isBirimi.Bakimlar.BakimSay(id)
            };
            return View("Detaylar", viewModel);
        }




        [Authorize]
        public ActionResult Olustur()
        {
            var viewModel = new HastaFormuViewModel
            {
                Sehirler = _isBirimi.Sehirler.SehirleriGetir(),
                Baslik = "Yeni Hasta"
            };
            return View("HastaFormu", viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Olustur(HastaFormuViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Sehirler = _isBirimi.Sehirler.SehirleriGetir();
                return View("HastaFormu", viewModel);

            }

            var hasta = new Hasta
            {
                Ad = viewModel.Ad,
                Telefon = viewModel.TelefonNumarasi,
                Adres = viewModel.Adres,
                TarihSure = DateTime.Now,
                DogumTarihi = viewModel.DogumTarihiniGetir(),
                Boy = viewModel.Boy,
                Kilo = viewModel.Kilo,
                SehirId = viewModel.Sehir,
                Cinsiyet = viewModel.Cinsiyet,
                HastaNumarasi = (2018 + _isBirimi.Hastalar.HastalariGetir().Count()).ToString().PadLeft(7, '0')
            };

            _isBirimi.Hastalar.Ekle(hasta);
            _isBirimi.Tamamla();
            return RedirectToAction("Index", "Hastalar");


        }


        public ActionResult Duzenle(int id)
        {
            var hasta = _isBirimi.Hastalar.HastaGetir(id);

            var viewModel = new HastaFormuViewModel
            {
                Baslik = "Hasta Düzenle",
                Id = hasta.Id,
                Ad = hasta.Ad,
                TelefonNumarasi = hasta.Telefon,
                Tarih = hasta.TarihSure,
                DogumTarihi = hasta.DogumTarihi.ToString("dd/MM/yyyy"),
                Adres = hasta.Adres,
                Boy = hasta.Boy,
                Kilo = hasta.Kilo,
                Cinsiyet = hasta.Cinsiyet,
                Sehir = hasta.SehirId,
                Sehirler = _isBirimi.Sehirler.SehirleriGetir()
            };
            return View("HastaFormu", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(HastaFormuViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Sehirler = _isBirimi.Sehirler.SehirleriGetir();
                return View("HastaFormu", viewModel);
            }


            var hastaInDb = _isBirimi.Hastalar.HastaGetir(viewModel.Id);
            hastaInDb.Id = viewModel.Id;
            hastaInDb.Ad = viewModel.Ad;
            hastaInDb.Telefon = viewModel.TelefonNumarasi;
            hastaInDb.DogumTarihi = viewModel.DogumTarihiniGetir();
            hastaInDb.Adres = viewModel.Adres;
            hastaInDb.Boy = viewModel.Boy;
            hastaInDb.Kilo = viewModel.Kilo;
            hastaInDb.Cinsiyet = viewModel.Cinsiyet;
            hastaInDb.SehirId = viewModel.Sehir;

            _isBirimi.Tamamla();
            return RedirectToAction("Index", "Hastalar")
;
        }



    }
}