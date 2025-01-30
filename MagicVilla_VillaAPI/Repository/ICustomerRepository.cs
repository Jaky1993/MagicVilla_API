using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repository
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> UpdateCustomer(Customer entity);
    }
}
