namespace Apllication.DTOs;

public class UserDto
{
  public string DisplayName { get; set; } = "";
  public string Token { get; set; } = "";
  public string Image { get; set; } = "";
  public string Id { get; set; } = "";
  public string? RoleName { get; set; }
}
