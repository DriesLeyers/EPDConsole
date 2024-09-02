using System.ComponentModel.DataAnnotations;

namespace Chipsoft.Assignments.EPDConsole
{
    public class Patient
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public string City { get; set; } = string.Empty;
        [Range(1000, 9999)]
        public int PostalCode { get; set; }
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string NationalRegisterNumber { get; set; } = string.Empty;
        public ICollection<Appointment> Appointments { get; set; } = [];
    }
}