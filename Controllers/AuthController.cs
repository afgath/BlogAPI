using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using zmgTestBack.Filters;
using zmgTestBack.Helpers;
using zmgTestBack.Models;
using zmgTestBack.Services;

namespace zmgTestBack.Controllers
{
    [Route("[controller]")]
    [TypeFilter(typeof(BlogExceptionFilter))]
    public class AuthController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Login(UserRequest model)
        {
            var user = _userService.LogIn(model);

            var secretKey = _configuration.GetValue<string>("SecretKey");
            var key = Encoding.ASCII.GetBytes(secretKey);

            List<Claim> lstClaims = new List<Claim>();
            lstClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()));
            lstClaims.Add(new Claim(ClaimTypes.Email, user.Email));
            foreach (UsersRole usersRole in user.UsersRoles)
                lstClaims.Add(new Claim(ClaimTypes.Role, usersRole.RoleId.ToString()));
            
            var claims = lstClaims.ToArray();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);
            TokenHelper token = new TokenHelper() { Token = tokenHandler.WriteToken(createdToken) };
            return Ok(token);
        }
    }
}
