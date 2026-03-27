namespace JN_WEB.Models
{
    public class Tienda
    {
        public int Consecutivo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Contacto { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Ubicacion { get; set; } = string.Empty;
        public string ImagenPerfil { get; set; } = string.Empty;
    }
}
