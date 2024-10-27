using Layer_Entities.ModelsDTO;
using Layer_Entities.Wrappers;

namespace Layer_Service.Interfaces
{
    public interface IUserService
    {
        Task<JsonResponse<AuthenticationResponse>> LoginAsync(AuthenticationRequest vm);
        Task<JsonResponse<RegisterResponseUser>> RegisterUsers(RegisterRequest request);
    }
}