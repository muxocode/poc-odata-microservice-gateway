using System;
using System.Collections.Generic;

namespace entities
{
    public class Alumno : _base.EntityBase
    {
        public string Nombre { get; set; }
        public string Apellido1 { get; set; }
        public string Apellido2 { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public DateTime? FechaBaja { get; set; }
        public Centro Centro {get; set;}
        public ICollection<AlumnoAsignatura> AlumnoAsignaturas { get; set; }
    }
}
