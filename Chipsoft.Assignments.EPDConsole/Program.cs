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
        }

        private static void AddAppointment()
        {
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