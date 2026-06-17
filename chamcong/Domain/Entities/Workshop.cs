using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace chamcong.Domain.Entities
{
    public class Workshop
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public bool IsInternal { get; set; }

        [StringLength(255)]
        public string? Address { get; set; }

        // Navigation properties
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
