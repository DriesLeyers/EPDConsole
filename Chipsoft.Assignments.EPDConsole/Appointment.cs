namespace Chipsoft.Assignments.EPDConsole
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public Physician Physician { get; set; } = default!;
        public Patient Patient { get; set; } = default!;
    }
}