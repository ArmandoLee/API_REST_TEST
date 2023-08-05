namespace API_WEB.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public class PersonaDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "El campo Nombre no puede tener más de 30 caracteres.")]
        [MinLength(5, ErrorMessage = "El campo Nombre no puede tener menos de 5 caracteres.")]
        public string Nombre { get; set; }

        [RegularExpression("Masculino|Femenino|Comunidad LGBT|Otros", ErrorMessage = "El campo Genero debe ser 'Masculino', 'Femenino', 'Comunidad LGBT' o 'Otros'.")]
        public string Genero { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "El campo Edad debe ser un número positivo o cero.")]
        public int Edad { get; set; }

        [MaxLength(50, ErrorMessage = "El campo Identificacion no puede tener más de 50 caracteres.")]
        public string Identificacion { get; set; }

        [MaxLength(200, ErrorMessage = "El campo Direccion no puede tener más de 200 caracteres.")]
        public string Direccion { get; set; }

        [MaxLength(20, ErrorMessage = "El campo Telefono no puede tener más de 20 caracteres.")]
        public string Telefono { get; set; }
    }
}
