using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using bytegr.candidates.manager.data.Entities.Common;

namespace bytegr.candidates.manager.data.Entities
{
	public class CandidateEntity : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Mobile { get; set; } = string.Empty;

        public ICollection<DegreeEntity> Degrees { get; set; } = new List<DegreeEntity>();

        [Column(TypeName = "varbinary(MAX)")]
        public byte[] CvBlob { get; set; } = Array.Empty<byte>();
    }
}

