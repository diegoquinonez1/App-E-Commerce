namespace Dominio.DTO
{
    public class ComentarioDto
    {
        public Guid ComentarioID { get; set; }
        public string? Alumno { get; set; }
        public int Puntaje { get; set; }
        public string? ComentarioTexto { get; set; }
        public Guid CursoID { get; set; }
    }
}