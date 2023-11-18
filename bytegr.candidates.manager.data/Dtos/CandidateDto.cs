using System;
namespace bytegr.candidates.manager.data.Dtos
{
    public class CandidateDto
    {
        public int Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string Email { get; set; }
        public string Mobile { get; set; }
        public ICollection<DegreeDto> Degrees { get; set; } = new List<DegreeDto>();

        public byte[] CvBlob { get; set; } = new byte[] { };
    }
}

