using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;
using bytegr.candidates.manager.data.Dtos.Common;

namespace bytegr.candidates.manager.data.Dtos
{
    public class CandidateDto : BaseDto
    {
        public int Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public ICollection<DegreeDto> Degrees { get; set; } = new List<DegreeDto>();

        public byte[] CvBlob { get; set; } = Array.Empty<byte>();
        public IFormFile CvFile { get; set; } = new FormFile(Stream.Null, 0, 0, "file", "file");
    }
}

