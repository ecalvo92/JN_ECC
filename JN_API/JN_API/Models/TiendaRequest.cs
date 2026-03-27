using System.ComponentModel.DataAnnotations;

namespace JN_API.Models
{
    public class TiendaRequest
    {
        [Required]
        public string Nombre { get; set; } = string.Empty;
        [Required]
        public string Contacto { get; set; } = string.Empty;
        [Required]
        public string Descripcion { get; set; } = string.Empty;
        [Required]
        public string Ubicacion { get; set; } = string.Empty;
    }
}
