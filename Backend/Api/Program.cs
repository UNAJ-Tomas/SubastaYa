using System.Text;
using Application.Interfaces.Repositories;
using Application.UseCases.Billetera.Handlers;
using Application.UseCases.Categoria.Handlers;
using Application.UseCases.Subasta.Handlers;
using Application.UseCases.Usuario.Handlers;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// REGISTRAR EL DBCONTEXT AQUÍ:
builder.Services.AddDbContext<SubastaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//-------------------------------------------------------------------------------------------------

// Registrar DbContext y Repositorios
builder.Services.AddScoped<ISubastaRepository, SubastaRepository>();
builder.Services.AddScoped<IBilleteraRepository, BilleteraRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<ITransaccionLedgerRepository, TransaccionLedgerRepository>();

// Registrar Handlers (Commands y Queries)
builder.Services.AddScoped<CrearSubastaCommandHandler>();
builder.Services.AddScoped<ObtenerSubastaPorIdQueryHandler>();
builder.Services.AddScoped<ObtenerSubastasActivasQueryHandler>();
builder.Services.AddScoped<ActualizarSubastaCommandHandler>();
builder.Services.AddScoped<CancelarSubastaCommandHandler>();
builder.Services.AddScoped<FinalizarSubastaCommandHandler>();
builder.Services.AddScoped<ObtenerSubastasConFiltroQueryHandler>();
builder.Services.AddScoped<ObtenerSubastasPorVendedorQueryHandler>();
builder.Services.AddScoped<ObtenerSubastasPorCompradorQueryHandler>();

builder.Services.AddScoped<RegistrarPujaCommandHandler>();

builder.Services.AddScoped<CargarSaldoCommandHandler>();
builder.Services.AddScoped<ObtenerHistorialTransaccionesQueryHandler>();
builder.Services.AddScoped<ObtenerSaldoPorUsuarioQueryHandler>();

builder.Services.AddScoped<ObtenerCategoriaPorIdQueryHandler>();
builder.Services.AddScoped<ObtenerCategoriaPorNombreQueryHandler>();
builder.Services.AddScoped<ObtenerCategoriasQueryHandler>();

builder.Services.AddScoped<RegistrarUsuarioCommandHandler>();

// Registro del Background Worker para cierre automático
builder.Services.AddHostedService<Infrastructure.Workers.SubastaWorker>();

// 4. Autenticación JWT
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

// 5. Configuración de Swagger para probar los Tokens
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

//-------------------------------------------------------------------------------------------------

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<Api.Middlewares.ExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<SubastaDbContext>();

    try
    {
        //context.Database.EnsureDeleted();
        //context.Database.EnsureCreated();
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
