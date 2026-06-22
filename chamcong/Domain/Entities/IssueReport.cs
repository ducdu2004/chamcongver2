using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chamcong.Domain.Entities
{
    public class IssueReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int EmployeeId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public int Status { get; set; } // 0: Pending, 1: Resolved

        public DateTime CreatedAt { get; set; }

        // Navigation property
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
    }
}
