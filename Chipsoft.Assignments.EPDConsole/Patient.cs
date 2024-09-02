using System.ComponentModel.DataAnnotations;

namespace Chipsoft.Assignments.EPDConsole
{
    public class Patient : Person
    {
        [Required]
        public string? NationalRegisterNumber { get; set; }

        public Patient() { }

    }
}