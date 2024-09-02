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

                Console.WriteLine("Enter the patient's first name:");
                newPatient.FirstName = Console.ReadLine();

                Console.WriteLine("Enter the patient's last name:");
                newPatient.LastName = Console.ReadLine();

                Console.WriteLine("Enter the patient's email:");
                newPatient.Email = Console.ReadLine();

                Console.WriteLine("Enter the patient's Birthdate (YYYY-MM-DD):");
                DateTime birthDate;
                var inputBirthDate = Console.ReadLine();

                while (string.IsNullOrWhiteSpace(inputBirthDate) || !DateTime.TryParse(inputBirthDate, out birthDate))
                {
                    Console.WriteLine("Invalid date format. Please enter a valid date (YYYY-MM-DD):");
                    inputBirthDate = Console.ReadLine();
                }

                newPatient.BirthDate = birthDate;

                Console.WriteLine("Enter the patient's city:");
                newPatient.City = Console.ReadLine();

                Console.WriteLine("Enter the patient's PostalCode:");
                newPatient.PostalCode = int.Parse(Console.ReadLine());

                Console.WriteLine("Enter the patient's address:");
                newPatient.Address = Console.ReadLine();

                Console.WriteLine("Enter the patient's national register number:");
                newPatient.NationalRegisterNumber = Console.ReadLine();

                Console.WriteLine("Enter the patient's phone numer:");
                newPatient.PhoneNumber = Console.ReadLine();

                Console.WriteLine("Enter the patient's gender (M/W/X)");
                newPatient.Gender = Console.ReadLine();

                dbContext.Add(newPatient);
                dbContext.SaveChanges();
            }

            //return to show menu again.
        }

        private static void ShowAppointment()
        {
        }

        private static void AddAppointment()
        {
        }

        private static void DeletePhysician()
        {
        }

        private static void AddPhysician()
        {
        }

        private static void DeletePatient()
        {
            Console.WriteLine("Enter the id of the patient you want to delete:");

            using (var dbContext = new EPDDbContext())
            {
                int patientId = 0;
                Patient? patient = null;

                do
                {
                    patientId = int.Parse(Console.ReadLine());
                    patient = dbContext.Patients.FirstOrDefault(x => x.Id == patientId);

                    if (patient == null)
                        Console.WriteLine("No patient found with this id. Please try again.");

                } while (patient == null);

                Console.WriteLine($"Please check if you want to delete this user: {patient.FirstName} {patient.LastName} with id: {patient.Id} \n Press enter to continue: ");
                Console.ReadLine();

                dbContext.Patients.Remove(patient);
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