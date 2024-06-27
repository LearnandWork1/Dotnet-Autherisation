using CoreSample.Model;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace CoreSample
{

    public class ToDoModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Wish { get; set; }
    }
    public class Startup
    {



    //    public static IServiceCollection AddSwaggerGen(
    //this IServiceCollection services,
    //Action<SwaggerGenOptions> setupAction = null)
    //    {
    //        // Add Mvc convention to ensure ApiExplorer is enabled for all actions
    //        services.Configure<MvcOptions>(c =>
    //            c.Conventions.Add(new SwaggerApplicationConvention()));

    //        // Register generator and it's dependencies
    //        services.AddTransient<ISwaggerProvider, SwaggerGenerator>();
    //        services.AddTransient<ISchemaGenerator, SchemaGenerator>();
    //        services.AddTransient<IApiModelResolver, JsonApiModelResolver>();

    //        // Register custom configurators that assign values from SwaggerGenOptions (i.e. high level config)
    //        // to the service-specific options (i.e. lower-level config)
    //        services.AddTransient<IConfigureOptions<SwaggerGeneratorOptions>, ConfigureSwaggerGeneratorOptions>();
    //        services.AddTransient<IConfigureOptions<SchemaGeneratorOptions>, ConfigureSchemaGeneratorOptions>();

    //        // Used by the <c>dotnet-getdocument</c> tool from the Microsoft.Extensions.ApiDescription.Server package.
    //        services.TryAddSingleton<IDocumentProvider, DocumentProvider>();

    //        if (setupAction != null) services.ConfigureSwaggerGen(setupAction);

    //        return services;

    //    }



        IConfiguration configuration;
        public Startup(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public void Configure(IApplicationBuilder app,
                      IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
        public void ConfigureServices(IServiceCollection services)
        {
            // services.AddMvc();
            services.AddDbContext<rentDBContext>(
                options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("CoreSampleCon"));
                });
            //options => options.UseInMemoryDatabase("ToDoList"));
           // services.addsw

        }
    }
}
