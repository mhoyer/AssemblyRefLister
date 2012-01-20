using System;
using System.Reflection;

namespace Pixelplastic
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                PrintUsage();
                return;
            }

            var ignoreGAC = args.Length == 2 && args[1].ToLower() == "--no-gac";

            new AssemblyRefLister(ignoreGAC).PrintAssemblyNameAndReferences(args[0]);
        }

        static void PrintUsage()
        {
            Console.WriteLine("Usage: {0}.exe <NameOfAssembly>     # without .dll or .exe extension", Assembly.GetExecutingAssembly().GetName().Name);
            Console.WriteLine("       --no-gac   # will skip assemblies from GAC");
        }
    }
}
