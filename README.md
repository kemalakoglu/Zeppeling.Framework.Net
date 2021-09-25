## Zeppeling.Net
Zeppeling.Infrastructure .Net 5.x support !

## IoC
ASP.NET Core Dependency <br/>

## Principles
SOLID <br/>
Domain Driven Design<br/>

## Persistance
EntityFramework Core<br/>
Dapper<br/>

## Object Mappers
AutoMapper

## Cache
In-Memory
Redis

## Object Validation
FluentValidation

## Log
Serilog support
Elasticsearch
Kibana

## Documentation
Swagger

## CQRS
Mediatr

## Benefits
 - Conventional Dependency Registering
 - Async await first 
 - Multi Tenancy
 - GlobalQuery Filtering
 - Domain Driven Design Concepts
 - Repository and UnitofWork pattern implementations
 - Object to object mapping with abstraction
 - .Net 5.x support
 - Auto object validation support
 - Aspect Oriented Programming
 - Simple Usage
 - Modularity
 - Event Sourcing
 
   

***Basic Usage***

     WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>();
                         
***MultiTenancy Activation***

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
					
					
***Conventional Registration***	 	

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
            services.AddScoped<IDealerRepository, DealerRepository>();
        }

        public static void ConfigureMediatr(this IServiceCollection services)
        {
            services.AddScoped<IDealerEventHandler, DealerEventHandler>();
            services.AddMediatR(typeof(DealerServiceHandler).GetTypeInfo().Assembly);
        }

***FluentValidators Activation***

    public static class FluentValidator
    {
        public static void ConfigureFluentValidation(this IServiceCollection services)
        {
            services.AddTransient<IValidator<Dealer>, DealerValidator>();
        }
    }

  
***AutoMapper Activation***

    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<RC, GetRCByIdResponseDTO>().ForMember(x=>x.Id, opt=>opt.Ignore());
        }
    }
	 
***Swagger Activation***

	 services.ConfigureSwagger();
     app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/ZeppelingInfrastructure/swagger.json", "ZeppelingInfrastructure"); });


***Serilog Activation***

		
		 Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .Enrich.WithProperty("Application", "app")
               .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
               .MinimumLevel.Override("System", LogEventLevel.Warning)
               //.WriteTo.File(new JsonFormatter(), "log.json")
               //.ReadFrom.Configuration(configuration)
               .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("localhost:9200"))
               {
                   AutoRegisterTemplate = true,
                   AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                   FailureCallback = e => Console.WriteLine("fail message: " + e.MessageTemplate),
                   EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                      EmitEventFailureHandling.WriteToFailureSink |
                                      EmitEventFailureHandling.RaiseCallback,
                   FailureSink = new FileSink("log" + ".json", new JsonFormatter(), null)
               })
               .MinimumLevel.Verbose()
               .CreateLogger();
           Log.Information("WebApi Starting...");
		
		

***ErrorHandlingMiddleware Interceptor Activation***

     public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                Log.Write(LogEventLevel.Information, "Service path is:" + context.Request.Path.Value,
                    context.Request.Body);
                await next(context);
            }
            catch (HttpRequestException ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message, "Service path is:" + context.Request.Path.Value, ex);
                await HandleExceptionAsync(context, ex);
            }
            catch (AuthenticationException ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message, "Service path is:" + context.Request.Path.Value, ex);
                await HandleExceptionAsync(context, ex);
            }
            catch (BusinessException ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message, "Service path is:" + context.Request.Path.Value, ex);
                await HandleExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                Log.Write(LogEventLevel.Error, ex.Message, ex.Source, ex.TargetSite, ex);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, object exception)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected
            var message = string.Empty;
            var RC = string.Empty;

            if (exception.GetType() == typeof(HttpRequestException))
            {
                code = HttpStatusCode.NotFound;
                RC = ResponseMessage.NotFound;
                message = BusinessException.GetDescription(RC);
            }
            else if (exception.GetType() == typeof(AuthenticationException))
            {
                code = HttpStatusCode.Unauthorized;
                RC = ResponseMessage.Unauthorized;
                message = BusinessException.GetDescription(RC);
            }
            else if (exception.GetType() == typeof(BusinessException))
            {
                var businesException = (BusinessException) exception;
                message = BusinessException.GetDescription(businesException.RC, businesException.param1);
                code = HttpStatusCode.InternalServerError;
                RC = businesException.RC;
            }
            else if (exception.GetType() == typeof(Exception))
            {
                code = HttpStatusCode.BadRequest;
                RC = ResponseMessage.BadRequest;
                message = BusinessException.GetDescription(RC);
            }

            var response = new Error
            {
                Message = message,
                RC = RC
            };
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            return context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
	
	
***Commands definitions***

public class DealerCommands:IDealerCommands
    {
        private readonly IMediator mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DealerCommands"/> class.
        /// </summary>
        /// <param name="mediator">The mediator.</param>
        public DealerCommands(IMediator mediator)
        {
            this.mediator = mediator;
        }

        /// <summary>
        /// Inserts the dealer.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<bool> InsertDealer(InsertDealerRequestDTO request)
        {
            return this.mediator.Send(request).Result;
        }

        /// <summary>
        /// Updates the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public async Task<ResponseDTO> UpdateDealer(DealerDTO request)
        {
            bool result = this.mediator.Send(request).Result;
            return await CreateAsyncResponse<ResponseDTO>.Return(result, "UpdateDealer");
        }
    }
	
***Handlers definitions***	
		 public class DealerHandler : IRequestHandler<InsertDealerRequestDTO, bool>, IRequestHandler<DealerDTO, bool>
    {
        private readonly IMapper mapper;
        private readonly IDealerRepository dealerRepository;
        public DealerHandler(IMapper mapper, IDealerRepository dealerRepository)
        {
            this.mapper = mapper;
            this.dealerRepository = dealerRepository;
        }

        /// <summary>
        /// Handles a request
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// Response from the request
        /// </returns>
        [UnitOfWork]
        public async Task<bool> Handle(InsertDealerRequestDTO request, CancellationToken cancellationToken)
        {
            await this.dealerRepository.Insert(mapper.Map(request, new Dealer()));
            return true;
            //this.unitOfWork.Repository<Dealer>().Create(mapper.Map(request, new Dealer()));
            //this.unitOfWork.EndTransaction();
        }

        /// <summary>
        /// Handles a request
        /// </summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// Response from the request
        /// </returns>
        [UnitOfWork]
        public async Task<bool> Handle(DealerDTO request, CancellationToken cancellationToken)
        {
            //Dealer entity = this.unitOfWork.Repository<Dealer>().GetByKey(request.Id);
            //entity.Update(request.Status, request.IsActive, request.DealerCode, request.DealerName, request.Desciription, request.Address, request.Phone, request.AccountNo);
            //this.unitOfWork.Repository<Dealer>().Update(entity);
            //this.unitOfWork.EndTransaction();
            return true;
        }
    }

***Base Repository Definitions***

       public interface IRepository<TEntity, T> where TEntity : EntityBase<T>
    {
        void Delete(TEntity entity);
        Task<TEntity> Get(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> GetAll();
        Task<IEnumerable<TEntity>> GetList(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> GetList(Expression<Func<TEntity, bool>> expression, string include);
        IEnumerable<TEntity> GetPage(Expression<Func<TEntity, bool>> expression, int pageIndex, int pageSize, out int totalCount, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        IEnumerable<TDomainObject> GetPage<TDomainObject>(IQueryable<TDomainObject> query, int pageIndex, int pageSize, out int totalCount) where TDomainObject : class;
        Task<TEntity> Insert(TEntity entity);
        Task<TEntity> Update(TEntity entity);
    }

***Custom Repository definitions***
 

  public class DealerRepository : Repository<Dealer,long>, IDealerRepository
    {
        private readonly IUnitOfWorkFactory UnitOfWorkFactory;
        public DealerRepository(
    IUnitOfWorkFactory unitOfWorkFactory)
    : base(unitOfWorkFactory)
        {
            this.UnitOfWorkFactory = unitOfWorkFactory;
        }

        public void Delete(Dealer entity)
        {
            throw new NotImplementedException();
        }

        public async Task<Dealer> Get(Expression<Func<Dealer, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Dealer>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Dealer>> GetList(Expression<Func<Dealer, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Dealer>> GetList(Expression<Func<Dealer, bool>> expression, string include)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Dealer> GetPage(Expression<Func<Dealer, bool>> expression, int pageIndex, int pageSize, out int totalCount, Func<IQueryable<Dealer>, IOrderedQueryable<Dealer>> orderBy = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TDomainObject> GetPage<TDomainObject>(IQueryable<TDomainObject> query, int pageIndex, int pageSize, out int totalCount) where TDomainObject : class
        {
            throw new NotImplementedException();
        }

        public async Task<Dealer> Insert(Dealer entity)
        {
            Dealer response = new Dealer();
            IsolationLevel isoLevel = IsolationLevel.ReadCommitted;//(customAttributes[0] as UnitOfWorkAttribute).IsoLevel;
            bool entityLazyLoad = true;//(customAttributes[0] as UnitOfWorkAttribute).EntityLazyLoad;

            using (IUnitOfWork uow = this.UnitOfWorkFactory.Create(
                "6019B864-E0EC-4E50-A574-CA3995D1B541",
                isoLevel,
                entityLazyLoad))
            {
                try
                {
                    uow.Begin(false);
                    response = await base.Insert(entity);
                    uow.Commit();
                }
                catch
                {
                    throw;
                }

            }

            return response;

        }

        public async Task<Dealer> Update(Dealer entity)
        {
            throw new NotImplementedException();
        }
    }


***Context Definitions***
   

     public class DomainContext : DbContext
    {
        private readonly IConfiguration Config;

        public DomainContext(
            IConfiguration config)
            : base()
        {
            this.Config = config;
        }

        public DomainContext(DbContextOptions<DomainContext> options,
            IConfiguration config)
            : base(options)
        {
            this.Config = config;
        }

        public virtual DbSet<Dealer> Dealer { get; set; }
        public virtual DbSet<POS> POS { get; set; }
        public virtual DbSet<UserSettings> UserSettings { get; set; }
        public virtual DbSet<VersionLoad> VersionLoad { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                ConnectionType domainContext = this.Config.GetSection("DomainContext:0").Get<ConnectionType>();

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
        }
    }
     

