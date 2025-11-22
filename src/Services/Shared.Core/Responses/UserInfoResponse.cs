namespace Shared.Core.Responses;

public class UserInfoResponse
{
    public Guid Id { get; set; }
    public List<string> Roles { get; set; } = [];
}
