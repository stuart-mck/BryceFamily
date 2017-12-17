using Amazon.SimpleEmail.Model;
using BryceFamily.Repo.Core.AWS;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BryceFamily.Repo.Core.Emails
{
    public class SesService : ISesService
    {
        private readonly ILogger<SesService> _logger;
        private readonly IAWSClientFactory _awsClientFactory;

        public SesService(ILogger<SesService> logger, IAWSClientFactory awsClientFactory)
        {
            this._logger = logger;
            _awsClientFactory = awsClientFactory;
        }

        public async Task SendEmail(string emailaddress, string message, string subject, CancellationToken cancellationToken)
        {
            var emailClient = _awsClientFactory.GetSesClient();

            try
            {
                await emailClient.SendEmailAsync(new SendEmailRequest()
                {
                    Destination = new Destination(new List<string>() { emailaddress }),
                    Message = new Message(new Content(subject), new Body()
                    {
                        Html = new Content(message),
                        Text = new Content(message)
                    }),
                    ReplyToAddresses = new List<string> { "info@brycefamily.net" },
                    Source = "info@brycefamily.net",
                    ReturnPath = "admin@brycefamily.net"
                }, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Faild to send email to {emailaddress}", ex);
                throw;
            }


        }
    }
}
