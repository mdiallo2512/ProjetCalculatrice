using Microsoft.Extensions.DependencyInjection;

namespace CalculatorDemo;

public class Program
{
    static void Main(string[] args)
    {

        var serviceCollection = new ServiceCollection();
        ConfigureService(serviceCollection);

        var serviceProvider = serviceCollection.BuildServiceProvider(); 
        var app = serviceProvider.GetService<Application>();

        if(app == null)
        {
            Console.WriteLine("\nImpossible de démarrer l'application.");
            return;
        }

        app.Run();        
    }

    private static void ConfigureService(IServiceCollection serviceCollections)
    {
        serviceCollections.AddScoped<IMathParser, MathParser>();
        serviceCollections.AddScoped<ICalculatorService, CalculatorService>();
        serviceCollections.AddSingleton<Application>();
    }
}