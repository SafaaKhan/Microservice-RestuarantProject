using IdentityModel;
using Mango.Services.Identity.Data;
using Mango.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Mango.Services.Identity.Initializer
{

    //data seed
    public class DbInitializer : IDbInittializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManage;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManage,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManage = userManage;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            //create the roles
            if (_roleManager.FindByNameAsync(SD.Admin).Result == null)
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
            }
            else
            {
                return;
            }

            //create the users
            //admin
            ApplicationUser adminUser = new ApplicationUser()
            {
                UserName="adminUser1@gmail.com",
                Email="adminUser1@gmail.com",
                EmailConfirmed=true,
                PhoneNumber="11111111",
                FirstName="Safaa",
                LastName="Admin"
            };

            _userManage.CreateAsync(adminUser,"Admin123*").GetAwaiter().GetResult();
            _userManage.AddToRoleAsync(adminUser,SD.Admin).GetAwaiter().GetResult();

            var temp1 = _userManage.AddClaimsAsync(adminUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name,adminUser.FirstName+" "+adminUser.LastName),
                new Claim(JwtClaimTypes.GivenName,adminUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName,adminUser.LastName),
                new Claim(JwtClaimTypes.Role,SD.Admin),
            }).Result;

            // customer
            ApplicationUser customerUser = new ApplicationUser()
            {
                UserName = "customerUser1@gmail.com",
                Email = "customerUser1@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "11111111",
                FirstName = "Safaa",
                LastName = "Cust"
            };

            _userManage.CreateAsync(customerUser, "UserC123*").GetAwaiter().GetResult();
            _userManage.AddToRoleAsync(customerUser, SD.Customer).GetAwaiter().GetResult();

            var temp2 = _userManage.AddClaimsAsync(customerUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name,customerUser.FirstName+" "+customerUser.LastName),
                new Claim(JwtClaimTypes.GivenName,customerUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName,customerUser.LastName),
                new Claim(JwtClaimTypes.Role,SD.Customer),
            }).Result;
        }
    }
}
