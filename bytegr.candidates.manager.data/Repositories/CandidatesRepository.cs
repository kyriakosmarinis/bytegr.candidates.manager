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
        public async Task InsertCandidateAsync(CandidateEntity candidate) {
            await _context.Candidates.AddAsync(candidate);
            await SaveDbContextChangesAsync();
        }

        public async Task InsertCandidateDegreeAsync(DegreeEntity degree) {
            await _context.Degrees.AddAsync(degree);
            await SaveDbContextChangesAsync();
        }

        //degree
        public async Task InsertDegreeAsync(DegreeEntity degree) {
            await _context.Degrees.AddAsync(degree);
            await SaveDbContextChangesAsync();
        }
        #endregion

        #region Get
        //candidate
        public async Task<CandidateEntity?> GetCandidateAsync(int candidateId, bool includeDegrees = true) {
            if (!await ExistsCandidateAsync(candidateId)) return new();//todo remove
            if (includeDegrees) return await _context.Candidates.Include(d => d.Degrees).Where(c => c.Id == candidateId).FirstOrDefaultAsync();
            else return await _context.Candidates.Where(c => c.Id == candidateId).FirstOrDefaultAsync();
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

        public int GetId() {
            return _context.GetId();
        }
        #endregion

        #region Exist
        //candidate
        public async Task<bool> ExistsCandidateAsync(int candidateId) {
            return await _context.Candidates.AnyAsync(c => c.Id == candidateId);
        }

        public async Task<bool> ExistsCandidateDegreeAsync(int candidateId) {
            return await _context.Degrees.AnyAsync(c => c.CandidateId == candidateId);
        }

        public async Task<bool> ExistsCandidateCvAsync(int candidateId) {
            return await _context.Candidates
                .Where(c => c.Id == candidateId)
                .Select(b => b.CvBlob != null && b.CvBlob.Length > 0)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> HasCandidateCvChanged(int candidateId, byte[] bytes) {
            if (bytes == null) return false;
            return !await _context.Candidates
                .Where(c => c.Id == candidateId && c.CvBlob.SequenceEqual(bytes))
                .AnyAsync();
        }

        public async Task<bool> AreCandidateEntitiesEqual(int candidateId, CandidateEntity newEntity) {
            var existingEntity = await GetCandidateAsync(candidateId);

            if (ReferenceEquals(existingEntity, newEntity)) return true;
            else return false;
        }

        //degree
        public async Task<bool> ExistsDegreeAsync(int degreeId) {
            return await _context.Degrees.AnyAsync(d => d.Id == degreeId);
        }
        #endregion

        #region Update
        //candidate
        public async Task UpdateCandidateAsync(CandidateEntity candidate) {
            var existing = await GetCandidateAsync(candidate.Id);

            if (existing != null) {
                _context.Entry(existing).CurrentValues.SetValues(candidate);
                await SaveDbContextChangesAsync();
            }
        }

        //degree
        public async Task UpdateDegreeAsync(DegreeEntity degree) {
            var existing = await GetDegreeAsync(degree.Id);

            if (existing != null) {
                _context.Entry(existing).CurrentValues.SetValues(degree);
                await SaveDbContextChangesAsync();
            }
        }
        #endregion

        #region Delete
        //candidate
        public async Task RemoveCandidateAsync(int candidateId)
        {
            if (await ExistsCandidateAsync(candidateId)) {
                var candidate = await GetCandidateAsync(candidateId);
                if (candidate != null) _context.Candidates.Remove(candidate);
                await SaveDbContextChangesAsync();
            }
            else throw new NotImplementedException();
        }

        //degree
        public async Task<bool> RemoveDegreeAsync(int degreeId)
        {
            if (await ExistsDegreeAsync(degreeId)) {
                var degree = await _context.Degrees.Where(d => d.Id == degreeId).FirstOrDefaultAsync();
                if (degree != null) _context.Remove(degree);
                return await SaveDbContextChangesAsync();
            }
            else throw new NotImplementedException();
        }

        //degree
        public async Task<bool> RemoveDegreesAsync(int candidateId) {
            var degrees = await GetCandidateDegreesAsync(candidateId);
            _context.Degrees.RemoveRange(degrees);
            return await SaveDbContextChangesAsync();
        }

        #endregion

        #region Common
        public async Task<bool> SaveDbContextChangesAsync() {
            return (await _context.SaveChangesAsync() >= 0);
        }
        #endregion
    }
}

