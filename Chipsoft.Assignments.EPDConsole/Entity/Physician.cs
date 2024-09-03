namespace Chipsoft.Assignments.EPDConsole.Entity
{
    public class Physician
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public ICollection<Appointment> Appointments { get; set; } = [];
    }
}