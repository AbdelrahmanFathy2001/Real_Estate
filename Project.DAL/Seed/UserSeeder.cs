using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Entities.Identity;

namespace Project.DAL.Seed
{
    public static class UserSeeder
    {

        public async static Task UserSeeding(UserManager<ApplicationUser> userManager)
        {

            var users = await userManager.Users.CountAsync();
            if (users <= 0)
            {
                var user = new ApplicationUser()
                {
                    UserName = "admin",
                    FirstName ="Abdelarhman",
                    LastName = "Fathy",
                    Email = "abdalrhmanfathy2001@gmail.com",
                    City = "Beni Suef",
                    Country = "Egypt",
                    PhoneNumber = "01143210112",
                    Street = "Beba",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true
                };
                await userManager.CreateAsync(user, "P@ssw0rd");
                await userManager.AddToRoleAsync(user, "Admin");


            }


        }
    }
}
