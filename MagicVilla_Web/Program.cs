using MagicVilla_Web;
using MagicVilla_Web.services;
using MagicVilla_Web.services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Routing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddHttpClient<IVillaService, VillaService>();
builder.Services.AddScoped<IVillaService, VillaService>(); //registriamo il servizio con dependency injection utilizzando AddScoped
//Per ogni singola richiesta ci sarà la creazione di un oggetto di tipi VillaService anche se verrà richiesto 10 volte si userà lo stesso oggetto

//AddHttpClient<IVillaNumberService, VillaNumberService>(): This registers VillaNumberService to be used
//when IVillaNumberService is requested, and it sets up an HTTP client specifically for this service.
builder.Services.AddHttpClient<IVillaNumberService, VillaNumberService>();

/*
By using AddScoped<IVillaNumberService, VillaNumberService>(), you're telling the dependency injection container 
that whenever an IVillaNumberService is requested, it should provide a new instance of VillaNumberService for
the duration of the request
*/
builder.Services.AddScoped<IVillaNumberService, VillaNumberService>();

builder.Services.AddHttpClient<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddDistributedMemoryCache();


//Questa riga aggiunge i servizi di autenticazione all'applicazione e specifica che lo schema di autenticazione
//predefinito sarà basato sui cookie 
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            //In questo esempio, stiamo configurando l'autenticazione tramite cookie e impostando la proprietà LoginPath
            //per specificare l'URL a cui gli utenti devono essere reindirizzati quando è necessario effettuare il login
            options.LoginPath = "/Auth/Login";
            options.AccessDeniedPath = "/Auth/AccessDenied";
            options.SlidingExpiration = true;
        });

//Questa riga aggiunge il servizio di sessione all'applicazione e configura le opzioni della sessione
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddRouting(options =>
{
    options.ConstraintMap.Add("apiVersion", typeof(ApiVersionRouteConstraint));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapStaticAssets();

//Aggiungio la session all'applicazione
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
