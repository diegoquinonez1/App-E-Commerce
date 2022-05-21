using System.ComponentModel.DataAnnotations.Schema;

namespace Dominio
{
    public class Precio
    {
        public Guid PrecioID { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal PrecioActual { get; set; }

        [Column(TypeName = "decimal(18,4)")]
        public decimal Promocion { get; set; }
        public Guid CursoID { get; set; }
        public Curso? Curso { get; set; }
    }
}