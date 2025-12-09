using BlazorBlog.Core.Configurations;
using BlazorBlog.Features;
using BlazorBlog.Features.Account;
using BlazorBlog.Features.Admin.Configurations;
using BlazorBlog.Features.CreateArticle.Configurations;
using BlazorBlog.Features.Data;
using BlazorBlog.Features.Home.Configurations;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

ILogger? logger = LoggerFactory.Create(builder =>
{
    builder.AddConsole();
}).CreateLogger<Program>();

try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    builder.Services.AddCascadingAuthenticationState();
    builder.Services.AddScoped<IdentityRedirectManager>();
    builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

    builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddIdentityCookies();

    builder.Configuration.Sources.Clear();
    builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .AddEnvironmentVariables(prefix: "BLOGENV_")
                        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true);

    builder.Services.AddLogging();

    string connectionString = builder.Services.ConfigureSettings(builder.Configuration);
    
    // Configuration MongoDB et services CreateArticle
    builder.Services.AddCreateArticleServices();
    
    // Configuration services Home
    builder.Services.AddHomeServices();
    
    // Configuration services Admin
    builder.Services.AddAdminServices();

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    builder.Services.AddIdentityCore<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;
            options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
            options.User.RequireUniqueEmail = false; // L'email n'est pas requis
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddSignInManager()
        .AddDefaultTokenProviders();

    builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

    WebApplication app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    // Migration de la base de données
    using (IServiceScope scope = app.Services.CreateScope())
    {
        // Appliquer les migrations EF Core en attente
        ApplicationDbContext? db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();

        //// Appliquer les migrations DbUp
        //IDbMigrationService dbMigrationService = scope.ServiceProvider.GetRequiredService<IDbMigrationService>();

        //if (dbMigrationService.UpgradeDatabase())
        //{
        //    logger?.LogInformation("Migration de la base de données réussie");
        //}
        //else
        //{
        //    throw new Exception("Erreur lors de la migration de la base de données");
        //}
    }



    app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
    app.UseHttpsRedirection();

    app.UseAntiforgery();

    app.MapStaticAssets();
    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    // Add additional endpoints required by the Identity /Account Razor components.
    app.MapAdditionalIdentityEndpoints();

    await app.RunAsync();
}
catch (Exception ex)
{
    logger?.LogCritical(ex, "MAIN Exception - Stopped program because of exception");
}