using Mango.Services.OrderAPI.Data;
using Mango.Services.OrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.OrderAPI.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DbContextOptions<ApplicationDbContext> _db;
        public OrderRepository(DbContextOptions<ApplicationDbContext> db)
        {
            _db = db;
        }
        public async Task<bool> AddOrderAsync(OrderHeader orderHeader)
        {
            //implement try catch
            await using var _context = new ApplicationDbContext(_db);
            _context.OrderHeaders.Add(orderHeader);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateOrderPaymentStatus(int orderHeaderId, bool paid)
        {
            await using var _context = new ApplicationDbContext(_db);
            var orderHeaderFromDb=await _context.OrderHeaders.FirstOrDefaultAsync(x=>x.OrderHeaderId==orderHeaderId); 
            if(orderHeaderFromDb != null)
            {
                orderHeaderFromDb.PaymentStatus=paid;
                await _context.SaveChangesAsync();
            }
        }
    }
}
