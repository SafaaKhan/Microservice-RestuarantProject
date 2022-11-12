﻿using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Mango.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Mango.Services.Identity.Services
{
    public class ProfileService : IProfileService //add claims to the token
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;

        public ProfileService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> role,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory)
        {
            _userManager = userManager;
            _roleManager = role;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            string sub =  context.Subject.GetSubjectId(); //sub: contains the client id
            ApplicationUser user = await _userManager.FindByIdAsync(sub);
            ClaimsPrincipal userClaims=await _userClaimsPrincipalFactory.CreateAsync(user);

            List<Claim> claims=userClaims.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();
            claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
            claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));

            if (_userManager.SupportsUserRole)
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);
                foreach(var rolename in roles)
                {
                    claims.Add(new Claim(JwtClaimTypes.Role, rolename));
                    //does not exists in this project
                    if (_roleManager.SupportsRoleClaims)
                    {
                        IdentityRole role = await _roleManager.FindByNameAsync(rolename);
                        if (role != null)
                        {
                            claims.AddRange(await _roleManager.GetClaimsAsync(role));
                        }
                    }
                }
            }

            context.IssuedClaims = claims;
        }

        public async Task  IsActiveAsync(IsActiveContext context)
        {
            string sub = context.Subject.GetSubjectId(); //sub: contains the client id
            ApplicationUser user = await _userManager.FindByIdAsync(sub);
            context.IsActive=user!= null;
        }
    }
}
