using System;
using bytegr.candidates.manager.data.Entities;

namespace bytegr.candidates.manager.data.Repositories
{
	public interface ICandidatesRepository
	{
        Task<bool> CandidateExistsAsync(int candidateId);
        Task<bool> CandidateHasDegreesAsync(int candidateId);

        Task AddCandidateAsync(CandidateEntity candidate);
        Task UpdateCandidateAsync(CandidateEntity candidate);
        Task DeleteCandidateAsync(int candidateId);
        Task AddDegreeAsync(int candidateId, DegreeEntity degree);
        Task AddDegreeToCandidateAsync(int candidateId, DegreeEntity degree);
        Task DeleteDegreeAsync(int degreeId);
        
        Task<CandidateEntity?> GetCandidateAsync(int candidateId);
        Task<IEnumerable<CandidateEntity>> GetCandidatesAsync();
        Task<IEnumerable<DegreeEntity>> GetDegreesAsync();
        Task<IEnumerable<DegreeEntity>> GetCandidateDegreesAsync(int candidateId);

        Task<bool> SaveChangesAsync();
    }
}

