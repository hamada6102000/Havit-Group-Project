namespace FreeLance.Services
{
    /// <summary>
    /// Interface for email service to send emails
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email asynchronously
        /// </summary>
        /// <param name="to">Recipient email address</param>
        /// <param name="subject">Email subject</param>
        /// <param name="body">Email body</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if email was sent successfully, false otherwise</returns>
        Task<bool> SendEmailAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
    }
}

