using MagicVilla_Web;
using MagicVilla_Web.services;
using MagicVilla_Web.services.IServices;

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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
