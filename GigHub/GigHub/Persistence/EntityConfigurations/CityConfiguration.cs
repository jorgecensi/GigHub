using GigHub.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace GigHub.Persistence.EntityConfigurations
{
    public class CityConfiguration : EntityTypeConfiguration<City>
    {
        public CityConfiguration()
        {
            Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(255);

            Property(c => c.IsPartner)
                .IsRequired();
        }
    }
}
