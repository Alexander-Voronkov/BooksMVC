using BooksMVC.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddDbContext<BookContext>(options =>
{
    string connection = builder.Configuration.GetConnectionString("Default");
    options.UseSqlServer(connection);
});


builder.Services.AddDbContext<UsersContext>(options =>
{
    string cs = builder.Configuration.GetConnectionString("Auth");
    options.UseSqlServer(cs);
});

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = new PathString("/Account/Login");
    });

builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.

using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider sp = scope.ServiceProvider;
    try
    {
        var context = sp.GetRequiredService<BookContext>();
        await DbInitializer.Init(context);
    }
    catch (Exception ex) 
    {
        var logger = sp.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Error while initializing");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");


app.Run();
