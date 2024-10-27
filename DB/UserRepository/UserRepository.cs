using Layer_DB.Context;
using Layer_Entities.ModelsDB;
using Layer_Entities.ModelsDTO;
using Layer_Entities.Settings;
using Layer_Entities.Wrappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Layer_DB.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManagementDbContext _dbContext;
        private readonly JWTSettings _jwtSettings;

        public UserRepository(UserManagementDbContext dbContext, IOptions<JWTSettings> jwtSettings)
        {
            _dbContext = dbContext;
            _jwtSettings = jwtSettings.Value;
        }


        #region Public Methods
        public async Task<JsonResponse<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();
            JsonResponse<AuthenticationResponse> jsonResponse;

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                jsonResponse = new JsonResponse<AuthenticationResponse>(null, $"No accounts registered with {request.Email}");
                return jsonResponse;
            }

            if (user.PasswordHash != request.Password)
            {
                jsonResponse = new JsonResponse<AuthenticationResponse>(null, "Invalid password.");
                return jsonResponse;
            }

            if ((bool)!user.IsActive)
            {
                jsonResponse = new JsonResponse<AuthenticationResponse>(null, "User account is not active.");
                return jsonResponse;
            }


            user.LastLogin = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();


            var refreshToken = GenerateRefreshToken();
            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);

            response.Id = user.UserId;
            response.Created = user.Created;
            response.Modified = user.Modified;
            response.LastLogin = user.LastLogin;
            response.IsActive = (bool)user.IsActive;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.RefreshToken = refreshToken.Token;
            response.HasError = false;

            jsonResponse = new JsonResponse<AuthenticationResponse>(response);
            return jsonResponse;
        }

        //Objetivo 
        public async Task<JsonResponse<RegisterResponseUser>> RegisterUsersAsync(User user, List<Phone> phones)
        {
            RegisterResponseUser response = new();
            JsonResponse<RegisterResponseUser> jsonResponse;

            var userWithSameEmail = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (userWithSameEmail != null)
            {
                jsonResponse = new JsonResponse<RegisterResponseUser>(null, $"Email '{user.Email}' is already registered.");
                return jsonResponse;
            }

            try
            {
                user.UserId = Guid.NewGuid().ToString();

                List<Phone> newPhones = new List<Phone>();

                foreach (var phone in phones)
                {
                    var userWithSamePhone = await _dbContext.Phones.AsNoTracking().FirstOrDefaultAsync(p => p.Number == phone.Number);

                    if (userWithSamePhone == null)
                    {
                        var newPhone = new Phone
                        {
                            Number = phone.Number,
                            UserId = user.UserId.ToString()
                        };

                        newPhones.Add(newPhone);
                    }
                    else
                    {
                        jsonResponse = new JsonResponse<RegisterResponseUser>(null, $"Phone '{phone.Number}' is already registered. Please use another.");
                        return jsonResponse;
                    }
                }

                _dbContext.Phones.AddRange(newPhones);
                _dbContext.Users.Add(user);

                await _dbContext.SaveChangesAsync(); 

                var userSaved = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                var refreshToken = GenerateRefreshToken();
                JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);

                response.Id = userSaved.UserId;
                response.Created = userSaved.Created;
                response.Modified = userSaved.Modified;
                response.LastLogin = userSaved.LastLogin;
                response.IsActive = (bool)userSaved.IsActive;
                response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                response.RefreshToken = refreshToken.Token;
            }
            catch (Exception ex)
            {
                response.HasError = true;
                jsonResponse = new JsonResponse<RegisterResponseUser>(null, $"An error occurred while inserting the user: {ex.Message}");
                return jsonResponse;
            }

            jsonResponse = new JsonResponse<RegisterResponseUser>(response);
            return jsonResponse;

        }
        #endregion


        #region Private Methods
        private async Task<JwtSecurityToken> GenerateJWToken(User user)
        {

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.NameUser),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.UserId)
            };

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken
            (
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials
            );

            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var ramdomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(ramdomBytes);

            return BitConverter.ToString(ramdomBytes).Replace("-", "");
        }

        #endregion
    }
}
