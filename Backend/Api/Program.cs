using Application.Interfaces.Repositories;
using Application.UseCases.Categoria.Handlers;
using Application.UseCases.Subasta.Handlers;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// REGISTRAR EL DBCONTEXT AQUÍ:
builder.Services.AddDbContext<SubastaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//-------------------------------------------------------------------------------------------------

// Registrar DbContext y Repositorios
builder.Services.AddScoped<ISubastaRepository, SubastaRepository>();
builder.Services.AddScoped<IBilleteraRepository, BilleteraRepository>();

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();

// Registrar Handlers (Commands y Queries)
builder.Services.AddScoped<CrearSubastaCommandHandler>();
builder.Services.AddScoped<ObtenerSubastaPorIdQueryHandler>();
builder.Services.AddScoped<ObtenerSubastasActivasQueryHandler>();
builder.Services.AddScoped<ActualizarSubastaCommandHandler>();
builder.Services.AddScoped<CancelarSubastaCommandHandler>();
builder.Services.AddScoped<RegistrarPujaCommandHandler>();

builder.Services.AddScoped<ObtenerCategoriaPorIdQueryHandler>();
builder.Services.AddScoped<ObtenerCategoriaPorNombreQueryHandler>();
builder.Services.AddScoped<ObtenerCategoriasQueryHandler>();

//-------------------------------------------------------------------------------------------------

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
