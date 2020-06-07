using System.Linq;
using System.Web.Mvc;
using HastaneYonetim.Core;
using HastaneYonetim.Core.Models;
using HastaneYonetim.Core.ViewModel;

namespace HastaneYonetim.Controllers
{
    public class RandevularController : Controller
    {
        private readonly IIsBirimi _isBirimi;

        public RandevularController(IIsBirimi isBirimi)
        {
            _isBirimi = isBirimi;
        }

        public ActionResult Index()
        {
            var randevular = _isBirimi.Randevular.RandevulariGetir();
            return View(randevular);
        }

        public ActionResult Detaylar(int id)
        {
            var randevu = _isBirimi.Randevular.HastaylaRandevuGetir(id);
            return View("_RandevuKısmi", randevu);
        }
        //public ActionResult Patients(int id)
        //{
        //    var viewModel = new DoctorDetailViewModel()
        //    {
        //        Appointments = _unitOfWork.Appointments.GetAppointmentByDoctor(id),
        //    };
        //    //var upcomingAppnts = _unitOfWork.Appointments.GetAppointmentByDoctor(id);
        //    return View(viewModel);
        //}

        public ActionResult Create(int id)
        {
            var viewModel = new RandevuFormuViewModel
            {
                Hasta = id,
                Doktorlar = _isBirimi.Doktorlar.MusaitDoktorlariGetir(),

                Baslik = "Yeni Randevu"
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Olustur(RandevuFormuViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Doktorlar = _isBirimi.Doktorlar.MusaitDoktorlariGetir();
                return View(viewModel);

            }
            var randevu = new Randevu()
            {
                BaslangicTarihSure = viewModel.BaslangicTarihiniGetir(),
                Detay= viewModel.Detay,
                Durum = false,
                HastaId = viewModel.Hasta,
                Doktor = _isBirimi.Doktorlar.DoktorGetir(viewModel.Doktor)

            };
            if (_isBirimi.Randevular.RandevulariDogrula(randevu.BaslangicTarihSure, viewModel.Doktor))
                return View("GecersizRandevu");

            _isBirimi.Randevular.Ekle(randevu);
            _isBirimi.Tamamla();
            return RedirectToAction("Index", "Randevular");
        }

        public ActionResult Duzenle(int id)
        {
            var randevu = _isBirimi.Randevular.RandevuGetir(id);
            var viewModel = new RandevuFormuViewModel()
            {
                Baslik = "Yeni Randevu",
                Id = randevu.Id,
                Tarih = randevu.BaslangicTarihSure.ToString("dd/MM/yyyy"),
                Saat = randevu.BaslangicTarihSure.ToString("HH:mm"),
                Detay = randevu.Detay,
                Durum = randevu.Durum,
                Hasta = randevu.HastaId,
                Doktor = randevu.DoktorId,
 
                Doktorlar= _isBirimi.Doktorlar.DoktorlariGetir()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Duzenle(RandevuFormuViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Doktorlar = _isBirimi.Doktorlar.DoktorlariGetir();
                viewModel.Hastalar= _isBirimi.Hastalar.HastalariGetir();
                return View(viewModel);
            }

            var randevuInDb = _isBirimi.Randevular.RandevuGetir(viewModel.Id);
            randevuInDb.Id = viewModel.Id;
            randevuInDb.BaslangicTarihSure = viewModel.BaslangicTarihiniGetir();
            randevuInDb.Detay = viewModel.Detay;
            randevuInDb.Durum= viewModel.Durum;
            randevuInDb.HastaId= viewModel.Hasta;
            randevuInDb.DoktorId = viewModel.Doktor;

            _isBirimi.Tamamla();
            return RedirectToAction("Index");

        }

        public ActionResult DoktorlarListesi()
        {
            var doktorlar = _isBirimi.Doktorlar.MusaitDoktorlariGetir();
            if (HttpContext.Request.IsAjaxRequest())
                return Json(new SelectList(doktorlar.ToArray(), "Id", "Ad"), JsonRequestBehavior.AllowGet);
            return RedirectToAction("Olustur");
        }

        public PartialViewResult YaklasanRandevulariGetir(int id)
        {
            var randevular = _isBirimi.Randevular.BugunRandevulariGetir(id);
            return PartialView(randevular);
        }

    }
}