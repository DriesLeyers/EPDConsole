namespace Chipsoft.Assignments.EPDConsole
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