using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Steam2.Controllers
{
    public class Extension : Controller
    {
        public static string CreateId()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[32];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }

        //public string GetId()
        //{
        //    var claimsIdentity = User.Identity as ClaimsIdentity;
        //    if (claimsIdentity != null)
        //    {
        //        var userIdClaim = claimsIdentity.Claims
        //            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

        //        if (userIdClaim != null)
        //        {
        //            return userIdClaim.Value;
        //        }
        //    }

        //    return string.Empty;
        //}
    }
}
