// See https://aka.ms/new-console-template for more information

using ProjetCalculatrice;

var calculator = new Calculator();

string choix;

do
{
    AfficherMenu();

    choix = Console.ReadLine();

    switch (choix)
    {
        case "1":

            Console.WriteLine("\nSVP entrez une expression mathématique: ");

            string expression = Console.ReadLine();

            try
            {

                double result = calculator.EvaluateExpression(expression);

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

} while (choix != null && choix != "2");

Console.ReadKey();

void AfficherMenu()
{    
    Console.WriteLine("========================================");
    Console.WriteLine("              CALCULATRICE              ");
    Console.WriteLine("========================================\n");
    Console.WriteLine("1. Éffectuer un test");
    Console.WriteLine("2. Quitter");
    Console.WriteLine("----------------------------------------\n");
    Console.Write("Faites votre choix : ");    
}