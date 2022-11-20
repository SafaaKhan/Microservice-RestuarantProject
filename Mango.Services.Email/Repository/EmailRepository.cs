using Mango.Services.Email.Data;
using Mango.Services.Email.Messages;
using Mango.Services.Email.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.Email.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _db;
        public EmailRepository(DbContextOptions<ApplicationDbContext> db)
        {
            _db = db;
        }

        public async Task SendAndLogEmailAsync(UpdatePaymentResultMessage message)
        {
            //implement email sender or call some other class library
            EmailLog emailLog = new EmailLog()
            {
                Email = message.Email,
                EmailSent = DateTime.Now,
                Log=$"Order - {message.OrderId} has been created succussfully."            
            };

            await using var _dbContext = new ApplicationDbContext(_db);
            _dbContext.EmailLogs.Add(emailLog);
            await _dbContext.SaveChangesAsync();
        }
    }
}
