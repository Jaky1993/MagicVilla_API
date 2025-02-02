using MagicVilla_VillaAPI;
using MagicVilla_VillaAPI.DATA;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Repository;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
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
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    //Default version
    options.DefaultApiVersion = new ApiVersion(1,0);
});
builder.Services.AddVersionedApiExplorer(
    options =>
    {
        options.GroupNameFormat = "'v'VVV";
    });

var key = builder.Configuration.GetValue<string>("ApiSettings:Secret");

builder.Services.AddAuthentication(x =>
{
    //JWTBearerDefaults: contiene valori di default utilizzati per configurare l'autenticazione
    //JWT (JSON Web Token) nella tua applicazione ASP.NET Core
    //Specifica lo schema di autenticazione predefinito, in questo caso, questo schema specifica che sto usando TOKEN JWT
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

    //In altre parole, se un utente cerca di accedere a una risorsa protetta senza un token JWT valido,
    //l'applicazione risponderà con una sfida che richiede la presentazione di un token JWT
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    //AddJwtBearer è un metodo di estensione.
    //Questo metodo viene utilizzato per configurare l'autenticazione JWT (JSON Web Token) all'interno della tua applicazione
    //ASP.NET Core 
}).AddJwtBearer(x =>
{
    //Questa impostazione indica che non è richiesto l'uso di HTTPS per la richiesta dei metadati 
    //I metadati relativi all'autenticazione contengono informazioni necessarie per la convalida e la gestione
    //delle richieste di autenticazione all'interno di un sistema. Questi metadati sono utilizzati per configurare
    //le modalità con cui i token di autenticazione vengono creati, trasmessi e validati
    x.RequireHttpsMetadata = false;

    //il token JWT verrà salvato nel contesto della richiesta dopo essere stato convalidato
    x.SaveToken = true;


    x.TokenValidationParameters = new TokenValidationParameters
    {
        //Questa impostazione indica che la chiave di firma del token deve essere convalidata
        ValidateIssuerSigningKey = true,

        //Qui stai specificando la chiave simmetrica utilizzata per convalidare la firma del token.
        //La chiave viene generata utilizzando Encoding.ASCII.GetBytes(key), dove key è la tua chiave segreta
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),

        //Questa impostazione disattiva la convalida dell'emittente del token. In un ambiente di produzione,
        //potresti voler impostare questa opzione su true per garantire che il token provenga da una fonte attendibile 
        ValidateIssuer = false,

        //Questa impostazione disattiva la convalida del pubblico destinatario del token.
        //Anche in questo caso, in un ambiente di produzione, potresti voler impostare questa opzione su true
        //per garantire che il token sia destinato al pubblico previsto.
        ValidateAudience = false
    };
});

builder.Services.AddControllers(option =>
{
    option.ReturnHttpNotAcceptable  = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(options =>
{
    //Il comando options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme) viene utilizzato per configurare
    //la sicurezza nell'API tramite Swagger/OpenAPI

    //options.AddSecurityDefinition: Questo metodo aggiunge una definizione di sicurezza alla configurazione di Swagger.
    //È necessario fornire un nome per la definizione di sicurezza e uno schema di sicurezza
    //"Bearer": Questo è il nome che viene assegnato alla definizione di sicurezza.
    //Indica che l'API utilizza l'autenticazione tramite token Bearer. Bearer tokens sono una forma comune di autenticazione
    //tramite token in cui il client invia un token di accesso nel campo di intestazione Authorization di ogni richiesta
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description =
            "JWT Authorization header using the bearer scheme. \r \r \r " +
            "Enter 'Bearer' [space] and then your token in the next input below. \r \r \r \n" +
            "Example: \"Bearer 12345abcdef\"",

        Name = "Authorization",
        In = ParameterLocation.Header,

        //Scheme = "Bearer" specifica che lo schema di autenticazione utilizzato è un token Bearer.
        //Questo significa che il client dovrà inviare un token di accesso nell'intestazione Authorization
        //della richiesta HTTP per autenticarsi e accedere alle risorse protette dell'API.
        //IN POSTMAN METTO IL TOKEN NELL'HEADER DELLA RICHIESTA E LO CHIAMO AUTHORIZATION
        Scheme = "Bearer"
    });

    //Aggiungo il requisito di sicurezza a Swagger
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            //Definisce come funziona la sicurezza dell'API
            new OpenApiSecurityScheme
            {
                //Specifica un riferimento a una definizione di sicurezza esistente
                Reference = new OpenApiReference
                    {
                        //Indica che il tipo di riferimento è uno schema di sicurezza
                        Type = ReferenceType.SecurityScheme,
                        //Specifica l'ID della definizione di sicurezza. In questo caso, è "Bearer"
                        Id = "Bearer"
                    },
                //Quando configuri Swagger/OpenAPI con OAuth2, stai preparando la tua documentazione API in
                //modo che sia chiaro agli sviluppatori come autenticarsi e quali permessi sono necessari
                //per accedere alle diverse risorse. Questo è particolarmente utile per API pubbliche o
                //per quelle che devono interagire con client di terze parti
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

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
VA AGGIUNTO PRIMA DELL'AUTORIZZAZIONE, UN UTENTE PRIMA DI ESSERE AUTORIZZATO DEVE ESSERE AUTENTICATO, è IMPORTANTE L'ORDINE
 
 */
app.UseAuthentication();

/*app.UseAuthorization();: Questa linea aggiunge il middleware di autorizzazione al pipeline delle richieste,
consentendo all'app di gestire l'autorizzazione per le risorse protette.*/
app.UseAuthorization();

app.MapControllers();

app.Run();
