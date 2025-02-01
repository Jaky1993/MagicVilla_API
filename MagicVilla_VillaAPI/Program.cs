using MagicVilla_VillaAPI;
using MagicVilla_VillaAPI.DATA;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Repository;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddAutoMapper(typeof(MappingConfig));

var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey= true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

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

/*
app.UseAuthentication();: Questa linea configura il middleware di autenticazione nel pipeline delle richieste.
Consente all'app di utilizzare i servizi di autenticazione.
VA AGGIUNTO PRIMA DELL'AUTORIZZAZIONE, UN UTENTE PRIMA DI ESSERE AUTORIZZATO DEVE ESSERE AUTENTICATO, è IMPORTNATE L'ORDINE
 
 */
app.UseAuthentication();

/*app.UseAuthorization();: Questa linea aggiunge il middleware di autorizzazione al pipeline delle richieste,
consentendo all'app di gestire l'autorizzazione per le risorse protette.*/
app.UseAuthorization();

app.MapControllers();

app.Run();
