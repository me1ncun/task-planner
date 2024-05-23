using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using taskplanner_user_service.Models;

namespace taskplanner_user_service.Helpers;

public class JwtProvider
{
    private readonly JwtOptions _options;
    
    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }
    
    public string GenerateToken(User user)
    {
        Claim[] claims = [new("userId", user.Id.ToString()), new("email", user.Email)];
        
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)), SecurityAlgorithms.HmacSha256Signature);

        var token = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials, expires: DateTime.Now.AddHours(_options.ExpiresHours));
        
        var tokenHandler = new JwtSecurityTokenHandler();
        
        return tokenHandler.WriteToken(token);
    }
}