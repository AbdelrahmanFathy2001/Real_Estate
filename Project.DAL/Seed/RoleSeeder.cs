using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Entities.Identity;

namespace Project.DAL.Seed
{
    public static class RoleSeeder
    {

        public async static Task RoleSeeding(RoleManager<Role> roleManager)
        {
            var RolesCount = await roleManager.Roles.CountAsync();
            if (RolesCount <= 0)
            {


                await roleManager.CreateAsync(new Role()
                {
                    Name = "Admin"
                });

                await roleManager.CreateAsync(new Role()
                {
                    Name = "User"
                });
            }
        }

    }
}
