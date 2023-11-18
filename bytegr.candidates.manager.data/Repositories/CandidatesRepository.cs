using System;
using bytegr.candidates.manager.data.DbContexts;
using bytegr.candidates.manager.data.Entities;
using Microsoft.EntityFrameworkCore;

namespace bytegr.candidates.manager.data.Repositories
{
	public class CandidatesRepository : ICandidatesRepository
    {
        private readonly AppDbContext _context;

        public CandidatesRepository(AppDbContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region Add
        //candidate
        public async Task AddCandidateAsync(CandidateEntity candidate) {
            await _context.Candidates.AddAsync(candidate);
            await SaveChangesAsync();
        }

        public async Task AddCandidateDegreeAsync(int candidateId, DegreeEntity degree)
        {
            if (await CandidateExistsAsync(candidateId)) {
                var candidate = await GetCandidateAsync(candidateId);

                if (candidate != null) {
                    candidate.Degrees.Add(degree);
                    await SaveChangesAsync();
                }
            }
            else throw new NotImplementedException();
        }

        //degree
        public async Task AddDegreeAsync(DegreeEntity degree) {
            await _context.Degrees.AddAsync(degree);
            await SaveChangesAsync();
        }
        #endregion

        #region Get
        //candidate
        public async Task<CandidateEntity?> GetCandidateAsync(int candidateId) {
            return await _context.Candidates.Include(d => d.Degrees).Where(c => c.Id == candidateId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CandidateEntity>> GetCandidatesAsync(bool includeDegrees = true) {
            if (includeDegrees) return await _context.Candidates.OrderBy(c => c.Id).Include(d => d.Degrees).ToListAsync();
            else return await _context.Candidates.OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<IEnumerable<DegreeEntity>> GetCandidateDegreesAsync(int candidateId) {
            return await _context.Degrees.Where(d => d.CandidateId == candidateId).ToListAsync();
        }

        //degree
        public async Task<DegreeEntity?> GetDegreeAsync(int degreeId) {
            return await _context.Degrees.Where(d => d.Id == degreeId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<DegreeEntity>> GetDegreesAsync() {
            return await _context.Degrees.OrderBy(d => d.Id).ToListAsync();
        }
        #endregion

        #region Exist
        //candidate
        public async Task<bool> CandidateExistsAsync(int candidateId) {
            return await _context.Candidates.AnyAsync(c => c.Id == candidateId);
        }

        public async Task<bool> CandidateHasAnyDegreeAsync(int candidateId) {
            return await _context.Degrees.AnyAsync(c => c.CandidateId == candidateId);
        }

        //degree
        public async Task<bool> DegreeExistsAsync(int degreeId) {
            return await _context.Degrees.AnyAsync(d => d.Id == degreeId);
        }
        #endregion

        #region Update
        //candidate
        public async Task UpdateCandidateAsync(CandidateEntity candidate) {
            var existing = await GetCandidateAsync(candidate.Id);

            if (existing != null) {
                _context.Entry(existing).CurrentValues.SetValues(candidate);
                await SaveChangesAsync();
            }
        }

        //degree
        public async Task UpdateDegreeAsync(DegreeEntity degree) {
            var existing = await GetDegreeAsync(degree.Id);

            if (existing != null) {
                _context.Entry(existing).CurrentValues.SetValues(degree);
                await SaveChangesAsync();
            }
        }
        #endregion

        #region Delete
        //candidate
        public async Task DeleteCandidateAsync(int candidateId)
        {
            if (await CandidateExistsAsync(candidateId)) {
                var candidate = await GetCandidateAsync(candidateId);

                if (await CandidateHasAnyDegreeAsync(candidateId)) {
                    var degrees = GetCandidateDegreesAsync(candidateId);
                    _context.Degrees.RemoveRange((IEnumerable<DegreeEntity>)degrees);
                }

                if (candidate != null) _context.Candidates.Remove(candidate);
                await SaveChangesAsync();
            }
            else throw new NotImplementedException();
        }

        //degree
        public async Task DeleteDegreeAsync(int degreeId)
        {
            if (await DegreeExistsAsync(degreeId))
            {
                var degree = await _context.Degrees.Where(d => d.Id == degreeId).FirstOrDefaultAsync();
                if (degree != null) _context.Remove(degree);
                await SaveChangesAsync();
            }
            else throw new NotImplementedException();
        }
        #endregion

        #region Common
        public async Task<bool> SaveChangesAsync() {
            return (await _context.SaveChangesAsync() >= 0);
        }
        #endregion
    }
}

