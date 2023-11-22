using System;
using bytegr.candidates.manager.data.DbContexts;
using bytegr.candidates.manager.data.Entities;

namespace bytegr.candidates.manager.data.DbSeeds
{
	public static class AppDbSeeder
	{
        public static void SeedData(AppDbContext context)
        {
            if (!context.Candidates.Any()) {

                context.Candidates.AddRange(
                    new CandidateEntity() {
                        LastName = "Marinis",
                        FirstName = "Kyriakos",
                        Email = "kyrmarinis@gmail.com",
                        Mobile = "6977232273",
                        Degrees = new List<DegreeEntity> {
                            new DegreeEntity() { Name = "Bsc" },
                            new DegreeEntity() { Name = "Msc" }
                        },
                        DateCreated = DateTime.Now
                        
                    });

                context.SaveChanges();
            }
        }
    }
}

