using Zeppeling.Framework.Core.Abstraction;
using Zeppeling.Framework.Core.UnitOfWork;
using Zeppeling.Infrastructure.Core.Configuration;
using Zeppeling.Infrastructure.Core.Contract;
using Zeppeling.Infrastructure.Core.Enumeration;
using Zeppeling.Infrastructure.Domain.Context.Context;
using Zeppeling.Infrastructure.Domain.Handlers;
using Zeppeling.Infrastructure.Application.Contract.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Zeppeling.Infrastructure.Application.Commands;
using Zeppeling.Infrastructure.Domain.Aggregate.ResponseCodes;
using AutoMapper.Execution;
using Castle.DynamicProxy;
using System.Linq;
using Zeppeling.Infrastructure.Core.Contract.Interceptors;

namespace Zeppeling.Infrastructure.Presentation.API.Extensions
{
    public static class DependencyInjection
    {
        public static void ConfigureContext(this IServiceCollection services, IConfiguration config)
        {
            ConnectionType domainContext = config.GetSection("DomainContext:0").Get<ConnectionType>();

            switch (domainContext.DbType)
            {
                case DatabaseProviders.SqlServer:
                    services.AddDbContext<DomainContext>(o => o.UseSqlServer(domainContext.ConnectionString));
                    break;

                case DatabaseProviders.Oracle:
                    services.AddDbContext<DomainContext>(o => o.UseOracle(domainContext.ConnectionString));
                    break;

                case DatabaseProviders.PostgreSql:
                    services.AddDbContext<DomainContext>(o => o.UseNpgsql(domainContext.ConnectionString));
                    break;

                case DatabaseProviders.MySql:
                    services.AddDbContext<DomainContext>(o => o.UseMySQL(domainContext.ConnectionString));
                    break;

                case DatabaseProviders.Sqlite:
                    services.AddDbContext<DomainContext>(o => o.UseSqlite(domainContext.ConnectionString));
                    break;

                default:
                    break;
            }
        }

        public static void ConfigureUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IDbObjectFactory, DbObjectFactory>();
            services.AddScoped<IDbObject, ZeppelingInfrastructureContextFactory>();
            services.AddScoped<IZeppelingInfrastructureConfiguration, ZeppelingInfrastructureConfiguration>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUnitOfWorkFactory, UnitOfWorkFactory>();
        }

        public static void ConfigureRepository(this IServiceCollection services)
        {
            services.AddProxiedScoped<IRCRepository, RCRepository>();
        }

        public static void ConfigureMediatr(this IServiceCollection services)
        {
            services.AddScoped<IApplicationCommandQuery, ApplicationCommandQuery>();
            services.AddMediatR(typeof(RCCommandHandler).GetTypeInfo().Assembly);
        }

        public static void ConfigureInterceptors(this IServiceCollection services)
        {
            services.AddSingleton(new Castle.DynamicProxy.ProxyGenerator());
            services.AddScoped<IInterceptor, UnitOfWorkInterceptor>();
            //services.AddScoped<IInterceptor, LoggerAspect>();
        }

        public static void AddProxiedScoped<TInterface, TImplementation>(this IServiceCollection services)
            where TInterface : class
            where TImplementation : class, TInterface
        {
            services.AddScoped<TImplementation>();
            services.AddScoped(typeof(TInterface), serviceProvider =>
            {
                var proxyGenerator = serviceProvider.GetRequiredService<Castle.DynamicProxy.ProxyGenerator>();
                var actual = serviceProvider.GetRequiredService<TImplementation>();
                var interceptors = serviceProvider.GetServices<IInterceptor>().ToArray();
                return proxyGenerator.CreateInterfaceProxyWithTarget(typeof(TInterface), actual, interceptors);
            });
        }
    }
}