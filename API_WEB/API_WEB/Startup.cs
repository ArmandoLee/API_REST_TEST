using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

namespace API_WEB
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApiDbContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("BaseDatosConnection")));


            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API GESTION DE CUENTAS, MOVIMIENTOS Y DE CLIENTES", Version = "v1" });
            });

            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API GESTION DE CUENTAS, MOVIMIENTOS Y DE CLIENTES");
                });
            }
            else
            {
                app.UseExceptionHandler("/error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
