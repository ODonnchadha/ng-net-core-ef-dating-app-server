using app.api.Entities;
using app.api.Helpers.Auth;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace app.api.Context
{
    public class Seed
    {
        public static void SeedUserData(DataContext context)
        {
            if (!context.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Context/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                foreach(var user in users)
                {
                    byte[] passwordHash, passwordSalt;
                    PasswordHash.Create("password", out passwordHash, out passwordSalt);

                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.Username = user.Username.ToLower();

                    context.Users.Add(user);
                }

                context.SaveChanges();
            }
        }
    }
}
