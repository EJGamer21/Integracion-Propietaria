using System;
using System.Collections.Generic;

namespace Iso810.Entities
{
    public partial class Asignaturas
    {
        public Asignaturas()
        {
            AsignaturasEstudiantes = new HashSet<AsignaturasEstudiantes>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public int? TandaId { get; set; }

        public virtual Tandas Tanda { get; set; }
        public virtual ICollection<AsignaturasEstudiantes> AsignaturasEstudiantes { get; set; }
    }
}
