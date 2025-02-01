using MagicVilla_Web.Models.DTO;

namespace MagicVilla_Web.services.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO objToCreate);
        Task<T> RegisterAsync<T>(RegistrationRequestDTO objToCreate);
    }
}
