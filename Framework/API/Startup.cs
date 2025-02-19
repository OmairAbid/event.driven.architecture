using Application.BoundedContexts.UserAccountManagement.Commands;
using Application.BoundedContexts.UserAccountManagement.QueryObjects;
using Domain.BoundedContexts.UserAccountManagement.Aggregates;
using EventSourcingRepository.Repository;
using EventSourcingRepository.Configuration;
using QueryRepository.Repository;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using Authentication.Configuration;
using Authentication.Repository;
using QueryRepository.Configuration;
using Application.Pipelines;
using Domain.BoundedContexts.TourPricingManagement.Aggregates;
using API.Extensions;
using API.Middleware;

namespace API
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
            services.AddControllers();
            services.AddHttpContextAccessor();
            services.AddSwaggerDocumentation();

            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            services.AddJwtBasedAuth(Configuration.GetSection("JwtSettings").Get<JwtSettings>());
            services.AddEventStoreDbService(Configuration.GetSection("EventStoreDb").Get<EventStoreDbSettings>());
            services.AddIdenityServices(Configuration.GetSection("MySqlDb").Get<MySqlSettings>());
            services.AddMongoDbService(Configuration.GetSection("MongoDb").Get<MongoDbSettings>());
            services.AddEmailService();

            services.AddMediatR(typeof(AdminAccountCreateCommand).GetTypeInfo().Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MetricsBehaviour<,>));

            services.AddTransient<IEventSourcingRepository<Admin>, EventSourcingRepository<Admin>>();
            services.AddTransient<IProjectionRepository<AdminInfo>, MongoDbRepository<AdminInfo>>();
            services.AddTransient<IQueryRepository<AdminInfo>, MongoDbRepository<AdminInfo>>();

            services.AddTransient<IEventSourcingRepository<Customer>, EventSourcingRepository<Customer>>();
            services.AddTransient<IProjectionRepository<CustomerInfo>, MongoDbRepository<CustomerInfo>>();
            services.AddTransient<IQueryRepository<CustomerInfo>, MongoDbRepository<CustomerInfo>>();

            services.AddTransient<IEventSourcingRepository<TourStopPricingModel>, EventSourcingRepository<TourStopPricingModel>>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IdentityContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseGlobalExceptionMiddleware();
            }

            app.UseSwaggerDocumentation();
            app.UseIdentityMySqlMigration(context);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseJwtBasedAuth();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


    }
}
