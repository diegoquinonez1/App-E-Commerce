namespace Dominio
{
    public class CursoInstructor
    {
        public Guid CursoID { get; set; }
        public Guid InstructorID { get; set; }
        public Curso? Curso { get; set; }
        public Instructor? Instructor { get; set; }
    }
}