# POC ODATA
## 1.Arranque
Para arrancar el proyecto solo se necesita cambiar el connection-string de la bbdd (la bbdd se crea ya que es code first)
![File8](doc/img/file8.png)
Ya por otro lado, habr�a que arrancar los dos proyectos que se indican a continuaci�n
![File12](doc/img/file12.png)
### 1.1.Pruebas a controladores
* https://www.getpostman.com/collections/e93903519b2d848e88f2
* https://localhost:44307/swagger
## 2.Arquitectura
La arquitectura propuesta se basa en los siguientes conceptos:
* __Api restfull__: Mediante las llamadas GET/POST/PATCH/PUT/DELETE se crea de forma intuitiva toda la funcionalidad
* __ODATA__: Mediante el uso de entity framework y odata, se pemiten hacer busquedas por cabecera. *ej: Alumnos que ocmiencen por M*, sin implementar c�digo
* __CQRS__: Utilizaci�n de un *dataContext* ligero para b�squedas y en otro pesado para mantenimiento
* __.net core__: Dado que tiene su propio motor de IOC, permite inyectar en los diferentes controladores los objetos de dominio, de tal maneta que se separa la arquitectura de la funcionalidad. <br/>Por otro lado al poder diferenciarse la capa HOST de la capa API, permite diferentes configuraciones o agrupaciones, aumentando as� la capa de diferenciaci�n en DDD.<br/>Comentar adem�s, que al estar basado en core es multiplataforma, modular (solo se trae lo que necesita), orientado a los comandos (todo una ventaja para devops) y __est� orientado al rendimiento y a microservicios__
* __DDD__: ([ref](https://es.wikipedia.org/wiki/Dise%C3%B1o_guiado_por_el_dominio)) Usando los beneficios de la arquitectura hexagonal, el modelo vendr�a dado por interfaces dise�adas en *crossapps* y cada aplicaci�n implementar�a su propio dominio.
* __POA__: *[Programaci�n orientada a aspectos](https://es.wikipedia.org/wiki/Programaci%C3%B3n_orientada_a_aspectos)* (en ingl�s: aspect-oriented programming) es un paradigma de programaci�n que permite una adecuada modularizaci�n de las aplicaciones y posibilita una mejor separaci�n de responsabilidades (Obligaci�n o correspondencia de hacer algo).
* __CODE FIRST__: El sistema se despliegua autom�ticamente, y crea/modifica la base de datos en el acto.
### 2.1.Principios de dise�o implementados
* KISS
* YAGNI
* DRY
* POLA
* S.O.L.I.D.
* CQS
* IOC
* LoD o "don't talk with extrangers"
### 2.2.Patrones de dise�o implementados
* UnitOfWork
* Repository
* CQRS
* Inyecci�n de dependencias
* Template
### 2.3.Llamadas a la api
#### 2.3.1.GET
Se basa en ODATA + EntityFramework (cliente ligero)
![File5](doc/img/file5.png)
#### 2.3.2.Llamadas para modificaciones
En este caso se usa el cliente pesado, que permite la trazabilidad de objetos
##### 2.3.2.1.POA en este contexto
Mediante el IOC, se establacen las siguientes interfaces
* Reglas: Tiene que ver con la aceptaci�n de entidades antes de insertar/modificar. *ej: �EL usuario es mayor de edad?�El nombre viene relleno??*
* Transformaciones: Hace referencia al trato especializado de una entidad seg�n el negocio. *ej: �C�mo se genera la facturaci�n en Chile?�C�mo se calcula el descuento?*
* Acciones de Base de Datos: Su especializaci�n trata en comprobaciones de tablas/campos para lanzar dentro de una transacci�n. *ej: Controles de concurrencia. Acceso a sistemas externos*

##### 2.3.2.2.POST/PUT/PATCH
![File4](doc/img/file4.png)
1. Lo primero, una vez que llega la entidad a petici�n se ejecuta la validez del modelo, si va todo bien pasamos el siguiente paso
2. El controlador pide al __repositorio__ que a�ada/modifique una nueva entidad
* Se ejecutan las reglas
* Se aplican las transformaciones
* Se a�aden las acciones al unitOfWork
3. Se lanza UnitOfWork, que engloba los cambios sobre BBDD y las acciones a�adidas desde el repositorio

##### 2.3.2.3.DELETE
![File6](doc/img/file6.png)
1. El controlador pide al __repositorio__ que borre una nueva entidad
* Se a�aden las acciones al unitOfWork
3. Se lanza UnitOfWork, que engloba los cambios sobre BBDD y las acciones a�adidas desde el repositorio


### 2.4.Proyectos en este entorno
#### 2.4.1.NuGets
![File7](doc/img/file7.png)
* CrossApp: Interfaces que hacen de modelo, sirve para la comunicaci�n entre todos los NuGets y aplicativos
* Entities: Entidades de base de datos
#### 2.4.2.MicroServicio
![File9](doc/img/file9.png)
Dado que estamos en .net core, nos permite diferenciar entre el host y los controladores. De este modo podemos tener diferentes HOST con diferenets configuraciones o agrupaciones.
* Data: Contiene el contexto de "Entity Framework", tanto el cliente ligero como el pesado
* Alumnos.api: Se generan los controladores (Todos heredan de una clase BASE, por lo que el c�digo es m�nimo o en *override*)
* Host: Contiene la inyecci�n de dependencias (y el dominio) que se realiza en el *startup.cs*
![File10](doc/img/file10.png)
NOTA: El dominio podr�a ir en un paquete a parte y cargarlo dirante el proceso de DevOps
#### 2.4.3.GateWay
Prueba de concepto de *api gateway* que permite fusionar varios microservicios

Se han creado varias queries como prueba de concepto

![File11](doc/img/file11.png)

##### 2.4.3.1.C�mo crear referencias a un servicio ODATA
![File1](doc/img/file1.png)
![File2](doc/img/file2.png)
![File3](doc/img/file3.png)

## 3.Beneficios
* __IOC__: Dada la arquitectura propuesta, los paquetes de reglas/transformaciones/acciones se pueden desplegar como un punto de DevOps, de tal manera que se pueden implementar a parte.
* __AGILE__: El modelo presenta una "agilidad" sin igual gracias a la suma de POA + IOC + ODATA
* __Code First__: El despliegue es autom�tico, ideal para levantar y "tirar" microservicios, orientado a la eficiencia y a la multiplataforma.


## 4.TODO
* Que los archivos de MIGRATION de Code-first vayan dentro del proyecto de data
* Temas de autenticaci�n y permisos
* Temas de identificaci�n del usuario

## 5.Estimaci�n de tiempos
En este apartado se pretende hacer una estimaci�n de tiempo (lo m�s real posible) para terminar de entender los beneficios del modelo.
* �Qu� hago si necesito a�adir un nuevo controlador y una nueva entidad?
1. Creamos la entidad en *entities*
2. A�adimos la entidad en *data*, a modo de colecci�n en el context
3. Lanzamos una nueva migraci�n con *"add-migration XXX"* 
4. Creamos un controlador en *alumnos.api* y que hereda de base
5. Creamos las reglas, acciones y transformaciones en *host*, y a�adimos la inyecci�n en *host/startup.cs*
<br/>__TOTAL:__ *max 1 hora* (con despliegue de base de datos incluido)

* �Qu� hago si necesito a�adir una nueva regla de comprobaci�n antes de guardar?
1. A�adimos la regla en *host/domain/\<entity>/rules*
2. A�adimos la inyecci�n en *host/domain/extension*
<br/>__TOTAL:__ *max 10 min*

* �Qu� hago si necesito a�adir una una nueva acci�n sobre una entidad, por ejemplo, modificar la fecha de acceso?
1. A�adimos la transformaci�n en *host/domain/\<entity>/transformations*
2. A�adimos la inyecci�n en *host/domain/extension*
<br/>__TOTAL:__ *max 10 min*

* �Qu� hago si necesito a�adir un nuevo campo en una tabla?
1. Modificamos la entidad en *entities*
2. Lanzamos una nueva migraci�n con *"add-migration XXX"* 
<br/>__TOTAL:__ *max 10 min* (con despliegue de base de datos incluido)

## 6.REFS
* https://www.odata.org/documentation/odata-version-2-0/uri-conventions/
* https://docs.microsoft.com/en-us/odata/
* https://www.campusmvp.es/recursos/post/10-diferencias-entre-net-core-y-net-framework.aspx
* https://docs.microsoft.com/es-es/dotnet/standard/choosing-core-framework-server
