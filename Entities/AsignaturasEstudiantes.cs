using System;
using System.Collections.Generic;

namespace Iso810.Entities
{
    public partial class AsignaturasEstudiantes
    {
        public int Id { get; set; }
        public int Calificacion { get; set; }
        public string CondicionAcademica { get; set; }
        public int? EstudianteId { get; set; }
        public int? AsignaturaId { get; set; }

        public virtual Asignaturas Asignatura { get; set; }
        public virtual Estudiantes Estudiante { get; set; }
    }
}
