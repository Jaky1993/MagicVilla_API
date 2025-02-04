using MagicVilla_VillaAPI;
using MagicVilla_VillaAPI.DATA;
using MagicVilla_VillaAPI.Logging;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
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
/*
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(): Questo aggiunge il sistema di Identità all'applicazione,
specificando IdentityUser per il tipo di utente e IdentityRole per il tipo di ruolo.

AddEntityFrameworkStores<ApplicationDbContext>(): Questo configura il sistema di Identità per utilizzare
Entity Framework Core per memorizzare le informazioni degli utenti in un database, utilizzando specificamente
ApplicationDbContext come contesto
*/
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddResponseCaching();
builder.Services.AddScoped<IVillaRepository, VillaRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddApiVersioning(options =>
{
    //Questa opzione dice all'applicazione di utilizzare la versione predefinita specificata quando un client
    //non fornisce una versione nella loro richiesta
    options.AssumeDefaultVersionWhenUnspecified = true;

    //Questa imposta la versione predefinita dell'API a 1.0
    /*
    Versione principale 1: Questo indica la prima versione significativa dell'API. La versione principale
    cambia solitamente per riflettere modifiche importanti o non retrocompatibili.
    Versione secondaria 0: Questo indica una versione secondaria, o "minor", che di solito riflette
    piccoli miglioramenti o correzioni di bug che sono retrocompatibili con la versione principale.
    */
    options.DefaultApiVersion = new ApiVersion(1, 0);

    /*
    L'opzione options.ReportApiVersions = true; nel contesto di API versioning in ASP.NET Core fa sì che l'applicazione
    includa informazioni sulle versioni API supportate e deprecate negli header delle risposte HTTP.
    Questo è utile per i client che desiderano sapere quali versioni dell'API sono disponibili o sono state deprecate
    */
    options.ReportApiVersions = true;
});

//VersionedApiExplorer :definisce un formato per i nomi dei gruppi delle versioni API
builder.Services.AddVersionedApiExplorer(
    options =>
    {
        //Questa opzione specifica il formato per i nomi dei gruppi delle versioni API.
        //Nell'esempio che hai fornito, "'v'VVV" significa che i gruppi di versioni saranno prefissi con la lettera "v"
        //seguita dai numeri di versione.
        //Ad esempio, una versione 1.0 sarà rappresentata come "v1"
        //Pertanto, VVV permette di rappresentare numeri di versione fino a tre cifre.
        //Ad esempio, una versione 1.0 sarà rappresentata come v001, e una versione 12.0 sarà rappresentata come v012
        options.GroupNameFormat = "'v'VVV";

        //Imposta di default la versione 1
        options.SubstituteApiVersionInUrl = true;
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
option.ReturnHttpNotAcceptable = true;
option.CacheProfiles.Add("Default30",
                new CacheProfile()
                {
                    Duration = 30
                });
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
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        //basic documentation for swagger UI
        Version = "v1.0",
        Title = "Magic Villa V1",
        Description = "API to manage Villa",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "DotNetMastery",
            Url = new Uri("https://dotnetmastery.com")
        },
        License = new OpenApiLicense
        {
            Name = "Example license",
            Url = new Uri("https://dotnetmastery.com/license")
        }
    });

    options.SwaggerDoc("v2", new OpenApiInfo
    {
        //basic documentation for swagger UI
        Version = "v2.0",
        Title = "Magic Villa V2",
        Description = "API to manage Villa",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "DotNetMastery",
            Url = new Uri("https://dotnetmastery.com")
        },
        License = new OpenApiLicense
        {
            Name = "Example license",
            Url = new Uri("https://dotnetmastery.com/license")
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
    app.UseSwaggerUI(options =>
    {
        //default endpoint
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Magic_VillaV1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Magic_VillaV2");
    });
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
