using Layer_Entities.ModelsDB;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace Layer_Entities.ModelsDTO
{
    public class RegisterRequest
    {
        [SwaggerParameter(Description = "Nombre del usuario que desea registrarse")]
        public string NameUser { get; set; }


        [SwaggerParameter(Description = "Correo el cual usuario tendra. Debe termianr con el siguiente formato: [correo]@bhd.com")]
        public string Email { get; set; }


        [SwaggerParameter(Description = "Contraseña siguiendo la expresion regular")]
        public string PasswordHash { get; set; }


        [JsonIgnore]
        public bool IsActive { get; set; }


        [SwaggerParameter(Description = "Numeros de telefonos del usuario")]
        public List<PhonesDTO> Phones { get; set; }
    }
}
