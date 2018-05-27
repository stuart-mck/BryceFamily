using BryceFamily.Repo.PG.Config;
using BryceFamily.Repo.PG.Repository;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BryceFamily.Repo.PG
{
    public static class RegisterDependencies
    {
        public static void RegisterPostgresDependencies(this IServiceCollection services, string environment)
        {
            services.AddSingleton(context => new DataEnvironment(environment));
            services.AddScoped<PersonRepository>();
            services.AddSingleton<ConnectionResolver>();
        }
    }
}
