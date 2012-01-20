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
            Console.WriteLine("\nAssemblyRefLister -- lists the reference of a given assembly\n");
            Console.WriteLine("    Usage: \n\t{0}.exe <NameOfAssembly|PathToAssembly> (Options)\n", Assembly.GetExecutingAssembly().GetName().Name);
            Console.WriteLine("    Options:  \n\t--no-gac          # will exclude assemblies from GAC");
        }
    }
}
