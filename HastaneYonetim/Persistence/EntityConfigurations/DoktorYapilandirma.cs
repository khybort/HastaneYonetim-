using System.Data.Entity.ModelConfiguration;
using HastaneYonetim.Core.Models;

namespace HastaneYonetim.Persistence.EntityConfigurations
{
    public class DoktorYapilandirma : EntityTypeConfiguration<Doktor>
    {
        public DoktorYapilandirma()
        {
            Property(d => d.HekimId).IsRequired();
            Property(d => d.UzmanlikId).IsRequired();
            Property(d => d.Ad).IsRequired().HasMaxLength(255);
            Property(d => d.Telefon).IsRequired();
        }
    }
}