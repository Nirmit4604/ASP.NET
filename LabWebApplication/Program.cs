
using LabWebApplication.Data;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication()
          .AddGitHub(o =>
          {
              o.ClientId = builder.Configuration["Authentication:GitHub:ClientId"];
              o.ClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"];
              o.CallbackPath = "/signin-github";
              // Grants access to read a user's profile data.
              // https://docs.github.com/en/developers/apps/building-oauth-apps/scopes-for-oauth-apps
              o.Scope.Add("read:user");
              // Optional
              // if you need an access token to call GitHub Apis
              o.Events.OnCreatingTicket += context =>
              {
                  if (context.AccessToken is { })
                  {
                      context.Identity?.AddClaim(new Claim("access_token", context.AccessToken));
                  }
                  return Task.CompletedTask;
              };
          });

var app = builder.Build();

// Configure the HTTP request pipeline.
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

// Add authentication
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();