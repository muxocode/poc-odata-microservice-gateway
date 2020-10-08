using entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace data
{
    public class AlexiaContext:DbContext
    {

        public AlexiaContext(DbContextOptions options):base(options)
        {

        }

        public DbSet<Asignatura> Asignaturas { get; set; }
        public DbSet<Alumno> Alumnos { get; set; }
        public DbSet<AlumnoAsignatura> AlumnosAsignaturas { get; set; }
        public DbSet<Centro> Centros { get; set; }
    }
}
