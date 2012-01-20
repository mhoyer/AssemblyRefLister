using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Pixelplastic
{
    public class AssemblyRefLister
    {
        readonly bool _includeGAC;
        int _indention;
        readonly IList<string> _touchedRefs;

        public Action<string> PrintAction { get; private set; }

        public AssemblyRefLister(bool includeGAC = true, Action<string> printAction = null)
        {
            _includeGAC = includeGAC;
            _touchedRefs = new List<string>();
            PrintAction = printAction ?? IndentedPrint;
        }

        void IndentedPrint(string value)
        {
            for(var i=0; i<_indention;i++) Console.Write(" ");

            Console.WriteLine(value);
        }

        public void PrintAssemblyNameAndReferences(Assembly assembly)
        {
            if (!_includeGAC && assembly.GlobalAssemblyCache) return;

            _touchedRefs.Add(assembly.FullName);
            PrintAction(assembly.FullName);

            _indention++;
            foreach (var referencedAssembly in assembly.GetReferencedAssemblies())
            {
                if (_touchedRefs.Contains(referencedAssembly.FullName)) continue;

                PrintAssemblyNameAndReferences(referencedAssembly);
            }
            _indention--;
        }

        public void PrintAssemblyNameAndReferences(AssemblyName fullName)
        {
            try
            {
                PrintAssemblyNameAndReferences(Assembly.Load(fullName));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void PrintAssemblyNameAndReferences(string fullName)
        {
            if (!File.Exists(fullName))
            {
                PrintAssemblyNameAndReferences(new AssemblyName(fullName));
                return;
            }

            fullName = Path.GetFullPath(fullName);

            AppDomain.CurrentDomain.AssemblyResolve +=
                (s, a) =>
                    {
                        var folderPath = Path.GetDirectoryName(fullName);
                        var assemblyName = new AssemblyName(a.Name);
                        var assemblyPath = Path.Combine(folderPath, assemblyName.Name + ".dll");

                        if (File.Exists(assemblyPath)) return Assembly.LoadFrom(assemblyPath);

                        return null;
                    };

            try
            {
                PrintAssemblyNameAndReferences(Assembly.LoadFrom(fullName));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}