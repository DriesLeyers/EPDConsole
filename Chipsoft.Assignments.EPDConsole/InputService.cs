namespace Chipsoft.Assignments.EPDConsole
{
    public class InputService
    {
        public string GetStringInput(string textToDisplay)
        {
            Console.WriteLine(textToDisplay);
            var inputReceived = Console.ReadLine();

            if (string.IsNullOrEmpty(inputReceived))
            {
                Console.WriteLine("Niet ingegeven, probeer opnieuw: ");
                return GetStringInput(textToDisplay);
            }

            return inputReceived;
        }

        public DateTime GetDateInput(string textToDisplay)
        {
            Console.WriteLine(textToDisplay);

            DateTime birthDate;
            var inputBirthDate = Console.ReadLine();

            if (string.IsNullOrEmpty(inputBirthDate) || !DateTime.TryParse(inputBirthDate, out birthDate))
            {
                Console.WriteLine("Ongeldig formaat. Probeer opnieuw (YYYY-MM-DD)");
                return GetDateInput(textToDisplay);
            }

            return birthDate;
        }

        public TimeSpan GetTimeInput(string textToDisplay)
        {
            Console.WriteLine(textToDisplay);

            TimeSpan appointmentTime;
            var inputTime = Console.ReadLine();

            if (string.IsNullOrEmpty(inputTime) || !TimeSpan.TryParse(inputTime, out appointmentTime))
            {
                Console.WriteLine("Ongeldig formaat. Probeer opnieuw (HH:MM)");
                return GetTimeInput(textToDisplay);
            }

            return appointmentTime;
        }

        public int GetNumberInput(string? textToDisplay)
        {
            Console.WriteLine(textToDisplay);
            var inputReceived = Console.ReadLine();

            int parsedNumber;
            if (!int.TryParse(inputReceived, out parsedNumber))
            {
                Console.WriteLine("Niet ingegeven, probeer opnieuw: ");
                return GetNumberInput(textToDisplay);
            }

            return parsedNumber;
        }

    }
}
