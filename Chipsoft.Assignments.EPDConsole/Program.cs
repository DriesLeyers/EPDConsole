using Chipsoft.Assignments.EPDConsole.Entity;
using Chipsoft.Assignments.EPDConsole.Services;

namespace Chipsoft.Assignments.EPDConsole
{
    public class Program
    {
        static InputService inputService = new InputService();
        static DataService dataService = new DataService();

        private static void AddPatient()
        {
            Patient newPatient = new Patient();

            newPatient.FirstName = inputService.GetStringInput("Voer de voornaam van de patient in:");
            newPatient.LastName = inputService.GetStringInput("Voer de achternaam van de patient in:");
            newPatient.Email = inputService.GetStringInput("Voer de email van de patient in:");
            newPatient.BirthDate = inputService.GetDateInput("Voer de geboortedatum van de patient in: (YYYY-MM-DD):");
            newPatient.City = inputService.GetStringInput("Voer het dorp of de stad van de patient in:");
            newPatient.PostalCode = inputService.GetNumberInput("Voer de postcode van het dorp / de stad in:");
            newPatient.Address = inputService.GetStringInput("Voer het adres van de patient in:");
            newPatient.NationalRegisterNumber = inputService.GetStringInput("Voer het rijksregisternummer van de patient in:");
            newPatient.PhoneNumber = inputService.GetStringInput("Voer het telefoonnummer van de patient in:");
            newPatient.Gender = inputService.GetStringInput("Geef het geslacht van de patient in: (M/V/X)");

            dataService.AddPatient(newPatient);
        }

        private static void ShowAppointment()
        {
            Console.WriteLine("1 - Alle afspraken");
            Console.WriteLine("2 - Alle afspraken zoeken voor patient");
            Console.WriteLine("3 - Alle afspraken zoeken voor arts");

            if (int.TryParse(Console.ReadLine(), out int option))
            {
                switch (option)
                {
                    case 1:
                        ShowAppointments(dataService.GetAllAppointments());
                        Console.WriteLine("Druk op enter om verder te gaan");
                        Console.ReadLine();

                        break;
                    case 2:
                        int patientId = inputService.GetNumberInput("Vul de ID in van de patient waarvan u alle afspraken wilt zien:");
                        ShowAppointments(dataService.GetAllAppointmentsByPatientId(patientId));

                        Console.WriteLine("Druk op enter om verder te gaan");
                        Console.ReadLine();

                        break;
                    case 3:
                        int physicianId = inputService.GetNumberInput("Vul de ID in van de arts waarvan u alle afspraken wilt zien:");
                        ShowAppointments(dataService.GetAllAppointmentsByPhysicianId(physicianId));

                        Console.WriteLine("Druk op enter om verder te gaan");
                        Console.ReadLine();

                        break;
                    default:
                        break;
                }
            }
        }

        private static void ShowAppointments(ICollection<Appointment> appointments)
        {
            foreach (Appointment appointment in appointments)
            {
                Console.WriteLine($"Afspraak met ID: {appointment.Id} tussen {appointment.Patient.FirstName} {appointment.Patient.LastName} en {appointment.Physician.FirstName} {appointment.Physician.LastName} op {appointment.Date}");
            }
        }

        private static void AddAppointment()
        {
            Appointment newAppointment = new Appointment();

            var appointmentDate = inputService.GetDateInput("Voor welke datum is de afspraak? (YYYY-MM-DD):");
            var appointmentTime = inputService.GetTimeInput("Hoelaat wilt u de afspraak? (HH:MM):");

            newAppointment.Date = appointmentDate.Date + appointmentTime;

            Console.WriteLine("Voor welke patient wilt u de afspraak inplannen? Geef de ID:");
            dataService.PrintAllPatients();

            newAppointment.Patient = PickPatientForAppointment(newAppointment.Date);

            Console.WriteLine("Voor welke arts wilt u de afspraak inplannen? Geef de ID:");
            dataService.PrintAllPhysicians();

            newAppointment.Physician = PickPhysicianForAppointment(newAppointment.Date);

            dataService.AddAppointment(newAppointment);
        }

        private static Physician PickPhysicianForAppointment(DateTime newAppointmentDate)
        {
            bool isPhysicianAvailable = true;

            Physician? physician;
            do
            {
                var physicianId = inputService.GetNumberInput("");
                physician = dataService.GetPhysicianById(physicianId);

                if (physician == null)
                    Console.WriteLine("Er is geen physician gevonden met deze ID. Probeer opnieuw");
                else
                {
                    isPhysicianAvailable = dataService.CheckIfPhysicianIsAvailable(physician, newAppointmentDate);
                    if (!isPhysicianAvailable)
                        Console.WriteLine($"Op dit moment heeft {physician.FirstName} {physician.LastName} al een afspraak. Geef een ander tijdstip op of probeer een andere patient.");
                }

            } while (physician == null || !isPhysicianAvailable);

            return physician;
        }

        private static Patient PickPatientForAppointment(DateTime newAppointmentDate)
        {
            bool isPatientAvailable = true;

            Patient? patient;
            do
            {
                var patientId = inputService.GetNumberInput("");
                patient = dataService.GetPatientById(patientId);

                if (patient == null)
                    Console.WriteLine("Er is geen patient gevonden met deze ID. Probeer opnieuw");
                else
                {
                    isPatientAvailable = dataService.CheckIfPatientIsAvailable(patient, newAppointmentDate);
                    if (!isPatientAvailable)
                        Console.WriteLine($"Op dit moment heeft {patient.FirstName} {patient.LastName} al een afspraak. Geef een ander tijdstip op of probeer een andere patient.");
                }
            } while (patient == null || !isPatientAvailable);

            return patient;
        }

        private static void DeletePhysician()
        {
            Physician? physician;

            do
            {
                var physicianId = inputService.GetNumberInput("Voer de ID in van de arts die u wilt verwijderen:");
                physician = dataService.GetPhysicianById(physicianId);

                if (physician == null)
                    Console.WriteLine("Er is geen arts gevonden met deze ID. Probeer opnieuw");

            } while (physician == null);

            Console.WriteLine($"Controleer de gegevens de arts die u wilt verwijderen: {physician.FirstName} {physician.LastName} with id: {physician.Id} \nDruk enter om verder te bevestigen: ");
            Console.ReadLine();

            dataService.DeletePhysician(physician);
        }

        private static void AddPhysician()
        {
            Physician newPhysician = new Physician();

            newPhysician.FirstName = inputService.GetStringInput("Voer de voornaam van de arts in:");
            newPhysician.LastName = inputService.GetStringInput("Voer de achternaam van de arts in:");

            dataService.AddPhysician(newPhysician);
        }

        private static void DeletePatient()
        {
            Patient? patient;

            do
            {
                var patientId = inputService.GetNumberInput("Voer de ID in van de patiënt die u wilt verwijderen:");
                patient = dataService.GetPatientById(patientId);

                if (patient == null)
                    Console.WriteLine("Er is geen patient gevonden met deze ID. Probeer opnieuw");

            } while (patient == null);

            Console.WriteLine($"Controleer de gegevens de patient die u wilt verwijderen: {patient.FirstName} {patient.LastName} with id: {patient.Id} \nDruk enter om verder te bevestigen: ");
            Console.ReadLine();

            dataService.DeletePatient(patient);
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
                if (option == 7)
                    return false;

                switch (option)
                {
                    case 1:
                        AddPatient();
                        break;
                    case 2:
                        DeletePatient();
                        break;
                    case 3:
                        AddPhysician();
                        break;
                    case 4:
                        DeletePhysician();
                        break;
                    case 5:
                        AddAppointment();
                        break;
                    case 6:
                        ShowAppointment();
                        break;
                    case 8:
                        EPDDbContext dbContext = new EPDDbContext();
                        dbContext.Database.EnsureDeleted();
                        dbContext.Database.EnsureCreated();
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        #endregion
    }
}