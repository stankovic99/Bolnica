using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bolnica.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Bolnica.Areas.Identity.Data;

namespace Bolnica.Data
{
    public class BolnicaContext : IdentityDbContext<BolnicaUser>
    {
        public BolnicaContext (DbContextOptions<BolnicaContext> options)
            : base(options)
        {
        }

        public DbSet<Bolnica.Models.Doktor> Doktor { get; set; }

        public DbSet<Bolnica.Models.Pacient> Pacient { get; set; }

        public DbSet<Bolnica.Models.LekuvanPacient> LekuvanPacient { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
           

            builder.Entity<LekuvanPacient>()
            .HasOne<Pacient>(p => p.Pacient)
            .WithMany(p => p.Doktors)
            .HasForeignKey(p => p.PacientId);

            builder.Entity<LekuvanPacient>()
            .HasOne<Doktor>(p => p.Doktor)
            .WithMany(p => p.Pacients)
            .HasForeignKey(p => p.DoktorId);

            base.OnModelCreating(builder);
        }
    }
}
