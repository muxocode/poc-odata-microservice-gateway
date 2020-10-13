# POC ODATA
## Arranque
### Pruebas a controladores
https://www.getpostman.com/collections/e93903519b2d848e88f2
## Arquitectura
La arquitectura propuesta se basa en los siguientes conceptos:
* __Api restfull__: Mediante las llamadas GET/POST/PATCH/PUT/DELETE se crea de forma intuitiva toda la funcionalidad
* __ODATA__: Mediante el uso de entity framework y odata, se pemiten hacer busquedas por cabecera. *ej: Alumnos que ocmiencen por M*, sin implementar código
* __CQRS__: Utilización de un *dataContext* ligero para búsquedas y en otro pesado para mantenimiento
* __.net core__: Dado que tiene su propio motor de IOC, permite inyectar en los diferentes controladores los objetos de dominio, de tal maneta que se separa la arquitectura de la funcionalidad. Por otro lado al poder diferenciarse la capa HOST de la capa API, permite diferentes configuraciones o agrupaciones, aumentando así la capa de diferenciación en DDD.
* __DDD__: ([ref](https://es.wikipedia.org/wiki/Dise%C3%B1o_guiado_por_el_dominio)) Usando los beneficios de la arquitectura hexagonal, el modelo vendría dado por ainterfaces diseñadas en *crossapps* y cada aplicación implementaría su propio dominio.
* __POA__: *[Programación orientada a aspectos](https://es.wikipedia.org/wiki/Programaci%C3%B3n_orientada_a_aspectos)* (en inglés: aspect-oriented programming) es un paradigma de programación que permite una adecuada modularización de las aplicaciones y posibilita una mejor separación de responsabilidades (Obligación o correspondencia de hacer algo).
### Principios de diseño implementados
* KISS
* YAGNI
* DRY
* POLA
* S.O.L.I.D.
* CQS
* IOC
* LoD o "don't talk with extrangers"
### Patrones de diseño implementados
* UnitOfWork
* Repository
* CQRS
* Inyección de dependencias
* Template
### GET
Se basa en ODATA + EntityFramework (cliente ligero)
![File5](doc/img/file5.png)
### POA en este contexto
Mediante el IOC, se establacen las siguientes interfaces
* Reglas: Tiene que ver con la aceptación de entidades antes de insertar/modificar. *ej: ¿EL usuario es mayor de edad?¿El nombre viene relleno??*
* Transformaciones: Hace referencia al trato especializado de una entidad según el negocio. *ej: ¿Cómo se genera la facturación en Chile?¿Cómo se calcula el descuento?*
* Acciones de Base de Datos: Su especialización trata en comprobaciones de tablas/campos para lanzar dentro de una transacción. *ej: Controles de concurrencia. Acceso a sistemas externos*

##### POST/PUT/PATCH
![File4](doc/img/file4.png)
1. Lo primero, una vez que llega la entidad a petición se ejecuta la validez del modelo, si va todo bien pasamos el siguiente paso
2. El controlador pide al __repositorio__ que añada/modifique una nueva entidad
* Se ejecutan las reglas
* Se aplican las transformaciones
* Se añaden las acciones al unitOfWork
3. Se lanza UnitOfWork, que engloba los cambios sobre BBDD y las acciones añadidas desde el reporitorio

##### DELETE
![File6](doc/img/file6.png)
1. El controlador pide al __repositorio__ que borre una nueva entidad
* Se añaden las acciones al unitOfWork
3. Se lanza UnitOfWork, que engloba los cambios sobre BBDD y las acciones añadidas desde el reporitorio


### Proyectos en este entorno
![File7](doc/img/file7.png)

## Cómo crear referencias a un servicio ODATA
![File1](doc/img/file1.png)
![File2](doc/img/file2.png)
![File3](doc/img/file3.png)

## Beneficios
* __IOC__: Dada la arquitectura propuesta, los paquetes de reglas/transformaciones/acciones se pueden desplegar como un punto de DevOps, de tal manera que se pueden implementar a parte.
* __AGILE__: El modelo presenta una "agilidad" sin igual gracias a la suma de POA + IOC + ODATA

## REFS
https://www.odata.org/documentation/odata-version-2-0/uri-conventions/
https://docs.microsoft.com/en-us/odata/
