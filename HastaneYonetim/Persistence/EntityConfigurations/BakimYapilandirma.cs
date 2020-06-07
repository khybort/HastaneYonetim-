using HastaneYonetim.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace HastaneYonetim.Persistence.EntityConfigurations
{
    public class BakimYapilandirma : EntityTypeConfiguration<Bakim>
    {
        public BakimYapilandirma()
        {
            Property(p => p.HastaId).IsRequired();
            Property(p => p.KlinikBulgular).IsRequired();
            Property(p => p.Teshis).IsRequired().HasMaxLength(255);
            Property(p => p.Terapi).IsRequired();
        }
    }
}