using System.Net;
using System.Net.Mail;

namespace cinema.api.Helpers.EmailSender;

public interface IEmailSender
{
    Task SendPasswordAsync(string recipientEmail, string newPassword);
    Task SendTicketAsync(string recipientEmail, TicketInfo ticketInfo);
}

public class EmailSender : IEmailSender
{
    private readonly SenderInfo _senderInfo;
    private readonly EmailOptions _emailOptions;

    public EmailSender(EmailOptions emailOptions)
    {
        _emailOptions = emailOptions;

        _senderInfo = new SenderInfo
        {
            Email = _emailOptions.Email,
            DisplayName = _emailOptions.DisplayName,
            AppPassword = _emailOptions.AppPassword,
            SmtpClientHost = _emailOptions.SmtpClientHost,
            SmtpClientPort = _emailOptions.SmtpClientPort,
        };
    }

    public Task SendTicketAsync(string recipientEmail, TicketInfo ticketInfo)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_senderInfo.Email, _senderInfo.DisplayName),
            Subject = $"Twój bilet: {ticketInfo.MovieName}",
            Body = generateTicketBody(ticketInfo),
            IsBodyHtml = true
        };
        return sendEmailAsync(recipientEmail, mailMessage);
    }

    public Task SendPasswordAsync(string recipientEmail, string newPassword)
    {
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_senderInfo.Email, _senderInfo.DisplayName),
            Subject = "Twoje nowe hasło",
            Body = generatePasswordBody(newPassword, $"{_emailOptions.WebsiteUrl}/admin"),
            IsBodyHtml = true
        };
        return sendEmailAsync(recipientEmail, mailMessage);
    }

    private Task sendEmailAsync(string recipientEmail, MailMessage mailMessage)
    {
        var smtpClient = new SmtpClient(_senderInfo.SmtpClientHost, (int)_senderInfo.SmtpClientPort)
        {
            Credentials = new NetworkCredential(_senderInfo.Email, _senderInfo.AppPassword),
            EnableSsl = true
        };

        mailMessage.To.Add(recipientEmail);

        return smtpClient.SendMailAsync(mailMessage);
    }

    private string generatePasswordBody(string newPassword, string loginUrl)
    {
        return $@"
    <!DOCTYPE html>
    <html>
    <head>
        <style>
            body {{
                font-family: Arial, sans-serif;
                margin: 0;
                padding: 0;
                background-color: #f4f4f4;
                color: #333333;
            }}
            .email-container {{
                max-width: 600px;
                margin: 20px auto;
                background: #ffffff;
                border-radius: 10px;
                overflow: hidden;
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            }}
            .header {{
                background-color: #2c3e50;
                color: #ffffff;
                text-align: center;
                padding: 20px;
            }}
            .header h1 {{
                margin: 0;
            }}
            .content {{
                padding: 20px;
            }}
            .content h2 {{
                color: #2c3e50;
            }}
            .password-container {{
                background-color: #2c3e50;
                color: white;
                padding: 10px;
                text-align: center;
                border-radius: 8px;
                margin: 20px 0;
                width: fit-content;
                margin-left: auto;
                margin-right: auto;
            }}
            .password-container p {{
                font-size: 28px;
                font-weight: bold;
                letter-spacing: 2px;
                margin: 0;
            }}
            .login-link {{
                text-align: center;
                margin: 20px 0;
            }}
            .login-link a {{
                display: inline-block;
                padding: 10px 20px;
                background-color: #2c3e50;
                color: #ffffff;
                text-decoration: none;
                border-radius: 5px;
                font-size: 16px;
            }}
            .login-link a:hover {{
                background-color: #1a252f;
            }}
            .footer {{
                text-align: center;
                background-color: #2c3e50;
                color: #ffffff;
                padding: 10px;
                font-size: 12px;
            }}
        </style>
    </head>
    <body>
        <div class='email-container'>
            <div class='header'>
                <h1>Resetowanie hasła</h1>
            </div>
            <div class='content'>
                <h2>Cześć!</h2>
                <p>Twoje hasło zostało zresetowane. Oto twoje nowe hasło:</p>
                <div class='password-container'>
                    <p>{newPassword}</p>
                </div>
                <p>Proszę użyj przycisku poniżej, aby zalogować się na swoje konto. Dla bezpieczeństwa, zaleca się zmienić hasło po zalogowaniu.</p>
                <div class='login-link'>
                    <a href='{loginUrl}'>Przejdź do logowania</a>
                </div>
            </div>
            <div class='footer'>
                <p>&copy; 2024 Cinema App. Wszelkie prawa zastrzeżone.</p>
            </div>
        </div>
    </body>
    </html>";
    }

    private string generateTicketBody(TicketInfo ticketInfo)
    {
        return $@"
    <!DOCTYPE html>
    <html>
    <head>
        <style>
            body {{
                font-family: Arial, sans-serif;
                margin: 0;
                padding: 0;
                background-color: #f4f4f4;
                color: #333333;
            }}
            .email-container {{
                max-width: 600px;
                margin: 20px auto;
                background: #ffffff;
                border-radius: 10px;
                overflow: hidden;
                box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
            }}
            .header {{
                background-color: #2c3e50;
                color: #ffffff;
                text-align: center;
                padding: 20px;
            }}
            .header h1 {{
                margin: 0;
            }}
            .content {{
                padding: 20px;
            }}
            .content h2 {{
                color: #2c3e50;
            }}
            .ticket-details {{
                margin: 20px 0;
            }}
            .ticket-details p {{
                margin: 5px 0;
            }}
            .website-link {{
                text-align: center;
                margin: 20px 0;
            }}
            .website-link a {{
                display: inline-block;
                padding: 10px 20px;
                background-color: #2c3e50;
                color: #ffffff;
                text-decoration: none;
                border-radius: 5px;
                font-size: 16px;
            }}
            .website-link a:hover {{
                background-color: #1a252f;
            }}
            .footer {{
                text-align: center;
                background-color: #2c3e50;
                color: #ffffff;
                padding: 10px;
                font-size: 12px;
            }}
            .code-container {{
                background-color: #2c3e50;
                color: white;
                padding: 10px;
                text-align: center;
                border-radius: 8px;
                margin: 20px 0;
                width: fit-content;
                margin-left: auto;
                margin-right: auto;
            }}
            .code-container p {{
                font-size: 28px;
                font-weight: bold;
                letter-spacing: 2px;
                margin: 0;
            }}
        </style>
    </head>
    <body>
        <div class='email-container'>
            <div class='header'>
                <h1>Twój bilet do kina</h1>
            </div>
            <div class='content'>
                <h2>Cześć!</h2>
                <p>Dziękujemy za zakup biletu. Oto szczegóły Twojego seansu:</p>
                <div class='ticket-details'>
                    <p><strong>Tytuł filmu:</strong> {ticketInfo.MovieName}</p>
                    <p><strong>Data:</strong> {ticketInfo.Date}</p>
                    <p><strong>Godzina:</strong> {ticketInfo.Time}</p>
                    <p><strong>Numer(y) miejsca:</strong> {ticketInfo.SeatNumbers}</p>
                </div>
            <div class='code-container'>
                <p>{ticketInfo.Code}</p>
            </div>
                <div class='website-link'>
                    <a href='{ticketInfo.WebsiteUrl}'>Przejdź na stronę kina</a>
                </div>
                <p>Miłego seansu! 🎬</p>
            </div>
            <div class='footer'>
                <p>&copy; 2024 Cinema App. Wszelkie prawa zastrzeżone.</p>
            </div>
        </div>
    </body>
    </html>";
    }

}
