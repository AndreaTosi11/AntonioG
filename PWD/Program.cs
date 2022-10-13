using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ProjectWorkDemo;
using ProjectWorkDemo.Areas.Identity.Data;
using ProjectWorkDemo.Data;
using ProjectWorkDemo.Models;
using ProjectWorkDemo.Services;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ProjectWorkDemoContextConnection");

builder.Services.AddDbContext<ProjectWorkDemoContext>(options =>
    options.UseSqlServer(connectionString)); ;

builder.Services.AddIdentity<ProjectWorkDemoUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                 .AddDefaultUI()
                 .AddEntityFrameworkStores<ProjectWorkDemoContext>()
                 .AddDefaultTokenProviders();

builder.Services.AddDbContext<ProjectWorkDatabaseContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole",
         policy => policy.RequireRole("Admin"));

    options.AddPolicy("UserRole",
        policy => policy.RequireRole("Officina", "Ricambista"));
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
// Add services to the container.
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc().AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization(option =>
{
    var type = typeof(SharedResource);
    var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName);
    var factory = builder.Services.BuildServiceProvider().GetService<IStringLocalizerFactory>();
    var localizer = factory.Create("SharedResource", assemblyName.Name);
    option.DataAnnotationLocalizerProvider = (t, f) => localizer;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseDeveloperExceptionPage();
    app.UseHsts();
}
app.UseStatusCodePages();

var supportedCultures = new[]
{
    new CultureInfo("it-IT"),
    new CultureInfo("en-US")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("it-IT"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); ;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapControllerRoute(
//    name: "fornitore",
//    pattern: "{controller=Fornitore}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();

