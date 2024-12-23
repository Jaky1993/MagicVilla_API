using MagicVilla_VillaAPI;
using MagicVilla_VillaAPI.DATA;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/*
Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
    .WriteTo.File("log/villaLogs.txt", rollingInterval:RollingInterval.Day).CreateLogger();

builder.Host.UseSerilog();
*/
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

builder.Services.AddScoped<IVillaRepository, VillaRepository>();
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddControllers(option =>
{
    option.ReturnHttpNotAcceptable  = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ILogging, Logging>(); //è l'ambito che fondmentalmente per ogni richiesta crea un nuovo oggetto e lo fornirà dove richiesto
//ci sono anche AddSinglelton: Questo oggetto viene creato all'avvio dell'applicazione e verrà utilizzato ogni volta che l'applicaione richiede l'implementazione
//AddTransient: Ogni volta che si acede a quell'oggetto, anche in una sola richiesta, se si accede a quell'oggetto 10 volte si creeranno 10 oggetti diversi di quell'oggetto e lo si assegnerà
//dove serve
//Questo ci permette di cambiare implementazione cambiando la classe solo in un punto -> Dependency injection
var app = builder.Build();

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
