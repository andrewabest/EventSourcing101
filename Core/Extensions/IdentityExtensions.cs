using System;
using System.Security.Principal;

namespace Core.Extensions
{
    public static class IdentityExtensions
    {
        public static Guid? Id(this IIdentity identity)
        {
            return !string.IsNullOrEmpty(identity.Name) ? new Guid(identity.Name) : (Guid?)null;
        }
    }
}