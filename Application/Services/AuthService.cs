using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Apllication.DTOs;
using Apllication.Exceptions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Apllication.Services;

public class AuthService : IAuthService
{
  private readonly UserManager<AppUser> _userManager;
  private readonly RoleManager<IdentityRole> _roleManager;
  private readonly IConfiguration _configuration;

  public AuthService(
    UserManager<AppUser> userManager,
    RoleManager<IdentityRole> roleManager,
    IConfiguration configuration
  )
  {
    _userManager = userManager;
    _roleManager = roleManager;
    _configuration = configuration;
  }

  public string CreateToken(AppUser user)
  {
    var claims = new List<Claim>
    {
      new Claim(ClaimTypes.Name, user.UserName),
      new Claim(ClaimTypes.NameIdentifier, user.Id),
      new Claim(ClaimTypes.Email, user.Email),
    };

    var key = new SymmetricSecurityKey(
      Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:PasswordKey").Value + "salt")
    );
    var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.UtcNow.AddDays(7),
      SigningCredentials = credentials
    };

    var tokenHandler = new JwtSecurityTokenHandler();

    var token = tokenHandler.CreateToken(tokenDescriptor);

    return tokenHandler.WriteToken(token);
  }

  public async Task<UserDto?> Login(LoginDTO loginDTO)
  {
    var user = await _userManager.FindByEmailAsync(loginDTO.Email);
    if (user == null)
      return null; //Unauthorized();

    var autorizeResult = await _userManager.CheckPasswordAsync(user, loginDTO.Password);
    if (autorizeResult)
    {
      return ParseUserDto(user);
    }
    return null; // Unauthorized();
  }

  public async Task<UserDto> Register(CreateUserDto userDto)
  {
    var checkDuplicatedUsername = await _userManager.Users.AnyAsync(u =>
      u.UserName == userDto.Username
    );
    if (checkDuplicatedUsername)
      throw new BadRequestException("Username is already taken!");
    var checkDuplicatedEmail = await _userManager.Users.AnyAsync(u => u.Email == userDto.Email);
    if (checkDuplicatedUsername)
      throw new BadRequestException("Email is already taken!");

    var user = new AppUser
    {
      DisplayName = userDto.DisplayName,
      Email = userDto.Email,
      UserName = userDto.Username,
      Bio = userDto.Bio ?? "",
    };

    var result = await _userManager.CreateAsync(user, userDto.Password);
    if (result.Succeeded)
    {
      if (await _roleManager.FindByNameAsync("User") == null)
        throw new Exception("Default user are not set");
      await _userManager.AddToRoleAsync(user, "User");
      return this.ParseUserDto(user);
    }
    throw new BadRequestException("Error saving user", new() { Errors = result.Errors.ToString() });
  }

  public async Task<UserDto> GetCurrentUser(string email)
  {
    var user =
      await _userManager.FindByEmailAsync(email ?? "")
      ?? throw new Exception("Auth User is not set!");
    return this.ParseUserDto(user);
  }

  private UserDto ParseUserDto(AppUser user)
  {
    return new UserDto
    {
      DisplayName = user?.DisplayName ?? "",
      //Image= null,
      Token = this.CreateToken(user),
      Username = user.UserName ?? ""
    };
  }

  public async Task AddRole(RoleDto roleDto)
  {
    var role =
      await _roleManager.FindByNameAsync(roleDto.UserRole)
      ?? throw new NotFoundException("This role does not exist!");
    var user =
      await _userManager.FindByEmailAsync(roleDto.UserEmail)
      ?? throw new NotFoundException("User does not exist for this e-mail!");

    await _userManager.AddToRoleAsync(user, roleDto.UserRole);
  }

  public async Task RemoveRole(RoleDto roleDto)
  {
    var role =
      await _roleManager.FindByNameAsync(roleDto.UserRole)
      ?? throw new NotFoundException("This role does not exist!");
    var user =
      await _userManager.FindByEmailAsync(roleDto.UserEmail)
      ?? throw new NotFoundException("User does not exist for this e-mail!");

    await _userManager.RemoveFromRoleAsync(user, roleDto.UserRole);
  }
}