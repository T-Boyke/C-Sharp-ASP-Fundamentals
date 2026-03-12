using System;
using System.Threading.Tasks;
using _10_Filmdatenbank.Web.Utilities;

namespace _10_Filmdatenbank.Web;

public class Program
{
    public static async Task Main(string[] args)
    {
        var tool = new TmdbEnrichmentTool();
        var data = await tool.GetMovieDataAsync("Inception", 2010);
        
        if (data != null)
        {
            Console.WriteLine("--- FETCHED DATA ---");
            Console.WriteLine(data);
        }
        else
        {
            Console.WriteLine("Failed to fetch data.");
        }
    }
}
