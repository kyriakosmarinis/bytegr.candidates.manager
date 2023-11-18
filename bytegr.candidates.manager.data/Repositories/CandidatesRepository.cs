using System;
using bytegr.candidates.manager.data.Entities;

namespace bytegr.candidates.manager.data.Repositories
{
	public class CandidatesRepository : ICandidatesRepository
    {
		public CandidatesRepository()
		{
		}

        public Task AddCandidateAsync(CandidateEntity candidate)
        {
            throw new NotImplementedException();
        }

        public Task AddDegreeAsync(int candidateId, DegreeEntity degree)
        {
            throw new NotImplementedException();
        }

        public Task AddDegreeToCandidateAsync(int candidateId, DegreeEntity degree)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CandidateExistsAsync(int candidateId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CandidateHasDegreesAsync(int candidateId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCandidateAsync(int candidateId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDegreeAsync(int degreeId)
        {
            throw new NotImplementedException();
        }

        public Task<CandidateEntity?> GetCandidateAsync(int candidateId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DegreeEntity>> GetCandidateDegreesAsync(int candidateId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CandidateEntity>> GetCandidatesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DegreeEntity>> GetDegreesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateCandidateAsync(CandidateEntity candidate)
        {
            throw new NotImplementedException();
        }
    }
}

