using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using HastaneYonetim.Core.Models;
using HastaneYonetim.Persistence;
using HastaneYonetim.Models;

namespace HastaneYonetim
{
    // Configure the  rolemanager used in the application.  Rolemanager is defined in asp.net identity core assembly
    public class UygulamaRolYöneticisi : RoleManager<IdentityRole>
    {
        public UygulamaRolYöneticisi(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore) { }
        public static UygulamaRolYöneticisi Olustur(IdentityFactoryOptions<UygulamaRolYöneticisi> options, IOwinContext context)
        {
            return new UygulamaRolYöneticisi(new RoleStore<IdentityRole>(context.Get<UygulamaDbContext>()));
        }
    }

    public class EpostaServisi : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage mesaj)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsServisi : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class UygulamaKullaniciYoneticisi : UserManager<UygulamaKullanici>
    {
        public UygulamaKullaniciYoneticisi(IUserStore<UygulamaKullanici> store)
            : base(store)
        {
        }

        public static UygulamaKullaniciYoneticisi Olustur(IdentityFactoryOptions<UygulamaKullaniciYoneticisi> options, IOwinContext context)
        {
            var yonetici = new UygulamaKullaniciYoneticisi(new UserStore<UygulamaKullanici>(context.Get<UygulamaDbContext>()));
            // Configure validation logic for usernames
            yonetici.UserValidator = new UserValidator<UygulamaKullanici>(yonetici)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            yonetici.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            yonetici.UserLockoutEnabledByDefault = true;
            yonetici.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            yonetici.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            yonetici.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<UygulamaKullanici>
            {
                MessageFormat = "Your security code is {0}"
            });
            yonetici.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<UygulamaKullanici>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            yonetici.EmailService = new EpostaServisi();
            yonetici.SmsService = new SmsServisi();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                yonetici.UserTokenProvider =
                    new DataProtectorTokenProvider<UygulamaKullanici>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return yonetici;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class UygulamaOturumAcmaYoneticisi : SignInManager <UygulamaKullanici, string>
    {
        public UygulamaOturumAcmaYoneticisi(UygulamaKullaniciYoneticisi kullaniciYoneticisi, IAuthenticationManager authenticationManager)
            : base(kullaniciYoneticisi, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(UygulamaKullanici kullanici)
        {
            return kullanici.GenerateUserIdentityAsync((UygulamaKullaniciYoneticisi)UserManager);
        }

        public static UygulamaOturumAcmaYoneticisi Olustur(IdentityFactoryOptions<UygulamaOturumAcmaYoneticisi> options, IOwinContext context)
        {
            return new UygulamaOturumAcmaYoneticisi(context.GetUserManager<UygulamaKullaniciYoneticisi>(), context.Authentication);
        }



        //override PasswordSignInAsyc aktif ve kilitli kullanıcılar için

        public override Task<SignInStatus> PasswordSignInAsync(string kullaniciAdi, string sifre, bool hatirlaBeni, bool kilitlenmeliMi)
        {
            var kullanici = UserManager.FindByEmailAsync(kullaniciAdi).Result;

            if ((kullanici.aktifMi.HasValue && !kullanici.aktifMi.HasValue) || !kullanici.aktifMi.HasValue)
                return Task.FromResult<SignInStatus>(SignInStatus.LockedOut);

            return base.PasswordSignInAsync(kullaniciAdi, sifre, hatirlaBeni, kilitlenmeliMi);
        }
    }
}
