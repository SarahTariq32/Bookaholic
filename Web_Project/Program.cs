//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Web_Project.Data;
//using Web_Project.Models;
//using Web_Project.Models.Interfaces;
//using Web_Project.Repository;
//using Web_Project.Services;
//using Web_Project.Services.Interfaces;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddControllersWithViews();

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminAccess", policy =>
//        policy.RequireClaim("Role", "Admin"));

//    options.AddPolicy("UserAccess", policy =>
//        policy.RequireClaim("Role", "User"));
//});

//builder.Services.AddScoped<IBookRepository, BookRepository>();
//builder.Services.AddScoped<IBookService, BookService>();
////builder.Services.AddScoped<ICartService, CartService>();
//builder.Services.AddScoped<ICategoryService, CategoryService>();
//builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
//builder.Services.AddScoped<IOrderService, OrderService>();
////builder.Services.AddScoped<ICartRepository, CartRepository>();
//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
//builder.Services.AddScoped<IOrderRepository, OrderRepository>();
//builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
//builder.Services.AddSingleton<DapperContext>();
//builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
//builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();

//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromDays(14);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});
//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseMigrationsEndPoint();
//}
//else
//{
//    app.UseExceptionHandler("/Home/Error");
//    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapRazorPages();
//app.UseSession();
//app.Run();
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Web_Project.Data;
//using Web_Project.Models;
//using Web_Project.Models.Interfaces;
//using Web_Project.Repository;
//using Web_Project.Services;
//using Web_Project.Services.Interfaces;
//using Web_Project.Hubs; 

//var builder = WebApplication.CreateBuilder(args);

//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
//    .AddEntityFrameworkStores<ApplicationDbContext>();
//builder.Services.AddControllersWithViews();

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminAccess", policy =>
//        policy.RequireClaim("Role", "Admin"));

//    //options.AddPolicy("UserAccess", policy =>
//    //    policy.RequireClaim("Role", "User"));
//    options.AddPolicy("UserAccess", policy =>
//        policy.RequireAuthenticatedUser());
//});

//builder.Services.AddScoped<IBookRepository, BookRepository>();
//builder.Services.AddScoped<IBookService, BookService>();
//builder.Services.AddScoped<ICategoryService, CategoryService>();
//builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
//builder.Services.AddScoped<IOrderService, OrderService>();
//builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
//builder.Services.AddScoped<IOrderRepository, OrderRepository>();
//builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
//builder.Services.AddSingleton<DapperContext>();
//builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
//builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();


//builder.Services.AddDistributedMemoryCache();
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromDays(14);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

//builder.Services.AddSignalR();

//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseMigrationsEndPoint();
//}
//else
//{
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();
//app.UseSession();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
//app.MapRazorPages();

//app.MapHub<OrderHub>("/orderHub");

//app.Run();




using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Web_Project.Data;
using Web_Project.Hubs;
using Web_Project.Models;
using Web_Project.Models.Interfaces;
using Web_Project.Repository;
using Web_Project.Services;
using Web_Project.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Enable Identity with roles so IsInRole works
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminAccess", policy =>
//        policy.RequireClaim("Role", "Admin"));

//    options.AddPolicy("UserAccess", policy =>
//        policy.RequireAuthenticatedUser());

//});

// replace your existing AddAuthorization(...) block with this
builder.Services.AddAuthorization(options =>
{
    // admin if email contains "admin" (case-insensitive)
    options.AddPolicy("AdminAccess", policy =>
        policy.RequireAssertion(ctx =>
        {
            var identity = ctx.User?.Identity;
            if (identity == null || !identity.IsAuthenticated) return false;
            var email = ctx.User.FindFirst(ClaimTypes.Email)?.Value ?? ctx.User.Identity?.Name ?? string.Empty;
            return email.IndexOf("admin", StringComparison.OrdinalIgnoreCase) >= 0;
        }));

    // user = authenticated and NOT admin (email does not contain "admin")
    options.AddPolicy("UserAccess", policy =>
        policy.RequireAssertion(ctx =>
        {
            var identity = ctx.User?.Identity;
            if (identity == null || !identity.IsAuthenticated) return false;
            var email = ctx.User.FindFirst(ClaimTypes.Email)?.Value ?? ctx.User.Identity?.Name ?? string.Empty;
            return email.IndexOf("admin", StringComparison.OrdinalIgnoreCase) < 0;
        }));
});


builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(14);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapHub<OrderHub>("/orderHub");

app.Run();