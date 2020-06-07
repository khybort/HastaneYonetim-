using System;
using HastaneYonetim.Persistence;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace HastaneYonetim.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UygulamaDbContext _context;
        public HomeController()
        {
            _context = new UygulamaDbContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        #region Dashboard İstatistikleri
        public ActionResult ToplamHasta()
        {
            var hastalar = _context.Hastalar.ToList();
            return Json(hastalar.Count(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ToplamRandevu()
        {
            var randevular =_context.Randevular.ToList();
            return Json(randevular.Count(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ToplamDoktor()
        {
            var doktorlar = _context.Doktorlar.ToList();
            return Json(doktorlar.Count(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ToplamKullanici()
        {
            var kullanicilar=_context.Users.ToList();
            return Json(kullanicilar.Count(), JsonRequestBehavior.AllowGet);
        }

        //Bugünkü Hastalar
        public ActionResult BugunkuHastalar()
        {
            DateTime bugun = DateTime.Now.Date;
            var hastalar = _context.Hastalar.Where(p => p.TarihSure >= bugun).ToList();
            return Json(hastalar.Count(), JsonRequestBehavior.AllowGet);
        }
        //Bugünkü Randevular
        public ActionResult BugunkuRandevular()
        {
            DateTime bugun = DateTime.Now.Date;
            var randevular =_context.Randevular
                .Where(a => a.BaslangicTarihSure>= bugun)
                .ToList();
            return Json(randevular.Count(), JsonRequestBehavior.AllowGet);
        }
        //Müsait Doktorlar
        public ActionResult MusaitDoktorlar()
        {
            var doktorlar=_context.Doktorlar
                .Where(d => d.musaitMi)
                .ToList();
            return Json(doktorlar.Count(), JsonRequestBehavior.AllowGet);
        }
        //Active Accounts
        public ActionResult AktifHesaplar()
        {
            var kullanicilar =_context.Users
                .Where(u => u.aktifMi == true)
                .ToList();
            return Json(kullanicilar.Count(), JsonRequestBehavior.AllowGet);
        }
        
        #endregion



        public ActionResult Hakkinda()
        {
            ViewBag.Message = "Uygulamanın tanıtım kısmı";

            return View();
        }

        public ActionResult Iletisim()
        {
            ViewBag.Message = "Uygulamanın iletişim sayfası.";

            return View();
        }
    }
}