using System;
using bytegr.candidates.manager.data.Entities;

namespace bytegr.candidates.manager.data.Repositories
{
	public interface ICandidatesRepository
	{
        //candidate
        Task AddCandidateAsync(CandidateEntity candidate);
        Task AddCandidateDegreeAsync(int candidateId, DegreeEntity degree);
        Task UpdateCandidateAsync(CandidateEntity candidate);
        Task DeleteCandidateAsync(int candidateId);

        //degree
        Task AddDegreeAsync(DegreeEntity degree);
        Task UpdateDegreeAsync(DegreeEntity degree);
        Task DeleteDegreeAsync(int degreeId);

        //collections
        Task<CandidateEntity?> GetCandidateAsync(int candidateId);
        Task<IEnumerable<CandidateEntity>> GetCandidatesAsync(bool includeDegrees);
        Task<DegreeEntity?> GetDegreeAsync(int degreeId);
        Task<IEnumerable<DegreeEntity>> GetDegreesAsync();
        Task<IEnumerable<DegreeEntity>> GetCandidateDegreesAsync(int candidateId);

        //exists
        Task<bool> CandidateExistsAsync(int candidateId);
        Task<bool> CandidateHasAnyDegreeAsync(int candidateId);
        Task<bool> DegreeExistsAsync(int degreeId);

        //common
        Task<bool> SaveChangesAsync();
    }
}

