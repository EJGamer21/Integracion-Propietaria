using System;
using System.Collections.Generic;

namespace Iso810.Entities
{
    public partial class StudentsView
    {
        public int CodigoDelCentro { get; set; }
        public string NombreDelCentro { get; set; }
        public string Sector { get; set; }
        public string Provincia { get; set; }
        public int? Matricula { get; set; }
        public string NombreDelEstudiante { get; set; }
        public string Asignatura { get; set; }
        public string Tanda { get; set; }
        public int? Calificacion { get; set; }
        public string CondicionAcademica { get; set; }
        public string Seccion { get; set; }
        public string Grado { get; set; }
    }
}
