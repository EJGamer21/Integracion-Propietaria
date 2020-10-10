using System;
using System.Collections.Generic;

namespace Iso810.Entities
{
    public partial class Estudiantes
    {
        public Estudiantes()
        {
            AsignaturasEstudiantes = new HashSet<AsignaturasEstudiantes>();
        }

        public int Matricula { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? EscuelaId { get; set; }
        public int? SeccionGradoId { get; set; }

        public virtual Escuelas Escuela { get; set; }
        public virtual SeccionesGrados SeccionGrado { get; set; }
        public virtual ICollection<AsignaturasEstudiantes> AsignaturasEstudiantes { get; set; }
    }
}
