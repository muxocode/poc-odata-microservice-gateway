using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using data;
using entities;
using host.domain;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OData.Edm;

namespace host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("Alexia");
            ILoggerFactory MyLoggerFactory= LoggerFactory.Create(builder => { builder.AddConsole(); });

            services.AddDbContext<AlexiaLightContext>(builder => {
                builder
                .UseLoggerFactory(MyLoggerFactory);
            });

            services.AddDbContext<AlexiaContext>(builder => {
                builder
                .UseLoggerFactory(MyLoggerFactory)
                .UseSqlServer(connectionString, b=>b.MigrationsAssembly(this.GetType().Assembly.FullName));
            });

            //Añadimos el IOC de Alumno
            services.AddUnitOfWork();
            services.AddAlumno();
            services.AddCentro();
            services.AddAsigntura();

            services.AddControllers(options =>
            {
                options.EnableEndpointRouting = false;
            });

            services.AddOData();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Creamos la bbdd si no existe, aunque esto sería mejor en un  UTIL.APP
            var connectionString = Configuration.GetConnectionString("Alexia");

            var optionBuilder = new DbContextOptionsBuilder<AlexiaContext>();
            optionBuilder.UseSqlServer(connectionString, b => b.MigrationsAssembly(this.GetType().Assembly.FullName));

            using (var context = new AlexiaContext(optionBuilder.Options))
            {
                //Lanzamos las migraciones
                context.Database.Migrate();

                //Ponemos datos de prueba
                if(!context.Alumnos.Any())
                {

                    var centros = new List<Centro>();

                    for (int i = 0; i < 10; i++)
                    {
                        centros.Add(new Centro
                        {
                            Id = Guid.NewGuid(),
                            Nombre = $"Centro de estudios {i}"
                        });
                    }

                    var asignaturas = new List<Asignatura>();
                    for (int i = 0; i < 20; i++)
                    {
                        asignaturas.Add(new Asignatura
                        {
                            Id = Guid.NewGuid(),
                            Nombre = $"Asignarura {i}",
                            Creditos = i % 2 + 1
                        });
                    }

                    var apellidos = new List<string>() { "García", "González", "Rodríguez", "Fernández", "López", "Martínez", "Sánchez", "Pérez", "Gómez", "Martin", "Jiménez", "Ruiz", "Hernández", "Diaz", "Moreno", "Muñoz", "Álvarez", "Romero", "Alonso", "Gutiérrez", "Navarro", "Torres", "Domínguez", "Vázquez", "Ramos", "Gil", "Ramírez", "Serrano", "Blanco", "Molina", "Morales", "Suarez", "Ortega", "Delgado", "Castro", "Ortiz", "Rubio", "Marín", "Sanz", "Núñez", "Iglesias", "Medina", "Garrido", "Cortes", "Castillo", "Santos", "Lozano", "Guerrero", "Cano", "Prieto", "Méndez", "Cruz", "Calvo", "Gallego", "Vidal", "León", "Márquez", "Herrera", "Peña", "Flores", "Cabrera", "Campos", "Vega", "Fuentes", "Carrasco", "Diez", "Caballero", "Reyes", "Nieto", "Aguilar", "Pascual", "Santana", "Herrero", "Lorenzo", "Montero", "Hidalgo", "Giménez", "Ibáñez", "Ferrer", "Duran", "Santiago", "Benítez", "Mora", "Vicente", "Vargas", "Arias", "Carmona", "Crespo", "Román", "Pastor", "Soto", "Sáez", "Velasco", "Moya", "Soler", "Parra", "Esteban", "Bravo", "Gallardo", "Rojas" };
                    var nombres = new List<string>() { "Borja", "Ilia", "Valentín", "Julio", "Jorge", "Miguel Angel", "Alberto", "Ana", "Berta", "Carmen", "Ginebra" };

                    foreach (var centro in centros)
                    {
                        var counter = 0;

                        for (int i = 0; i < 100; i++)
                        {
                            var alumno = new Alumno
                            {
                                Id = Guid.NewGuid(),
                                Nombre = nombres[(i * 11) / 100],
                                Apellido1 = apellidos[i],
                                Apellido2 = apellidos[apellidos.Count - i - 1],
                                FechaNacimiento = new DateTime((2012 - counter * 3), i % 12 + 1, i % 28 + 1),
                                Centro=centro,
                            };


                            asignaturas.Skip(counter).ToList().ForEach(x =>
                            {

                                context.AlumnosAsignaturas.Add(new AlumnoAsignatura
                                {
                                    Id = Guid.NewGuid(),
                                    Asignatura = x,
                                    Alumno = alumno
                                });
                            });


                            context.Alumnos.Add(alumno);
                        }

                        counter++;
                    }

                    context.SaveChanges();
                }
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.Select().Filter().Count().Expand().OrderBy().MaxTop(100);
                routeBuilder.MapODataServiceRoute("api", "api", GetEdmModel());
            });

            IEdmModel GetEdmModel()
            {
                var odataBuilder = new ODataConventionModelBuilder();

                //Registramos los controladores
                odataBuilder.EntitySet<Centro>("Centros");
                odataBuilder.EntitySet<Asignatura>("Asignaturas");

                var alumno =odataBuilder.EntitySet<Alumno>("Alumnos");

                // New code:
                odataBuilder.Function("Alumnos({key})/Centro").Returns<IActionResult>();
                odataBuilder.Function("Alumnos({key})/AlumnoAsignaturas").Returns<IActionResult>();
                odataBuilder.Function("Centros({key})/Alumnos").Returns<IActionResult>();


                return odataBuilder.GetEdmModel();
            }
        }
    }
}
