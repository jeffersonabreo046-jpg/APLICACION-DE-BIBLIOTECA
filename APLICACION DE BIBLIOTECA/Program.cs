using APLICACION_DE_BIBLIOTECA.Repositories;
using APLICACION_DE_BIBLIOTECA.Services;
using APLICACION_DE_BIBLIOTECA.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

//MVC y DbContext
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BibliotecaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BibliotecaDB"))
);

//Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.AllowedForNewUsers = true;
}).AddEntityFrameworkStores<BibliotecaContext>();

//Cookie 
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

//Inyección de dependencias
builder.Services.AddScoped<IAutorService, AutorService>();
builder.Services.AddScoped<IRepositorioLibro, RepositorioLibro>();

var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var usuarios = await userManager.Users.ToListAsync();  
    foreach (var usuario in usuarios)
    {
        if((await userManager.GetRolesAsync(usuario)).Count == 0 )
        {
            var result = await userManager.AddToRoleAsync(usuario, "Usuario");
            if (result.Succeeded)
            {
                throw new InvalidOperationException(
                    $"No se puede asignar el rol de usuario a {usuario.UserName}: {string.Join(",", result.Errors.Select(error => error.Description))}"
                    );
            }

        }
    }
}

//htpp
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roleNames = { "Administrador", "Usuario" };

    foreach (var roleName in roleNames)
    {
        var roleExist = await roleManager.RoleExistsAsync(roleName);
        if (!roleExist)
        {
           
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
}


app.Run();