using System;
using System.Collections.Generic;
using System.Text;

namespace entities
{
    public class Asignatura:_base.EntityBase
    {
        public string Nombre { get; set; }
        public int Creditos { get; set; }

        public ICollection<AlumnoAsignatura> AlumnosAsignatura { get; set; }
    }
}
