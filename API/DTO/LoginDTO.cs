using System;
using System.ComponentModel.DataAnnotations;

namespace API.DTO;

public class LoginDTO
{
    [Required]
    [MaxLength(50)]
    public required string Username { get; set; }
    [Required]
    [MaxLength(50)]
    public required string Password { get; set; }
}
