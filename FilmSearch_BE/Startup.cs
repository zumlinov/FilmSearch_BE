using AutoMapper;
using FilmSearch_BE.Binders;
using FilmSearch_BE.Filters;
using FilmSearchClasses.Extensions;
using FilmSearchClasses.Mappers;
using FilmSearchClasses.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;

namespace FilmSearch_BE
{
    public class Startup
    {
        //private const string AllowedOrigins = "_allowedOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                                  {
                                      builder.WithOrigins("http://localhost:4200")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                  });
            });

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<FilmSearchMapperProfile>();
            });

            mapperConfig.AssertConfigurationIsValid();
            services.AddTransient(x => mapperConfig.CreateMapper());

            services.Configure<OpenMovieDtabaseSettings>(Configuration.GetSection(OpenMovieDtabaseSettings.SectionName));

            services.AddFilmSearchService();
            services.AddHttpClient();

            services.AddMvc(opt => opt.Filters.Add<AsyncExceptionFilter>());
            services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
                .AddMvcOptions(opt =>
                {
                    opt.ModelBinderProviders.Insert(0, new FilmSearchCriteriaModelBinderProvider());
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "FilmSearch_BE", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "FilmSearch_BE v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
