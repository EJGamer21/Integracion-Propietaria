using System;
using System.Collections.Generic;

namespace Iso810.Entities
{
    public partial class Secciones
    {
        public Secciones()
        {
            SeccionesGrados = new HashSet<SeccionesGrados>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<SeccionesGrados> SeccionesGrados { get; set; }
    }
}
