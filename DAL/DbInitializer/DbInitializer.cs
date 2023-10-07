using DAL.AppDbContext;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Utilities.Constants.Role;

namespace DAL.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DbInitializer> _logger;

        public DbInitializer(
            UserManager<IdentityUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext dbContext,
            ILogger<DbInitializer> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _dbContext = dbContext;
            _logger = logger;
        }

        public void Initialize()
        {
            try
            {
                if(_dbContext.Database.GetPendingMigrations().Any())
                {
                    _dbContext.Database.Migrate();
                }

                if (!_roleManager.RoleExistsAsync(AppRoles.ADMIN).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(AppRoles.ADMIN)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(AppRoles.CUSTOMER)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(AppRoles.COMPANY)).GetAwaiter().GetResult();

                    _userManager.CreateAsync(new User
                    {
                        UserName = "admin@gmail.com",
                        Email = "admin@gmail.com",
                        Name = "Admin",
                        Phone = "1234567",
                        StreetAddress = "Address 1",
                        City = "Zagreb",
                        PostalCode = "10000"
                    }, "Admin123*").GetAwaiter().GetResult();

                    var adminUser = _dbContext.Users.FirstOrDefault(u => u.Email == "admin@gmail.com");
                    _userManager.AddToRoleAsync(adminUser, AppRoles.ADMIN).GetAwaiter().GetResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}
