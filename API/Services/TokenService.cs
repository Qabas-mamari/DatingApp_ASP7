using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interface;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        /*
        a symmetric security key is a secret key that is shared between the token issuer (server) and the token consumer (client).
         It is used for generating and validating secure tokens, such as JSON Web Tokens (JWTs).
        */
        private readonly SymmetricSecurityKey _key;
        
        /*
        IConfiguration: interface provides access to the application's configuration settings.
        _key: retrieves the token key from the configuration using the config["TokenKey"] syntax.
              It assumes that there is a configuration key named "TokenKey" that contains the secret key for generating tokens.
        */
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
            
        }

        public string CreateToken(AppUser user)
        {
            //1. Initializes a new List<Claim> named claims. A claim is a statement about a user that carries information. 
            //1.1. A claim with the name identifier (JwtRegisteredClaimNames.NameId) is added to the list.
            //1.2. The name identifier claim represents the username of the user
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.NameId,  user.UserName)
            };

            //2. The SigningCredentials class represents the cryptographic credentials used to sign the token. 
            //2.1. _key: represents the secret key used for signing the token.
            //2.2. SecurityAlgorithms.HmacSha512Signature: This is a constant value specifying the signature algorithm to use for the token.
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            //3. SecurityTokenDescriptor class used to configure the properties and settings for generating the security token.
            //3.1. Subject of the token: represents the entity the token refers to ClaimsIdentity represents a collection of claims associated with a single subject.
            //3.2. Expiration time for the token: specifies that the token should be valid for 7 days from the current date and time.
            //3.3. Signing credentials for the token: contain the security key and algorithm needed to sign the token.
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials= creds
            };

            //4. provides methods to generate and handle JWTs, such as creating a token, validating a token, and extracting information from a token.
            var tokenHandler = new JwtSecurityTokenHandler();

            //5. Takes in the SecurityTokenDescriptor object (tokenDescriptor) that contains the configuration for generating the security token.
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //6. The WriteToken method is responsible for converting the security token into its string representation.
            //6.1. returning the string representation of the generated security token from the CreateToken method. 
            return tokenHandler.WriteToken(token);
        }
    }
}