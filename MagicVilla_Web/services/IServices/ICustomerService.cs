using MagicVilla_VillaAPI.Models.DTO;

namespace MagicVilla_Web.services.IServices
{
    public interface ICustomerService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int customerId);
        Task<T> CreateAsync<T>(CustomerCreateDTO customerCreateDTO);
        Task<T> UpdateAsync<T>(CustomerUpdateDTO customerUpdateDTO);
        Task<T> DeleteAsync<T>(int customerId);
    }
}
