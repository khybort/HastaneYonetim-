using System.Data.Entity.ModelConfiguration;
using HastaneYonetim.Core.Models;

namespace HastaneYonetim.Persistence.EntityConfigurations
{
    public class SehirYapilandirma : EntityTypeConfiguration<Sehir>
    {
        public SehirYapilandirma()
        {
            Property(p => p.Ad).IsRequired().HasMaxLength(255);
        }
    }
}