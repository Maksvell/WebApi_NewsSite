using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BLL.Services;

public class AuthService(IUnitOfWork unit) : IAuthService
{
    public IUnitOfWork _unit = unit;

    public async Task<Author> GetEntity(LoginRequest author)
    {
        return await _unit.AuthorRepository.GetByEmailAndPassword(author.Email, author.Password) ?? throw new NullReferenceException();
    }

    public string CreateToken(Author author)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes("DEC52CDE-06FD-434D-AAFF-F937784FA1B4");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Sid, author.Id.ToString()),
            new Claim(ClaimTypes.Email, author.Email)
        };

        var identity = new ClaimsIdentity(claims);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = identity,
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
