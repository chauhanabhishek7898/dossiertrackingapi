using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TrackingAPI.Utilities
{
    public class JwtTokenGenerater
    {
        public JwtTokenGenerater()
        {

        }
        public static string GetJsonWebToken(string userName, IConfiguration configration)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configration["Jwt:key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
              new Claim(JwtRegisteredClaimNames.Sub,userName)
            };
            var token = new JwtSecurityToken(
                issuer: configration["Jwt:Issuer"],
                audience: configration["Jwt:Issuer"],
                claims: null,
                expires: DateTime.Now.AddHours(Convert.ToInt32(configration["Jwt:tokenIntervalhour"])),
                signingCredentials: credentials
                );

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodeToken;
        }
    }
}
