using ClinicalFlow.Dtos.Auth;

namespace ClinicalFlow.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }
}
