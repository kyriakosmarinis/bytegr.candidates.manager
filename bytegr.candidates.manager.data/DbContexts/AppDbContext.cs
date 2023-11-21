using System;
using bytegr.candidates.manager.data.Entities;
using Microsoft.EntityFrameworkCore;

namespace bytegr.candidates.manager.data.DbContexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<CandidateEntity> Candidates { get; set; } = null!;
        public DbSet<DegreeEntity> Degrees { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) {
        }

        public int GetId() {
            return Candidates.Max(i => (int?)i.Id) ?? 0;
        }


    }
}


