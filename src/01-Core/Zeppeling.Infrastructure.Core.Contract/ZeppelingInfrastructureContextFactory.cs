using Zeppeling.Framework.Core.Abstraction;
using Zeppeling.Infrastructure.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace Zeppeling.Infrastructure.Core.Contract
{
    public class ZeppelingInfrastructureContextFactory:IDbObject
    {
        private readonly string ContextCode;
        private readonly bool IsDefaultContext;
        private readonly string ContextName;
        private readonly ZeppelingInfrastructureConfiguration ZeppelingInfrastructureConfiguration;
        private readonly IConfiguration Configuration;

        public ZeppelingInfrastructureContextFactory(
            ZeppelingInfrastructureConfiguration ZeppelingInfrastructureConfiguration, IConfiguration configuration)
        {
            this.ZeppelingInfrastructureConfiguration = ZeppelingInfrastructureConfiguration;
            this.ContextCode = this.ZeppelingInfrastructureConfiguration.ContextKey;
            this.ContextName = this.ZeppelingInfrastructureConfiguration.ContextName;
            this.IsDefaultContext = false;
            this.Configuration = configuration;
        }
        public string Code
        {
            get { return this.ContextCode; }
        }

        public string Name
        {
            get { return this.ContextName; }
        }

        public DbObject DbObject()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(DbContext)))
                    {
                        if (type.Name == this.Name)
                        {
                            return new DbObject
                            {
                                DbProvider = DbProvider.EntityFramework,
                                DbObjectContext = type.GetConstructor(Array.Empty<Type>()).Invoke(Array.Empty<object>())
                            };
                        }
                    }
                }
            }

            return null;
        }

        public bool IsDefault
        {
            get { return this.IsDefaultContext; }
        }

        public void Migrate()
        {
            throw new NotImplementedException();
        }

        public DbObject DbObject(bool entityLazyLoad = false)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(DbContext)))
                    {
                        if (type.Name==this.Name)
                        {
                            //Type[] types = new Type[1];
                            //types[0] = typeof(IConfiguration);
                            //var constructor = type.GetConstructor(types);
                            return new DbObject
                            {
                                DbProvider = DbProvider.EntityFramework,
                                //DbObjectContext = constructor.Invoke(new object[] { this.Configuration })
                                DbObjectContext = type.GetConstructor(Array.Empty<Type>()).Invoke(Array.Empty<object>())
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}
