using System;
using bytegr.candidates.manager.data.Entities;

namespace bytegr.candidates.manager.data.Repositories
{
	public interface ICandidatesRepository
	{
        //candidate
        Task InsertCandidateAsync(CandidateEntity candidate);
        Task InsertCandidateDegreeAsync(DegreeEntity degree);
        Task UpdateCandidateAsync(CandidateEntity candidate);//todo not use - delete
        Task RemoveCandidateAsync(int candidateId);

        //degree
        Task InsertDegreeAsync(DegreeEntity degree);//todo not use - delete
        Task UpdateDegreeAsync(DegreeEntity degree);//todo not use - delete
        Task<bool> RemoveDegreeAsync(int degreeId);//todo not use - delete
        Task<bool> RemoveDegreesAsync(int candidateId);

        //collections
        Task<CandidateEntity?> GetCandidateAsync(int candidateId, bool includeDegrees);
        Task<IEnumerable<CandidateEntity>> GetCandidatesAsync(bool includeDegrees);
        Task<DegreeEntity?> GetDegreeAsync(int degreeId);
        Task<IEnumerable<DegreeEntity>> GetDegreesAsync();
        Task<IEnumerable<DegreeEntity>> GetCandidateDegreesAsync(int candidateId);
        int GetId();

        //exists
        Task<bool> ExistsCandidateAsync(int candidateId);
        Task<bool> ExistsCandidateCvAsync(int candidateId);
        Task<bool> ExistsCandidateDegreeAsync(int candidateId);
        Task<bool> ExistsDegreeAsync(int degreeId);

        //has
        Task<bool> HasCandidateCvChanged(int candidateId, byte[] bytes);

        //are
        Task<bool> AreCandidateEntitiesEqual(int candidateId, CandidateEntity newEntity);

        //common
        Task<bool> SaveDbContextChangesAsync();
    }
}

