using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Blog_Rest_Api.Persistent_Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Blog_Rest_Api.Jwt{
    public class JwtSuit{
        private readonly IOptions<JwtInfo> jwtInfo;

        public JwtSuit(IOptions<JwtInfo> jwtInfo)
        {
            this.jwtInfo = jwtInfo;
        }
        
        public string GetToken(User user){
           
            var securityKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtInfo.Value.Key));
            var claims=new List<Claim>{new Claim(ClaimTypes.Sid,user.UserId),new Claim(ClaimTypes.Name,user.FirstName+" "+user.LastName),new Claim("Dev","akash")};  
            
            var securityToken=new JwtSecurityToken(
                issuer:jwtInfo.Value.ValidIssuer,
                audience:jwtInfo.Value.ValidAudience,
                expires:DateTime.UtcNow.AddHours(jwtInfo.Value.Expires),
                claims:claims,
                signingCredentials:new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256Signature)
            );

            var token=new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }
    }
}