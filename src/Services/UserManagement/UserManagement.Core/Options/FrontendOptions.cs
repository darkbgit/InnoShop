namespace UserManagement.Core.Options;

public class FrontendOptions
{
    public const string SectionName = "Frontend";
    public string Url { get; set; } = string.Empty;
    public string ResetPasswordUrl { get; set; } = string.Empty;
}
