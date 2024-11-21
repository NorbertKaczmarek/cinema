using System.Net.Mail;
using System.Net;

namespace cinema.api.Helpers.EmailSender;

public interface IEmailSender
{
    Task sendEmailAsync(SenderInfo senderInfo, string recipientEmail, TicketInfo ticketInfo);
}

public class EmailSender : IEmailSender
{
    public Task sendEmailAsync(SenderInfo senderInfo, string recipientEmail, TicketInfo ticketInfo)
    {
        var smtpClient = new SmtpClient(senderInfo.SmtpClientHost, (int)senderInfo.SmtpClientPort)
        {
            Credentials = new NetworkCredential(senderInfo.Email, senderInfo.AppPassword),
            EnableSsl = true
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(senderInfo.Email, senderInfo.DisplayName),
            Subject = $"Your Movie Ticket: {ticketInfo.MovieName}",
            Body = generateEmailBody(ticketInfo),
            IsBodyHtml = true
        };
        mailMessage.To.Add(recipientEmail);

        return smtpClient.SendMailAsync(mailMessage);
    }

    private string generateEmailBody(TicketInfo ticketInfo)
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
                    <h1>Your Movie Ticket</h1>
                </div>
                <div class='content'>
                    <h2>Hello!</h2>
                    <p>Thank you for booking your ticket with us. Here are the details of your movie:</p>
                    <div class='ticket-details'>
                        <p><strong>Movie Name:</strong> {ticketInfo.MovieName}</p>
                        <p><strong>Date:</strong> {ticketInfo.Date}</p>
                        <p><strong>Time:</strong> {ticketInfo.Time}</p>
                        <p><strong>Seat Number(s):</strong> {ticketInfo.SeatNumbers}</p>
                    </div>
                <div class='code-container'>
                    <p>{ticketInfo.Code}</p>
                </div>
                    <div class='website-link'>
                        <a href='{ticketInfo.WebsiteUrl}'>Cinema Website</a>
                    </div>
                    <p>Enjoy your movie! 🎬</p>
                </div>
                <div class='footer'>
                    <p>&copy; 2024 Cinema App. All rights reserved.</p>
                </div>
            </div>
        </body>
        </html>";
    }
}
