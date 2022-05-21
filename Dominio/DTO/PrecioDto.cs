namespace Dominio.DTO
{
    public class PrecioDto
    {
        public Guid PrecioID { get; set; }
        public decimal PrecioActual { get; set; }
        public decimal Promocion { get; set; }
        public Guid CursoID { get; set; }
    }
}