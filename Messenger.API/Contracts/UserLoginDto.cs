using System.ComponentModel.DataAnnotations;

namespace Messenger.Contracts;

public record UserLoginDto(
    [Required]string Email, 
    [Required]string Password);