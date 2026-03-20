using System.ComponentModel.DataAnnotations;

namespace JN_API.Models
{
    public class PerfilRequest
    {
        [Required]
        public string Identificacion { get; set; } = string.Empty;
        [Required]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public string CorreoElectronico { get; set; } = string.Empty;

        public string ImagenPerfil { get; set; } = string.Empty;
    }
}

