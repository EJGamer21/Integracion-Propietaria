﻿using System;
using System.Collections.Generic;

namespace Iso810.Entities
{
    public partial class Sectores
    {
        public Sectores()
        {
            Escuelas = new HashSet<Escuelas>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }

        public virtual ICollection<Escuelas> Escuelas { get; set; }
    }
}
