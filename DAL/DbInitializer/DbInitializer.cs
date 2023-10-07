using DAL.AppDbContext;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task Initialize()
        {
            try
            {
                if(_dbContext.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Count() > 0)
                {
                    _dbContext.Database.Migrate();
                }

                if (!_roleManager.RoleExistsAsync(AppRoles.ADMIN).GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole(AppRoles.ADMIN));
                    await _roleManager.CreateAsync(new IdentityRole(AppRoles.CUSTOMER));
                    await _roleManager.CreateAsync(new IdentityRole(AppRoles.COMPANY));

                    await _userManager.CreateAsync(new User
                    {
                        Email = "admin@gmail.com",
                        Name = "Admin",
                        Phone = "1234567",
                        StreetAddress = "Address 1",
                        City = "Zagreb",
                        PostalCode = "10000"
                    }, "Admin123*");

                    var adminUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == "admin@gmail.com");
                    await _userManager.AddToRoleAsync(adminUser, AppRoles.ADMIN);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + "\n" + ex.StackTrace);
            }
        }
    }
}
