using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using UserManagement.Core.Interfaces;
using UserManagement.Infrastructure.Options;

namespace UserManagement.Infrastructure.Services;

public class EmailService(IOptions<EmailOptions> options) : IEmailService
{
    private readonly EmailOptions _emailOptions = options.Value;


    public async Task SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_emailOptions.FromName, _emailOptions.FromAddress));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html) { Text = body };

        using var smtp = new SmtpClient();
        
        try
        {
            await smtp.ConnectAsync(_emailOptions.Server, _emailOptions.Port, 
                _emailOptions.UseSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None, cancellationToken);

            if (!string.IsNullOrEmpty(_emailOptions.Username))
            {
                await smtp.AuthenticateAsync(_emailOptions.Username, _emailOptions.Password, cancellationToken);
            }

            await smtp.SendAsync(email, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Could not send email to {to}", ex);
        }
        finally
        {
            await smtp.DisconnectAsync(true, cancellationToken);
        }
    }
}
