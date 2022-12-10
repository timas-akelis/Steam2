using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Steam2.Views
{
    public static class MyExtensions
    {
        public static string GenerateRandomString(this HtmlHelper htmlHelper)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 32)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
