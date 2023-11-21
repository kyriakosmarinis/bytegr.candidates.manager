using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace bytegr.candidates.manager.data.Dtos
{
    public class DegreeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CandidateId { get; set; }
    }
}

