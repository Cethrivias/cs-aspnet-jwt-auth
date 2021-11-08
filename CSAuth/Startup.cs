using System;
using System.Collections.Generic;
using System.Text;
using CSAuth.Services;
using CSAuth.Settings;
using CSAuth.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace CSAuth {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.AddControllers();
      var jwtSettings = Configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>();
      
      services.AddScoped<IUserService, UserService>();
      services.AddTransient<IJwtIssuer, JwtIssuer>(_ => new JwtIssuer(jwtSettings));

      services.AddAuthentication(
          x => {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
          }
        )
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
          x => {
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters {
              ValidateIssuerSigningKey = true,
              IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
              ValidateIssuer = false,
              ValidateAudience = false,
              RequireExpirationTime = true,
              ValidateLifetime = true,
              ClockSkew = TimeSpan.FromMinutes(1)
            };
          }
        );
      services.AddSwaggerGen(
        c => {
          c.SwaggerDoc("v1", new OpenApiInfo { Title = "CSAuth", Version = "v1" });
          c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
          });
          c.AddSecurityRequirement(new OpenApiSecurityRequirement()
          {
            {
              new OpenApiSecurityScheme
              {
                Reference = new OpenApiReference
                {
                  Type = ReferenceType.SecurityScheme,
                  Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "oauth2",
                Name = "Authorization",
                In = ParameterLocation.Header,
              },
              new List<string>()
            }
          });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CSAuth v1"));
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
  }
}