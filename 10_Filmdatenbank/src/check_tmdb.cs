using System;
using System.Net.Http;
using TMDbLib.Client;
using System.Reflection;

public class Program
{
    public static void Main()
    {
        var type = typeof(TMDbClient);
        Console.WriteLine($"Constructors for {type.Name}:");
        foreach (var ctor in type.GetConstructors())
        {
            Console.Write(" - ");
            foreach (var param in ctor.GetParameters())
            {
                Console.Write($"{param.ParameterType.Name} {param.Name}, ");
            }
            Console.WriteLine();
        }
    }
}
