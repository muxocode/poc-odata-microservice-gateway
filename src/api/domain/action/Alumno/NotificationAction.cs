using crossapp.action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace host.domain.action.Alumno
{
    public class NotificationAction : IDeleteAction<entities.Alumno>, IInsertAction<entities.Alumno>, IUpdateAction<entities.Alumno>
    {
        Action IDeleteAction<entities.Alumno>.Create(entities.Alumno obj) => 
            () => Console.WriteLine($"El alumno {obj.Nombre} con ID {obj.Id} ha sido eliminado");

        Action IInsertAction<entities.Alumno>.Create(entities.Alumno obj) =>
            () => Console.WriteLine($"El alumno {obj.Nombre} con ID {obj.Id} ha sido insertado");

        Action IUpdateAction<entities.Alumno>.Create(entities.Alumno obj) =>
            () => Console.WriteLine($"El alumno {obj.Nombre} con ID {obj.Id} ha sido actualizado");
    }
}
