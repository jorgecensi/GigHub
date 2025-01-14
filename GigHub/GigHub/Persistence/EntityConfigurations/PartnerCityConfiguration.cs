using GigHub.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace GigHub.Persistence.EntityConfigurations
{
    public class PartnerCityConfiguration : EntityTypeConfiguration<PartnerCity>
    {
        public PartnerCityConfiguration()
        {
            Property(pc => pc.Name)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
