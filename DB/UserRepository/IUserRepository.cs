using Layer_Entities.ModelsDB;
using Layer_Entities.ModelsDTO;
using Layer_Entities.Wrappers;

namespace Layer_DB.UserRepository
{
    public interface IUserRepository
    {
        Task<JsonResponse<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request);
        Task<JsonResponse<RegisterResponseUser>> RegisterUsersAsync(User user, List<Phone> phones);
    }
}