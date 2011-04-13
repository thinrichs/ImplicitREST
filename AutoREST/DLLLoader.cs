using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace AutoREST
{
    internal class DllPreLoader
    {
        // the code in this class is from
        //http://stackoverflow.com/questions/3021613/how-to-pre-load-all-deployed-assemblies-for-an-appdomain
        // Stack Overflow rocks hardest of all things
        private static IEnumerable<string> GetBinFolders()
        {
            var directory = HttpContext.Current != null
                                ? HttpRuntime.BinDirectory
                                : AppDomain.CurrentDomain.BaseDirectory;
            //TODO: The AppDomain.CurrentDomain.BaseDirectory usage is not correct in 
            //some cases. Need to consider PrivateBinPath too
            var toReturn = new List<string> {directory};
            //slightly dirty - needs reference to System.Web.  Could always do it really
            //nasty instead and bind the property by reflection!
            return toReturn;
        }

        internal static void PreLoadDeployedAssemblies()
        {
            // which is better? Brevity vs Clarity - This one liner is equivalent GetBinFolders().ToList().ForEach(PreLoadAssembliesFromPath);
            var binFolders = GetBinFolders();
            foreach (var path in binFolders)
            {
                PreLoadAssembliesFromPath(path);
            }
        }

        private static void PreLoadAssembliesFromPath(string p)
        {
            //Props to Andras Zoltan http://stackoverflow.com/users/157701/andras-zoltan
            // The below is from question http://stackoverflow.com/questions/3021613/how-to-pre-load-all-deployed-assemblies-for-an-appdomain
            //get all .dll files from the specified path and load the lot
            var files = new DirectoryInfo(p)
                .GetFiles("*.dll", SearchOption.AllDirectories);
            //you might not want recursion - handy for localised assemblies 
            //though especially.
            AssemblyName a = null;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var s in files.Select(fi => fi.FullName))
            {
                //now get the name of the assembly you've found, without loading it
                //though (assuming .Net 2+ of course).
                a = AssemblyName.GetAssemblyName(s);
                //sanity check - make sure we don't already have an assembly loaded
                //that, if this assembly name was passed to the loaded, would actually
                //be resolved as that assembly.  Might be unnecessary - but makes me
                //happy :)
                if (assemblies.Any(assembly => AssemblyName.ReferenceMatchesDefinition(a, assembly.GetName()))) continue;
                //crucial - USE THE ASSEMBLY NAME.
                //in a web app, this assembly will automatically be bound from the 
                //Asp.Net Temporary folder from where the site actually runs.
                Assembly.Load(a);
            }
        }
    }
}