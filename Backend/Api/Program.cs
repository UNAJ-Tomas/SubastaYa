using System.Text;
using Application.Interfaces;
using Application.Interfaces.Repositories;
using Application.UseCases.Auditoria_log.Handlers;
using Application.UseCases.Billetera.Handlers;
using Application.UseCases.Categoria.Handlers;
using Application.UseCases.Puja.Handlers;
using Application.UseCases.Subasta.Handlers;
using Application.UseCases.Usuario.Handlers;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SubastaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContextFactory<SubastaDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")),
    ServiceLifetime.Scoped);




builder.Services.AddScoped<ISubastaRepository, SubastaRepository>();
builder.Services.AddScoped<IBilleteraRepository, BilleteraRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ITransaccionLedgerRepository, TransaccionLedgerRepository>();
builder.Services.AddScoped<IAuditoria_LogRepository, Auditoria_LogRepository>();

builder.Services.AddScoped<CrearSubastaCommandHandler>();
builder.Services.AddScoped<ObtenerSubastaPorIdQueryHandler>();
builder.Services.AddScoped<ObtenerSubastasActivasQueryHandler>();
builder.Services.AddScoped<ActualizarSubastaCommandHandler>();
builder.Services.AddScoped<CancelarSubastaCommandHandler>();
builder.Services.AddScoped<FinalizarSubastaCommandHandler>();
builder.Services.AddScoped<ObtenerSubastasConFiltroQueryHandler>();
builder.Services.AddScoped<ObtenerSubastasPorVendedorQueryHandler>();
builder.Services.AddScoped<ObtenerSubastasPorCompradorQueryHandler>();
builder.Services.AddScoped<CrearAuditoria_LogCommandHandler>();
builder.Services.AddScoped<IniciarSubastasProgramadasCommandHandler>();

builder.Services.AddScoped<RegistrarPujaCommandHandler>();

builder.Services.AddScoped<CargarSaldoCommandHandler>();
builder.Services.AddScoped<ObtenerHistorialTransaccionesQueryHandler>();
builder.Services.AddScoped<ObtenerSaldoPorUsuarioQueryHandler>();

builder.Services.AddScoped<ObtenerCategoriaPorIdQueryHandler>();
builder.Services.AddScoped<ObtenerCategoriaPorNombreQueryHandler>();
builder.Services.AddScoped<ObtenerCategoriasQueryHandler>();

builder.Services.AddScoped<RegistrarUsuarioCommandHandler>();

builder.Services.AddHostedService<Infrastructure.Workers.SubastaWorker>();
builder.Services.AddHostedService<Infrastructure.Workers.SubastaProgramadaWorker>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!))
    };
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SubastaYa API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ingresá el token JWT con el formato: Bearer {tu_token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});



builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
var app = builder.Build();
app.UseMiddleware<Api.Middlewares.ExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<SubastaDbContext>();

    try
    {
        //context.Database.Migrate();
        context.Database.EnsureDeleted();   
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error al crear la base de datos.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseCors("PermitirFrontend");

app.UseAuthorization();

app.MapControllers();



app.Run();
