using UserManagement.Core.Interfaces;

namespace InnoShop.IntegrationTests.UserManagement.Helpers;

public class FakeEmailService : IEmailService
{
    public Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
