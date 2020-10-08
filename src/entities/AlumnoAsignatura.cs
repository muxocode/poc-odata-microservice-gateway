using System;
using System.Collections.Generic;
using System.Text;

namespace entities
{
    public class AlumnoAsignatura:_base.EntityBase
    {
        public Alumno Alumno { get; set; }
        public Asignatura Asignatura { get; set; }
    }
}
