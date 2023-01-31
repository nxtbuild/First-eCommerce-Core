using FirstCoreProject.CommonHelper;
using FirstCoreProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FirstCoreProject.DAL.DbInitalizer
{
    public class DbInitalizer : IDbInitalizer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public DbInitalizer(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public void Initalizer()
        {
            try
            {
                if(_context.Database.GetPendingMigrations().Count()>0)
                {
                    _context.Database.Migrate();

                }

    }
            catch (Exception)
            {

                throw;
            }

            // Roles

            if (!_roleManager.RoleExistsAsync(WebRole.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(WebRole.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebRole.Role_User)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(WebRole.Role_Employee)).GetAwaiter().GetResult();
            

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName="admin@gmail.com",
                Email="admin@gmail.com",
                Name="Admin",
                PhoneNumber="12345679",
                Address="XYZ",
                City="GKP",
                State="UP",
                PinCode="273001"


            },"Admin@123").GetAwaiter().GetResult();


            ApplicationUser user = _context.ApplicationUsers.FirstOrDefault(x => x.Email == "admin@gmail.com");
            _userManager.AddToRoleAsync(user, WebRole.Role_Admin).GetAwaiter().GetResult();

        }

            return;
}
    }
}
