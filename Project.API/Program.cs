


using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Project.DAL.Seed;
using StackExchange.Redis;
using System.Reflection;
using Role = Project.DAL.Entities.Identity.Role;


var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddSingleton<IConnectionMultiplexer>(S =>
//{

//    var connection = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"));
//    return ConnectionMultiplexer.Connect(connection);
//});

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IEmailServices, EmailServices>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddAutoMapper(typeof(MappingProfiles));

builder.Services.AddApplicationServices()
    .AddServiceRegisteration(builder.Configuration)
    .AddServiceAuth();

builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddTransient<IUrlHelper>(x =>
{
    var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
    var factory = x.GetRequiredService<IUrlHelperFactory>();
    return factory.GetUrlHelper(actionContext);
});

builder.Services.AddControllers().AddFluentValidation(v =>
{
    v.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});




builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder =>
        {

            policyBuilder.WithOrigins("http://localhost:4200", "https://localhost:7167")
                         .AllowAnyHeader()
                         .AllowAnyMethod();
        });
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
await RoleSeeder.RoleSeeding(roleManager);
await UserSeeder.UserSeeding(userManager);
var loggerFactory = services.GetRequiredService<ILoggerFactory>();
try
{
    var ApplicationContext = services.GetRequiredService<ApplicationDbContext>();
    await ApplicationContext.Database.MigrateAsync();
    await ApplicationDbContextSeed.SeedAsync(ApplicationContext, loggerFactory);


}
catch (Exception ex)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "an error occure during applying migration");
}




// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
