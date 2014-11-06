using System;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WebAuthMe.Core.AuthProvider;

namespace WebAuthMe.Core
{
    public class TokenFactory
    {
        public string GetSampleToken()
        {

            var securityKey = Encoding.UTF8.GetBytes("ThisIsAnImportantStringAndIHaveNoIdeaIfThisIsVerySecureOrNot!");

            var tokenHandler = new JwtSecurityTokenHandler();

            // Token Creation
            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "Pedro"),
                    new Claim(ClaimTypes.Role, "Author"),
                }),

                TokenIssuerName = "self",
                AppliesToAddress = "http://www.example.com",
                Lifetime = new Lifetime(now, now.AddMinutes(2)),
                SigningCredentials = new SigningCredentials(
                    new InMemorySymmetricSecurityKey(securityKey),
                    "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                    "http://www.w3.org/2001/04/xmlenc#sha256"),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Generate Token and return string
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public string CreateToken(UserIdentity userIdentity, string symmetricSecurityKey, string audience)
        {
            var securityKey = Encoding.UTF8.GetBytes(symmetricSecurityKey);

            var tokenHandler = new JwtSecurityTokenHandler();

            // Token Creation
            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userIdentity.LastName),
                    new Claim(ClaimTypes.GivenName, userIdentity.FirstName),
                    new Claim(ClaimTypes.Email, userIdentity.MailAddress), 
                    new Claim(ClaimTypes.Surname, userIdentity.LastName), 
                    new Claim("AccountPicture", userIdentity.AccountPictureUrl), 
                }),

                TokenIssuerName = "self",
                AppliesToAddress = audience,
                
                Lifetime = new Lifetime(now, now.AddMinutes(2)),
                SigningCredentials = new SigningCredentials(
                    new InMemorySymmetricSecurityKey(securityKey),
                    "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                    "http://www.w3.org/2001/04/xmlenc#sha256"),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Generate Token and return string
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }
    }
}
