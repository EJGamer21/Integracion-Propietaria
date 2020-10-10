using System;
using System.Collections.Generic;

namespace Iso810.Entities
{
    public partial class Tandas
    {
        public Tandas()
        {
            Asignaturas = new HashSet<Asignaturas>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string HoraInicio { get; set; }
        public string HoraFin { get; set; }

        public virtual ICollection<Asignaturas> Asignaturas { get; set; }
    }
}
