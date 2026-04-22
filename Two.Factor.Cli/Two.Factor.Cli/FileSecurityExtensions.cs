using System.Runtime.Versioning;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Two.Factor.Cli;

[SupportedOSPlatform("windows")]
public static class FileSecurityExtensions
{
    extension(FileSecurity)
    {
        public static FileSecurity CurrentUserAccess
        {
            get
            {
                var currentUser = WindowsIdentity.GetCurrent().User;

                if (currentUser == null)
                    throw new InvalidOperationException("Cannot get current user SID");

                var security = new FileSecurity();
                security.SetAccessRuleProtection(isProtected: true, preserveInheritance: false);
                security.AddAccessRule(
                    new FileSystemAccessRule(
                        currentUser,
                        FileSystemRights.FullControl,
                        AccessControlType.Allow));

                return security;
            }
        }
    }
}