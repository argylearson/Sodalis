using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using SodalisCore.DataTransferObjects;

namespace SodalisCore.Services {
    internal class ClaimService : IClaimService {
        //TODO take token key out of the codebase
        private static readonly byte[] _key = Encoding.ASCII.GetBytes("this key shouldn't be in the codebase");

        string IClaimService.CreateToken(UserDto user) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(CreateClaim(user)),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static IEnumerable<Claim> CreateClaim(UserDto user) {
            //making it a list in case I need to add roles or permissions later
            var result = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.EmailAddress)
            };
            
            return result.ToArray();
        }
    }
}