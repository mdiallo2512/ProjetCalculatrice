namespace CalculatorDemo
{
    public class Application
    {
        private readonly ICalculatorService _calculatorService; 

        public Application(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        public void Run()
        {
            string userInput;

            do
            {
                AfficherMenu();

                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case "1":

                        Console.WriteLine("\nSVP entrez une expression mathématique: ");

                        string expression = Console.ReadLine();

                        try
                        {

                            double result = _calculatorService.EvaluateExpression(expression);

                            Console.WriteLine($"Le résultat est: {result} ");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        Console.WriteLine("\n");

                        break;

                    case "2":
                        Console.WriteLine("\nMerci d'avoir utilisé la calculatrice.\n");
                        break;

                    default:
                        Console.WriteLine("\nChoix invalide, veuillez réessayer.\n");
                        break;
                }

            } while (userInput != null && userInput != "2");

            Console.ReadKey();

            void AfficherMenu()
            {
                Console.WriteLine("========================================");
                Console.WriteLine("              CALCULATRICE              ");
                Console.WriteLine("========================================\n");
                Console.WriteLine("1. Effectuer un test");
                Console.WriteLine("2. Quitter");
                Console.WriteLine("----------------------------------------\n");
                Console.Write("Faites votre choix : ");
            }
        }
    }
}
