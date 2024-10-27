using AutoMapper;
using Layer_DB.UserRepository;
using Layer_Entities.ModelsDB;
using Layer_Entities.ModelsDTO;
using Layer_Entities.Wrappers;
using Layer_Service.Helpers;
using Layer_Service.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Layer_Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<JsonResponse<AuthenticationResponse>> LoginAsync(AuthenticationRequest vm)
        {
            var userResponse = await _userRepository.AuthenticateAsync(vm);
            return userResponse;
        }


        public async Task<JsonResponse<RegisterResponseUser>> RegisterUsers(RegisterRequest request)
        {
            RegisterResponseUser response = new();
            JsonResponse<RegisterResponseUser> jsonResponse = new JsonResponse<RegisterResponseUser>
            {
                Errors = new List<string>()
            };

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            string passwordRegex = configuration["PasswordRegex"];

            if (!EmailValidator.IsValidEmailFormat(request.Email) || !request.Email.EndsWith("@bhd.com"))
            {
                jsonResponse.Errors.Add($"The email does not match the domain. {request.Email} does not match the domain: [correo]@bhd.com");
            }

            if (!PasswordValidator.ValidatePassword(request.PasswordHash, passwordRegex))
            {
                jsonResponse.Errors.Add($"The password must be at least 8 characters, include at least one lowercase letter, one uppercase letter, and one number.");
            }

            if (jsonResponse.Errors.Count > 0)
            {
                jsonResponse.Succeeded = false;
                jsonResponse.Message = "Debes cumplir con los estandares de registro. Revise la lista de errores.";
                return jsonResponse;
            }

            var user = _mapper.Map<User>(request);
            var phones = _mapper.Map<List<Phone>>(request.Phones);

            var userSavedOrNot = await _userRepository.RegisterUsersAsync(user, phones);
            if (userSavedOrNot.Message == null)
            {
                response = _mapper.Map<RegisterResponseUser>(userSavedOrNot.Data);
                return new JsonResponse<RegisterResponseUser>(response);                
            }
            return userSavedOrNot;

        }


    }
}
