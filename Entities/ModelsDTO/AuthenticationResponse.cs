using System.Text.Json.Serialization;

namespace Layer_Entities.ModelsDTO
{
    public class AuthenticationResponse
    {
        public string Id { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }
        public string? JWToken { get; set; }
        [JsonIgnore]
        public string? RefreshToken { get; set; }
    }
}
