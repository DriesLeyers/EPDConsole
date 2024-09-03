namespace Chipsoft.Assignments.EPDConsole.Entity
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Physician Physician { get; set; } = default!;
        public Patient Patient { get; set; } = default!;
    }
}