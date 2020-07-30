using CallForCodeApi.Model;
using IBM.EntityFrameworkCore;
using IBM.EntityFrameworkCore.Storage.Internal;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CallForCodeApi
{
    public class Startup
    {
        //public IConfigurationRoot Configuration { get; }

        //public Startup(IHostingEnvironment env)
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(env.ContentRootPath)
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
        //        .AddJsonFile("vcap-local.json", optional: true); // when running locally, store VCAP_SERVICES credentials in vcap-local.json

        //    Configuration = builder.Build();
        //}

        public Startup(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }
        private Microsoft.AspNetCore.Hosting.IWebHostEnvironment _env;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //var connection = "Server=dashdb-txn-sbox-yp-lon02-07.services.eu-gb.bluemix.net:50001;Database=BLUDB;UID=wxg76063;PWD=2+rrdsjwr2t51532;Security=SSL;";
            //var connection = "data source=tcp:IDCSQL2.znetlive.com,1234;database=CallForCode;uid=abahan123;pwd=Abahan1234@;Connection Timeout=120;";
            // services.AddDbContext<DatabaseContext>(options => options.UseDb2(connection, p => p.SetServerInfo(IBMDBServerType.AS400,IBMDBServerVersion.None)));
            // services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(connection));
            services.AddDbContext<DatabaseContext>();
            services.AddMvc();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "My First ASP.NET Core Web API",
                    TermsOfService = "None"
                    //Contact = new Contact() { Name = "Talking Dotnet", Email = "contact@talkingdotnet.com", Url = "www.talkingdotnet.com" }
                });
            });
            //services.AddCors(c =>
            //{
            //    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            //});

            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("MyPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
