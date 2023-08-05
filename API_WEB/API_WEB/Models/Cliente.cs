namespace API_WEB.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Contraseña { get; set; }
        public string Estado { get; set; }

        public int PersonaId { get; set; }
        public Persona Persona { get; internal set; }
    }
}
