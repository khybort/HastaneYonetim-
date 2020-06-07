using HastaneYonetim.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace HastaneYonetim.Persistence.EntityConfigurations
{
    public class RandevuYapilandirma : EntityTypeConfiguration<Randevu>
    {
        public RandevuYapilandirma()
        {
            Property(a => a.HastaId).IsRequired();
            Property(a => a.DoktorId).IsRequired();
            Property(a => a.BaslangicTarihSure).IsRequired();
            Property(a => a.Detay).IsRequired();
            Property(a => a.Durum).IsRequired();
        }
    }
}