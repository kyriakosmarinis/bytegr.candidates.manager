using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using bytegr.candidates.manager.data.Entities.Common;

namespace bytegr.candidates.manager.data.Entities
{
	public class DegreeEntity : BaseEntity
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [ForeignKey("CandidateId")]
        public CandidateEntity? Candidate { get; set; }
        public int CandidateId { get; set; }
    }
}

