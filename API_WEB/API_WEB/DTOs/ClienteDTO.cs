using API_WEB.Models;

namespace API_WEB.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class ClienteDTO
    {
        public int ClienteId { get; set; }

        [Required]
        [MinLength(4, ErrorMessage = "La contraseña debe tener al menos 4 caracteres.")]
        public string contraseña { get; set; }

        [RegularExpression("Activo|Inactivo", ErrorMessage = "El campo Estado solo puede ser 'Activo' o 'Inactivo'.")]
        public string Estado { get; set; }

        public int PersonaId { get; set; }

        [Required]
        public PersonaDTO Persona { get; set; }
    }

}
