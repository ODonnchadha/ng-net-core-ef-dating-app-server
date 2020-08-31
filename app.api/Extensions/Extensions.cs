using Microsoft.AspNetCore.Http;
using System;

namespace app.api.Extensions
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);

            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static int Age(this DateTime birth)
        {
            var age = DateTime.Today.Year - birth.Year;
            if (birth.AddYears(age) > DateTime.Today)
            {
                age--;
            }

            return age;
        }
    }
}
