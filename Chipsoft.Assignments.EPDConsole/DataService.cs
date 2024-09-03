namespace Chipsoft.Assignments.EPDConsole
{
    public class DataService
    {
        EPDDbContext dbContext = new EPDDbContext();

        public void AddPatient(Patient newPatient)
        {
            dbContext.Patients.Add(newPatient);
            dbContext.SaveChanges();
        }

        public void PrintAllPatients()
        {
            foreach (Patient patient in dbContext.Patients.Where(x => x.IsActive))
            {
                Console.WriteLine($"ID: {patient.Id} Naam: {patient.FirstName} Achternaam: {patient.LastName}");
            }
        }

        public Patient GetPatientById(int patientId)
        {
            return dbContext.Patients.FirstOrDefault(x => x.Id == patientId && x.IsActive);
        }

        public bool CheckIfPatientIsAvailable(Patient patient, DateTime date)
        {
            return !dbContext.Appointments.Where(x => x.Patient.Id == patient.Id).ToList()
               .Where(x => x.Date == date).Any();
        }

        public void DeletePatient(Patient patient)
        {
            patient.IsActive = false;

            dbContext.Patients.Update(patient);
            dbContext.SaveChanges();
        }

        public ICollection<Appointment> GetAllAppointments()
        {
            return dbContext.Appointments.ToList();
        }

        public ICollection<Appointment> GetAllAppointmentsByPatientId(int patientId)
        {
            return dbContext.Patients.First(x => x.Id == patientId).Appointments.ToList();
        }

        public ICollection<Appointment> GetAllAppointmentsByPhysicianId(int physicianId)
        {
            return dbContext.Physicians.First(x => x.Id == physicianId).Appointments.ToList();
        }

        public void AddAppointment(Appointment newAppointment)
        {
            dbContext.Appointments.Add(newAppointment);
            dbContext.SaveChanges();
        }

        public void AddPhysician(Physician newPhysician)
        {
            dbContext.Physicians.Add(newPhysician);
            dbContext.SaveChanges();
        }

        public void PrintAllPhysicians()
        {
            foreach (Physician physicians in dbContext.Physicians.Where(x => x.IsActive))
            {
                Console.WriteLine($"ID: {physicians.Id} Naam: {physicians.FirstName} Achternaam: {physicians.LastName}");
            }
        }

        public Physician GetPhysicianById(int physicianId)
        {
            return dbContext.Physicians.FirstOrDefault(x => x.Id == physicianId && x.IsActive);
        }

        public bool CheckIfPhysicianIsAvailable(Physician physician, DateTime date)
        {
            return !dbContext.Appointments.Where(x => x.Physician.Id == physician.Id).ToList()
                .Where(x => x.Date == date).Any();
        }

        public void DeletePhysician(Physician physician)
        {
            physician.IsActive = false;

            dbContext.Physicians.Update(physician);
            dbContext.SaveChanges();
        }


    }
}
