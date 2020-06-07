using HastaneYonetim.Core.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNetCore.Identity;
using Microsoft.Owin.Security;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IdentityResult = Microsoft.AspNet.Identity.IdentityResult;
using UserLoginInfo = Microsoft.AspNet.Identity.UserLoginInfo;

namespace HastaneYonetim.Controllers
{
    [Authorize]
    public class YonetController : Controller
    {
        private UygulamaOturumAcmaYoneticisi _oturumAcmaYoneticisi;
        private UygulamaKullaniciYoneticisi _kullaniciYoneticisi;

        public YonetController()
        {
        }

        public YonetController(UygulamaKullaniciYoneticisi kullaniciYoneticisi, UygulamaOturumAcmaYoneticisi oturumAcmaYoneticisi)
        {
            KullaniciYoneticisi = kullaniciYoneticisi;
            OturumAcmaYoneticisi = oturumAcmaYoneticisi;
        }

        public UygulamaOturumAcmaYoneticisi OturumAcmaYoneticisi
        {
            get
            {
                return _oturumAcmaYoneticisi ?? HttpContext.GetOwinContext().Get<UygulamaOturumAcmaYoneticisi>();
            }
            private set
            {
                _oturumAcmaYoneticisi = value;
            }
        }

        public UygulamaKullaniciYoneticisi KullaniciYoneticisi
        {
            get
            {
                return _kullaniciYoneticisi ?? HttpContext.GetOwinContext().GetUserManager<UygulamaKullaniciYoneticisi>();
            }
            private set
            {
                _kullaniciYoneticisi = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(MesajIdYonet? mesaj)
        {
            ViewBag.StatusMessage =
                mesaj == MesajIdYonet.SifreDegistirBasarili ? "Your password has been changed."
                : mesaj == MesajIdYonet.SifreYerlestirBasarili ? "Your password has been set."
                : mesaj == MesajIdYonet.IkiAsamaliYerlestirBasarili ? "Your two-factor authentication provider has been set."
                : mesaj == MesajIdYonet.Hata ? "An error has occurred."
                : mesaj == MesajIdYonet.TelefonEkleBasarili ? "Your phone number was added."
                : mesaj == MesajIdYonet.TelefonKaldirBasarili ? "Your phone number was removed."
                : "";

            var kullaniciId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                SifresiVarMi = SifresiVarMi(),
                TelefonNumarasi = await KullaniciYoneticisi.GetPhoneNumberAsync(kullaniciId),
                IkiAsama = await KullaniciYoneticisi.GetTwoFactorEnabledAsync(kullaniciId),
                Girisler = await KullaniciYoneticisi.GetLoginsAsync(kullaniciId),
                TarayiciHatirladiMi= await AuthenticationManager.TwoFactorBrowserRememberedAsync(kullaniciId)
            };
            return View(model);
        }

        //
        // POST: /Yonet/GirisKaldir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GirisKaldir(string girisSaglayici, string saglayiciAnahtar)
        {
            MesajIdYonet? mesaj;
            var sonuc = await KullaniciYoneticisi.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(girisSaglayici, saglayiciAnahtar));
            if (sonuc.Succeeded)
            {
                var kullanici = await KullaniciYoneticisi.FindByIdAsync(User.Identity.GetUserId());
                if (kullanici != null)
                {
                    await OturumAcmaYoneticisi.SignInAsync(kullanici, isPersistent: false, rememberBrowser: false);
                }
                mesaj = MesajIdYonet.GirisKaldirBasarili;
            }
            else
            {
                mesaj = MesajIdYonet.Hata;
            }
            return RedirectToAction("GirisleriYonet", new { Message = mesaj });
        }

        //
        // GET: /Manage/TelefonNumarasiEkle
        public ActionResult TelefonNumarasiEkle()
        {
            return View();
        }

        //
        // POST: /Yonet/TelefonNumarasiEkle
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TelefonNumarasiEkle(TelefonNumarasiEkleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Token üret ve gönder
            var kod = await KullaniciYoneticisi.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Numara);
            if (KullaniciYoneticisi.SmsService != null)
            {
                var mesaj = new IdentityMessage
                {
                    Destination = model.Numara,
                    Body = "Güvenlik Kodunuz: " + kod
                };
                await KullaniciYoneticisi.SmsService.SendAsync(mesaj);
            }
            return RedirectToAction("TelefonNumarasiDogrula", new { PhoneNumber = model.Numara });
        }

        //
        // POST: /Yonet/IkiAsamaliDogrulamaDevrede
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> IkiAsamaliDogrulamaDevrede()
        {
            await KullaniciYoneticisi.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var kullanici = await KullaniciYoneticisi.FindByIdAsync(User.Identity.GetUserId());
            if (kullanici != null)
            {
                await OturumAcmaYoneticisi.SignInAsync(kullanici, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Yonet");
        }

        //
        // POST: /Yonet/IkiAsamaliDogrulamaDevredisi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> IkiAsamaliDogrulamaDevredisi()
        {
            await KullaniciYoneticisi.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var kullanici = await KullaniciYoneticisi.FindByIdAsync(User.Identity.GetUserId());
            if (kullanici != null)
            {
                await OturumAcmaYoneticisi.SignInAsync(kullanici, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Yonet");
        }

        //
        // GET: /Yonet/TelefonNumarasiDogrula
        public async Task<ActionResult> TelefonNumarasiDogrula(string telefonNumarasi)
        {
            var kod = await KullaniciYoneticisi.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), telefonNumarasi);

            return telefonNumarasi == null ? View("Hata") : View(new TelefonNumarasiDogrulaViewModel { TelefonNumarasi = telefonNumarasi });
        }

        //
        // POST: /Yonet/TelefonNumarasiDogrula
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TelefonNumarasiDogrula(TelefonNumarasiDogrulaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var sonuc = await KullaniciYoneticisi.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.TelefonNumarasi, model.Kod);
            if (sonuc.Succeeded)
            {
                var kullanici = await KullaniciYoneticisi.FindByIdAsync(User.Identity.GetUserId());
                if (kullanici != null)
                {
                    await OturumAcmaYoneticisi.SignInAsync(kullanici, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = MesajIdYonet.TelefonEkleBasarili });
            }

            ModelState.AddModelError("", "Telefonu Doğrulama Başarısız");
            return View(model);
        }

        //
        // POST: /Yonet/TelefonNumarasiKaldir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TelefonNumarasiKaldir()
        {
            var sonuc = await KullaniciYoneticisi.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!sonuc.Succeeded)
            {
                return RedirectToAction("Index", new { Message = MesajIdYonet.Hata });
            }
            var kullanici = await KullaniciYoneticisi.FindByIdAsync(User.Identity.GetUserId());
            if (kullanici != null)
            {
                await OturumAcmaYoneticisi.SignInAsync(kullanici, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = MesajIdYonet.TelefonKaldirBasarili });
        }

        //
        // GET: /Yonet/SifreDegistir
        public ActionResult SifreDegistir()
        {
            return View();
        }

        //
        // POST: /Yonet/SifreDegistir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SifreDegistir(SifreDegistirViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var sonuc = await KullaniciYoneticisi.ChangePasswordAsync(User.Identity.GetUserId(), model.EskiSifre, model.YeniSifre);
            if (sonuc.Succeeded)
            {
                var kullanici = await KullaniciYoneticisi.FindByIdAsync(User.Identity.GetUserId());
                if (kullanici != null)
                {
                    await OturumAcmaYoneticisi.SignInAsync(kullanici, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = MesajIdYonet.SifreDegistirBasarili });
            }
            HatalariEkle(sonuc);
            return View(model);
        }

        //
        // GET: /Yonet/SifreYerlestir
        public ActionResult SifreYerlestir()
        {
            return View();
        }

        //
        // POST: /Yonet/SifreYerlestir
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SifreYerlestir(SifreYerlestirViewModel model)
        {
            if (ModelState.IsValid)
            {
                var sonuc = await KullaniciYoneticisi.AddPasswordAsync(User.Identity.GetUserId(), model.YeniSifre);
                if (sonuc.Succeeded)
                {
                    var kullanici = await KullaniciYoneticisi.FindByIdAsync(User.Identity.GetUserId());
                    if (kullanici != null)
                    {
                        await OturumAcmaYoneticisi.SignInAsync(kullanici, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = MesajIdYonet.SifreYerlestirBasarili });
                }
                HatalariEkle(sonuc);
            }
            return View(model);
        }

        //
        // GET: /Yonet/GirisleriYonet
        public async Task<ActionResult> GirisleriYonet(MesajIdYonet? mesaj)
        {
            ViewBag.StatusMessage =
                mesaj == MesajIdYonet.GirisKaldirBasarili ? "Harici giriş kaldırıldı."
                : mesaj == MesajIdYonet.Hata ? "Bir hata oluştu."
                : "";
            var kullanici = await KullaniciYoneticisi.FindByIdAsync(User.Identity.GetUserId());
            if (kullanici == null)
            {
                return View("Hata");
            }
            var kullaniciGirisleri = await KullaniciYoneticisi.GetLoginsAsync(User.Identity.GetUserId());
            var digerGirisler = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => kullaniciGirisleri.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = kullanici.PasswordHash != null || kullaniciGirisleri.Count > 1;
            return View(new GirisleriYonetViewModel
            {
                SimdikiGirisler = kullaniciGirisleri,
 /*BakBi*/               DigerGirisler = (System.Collections.Generic.IList<Microsoft.AspNetCore.Http.Authentication.AuthenticationDescription>)digerGirisler
            });
        }

        //
        // POST: /Yonet/GirisLinki
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GirisLinki(string saglayici)
        {
            return new HesapController.ChallengeResult(saglayici, Url.Action("LinkLoginCallback", "Yonet"), User.Identity.GetUserId());
        }

        //
        // GET: /Yonet/GirisLinkiCallback
        public async Task<ActionResult> GirisLinkiCallback()
        {
            var girisBilgisi = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (girisBilgisi == null)
            {
                return RedirectToAction("GirisleriYonet", new { Message = MesajIdYonet.Hata });
            }
            var result = await KullaniciYoneticisi.AddLoginAsync(User.Identity.GetUserId(), girisBilgisi.Login);
            return result.Succeeded ? RedirectToAction("GirisleriYonet") : RedirectToAction("GirisleriYonet", new { Message = MesajIdYonet.Hata });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _kullaniciYoneticisi != null)
            {
                _kullaniciYoneticisi.Dispose();
                _kullaniciYoneticisi = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void HatalariEkle(IdentityResult sonuc)
        {
            foreach (var hata in sonuc.Errors)
            {
                ModelState.AddModelError("", hata);
            }
        }

        private bool SifresiVarMi()
        {
            var user = KullaniciYoneticisi.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool TelefonNumarasiVarMi()
        {
            var kullanici = KullaniciYoneticisi.FindById(User.Identity.GetUserId());
            if (kullanici != null)
            {
                return kullanici.PhoneNumber != null;
            }
            return false;
        }

        public enum MesajIdYonet
        {
            TelefonEkleBasarili,
            SifreDegistirBasarili,
            IkiAsamaliYerlestirBasarili,
            SifreYerlestirBasarili,
            GirisKaldirBasarili,
            TelefonKaldirBasarili,
            Hata
        }

        #endregion
    }
}