using System;
using AutoMapper;
using bytegr.candidates.manager.data.Dtos;
using bytegr.candidates.manager.data.Entities;

namespace bytegr.candidates.manager.web.Profiles
{
	public class Profiles : Profile
    {
		public Profiles()
		{
            //candidate
            //CreateMap<CandidateEntity, CandidateDto>().ForMember(d => d.CvFile, o => o.Ignore());
            CreateMap<CandidateEntity, CandidateDto>();
            CreateMap<CandidateDto, CandidateEntity>();

            //degree
            CreateMap<DegreeEntity, DegreeDto>();
            CreateMap<DegreeDto, DegreeEntity>();
        }
	}
}

