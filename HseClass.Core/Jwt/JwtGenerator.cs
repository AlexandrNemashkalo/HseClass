using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HseClass.Core.EF;
using HseClass.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HseClass.Core.Jwt
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly HseClassContext _data;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public JwtGenerator(
            HseClassContext data,
            IConfiguration conf,
            UserManager<User> userManager)
        {
            _data = data;
            _configuration = conf;
            _userManager = userManager;
        }

        public async Task<object> GenerateJwt(User user)
        {
            try
            {
                var roles = await _userManager.GetRolesAsync(user);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, "Token");
                
                claimsIdentity.AddClaims(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expires = DateTime.Now.AddMinutes(Convert.ToDouble(600));
                //var dateTimeOffset = new DateTimeOffset(expires);

                var token = new JwtSecurityToken(
                    _configuration["Issuer"],
                    _configuration["Audience"],
                    claimsIdentity.Claims,
                    signingCredentials: creds,
                    expires:expires
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}