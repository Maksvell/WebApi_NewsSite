using DAL.Entities;

namespace BLL.DTOs;

public class AuthorDTO : BaseDTO
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}