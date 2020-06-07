using System.Data.Entity.ModelConfiguration;
using HastaneYonetim.Core.Models;

namespace HastaneYonetim.Persistence.EntityConfigurations
{
    public class HastaYapilandirma : EntityTypeConfiguration<Hasta>
    {
        public HastaYapilandirma()
        {
            Property(p => p.SehirId).IsRequired();
            Property(p => p.Ad).IsRequired().HasMaxLength(255);
            Property(p => p.Telefon).IsRequired().HasMaxLength(255);
            Property(p => p.Adres).IsRequired().HasMaxLength(255);
            Property(p => p.DogumTarihi).IsRequired();
            Property(p => p.HastaNumarasi).IsRequired();
            HasMany(p => p.Randevular)
                .WithRequired(a => a.Hasta)
                .WillCascadeOnDelete(false);
        }
    }
}