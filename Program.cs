using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Mottu.Backend.Data;
using Mottu.Backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Configura conexão com PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Serviços do app
builder.Services.AddScoped<MessagePublisher>();
builder.Services.AddScoped<Mottu.Backend.Services.StorageService>();
builder.Services.AddHostedService<MotoCreatedConsumer>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mottu API",
        Version = "v1",
        Description = "API de gerenciamento de motos e entregas (Desafio Backend Mottu)"
    });
});

var app = builder.Build();

// Swagger apenas em dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mottu API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
// 409 em violação de unicidade (Postgres 23505)
app.Use(async (ctx, next) =>
{
    try { await next(); }
    catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
        when (ex.InnerException is Npgsql.PostgresException pg && pg.SqlState == "23505")
    {
        ctx.Response.StatusCode = StatusCodes.Status409Conflict;
        await ctx.Response.WriteAsync("Conflito de unicidade.");
    }
});

app.MapControllers();
// DEBUG_LISTS_START
app.MapGet("/debug/deliverers", (Mottu.Backend.Data.AppDbContext db) =>
    db.Deliverers.Select(d => new { d.Id, d.Identifier, d.CNPJ }).ToList());
app.MapGet("/debug/rentals", (Mottu.Backend.Data.AppDbContext db) =>
    db.Rentals.Select(r => new { r.Id, r.MotoId, r.DelivererId, r.EndDate }).ToList());
// DEBUG_LISTS_END// ==== BLOCO DE SEED INICIAL ====
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.Motos.Any())
    {
        db.Motos.AddRange(
            new Mottu.Backend.Models.Moto
            {
                Identificador = Guid.NewGuid().ToString(),
                Ano = 2023,
                Modelo = "Honda CG 160",
                Placa = "ABC1D23"
            },
            new Mottu.Backend.Models.Moto
            {
                Identificador = Guid.NewGuid().ToString(),
                Ano = 2024,
                Modelo = "Yamaha Fazer 250",
                Placa = "XYZ9Z99"
            }
        );
    }

    if (!db.Deliverers.Any())
    {
        db.Deliverers.AddRange(
            new Mottu.Backend.Models.Deliverer
            {
                Identifier = Guid.NewGuid().ToString(),
                Name = "João Silva",
                CNPJ = "12345678000100",
                BirthDate = DateTime.SpecifyKind(new DateTime(1990, 5, 10), DateTimeKind.Utc),
                CNHNumber = "A1234567",
                CNHType = "A",
                CNHImagePath = "/tmp/cnh_joao.png"
            },
            new Mottu.Backend.Models.Deliverer
            {
                Identifier = Guid.NewGuid().ToString(),
                Name = "Carlos Souza",
                CNPJ = "98765432000100",
                BirthDate = DateTime.SpecifyKind(new DateTime(1988, 11, 22), DateTimeKind.Utc),
                CNHNumber = "B7654321",
                CNHType = "B",
                CNHImagePath = "/tmp/cnh_carlos.png"
            }
        );
    }

    db.SaveChanges();
}


app.Run();


