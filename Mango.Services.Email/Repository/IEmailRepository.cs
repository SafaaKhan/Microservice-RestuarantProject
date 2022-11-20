using Mango.Services.Email.Messages;
using System.Diagnostics.Eventing.Reader;

namespace Mango.Services.Email.Repository
{
    public interface IEmailRepository
    {
        Task SendAndLogEmailAsync(UpdatePaymentResultMessage message);
    }
}
