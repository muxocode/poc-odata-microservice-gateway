using crossapp.action;
using crossapp.repository;
using crossapp.rule;
using crossapp.unitOfWork;
using entities;
using host.domain.action.Alumno;
using host.domain.repostory._base;
using host.domain.rules;
using host.domain.rules.Alumno;
using host.domain.unitofwork;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace host.domain
{
    public static class Extension
    {
        public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            return services.AddScoped<IUnitOfWork, AlexiaUnitOfWork>();
        }

        public static IServiceCollection AddAlumno(this IServiceCollection services)
        {
            return services
                //Actions
                .AddTransient<IInsertAction<Alumno>, NotificationAction>()
                .AddTransient<IUpdateAction<Alumno>, NotificationAction>()
                .AddTransient<IDeleteAction<Alumno>, NotificationAction>()

                .AddTransient<IUpdateAction<Alumno>, IsAliveAction>()

                //Rules
                .AddTransient<IRule<Alumno>, BirthJuniorRule>()
                .AddTransient<IRule<Alumno>, NameFillRule>()
                .AddTransient<IRule<Alumno>, NameLenghtRule>()

                //RuleProcessor
                .AddTransient<IRuleProcessor<Alumno>, RuleProcessor<Alumno>>()

                //Repository
                .AddTransient<IRepository<Alumno>, RepositoryGeneric<Alumno>>();
        }

        public static IServiceCollection AddCentro(this IServiceCollection services)
        {
            return services
                //Actions

                //Rules

                //RuleProcessor
                .AddTransient<IRuleProcessor<Centro>, RuleProcessor<Centro>>()

                //Repository
                .AddTransient<IRepository<Centro>, RepositoryGeneric<Centro>>();
        }

        public static IServiceCollection AddAsigntura(this IServiceCollection services)
        {
            return services
                //Actions

                //Rules

                //RuleProcessor
                .AddTransient<IRuleProcessor<Asignatura>, RuleProcessor<Asignatura>>()

                //Repository
                .AddTransient<IRepository<Asignatura>, RepositoryGeneric<Asignatura>>();
        }
    }
}
