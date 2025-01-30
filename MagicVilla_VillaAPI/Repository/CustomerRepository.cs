using MagicVilla_VillaAPI.DATA;
using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public readonly ApplicationDbContext _db;

        public CustomerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Customer> UpdateCustomer(Customer entity)
        {
            entity.UpdateDate = DateTime.Now;
            dbSet.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
