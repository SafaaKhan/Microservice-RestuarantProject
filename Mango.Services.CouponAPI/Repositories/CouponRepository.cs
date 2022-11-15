using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.CouponAPI.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public CouponRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<CouponDto> GetCouponByCodeAsync(string couponCode)
        {
            var couponFromDb = await _db.Coupons.FirstOrDefaultAsync(x => x.CouponCode == couponCode); //must compare (Case sentitive)
            return _mapper.Map<CouponDto>(couponFromDb);
        }
    }
}
