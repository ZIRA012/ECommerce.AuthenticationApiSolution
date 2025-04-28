
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Data;
using ECommmerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationApi.Infrastructure.Repositories;

public class UserRepository(AuthenticationDbContext context, IConfiguration config) : IUser
{
    private async Task<AppUser> GetUserEmail(string email)
    {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Email == email);
        return user is null ? null! : user;
    }

    public async Task<GetUserDTO> GetUser(int userId)
    {
        var user = await context.Users.FindAsync(userId);
        return user is not null ? new GetUserDTO(user.Id,
            user.Name!,
            user.TelephoneNumber!,
            user.Address!,
            user.Email!,
            user.Role!): null!;
    
    }

    public async Task<Response> Login(LoginDTO loginDTO)
    {
        var getUser = await GetUserEmail(loginDTO.Email);
        if (getUser is null) 
            return new Response(false, "Credenciales invalidas el correo no tiene cuenta");

        bool verifyPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, getUser.Password);

        if (!verifyPassword)
            return new Response(false, "Credenciales Invalida ");

        string token = GenerateToken(getUser);
        return new Response(true, token);
    }

    private string GenerateToken(AppUser user)
    {
        var key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:Key").Value!);
        var securityKet =new SymmetricSecurityKey(key);
        var credential = new SigningCredentials(securityKet, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Name!),
            new(ClaimTypes.Email, user.Email!)
        };
        if (string.IsNullOrEmpty(user.Role) || !Equals("string", user.Role))
            claims.Add(new(ClaimTypes.Role, user.Role!));


        var token = new JwtSecurityToken(
            issuer: config["Authentication:Issuer"],
            audience: config["Authentication:Audience"],
            claims: claims,
            expires: null,
            signingCredentials: credential
            );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<Response> Register(AppUserDTO appUserDTO)
    {
        var getUser = await GetUserEmail(appUserDTO.Email);

        if (getUser is not null)
            return new Response(false, "No puede usar el correo por que ya esta registrado");

        var result = context.Users.Add(new AppUser()
        {
            Name = appUserDTO.Name,
            Email = appUserDTO.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(appUserDTO.Password),
            Address = appUserDTO.Address,
            Role = appUserDTO.Role,
        });
        await context.SaveChangesAsync();
        return result.Entity.Id > 0 ? new Response(true, "Usuario Registrdo correctamente") :
            new Response(false, "No se puedo completar la accion");
    }
}
