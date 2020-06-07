using HastaneYonetim.Core;
using HastaneYonetim.Core.Models;
using HastaneYonetim.Core.ViewModel;
using HastaneYonetim.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;

namespace HastaneYonetim.Controllers
{
    [Authorize]
    public class HesapController : Controller
    {
        private UygulamaOturumAcmaYoneticisi _oturumAcmaYoneticisi;
        private UygulamaKullaniciYoneticisi _kullaniciYoneticisi;
        private UygulamaRolYöneticisi _rolYoneticisi;
        private readonly IIsBirimi _isBirimi;

        public HesapController(IIsBirimi isBirimi)
        {
            _isBirimi = isBirimi;
        }

        public HesapController(UygulamaKullaniciYoneticisi kullaniciYoneticisi, UygulamaOturumAcmaYoneticisi oturumAcmaYoneticisi,
            UygulamaRolYöneticisi rolYoneticisi)
        {
            KullaniciYoneticisi = kullaniciYoneticisi;
            OturumAcmaYoneticisi = oturumAcmaYoneticisi;
            RolYoneticisi = rolYoneticisi;
        }

        public UygulamaOturumAcmaYoneticisi OturumAcmaYoneticisi
        {
            get { return _oturumAcmaYoneticisi ?? HttpContext.GetOwinContext().Get<UygulamaOturumAcmaYoneticisi>(); }
            private set { _oturumAcmaYoneticisi = value; }
        }

        public UygulamaKullaniciYoneticisi KullaniciYoneticisi
        {
            get { return _kullaniciYoneticisi ?? HttpContext.GetOwinContext().GetUserManager<UygulamaKullaniciYoneticisi>(); }
            private set { _kullaniciYoneticisi = value; }
        }

        public UygulamaRolYöneticisi RolYoneticisi
        {
            get { return _rolYoneticisi ?? HttpContext.GetOwinContext().Get<UygulamaRolYöneticisi>(); }
            private set { _rolYoneticisi = value; }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Giris(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Giris(GirisViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

           
            var sonuc = await OturumAcmaYoneticisi.PasswordSignInAsync(model.Eposta, model.Sifre, model.HatirlaBeni,
                kilitlenmeliMi: false);
            switch (sonuc)
            {
                case SignInStatus.Success:
                    return LocaleYonlendir(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Kilitleme");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("KoduGonder", new { ReturnUrl = returnUrl, RememberMe = model.HatirlaBeni });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Gecersiz Giris");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> KodDogrula(string saglayici, string returnUrl, bool hatirlaBeni)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await OturumAcmaYoneticisi.HasBeenVerifiedAsync())
            {
                return View("Hata");
            }

            return View(new KoduDogrulaViewModel { Saglayici = saglayici, ReturnUrl = returnUrl, HatirlaBeni = hatirlaBeni });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> KoduDogrula(KoduDogrulaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }


            var sonuc = await OturumAcmaYoneticisi.TwoFactorSignInAsync(model.Saglayici, model.Kod,
                isPersistent: model.HatirlaBeni, rememberBrowser: model.TarayiciHatirla);
            switch (sonuc)
            {
                case SignInStatus.Success:
                    return LocaleYonlendir(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Kilitleme");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Gecersiz Kod.");
                    return View(model);
            }
        }

        //
        // GET: /Hesap/Kayit
        [AllowAnonymous]
        public ActionResult Kayit()
        {
            return View();
        }

        //
        // POST: /Hesap/Kayit
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Kayit(KayitViewModel model)
        {
            if (ModelState.IsValid)
            {
                var kullanici = new UygulamaKullanici { Ad = model.Ad, UserName = model.Eposta, Email = model.Eposta, aktifMi=true };
                var sonuc = await KullaniciYoneticisi.CreateAsync(kullanici, model.Sifre);
                if (sonuc.Succeeded)
                {
                    KullaniciYoneticisi.AddToRole(kullanici.Id, RolAdi.AdminRolAdi);
                    KullaniciYoneticisi.AddClaim(kullanici.Id, new Claim(ClaimTypes.GivenName, model.Ad));
                    await OturumAcmaYoneticisi.SignInAsync(kullanici, isPersistent: false, rememberBrowser: false);

                    return RedirectToAction("Index", "Home");
                }

                HatalariEkle(sonuc);
            }


            return View(model);
        }


        //Doktor Kayit
        [AllowAnonymous]
        public ActionResult DoktorKayit()
        {
            var viewModel = new DoktorFormuViewModel()
            {
                Uzmanliklar = _isBirimi.Uzmanliklar.UzmanliklariGetir()

            };
            return View("DoktorFormu", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DoktorKayit(DoktorFormuViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var kullanici = new UygulamaKullanici()
                {
                    UserName = viewModel.KayitViewModel.Eposta,
                    Email = viewModel.KayitViewModel.Eposta,
                    aktifMi = true
                };
                var sonuc = await KullaniciYoneticisi.CreateAsync(kullanici, viewModel.KayitViewModel.Sifre);

                if (sonuc.Succeeded)
                {

                    KullaniciYoneticisi.AddToRole(kullanici.Id, RolAdi.DoktorRolAdi);
                  

                    Doktor doktor = new Doktor()
                    {
                        Ad = viewModel.Ad,
                        Telefon = viewModel.Telefon,
                        Adres = viewModel.Adres,
                        musaitMi= true,
                        UzmanlikId= viewModel.Uzmanlik,
                        HekimId = kullanici.Id
                    };
                    KullaniciYoneticisi.AddClaim(kullanici.Id, new Claim(ClaimTypes.GivenName, doktor.Ad));
                    _isBirimi.Doktorlar.Ekle(doktor);
                    _isBirimi.Tamamla();
                    return RedirectToAction("Index", "Doktorlar");
                }

                this.HatalariEkle(sonuc);
            }

            viewModel.Uzmanliklar = _isBirimi.Uzmanliklar.UzmanliklariGetir();

            
            return View("DoktorFormu", viewModel);
        }

        //Kullanıcıları Listele
        public ActionResult Index()
        {
            var rolleriyleKullanicilar = _isBirimi.Kullanicilar.KullanicilariGetir();
            return View(rolleriyleKullanicilar);
        }


        public ActionResult Duzenle(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var kullanici = _isBirimi.Kullanicilar.KullaniciGetir(id);
            if (kullanici == null)
            {
                return HttpNotFound();
            }


            var viewModel = new KullaniciViewModel()
            {
                Id = kullanici.Id,
                Eposta = kullanici.Email,
                aktifMi = kullanici.aktifMi,
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Duzenle(KullaniciViewModel kullaniciDuzenle)
        {
            if (ModelState.IsValid)

            {
                var kullanici = _isBirimi.Kullanicilar.KullaniciGetir(kullaniciDuzenle.Id);
                if (kullanici== null)
                {
                    return HttpNotFound();
                }

                kullanici.Email = kullaniciDuzenle.Eposta ;
                kullanici.aktifMi= kullaniciDuzenle.aktifMi;
                _isBirimi.Tamamla();

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Bir şeyler başarısız sonuçlandı");
            return View(kullaniciDuzenle);
        }



        // GET: /Account/EpostaOnayla
        [AllowAnonymous]
        public async Task<ActionResult> EpostaOnayla(string kullaniciId, string kod)
        {
            if (kullaniciId== null || kod == null)
            {
                return View("Hata");
            }

            var sonuc = await KullaniciYoneticisi.ConfirmEmailAsync(kullaniciId, kod);
            return View(sonuc.Succeeded ? "EpostaOnayla" : "Hata");
        }

        //
        // GET: /Account/SifreUnuttum
        [AllowAnonymous]
        public ActionResult SifreUnuttum()
        {
            return View();
        }

        //
        // POST: /Account/SifreUnuttum
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SifreUnuttum(SifreUnuttumViewModel model)
        {
            if (ModelState.IsValid)
            {
                var kullanici = await KullaniciYoneticisi.FindByNameAsync(model.Eposta);
                if (kullanici== null || !(await KullaniciYoneticisi.IsEmailConfirmedAsync(kullanici.Id)))
                {

                    return View("SifreUnuttumOnaylama");
                }


            }

            return View(model);
        }

        //
        // GET: /Account/SifreUnuttumOnaylama
        [AllowAnonymous]
        public ActionResult SifreUnuttumOnaylama()
        {
            return View();
        }

        //
        // GET: /Hesap/SifreSifirla
        [AllowAnonymous]
        public ActionResult SifreSifirla(string kod)
        {
            return kod == null ? View("Hata") : View();
        }

        //
        // POST: /Hesap/SifreSifirla
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SifreSifirla(SifreSifirlaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var kullanici = await KullaniciYoneticisi.FindByNameAsync(model.Eposta);
            if (kullanici == null)
            {

                return RedirectToAction("SifreSifirlaOnaylama", "Hesap");
            }

            var sonuc = await KullaniciYoneticisi.ResetPasswordAsync(kullanici.Id, model.Kod, model.Sifre);
            if (sonuc.Succeeded)
            {
                return RedirectToAction("SifreSifirlaOnaylama", "Hesap");
            }

            HatalariEkle(sonuc);
            return View();
        }

        //
        // GET: /Hesap/SifreSifirlaOnaylama
        [AllowAnonymous]
        public ActionResult SifreSifirlaOnaylama()
        {
            return View();
        }

        //
        // POST: /Hesap/HariciGiris
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult HariciGiris(string saglayici, string returnUrl)
        {
 
            return new ChallengeResult(saglayici,
                Url.Action("HariciGirisCallback", "Hesap", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Hesap/KodGönder
        [AllowAnonymous]
        public async Task<ActionResult> KoduGonder(string returnUrl, bool hatirlaBeni)
        {
            var kullaniciId = await OturumAcmaYoneticisi.GetVerifiedUserIdAsync();
            if (kullaniciId == null)
            {
                return View("Hata");
            }

            var kullaniciAsamalari = await KullaniciYoneticisi.GetValidTwoFactorProvidersAsync(kullaniciId);
            var asamaSecenekleri = kullaniciAsamalari.Select(purpose => new SelectListItem { Text = purpose, Value = purpose })
                .ToList();
            return View(new KoduGonderViewModel
            {
                Saglayicilar = asamaSecenekleri,
                ReturnUrl = returnUrl,
                HatirlaBeni = hatirlaBeni
            });
        }

        //
        // POST: /Hesap/KoduGonder
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> KoduGonder(KoduGonderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

  
            if (!await OturumAcmaYoneticisi.SendTwoFactorCodeAsync(model.SecilenSaglayici))
            {
                return View("Hata");
            }

            return RedirectToAction("KoduDogrula",
                new { Provider = model.SecilenSaglayici, ReturnUrl = model.ReturnUrl, RememberMe = model.HatirlaBeni });
        }


        [AllowAnonymous]
        public async Task<ActionResult> HariciGirisCallback(string returnUrl)
        {
            var girisBilgisi = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (girisBilgisi == null)
            {
                return RedirectToAction("Giris");
            }

            // Sign in the user with this external login provider if the user already has a login
            var sonuc = await OturumAcmaYoneticisi.ExternalSignInAsync(girisBilgisi, isPersistent: false);
            switch (sonuc)
            {
                case SignInStatus.Success:
                    return LocaleYonlendir(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Kilitleme");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("KoduGonder", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                   
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = girisBilgisi.Login.LoginProvider;
                    return View("HariciGirisOnaylama",
                        new HariciGirisOnaylamaViewModel { Eposta = girisBilgisi.Email });
            }
        }

        //
        // POST: /Hesap/HariciGirisOnaylama
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> HariciGirisOnaylama(HariciGirisOnaylamaViewModel model,
            string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Yonet");
            }

            if (ModelState.IsValid)
            {
                // Harici giris sağlayıcıdan kullanıcı hakkında bilgi almak
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("HariciGirisBasarisiz");
                }

                var kullanici = new UygulamaKullanici { UserName = model.Eposta, Email = model.Eposta };
                var sonuc = await KullaniciYoneticisi.CreateAsync(kullanici);
                if (sonuc.Succeeded)
                {
                    sonuc = await KullaniciYoneticisi.AddLoginAsync(kullanici.Id, info.Login);
                    if (sonuc.Succeeded)
                    {
                        await OturumAcmaYoneticisi.SignInAsync(kullanici, isPersistent: false, rememberBrowser: false);
                        return LocaleYonlendir(returnUrl);
                    }
                }

                HatalariEkle(sonuc);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Hesap/Cikis
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cikis()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Hesap/HariciGirisBasarisiz
        [AllowAnonymous]
        public ActionResult HariciGirisBasarisiz()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_kullaniciYoneticisi != null)
                {
                    _kullaniciYoneticisi.Dispose();
                    _kullaniciYoneticisi = null;
                }

                if (_oturumAcmaYoneticisi != null)
                {
                    _oturumAcmaYoneticisi.Dispose();
                    _oturumAcmaYoneticisi = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private void HatalariEkle(IdentityResult sonuc)
        {
            foreach (var hata in sonuc.Errors)
            {
                ModelState.AddModelError("", hata);
            }
        }

        private ActionResult LocaleYonlendir(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string saglayici, string yonlendirUri, string kullaniciId)
            {
                GirisSaglayici = saglayici;
                YonlendirUri = yonlendirUri;
                KullaniciId = kullaniciId;
            }

            public string GirisSaglayici { get; set; }
            public string YonlendirUri { get; set; }
            public string KullaniciId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var ozellikler = new AuthenticationProperties { RedirectUri = YonlendirUri };
                if (KullaniciId != null)
                {
                    ozellikler.Dictionary[XsrfKey] = KullaniciId;
                }

                context.HttpContext.GetOwinContext().Authentication.Challenge(ozellikler, GirisSaglayici);
            }
        }

        #endregion
    }
}