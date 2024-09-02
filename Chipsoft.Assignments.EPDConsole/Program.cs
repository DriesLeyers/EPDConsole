using Microsoft.EntityFrameworkCore;

namespace Chipsoft.Assignments.EPDConsole
{
    public class Program
    {
        //Don't create EF migrations, use the reset db option
        //This deletes and recreates the db, this makes sure all tables exist

        private static void AddPatient()
        {
            Patient newPatient = new Patient();

            using (var dbContext = new EPDDbContext())
            {
                Console.WriteLine("Voer de voornaam van de patient in:");
                newPatient.FirstName = Console.ReadLine();

                Console.WriteLine("Voer de achternaam van de patient in:");
                newPatient.LastName = Console.ReadLine();

                Console.WriteLine("Voer de email van de patient in:");
                newPatient.Email = Console.ReadLine();

                Console.WriteLine("Voer de geboortedatum van de patient in: (YYYY-MM-DD):");
                DateTime birthDate;
                var inputBirthDate = Console.ReadLine();

                while (string.IsNullOrWhiteSpace(inputBirthDate) || !DateTime.TryParse(inputBirthDate, out birthDate))
                {
                    Console.WriteLine("Ongeldig formaat. Probeer opnieuw (YYYY-MM-DD):");
                    inputBirthDate = Console.ReadLine();
                }

                newPatient.BirthDate = birthDate;

                Console.WriteLine("Voer het dorp of de stad van de patient in:");
                newPatient.City = Console.ReadLine();

                Console.WriteLine("Voer de postcode van de patient in:");
                newPatient.PostalCode = int.Parse(Console.ReadLine());

                Console.WriteLine("Voer het adres van de patient in:");
                newPatient.Address = Console.ReadLine();

                Console.WriteLine("Voer het rijksregisternummer van de patient in:");
                newPatient.NationalRegisterNumber = Console.ReadLine();

                Console.WriteLine("Voer het telefoonnummer van de patient in:");
                newPatient.PhoneNumber = Console.ReadLine();

                Console.WriteLine("Geef het geslacht van de patient in: (M/V/X)");
                newPatient.Gender = Console.ReadLine();

                dbContext.Add(newPatient);
                dbContext.SaveChanges();
            }
        }

        private static void ShowAppointment()
        {
            Console.WriteLine("1 - Alle afspraken");
            Console.WriteLine("2 - Alle afspraken zoeken voor patient");
            Console.WriteLine("3 - Alle afspraken zoeken voor arts");

            using (var dbContext = new EPDDbContext())
            {
                if (int.TryParse(Console.ReadLine(), out int option))
                {
                    switch (option)
                    {
                        case 1:
                            foreach (Appointment appointment in dbContext.Appointments.Include(x => x.Physician).Include(x => x.Patient))
                            {
                                Console.WriteLine($"Afspraak met ID: {appointment.Id} tussen {appointment.Patient.FirstName} {appointment.Patient.LastName} en {appointment.Physician.FirstName} {appointment.Physician.LastName}");
                            }
                            Console.WriteLine("Druk op enter om verder te gaan");
                            Console.ReadLine();

                            break;
                        case 2:
                            Console.WriteLine("Vul de ID in van de patient waarvan u alle afspraken wilt zien:");
                            int patientId = int.Parse(Console.ReadLine());

                            foreach (Appointment appointment in dbContext.Appointments.Include(x => x.Physician).Include(x => x.Patient).Where(x => x.PatientId == patientId))
                            {
                                Console.WriteLine($"Afspraak met ID: {appointment.Id} tussen {appointment.Patient.FirstName} {appointment.Patient.LastName} en {appointment.Physician.FirstName} {appointment.Physician.LastName}");
                            }
                            Console.WriteLine("Druk op enter om verder te gaan");
                            Console.ReadLine();

                            break;
                        case 3:
                            Console.WriteLine("Vul de ID in van de arts waarvan u alle afspraken wilt zien:");
                            int physicianId = int.Parse(Console.ReadLine());

                            foreach (Appointment appointment in dbContext.Appointments.Include(x => x.Physician).Include(x => x.Patient).Where(x => x.PhysicianId == physicianId))
                            {
                                Console.WriteLine($"Afspraak met ID: {appointment.Id} tussen {appointment.Patient.FirstName} {appointment.Patient.LastName} en {appointment.Physician.FirstName} {appointment.Physician.LastName}");
                            }
                            Console.WriteLine("Druk op enter om verder te gaan");
                            Console.ReadLine();

                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private static void AddAppointment()
        {
            Appointment newAppointment = new Appointment();

            using (var dbContext = new EPDDbContext())
            {
                Console.WriteLine("Voor welke datum is de afspraak?");
                DateTime appointmentDate;
                string? inputDate = Console.ReadLine();

                while (string.IsNullOrWhiteSpace(inputDate) || !DateTime.TryParse(inputDate, out appointmentDate))
                {
                    Console.WriteLine("Ongeldig formaat. Probeer opnieuw (YYYY-MM-DD):");
                    inputDate = Console.ReadLine();
                }

                Console.WriteLine("Hoelaat wilt u de afspraak? (HH:MM):");
                TimeSpan appointmentTime;
                string? inputTime = Console.ReadLine();

                while (string.IsNullOrWhiteSpace(inputTime) || !TimeSpan.TryParse(inputTime, out appointmentTime))
                {
                    Console.WriteLine("Ongeldig formaat. Probeer opnieuw (HH:MM):");
                    inputTime = Console.ReadLine();
                }

                var newAppointmentDate = appointmentDate.Date + appointmentTime;
                newAppointment.Date = newAppointmentDate;

                Console.WriteLine("Voor welke patient wilt u de afspraak inplannen? Geef de ID:");
                PrintAllPatients(dbContext);

                newAppointment.Patient = PickPatientForAppointment(dbContext, newAppointmentDate);

                Console.WriteLine("Voor welke arts wilt u de afspraak inplannen? Geef de ID:");
                PrintAllPhysicians(dbContext);

                newAppointment.Physician = PickPhysicianForAppointment(dbContext, newAppointmentDate);

                dbContext.Appointments.Add(newAppointment);
                dbContext.SaveChanges();
            }
        }

        private static Physician PickPhysicianForAppointment(EPDDbContext dbContext, DateTime newAppointmentDate)
        {
            Physician? physician = null;

            int physicianId = 0;
            bool isPhysicianAvailable = true;

            do
            {
                physicianId = int.Parse(Console.ReadLine());
                physician = dbContext.Physicians.FirstOrDefault(x => x.Id == physicianId && x.IsActive);

                if (physician == null)
                    Console.WriteLine("Er is geen physician gevonden met deze ID. Probeer opnieuw");
                else
                {
                    isPhysicianAvailable = CheckIfPhysicianIsAvailable(dbContext, physician, newAppointmentDate);
                    Console.WriteLine($"Op dit moment heeft {physician.FirstName} {physician.LastName} al een afspraak. Geef een ander tijdstip op of probeer een andere patient.");
                }

            } while (physician == null && !isPhysicianAvailable);

            return physician;
        }

        private static Patient PickPatientForAppointment(EPDDbContext dbContext, DateTime newAppointmentDate)
        {
            Patient? patient = null;
            int patientId = 0;
            bool isPatientAvailable = true;

            do
            {
                patientId = int.Parse(Console.ReadLine());
                patient = dbContext.Patients.FirstOrDefault(x => x.Id == patientId && x.IsActive);

                if (patient == null)
                    Console.WriteLine("Er is geen patient gevonden met deze ID. Probeer opnieuw");
                else
                {
                    isPatientAvailable = CheckIfPatientIsAvailable(dbContext, patient, newAppointmentDate);
                    Console.WriteLine($"Op dit moment heeft {patient.FirstName} {patient.LastName} al een afspraak. Geef een ander tijdstip op of probeer een andere patient.");
                }
            } while (patient == null || !isPatientAvailable);

            return patient;
        }

        private static bool CheckIfPatientIsAvailable(EPDDbContext dbContext, Patient patient, DateTime date)
        {
            return dbContext.Appointments.Where(x => x.PatientId == patient.Id).ToList()
                .Where(x => x.Date == date).Count() == 0;
        }

        private static bool CheckIfPhysicianIsAvailable(EPDDbContext dbContext, Physician physician, DateTime date)
        {
            return dbContext.Appointments.Where(x => x.PhysicianId == physician.Id).ToList()
                .Where(x => x.Date == date).Count() == 0;
        }

        private static void PrintAllPatients(EPDDbContext context)
        {
            foreach (Patient patient in context.Patients)
            {
                Console.WriteLine($"ID: {patient.Id} Naam: {patient.FirstName} Achternaam: {patient.LastName}");
            }
        }

        private static void PrintAllPhysicians(EPDDbContext context)
        {
            foreach (Physician physicians in context.Physicians)
            {
                Console.WriteLine($"ID: {physicians.Id} Naam: {physicians.FirstName} Achternaam: {physicians.LastName}");
            }
        }

        private static void DeletePhysician()
        {
            Console.WriteLine("Voer de ID in van de arts die u wilt verwijderen:");

            using (var dbContext = new EPDDbContext())
            {
                int physicianId = 0;
                Physician? physician = null;

                do
                {
                    physicianId = int.Parse(Console.ReadLine());
                    physician = dbContext.Physicians.FirstOrDefault(x => x.Id == physicianId && x.IsActive);

                    if (physician == null)
                        Console.WriteLine("Er is geen arts gevonden met deze ID. Probeer opnieuw");

                } while (physician == null);

                Console.WriteLine($"Controleer de gegevens de arts die u wilt verwijderen: {physician.FirstName} {physician.LastName} with id: {physician.Id} \nDruk enter om verder te bevestigen: ");
                Console.ReadLine();

                physician.IsActive = false;

                dbContext.Physicians.Update(physician);
                dbContext.SaveChanges();
            }
        }

        private static void AddPhysician()
        {
            Physician newPhysician = new Physician();

            using (var dbContext = new EPDDbContext())
            {
                Console.WriteLine("Voer de voornaam van de arts in:");
                newPhysician.FirstName = Console.ReadLine();

                Console.WriteLine("Voer de achternaam van de arts in:");
                newPhysician.LastName = Console.ReadLine();

                Console.WriteLine("Voer de email van de arts in:");
                newPhysician.Email = Console.ReadLine();

                Console.WriteLine("Voer de geboortedatum van de arts in (YYYY-MM-DD):");
                DateTime birthDate;
                var inputBirthDate = Console.ReadLine();

                while (string.IsNullOrWhiteSpace(inputBirthDate) || !DateTime.TryParse(inputBirthDate, out birthDate))
                {
                    Console.WriteLine("Ongeldig formaat. Probeer opnieuw (YYYY-MM-DD):");
                    inputBirthDate = Console.ReadLine();
                }

                newPhysician.BirthDate = birthDate;

                Console.WriteLine("Voer het dorp of de stad van de arts in:");
                newPhysician.City = Console.ReadLine();

                Console.WriteLine("Voer de postcode van de arts in:");
                newPhysician.PostalCode = int.Parse(Console.ReadLine());

                Console.WriteLine("Voer het adres van de arts in:");
                newPhysician.Address = Console.ReadLine();

                Console.WriteLine("Voer het tefeloonnummer van de arts in:");
                newPhysician.PhoneNumber = Console.ReadLine();

                Console.WriteLine("Geef het geslacht van de arts in (M/V/X):");
                newPhysician.Gender = Console.ReadLine();

                dbContext.Add(newPhysician);
                dbContext.SaveChanges();
            }
        }

        private static void DeletePatient()
        {
            Console.WriteLine("Voer de ID in van de patiënt die u wilt verwijderen:");

            using (var dbContext = new EPDDbContext())
            {
                int patientId = 0;
                Patient? patient = null;

                do
                {
                    patientId = int.Parse(Console.ReadLine());
                    patient = dbContext.Patients.FirstOrDefault(x => x.Id == patientId && x.IsActive);

                    if (patient == null)
                        Console.WriteLine("Er is geen patient gevonden met deze ID. Probeer opnieuw");

                } while (patient == null);

                Console.WriteLine($"Controleer de gegevens de patient die u wilt verwijderen: {patient.FirstName} {patient.LastName} with id: {patient.Id} \nDruk enter om verder te bevestigen: ");
                Console.ReadLine();

                patient.IsActive = false;

                dbContext.Patients.Update(patient);
                dbContext.SaveChanges();
            }
        }


        #region FreeCodeForAssignment
        static void Main(string[] args)
        {
            while (ShowMenu())
            {
                //Continue
            }
        }

        public static bool ShowMenu()
        {
            Console.Clear();
            foreach (var line in File.ReadAllLines("logo.txt"))
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("");
            Console.WriteLine("1 - Patient toevoegen");
            Console.WriteLine("2 - Patienten verwijderen");
            Console.WriteLine("3 - Arts toevoegen");
            Console.WriteLine("4 - Arts verwijderen");
            Console.WriteLine("5 - Afspraak toevoegen");
            Console.WriteLine("6 - Afspraken inzien");
            Console.WriteLine("7 - Sluiten");
            Console.WriteLine("8 - Reset db");

            if (int.TryParse(Console.ReadLine(), out int option))
            {
                switch (option)
                {
                    case 1:
                        AddPatient();
                        return true;
                    case 2:
                        DeletePatient();
                        return true;
                    case 3:
                        AddPhysician();
                        return true;
                    case 4:
                        DeletePhysician();
                        return true;
                    case 5:
                        AddAppointment();
                        return true;
                    case 6:
                        ShowAppointment();
                        return true;
                    case 7:
                        return false;
                    case 8:
                        EPDDbContext dbContext = new EPDDbContext();
                        dbContext.Database.EnsureDeleted();
                        dbContext.Database.EnsureCreated();
                        return true;
                    default:
                        return true;
                }
            }
            return true;
        }

        #endregion
    }
}