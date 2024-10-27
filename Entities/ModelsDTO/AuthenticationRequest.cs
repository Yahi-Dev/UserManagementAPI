
using System.ComponentModel.DataAnnotations;

namespace Layer_Entities.ModelsDTO
{
    public class AuthenticationRequest
    {
        [Required(ErrorMessage = "Debe colocar el nombre del usuario")]
        [DataType(DataType.Text)]
        public string Email { get; set; }



        [Required(ErrorMessage = "Debe colocar una contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
