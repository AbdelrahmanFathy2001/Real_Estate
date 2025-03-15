using Microsoft.Extensions.Logging;
using Project.DAL.Entities;
using System.Text.Json;

namespace Project.DAL.Data
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(ApplicationDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
               

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<ApplicationDbContextSeed>();
                logger.LogError(ex, ex.Message);
            }

        }
    }
}
