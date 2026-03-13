using System.ComponentModel.DataAnnotations;

namespace JN_API.Models
{
    public class SeguridadRequest
    {
        [Required]
        public int Consecutivo { get; set; }
        [Required]
        [MinLength(8)]
        public string NuevaContrasenna { get; set; } = string.Empty;
        [Required]
        [Compare("NuevaContrasenna")]
        public string ConfirmarContrasenna { get; set; } = string.Empty;
    }
}
