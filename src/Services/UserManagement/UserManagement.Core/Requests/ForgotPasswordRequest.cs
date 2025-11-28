using System.ComponentModel.DataAnnotations;

namespace UserManagement.Core.Requests;

public class ForgotPasswordRequest
{
    [Required]
    public string Email { get; set; } = string.Empty;
}
