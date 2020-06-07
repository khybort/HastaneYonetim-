using HastaneYonetim.Core.Models;
using HastaneYonetim.Models;
using HastaneYonetim.Persistence.EntityConfigurations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace HastaneYonetim.Persistence
{
    public class UygulamaDbContext : IdentityDbContext<UygulamaKullanici>
    {
        public DbSet<Hasta> Hastalar { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
        public DbSet<Bakim> Bakimlar { get; set; }
        public DbSet<Doktor> Doktorlar { get; set; }
        public DbSet<Uzmanlik> Uzmanliklar { get; set; }
        public DbSet<Sehir> Sehirler { get; set; }


        public UygulamaDbContext()
            : base("HastaneDB", throwIfV1Schema: false)
        {
        }

        public static UygulamaDbContext Olustur()
        {
            return new UygulamaDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new HastaYapilandirma());
            modelBuilder.Configurations.Add(new RandevuYapilandirma());
            modelBuilder.Configurations.Add(new DoktorYapilandirma());
            modelBuilder.Configurations.Add(new BakimYapilandirma());
            modelBuilder.Configurations.Add(new UzmanlikYapilandirma());
            modelBuilder.Configurations.Add(new SehirYapilandirma());
            base.OnModelCreating(modelBuilder);
        }
    }
}