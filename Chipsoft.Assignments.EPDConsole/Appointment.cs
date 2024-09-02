using System.ComponentModel.DataAnnotations;

namespace Chipsoft.Assignments.EPDConsole
{
    public class Appointment
    {
        public int Id { get; set; }
        [Required]
        public DateTime? Date { get; set; }
        public int PhysicianId { get; set; }
        public int PatientId { get; set; }
        [Required]
        public Physician Physician { get; set; }
        [Required]
        public Patient Patient { get; set; }

        public Appointment() { }

    }
}