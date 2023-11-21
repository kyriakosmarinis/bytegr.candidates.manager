using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using Microsoft.AspNetCore.Http;

namespace bytegr.candidates.manager.data.Dtos
{
    public class CandidateDto
    {
        public int Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public ICollection<DegreeDto> Degrees { get; set; } = new List<DegreeDto>();

        [Display(Name = "Selected Degree")]
        public int? SelectedDegreeId { get; set; }

        public byte[] CvBlob { get; set; } = Array.Empty<byte>();
        public IFormFile CvFile { get; set; } = new FormFile(Stream.Null, 0, 0, "file", "file");
    }
}

