using System;
using System.Collections.Generic;
using System.Text;

namespace entities
{
    public class Centro:_base.EntityBase
    {
        public string Nombre { get; set; }
        public ICollection<Alumno> Alumnos { get; set; }
    }
}
