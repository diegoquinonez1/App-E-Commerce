namespace Dominio
{
    public class Comentario
    {
        public Guid ComentarioID { get; set; }
        public string? Alumno { get; set; }
        public int Puntaje { get; set; }
        public string? ComentarioTexto { get; set; }
        public Guid CursoID { get; set; }
        public Curso? Curso { get; set; }
    }
}