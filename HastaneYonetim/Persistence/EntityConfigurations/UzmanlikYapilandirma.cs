using System.Data.Entity.ModelConfiguration;
using HastaneYonetim.Core.Models;

namespace HastaneYonetim.Persistence.EntityConfigurations
{
    public class UzmanlikYapilandirma : EntityTypeConfiguration<Uzmanlik>
    {
        public UzmanlikYapilandirma()
        {
            Property(s => s.Ad).IsRequired().HasMaxLength(255);
        }
    }
}