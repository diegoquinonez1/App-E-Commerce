namespace Dominio
{
    public class Instructor
    {
        public Guid InstructorID { get; set; }
        public string? Nombre  { get; set; }
        public string? Apellidos { get; set; }
        public string? Grado { get; set; }
        public byte[]? FotoPerfil { get; set; }
        public ICollection<CursoInstructor>? CursoLista { get; set; }
    }
}