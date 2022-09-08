using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using MakingSolutions.Desenv.WebApi.Domain.Interfaces;
using MakingSolutions.Desenv.WebApi.Domain.Interfaces.Generics;
using MakingSolutions.Desenv.WebApi.Domain.Interfaces.InterfaceServices;
using MakingSolutions.Desenv.WebApi.Domain.Services;
using MakingSolutions.Desenv.WebApi.Entities.Entities;
using MakingSolutions.Desenv.WebApi.Infra.Configuration;
using MakingSolutions.Desenv.WebApi.Infra.Repository.Generics;
using MakingSolutions.Desenv.WebApi.Infra.Repository.Repositories;
using MakingSolutions.Desenv.WebAPIs.Models;
using MakingSolutions.Desenv.WebAPIs.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ninject.Activation;
using Swashbuckle.Swagger;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//ConfigServices
builder.Services.AddDbContext<MyDbContext>(options =>
             options.UseSqlServer(
                 builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<MyDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// INTERFACE E REPOSITORIO
builder.Services.AddSingleton(typeof(IGeneric<>), typeof(RepositoryGenerics<>));
builder.Services.AddSingleton<IMessage, RepositoryMessage>();

// SERVIÇO DOMINIO
builder.Services.AddSingleton<IServiceMessage, ServiceMessage>();


// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
      .AddJwtBearer(option =>
      {
          option.TokenValidationParameters = new TokenValidationParameters
          {
              ValidateIssuer = false,
              ValidateAudience = false,
              ValidateLifetime = true,
              ValidateIssuerSigningKey = true,

              ValidIssuer = "MakingSolutions.Securiry.Bearer",
              ValidAudience = "MakingSolutions.Securiry.Bearer",
              IssuerSigningKey = JwtSecurityKey.Create("Secret_Key-12345678")
          };

          option.Events = new JwtBearerEvents
          {
              OnAuthenticationFailed = context =>
              {
                  Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                  return Task.CompletedTask;
              },
              OnTokenValidated = context =>
              {
                  Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                  return Task.CompletedTask;
              }
          };
      });

var config = new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.CreateMap<MessageViewModel, Message>();
    cfg.CreateMap<Message, MessageViewModel>();
});

IMapper mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);


builder.Services.AddVersionedApiExplorer(options =>
{

    //options.DefaultApiVersionParameterDescription = "Do NOT modify api-version!";
    //options.AssumeDefaultVersionWhenUnspecified = true;

    //options.DefaultApiVersion = new ApiVersion(1, 0);
    //options.AssumeDefaultVersionWhenUnspecified = true;
    options.GroupNameFormat = "'v'VVV";
    //options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddApiVersioning(setup =>
{
    setup.DefaultApiVersion = new ApiVersion(1, 0);
    //setup.ReportApiVersions = true;
});

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Integração",
        Description = "Um exemplo de aplicação Web API .NET Core 6 | DDD | AutoMapper | IdentityFramework - CodeFirst "
    });

    option.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "Integração",
        Description = "Um exemplo de aplicação Web API .NET Core 6 | DDD | AutoMapper | IdentityFramework - CodeFirst "
    });

    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Insira um token válido",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });

});


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();

//}


//var urlDev = "https://dominiodocliente.com.br";
//var urlHML = "https://dominiodocliente2.com.br";
//var urlPROD = "https://dominiodocliente3.com.br";

//app.UseCors(b => b.WithOrigins(urlDev, urlHML, urlPROD));


var devClient = "http://localhost:4200";
app.UseCors(x => x
.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader().WithOrigins(devClient));


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSwaggerUI(config =>
{
    var apiProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    foreach (var version in apiProvider.ApiVersionDescriptions.Reverse())
        config.SwaggerEndpoint($"/swagger/{version.GroupName}/swagger.json", $"v{version.ApiVersion}{(version.IsDeprecated ? " - Depreciada" : "")}");
});

app.Run();








