using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using bytegr.candidates.manager.data.Dtos.Common;

namespace bytegr.candidates.manager.data.Dtos
{
    public class DegreeDto : BaseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CandidateId { get; set; }
    }
}

