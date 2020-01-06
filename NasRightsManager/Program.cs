using System;
using System.Collections;
using System.IO;
using System.Security.Principal;

namespace NasRightsManager
{
    class Program
    {
        public static string[] Directories => new[] { "Animation", "Films", "Series" };
        // CF : https://stackoverflow.com/questions/40449973/how-to-modify-file-access-control-in-net-core
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            foreach (var directory in Directories)
            {
                Console.WriteLine($"Inspecting directory {directory}");
                foreach (var file in Directory.GetFiles($@"Z:\{directory}","*.*",SearchOption.AllDirectories))
                {
                    var descriptor=new FileInfo(Path.GetFullPath(file));
                    var accessControl = descriptor.GetAccessControl();
                    var authorization = accessControl.GetAccessRules(true,true, typeof(NTAccount));
                }
            }
        }

    }
}
