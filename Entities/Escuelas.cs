using System;
using System.Collections.Generic;

namespace Iso810.Entities
{
    public partial class Escuelas
    {
        public Escuelas()
        {
            Estudiantes = new HashSet<Estudiantes>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public int SectorId { get; set; }
        public int ProvinciaId { get; set; }

        public virtual Provincias Provincia { get; set; }
        public virtual Sectores Sector { get; set; }
        public virtual ICollection<Estudiantes> Estudiantes { get; set; }
    }
}
