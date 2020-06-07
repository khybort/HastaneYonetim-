using System.Data.Entity.ModelConfiguration;
using HastaneYonetim.Core.Models;

namespace HastaneYonetim.Persistence.EntityConfigurations
{
    public class HastaDurumuYapilandirma : EntityTypeConfiguration<HastaDurumu>
    {
        public HastaDurumuYapilandirma()
        {
            Property(s => s.Ad).IsRequired().HasMaxLength(255);
        }
    }
}