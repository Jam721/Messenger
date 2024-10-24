using System.ComponentModel.DataAnnotations;

namespace Messenger.Contracts;

public record UserRegisterDto(
    [Required]string UserName, 
    [Required]string Email, 
    [Required]string Password);