using System.ComponentModel.DataAnnotations;

namespace SolarWatch.Model.AuthModels;

public record RegistrationRequest(
    [Required]string Email, 
    [Required]string Username, 
    [Required]string Password);