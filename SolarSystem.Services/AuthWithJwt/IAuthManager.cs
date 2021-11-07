using System.Threading.Tasks;
using SolarSystem.Data.DTOs;

namespace SolarSystem.Services.AuthWithJwt
{
    public interface IAuthManager
    {
        Task<bool> ValidateUserAsync(SignInDTO request);
        Task<string> CreateTokenAsync();
    }
}