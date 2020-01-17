using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;

namespace NasRightsManager
{
    class Program
    {
        public static string[] Directories => new[] { "test" };
        //public static string[] Directories => new[] { "Animation", "Films", "Series" };

        private static readonly SecurityIdentifier Everyone = new SecurityIdentifier(WellKnownSidType.WorldSid, null);
        // CF : https://stackoverflow.com/questions/40449973/how-to-modify-file-access-control-in-net-core
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var  account = (NTAccount)Everyone.Translate(typeof(NTAccount));

            foreach (var directory in Directories)
            {
                Console.WriteLine($"Inspecting directory {directory}");

                foreach (var file in Directory.GetFiles($@"Z:\{directory}", "*.*", SearchOption.AllDirectories))
                {
                    var fullPath = Path.GetFullPath(file);

                    var descriptor = new FileInfo(fullPath);
                    var accessControl = descriptor.GetAccessControl();
                    var authorization = accessControl.GetAccessRules(true, true, typeof(NTAccount));
                    if (!CheckRights(authorization))
                    {
                        Console.WriteLine($"Patching file {fullPath}");
                        //accessControl.AddAccessRule(new FileSystemAccessRule(Everyone, FileSystemRights.FullControl, AccessControlType.Allow));
                        //descriptor.SetAccessControl(accessControl);
                    }

                    //authorization.AddRule(
                }
            }
        }

        private static bool CheckRights(AuthorizationRuleCollection authorization)
        {
            return authorization.Cast<AccessRule>()
                .Where(r => r is FileSystemAccessRule)
                .Cast<FileSystemAccessRule>()
                .Any(r => r.IdentityReference.Value == Everyone.Value && r.FileSystemRights == FileSystemRights.FullControl);
        }
    }
}
