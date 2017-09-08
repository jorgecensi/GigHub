﻿using GigHub.Core.Models;
using System.Data.Entity.ModelConfiguration;

namespace GigHub.Persistence.EntityConfigurations
{
    public class GigConfiguration : EntityTypeConfiguration<Gig>
    {
        public GigConfiguration()
        {
            Property(g => g.ArtistId)
                .IsRequired();

            Property(g => g.Venue)
                .IsRequired()
                .HasMaxLength(255);

            Property(g => g.GenreId)
                .IsRequired();

            //Create a relationship between Gig and Attendance
            //turn off cascade delete 
            HasMany(g => g.Attendances)
                .WithRequired(a => a.Gig)
                .WillCascadeOnDelete(false);

        }
    }
}