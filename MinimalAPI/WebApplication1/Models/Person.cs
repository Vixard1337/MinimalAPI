using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MinimalAPI.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        #pragma warning disable CS8618
        public string FirstName { get; set; }
        #pragma warning restore CS8618

        [Required]
        [StringLength(50)]
        #pragma warning disable CS8618
        public string LastName { get; set; }
        #pragma warning restore CS8618

        [Required]
        #pragma warning disable CS8618
        public DateOnly BirthDate { get; set; } // YYYY-MM-DD
        #pragma warning restore CS8618

        [Required]
        [StringLength(150)]
        #pragma warning disable CS8618
        public string Address { get; set; }
        #pragma warning restore CS8618
    }
}
