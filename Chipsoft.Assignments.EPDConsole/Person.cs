using System.ComponentModel.DataAnnotations;

namespace Chipsoft.Assignments.EPDConsole
{
    public class Person
    {
        public int Id { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public DateTime? BirthDate { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public int? PostalCode { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Gender { get; set; }
    }
}