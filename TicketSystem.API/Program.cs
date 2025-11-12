using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using TicketManagement.Application.Dispatcher;
using TicketManagement.Application.EventHandlers;
using TicketManagement.Application.Interfaces;
using TicketManagement.Application.Mapping;
using TicketManagement.Application.Publisher;
using TicketManagement.Application.Services;
using TicketManagementSystem.Infrastructure.Interface;
using TicketManagementSystem.Infrastructure.Logging;
using TicketManagementSystem.Infrastructure.Persistence;
using TicketManagementSystem.Infrastructure.Persistence.Repository;

var builder = WebApplication.CreateBuilder(args);

// === Repositories ===
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

// === Event Handlers ===
builder.Services.AddScoped<TicketCreatedEventHandler>();
builder.Services.AddScoped<TicketUpdatedEventHandler>();

// === Event Publisher & Dispatcher ===
builder.Services.AddScoped<EventPublisher>();
builder.Services.AddScoped<IEventPublisher>(sp => sp.GetRequiredService<EventPublisher>());
builder.Services.AddScoped<IEventDispatcher, EventDispatcher>();

builder.Services.AddAutoMapper(profile => profile.AddProfile(typeof(TicketMappingProfile)));

// === Services ===
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITicketService, TicketService>();

// === Database Context ===
builder.Services.AddDbContext<TicketDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
);

// === Logger Mongo ===
builder.Services.AddSingleton(sp =>
    new MongoLogger(
        builder.Configuration.GetConnectionString("MongoDB"),
        builder.Configuration["MongoSettings:Database"]
    )
);
builder.Services.AddSingleton<IAppLogger>(sp => sp.GetRequiredService<MongoLogger>());

// === CORS ===
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowUI5", cors =>
    {
        cors.WithOrigins("http://localhost:8080")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// === Controllers ===
builder.Services.AddControllers();

// === Swagger / OpenAPI ===
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SAPUI5 Ticket Management API",
        Version = "v1",
        Description = "API zur Verwaltung von Tickets und Benutzern",
        Contact = new OpenApiContact
        {
            Name = "Projektteam",
            Email = "support@spiratec.com"
        }
    });
});

// Health Checks
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy("API is running"));

var app = builder.Build();

// === Middleware ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SAPUI5 Ticket Management API v1");
        c.RoutePrefix = "";
    });
}

app.UseCors("AllowUI5");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
