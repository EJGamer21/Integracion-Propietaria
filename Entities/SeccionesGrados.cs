using System;
using System.Collections.Generic;

namespace Iso810.Entities
{
    public partial class SeccionesGrados
    {
        public SeccionesGrados()
        {
            Estudiantes = new HashSet<Estudiantes>();
        }

        public int Id { get; set; }
        public int? SeccionId { get; set; }
        public int? GradoId { get; set; }

        public virtual Grados Grado { get; set; }
        public virtual Secciones Seccion { get; set; }
        public virtual ICollection<Estudiantes> Estudiantes { get; set; }
    }
}
