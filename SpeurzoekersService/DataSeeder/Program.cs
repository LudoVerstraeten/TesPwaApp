using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using Speurzoekers.Data;
using Speurzoekers.Data.Entities.Identity;


namespace DataSeeder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting to seed");
            var connectionString = GetConnectionString();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddDbContext<SpeurzoekersDbContext>(options =>
                options.UseSqlServer(connectionString)
            );
            serviceCollection.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<SpeurzoekersDbContext>()
                .AddDefaultTokenProviders();

            var provider = serviceCollection.BuildServiceProvider();
            var authenticationDbContext = provider.GetService<SpeurzoekersDbContext>();

            authenticationDbContext.Database.EnsureDeleted();
            authenticationDbContext.Database.EnsureCreated();

            var userManager = provider.GetService<UserManager<ApplicationUser>>();

            var roleManager = provider.GetService<RoleManager<ApplicationRole>>();

            CreateAuthentication(roleManager, userManager).Wait();
            authenticationDbContext.SaveChanges();
            authenticationDbContext.Dispose();
            Console.WriteLine("Done seeding data. Press any key to continue.");
            Console.ReadKey();
        }

        private static async Task CreateAuthentication(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            await roleManager.CreateAsync(new ApplicationRole { Name = "Admin", NormalizedName = "Administrator" });

            Console.WriteLine("Removed and migrated database");

            await CreateUser(userManager, "admin@test.com", "Test@123", true);
            await CreateUser(userManager, "test@test.com", "Test@123");
        }

        private static async Task CreateUser(UserManager<ApplicationUser> userManager, string userName, string password, bool isAdmin = false)
        {
            var user = new ApplicationUser
            {

                UserName = userName,
                Email = userName
            };
            await userManager.CreateAsync(user, password);

            if (isAdmin)
            {
                user = await userManager.Users.FirstAsync(u => u.Email == userName);
                await userManager.AddToRoleAsync(user, "Admin");
            }
        }

        private static string GetConnectionString()
        {
            var builder = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    return configuration.GetSection("DatabaseConnectionStringWin").Value;
                case PlatformID.Unix:
                    return configuration.GetSection("DatabaseConnectionStringUnix").Value;
                default:
                    throw new Exception("Can't configurate the DB, OS not recognized.");
            }
        }
    }
}
