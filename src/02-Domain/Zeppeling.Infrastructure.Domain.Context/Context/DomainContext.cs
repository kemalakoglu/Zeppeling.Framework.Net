using Zeppeling.Infrastructure.Core.Contract;
using Zeppeling.Infrastructure.Core.Enumeration;
using Zeppeling.Infrastructure.Domain.Context.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Zeppeling.Infrastructure.Domain.Context.Context
{
    public class DomainContext : DbContext
    {
        public DomainContext()
        {
        }

        public DomainContext(DbContextOptions<DomainContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {

                ConnectionType domainContext = GetConnectionType.Get("DomainContext");

                switch (domainContext.DbType)
                {
                    case DatabaseProviders.SqlServer:
                        optionsBuilder.UseSqlServer(domainContext.ConnectionString);
                        break;

                    case DatabaseProviders.Oracle:
                        optionsBuilder.UseOracle(domainContext.ConnectionString);
                        break;

                    case DatabaseProviders.PostgreSql:
                        optionsBuilder.UseNpgsql(domainContext.ConnectionString);
                        break;

                    case DatabaseProviders.MySql:
                        optionsBuilder.UseMySQL(domainContext.ConnectionString);
                        break;

                    case DatabaseProviders.Sqlite:
                        optionsBuilder.UseSqlite(domainContext.ConnectionString);
                        break;

                    default:
                        break;
                }
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.BuildModels();
        }
    }
}