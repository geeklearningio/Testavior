namespace System.Security.Claims
{
    using System.Linq;

    public static class ClaimsIdentityHelper
    {
        public static void ReplaceClaim(this ClaimsIdentity identity, string type, string value)
        {
            identity.TryRemoveClaim(identity.Claims.FirstOrDefault(c => c.Type == type));
            identity.AddClaim(new Claim(type, value));
        }

        public static void ReplaceNameIdentifierClaim(this ClaimsIdentity identity, string value)
        {
            identity.ReplaceClaim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", value);
        }
    }
}
