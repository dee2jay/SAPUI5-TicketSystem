using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TicketManagement.Application.EventHandlers;
using TicketManagement.Application.Interfaces;
using TicketManagement.Application.Publisher;
using TicketManagement.Application.Services;
using TicketManagementSystem.Infrastructure.Interface;
using TicketManagementSystem.Infrastructure.Logging;
using TicketManagementSystem.Infrastructure.Persistence;
using TicketManagementSystem.Infrastructure.Persistence.Repository;

var builder = WebApplication.CreateBuilder(args);

// Repositories
builder.Services.AddScoped<ITicketRepository, TicketRepository>();

// Event Handlers
builder.Services.AddScoped<TicketCreatedEventHandler>();
builder.Services.AddScoped<TicketUpdatedEventHandler>();

// Event Publisher
// Event Dispatcher
builder.Services.AddScoped<EventPublisher>();
builder.Services.AddScoped<IEventPublisher>(sp => sp.GetRequiredService<EventPublisher>());
builder.Services.AddScoped<IEventDispatcher>(sp => sp.GetRequiredService<IEventDispatcher>());

// Services
builder.Services.AddScoped<ITicketService, TicketService>();

// Add services to the container.
builder.Services.AddDbContext<TicketDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"))
);

builder.Services.AddSingleton(sp =>
    new MongoLogger(
        builder.Configuration.GetConnectionString("MongoDB"),
        builder.Configuration["MongoSettings:Database"]
    )
);
// Logger Mongo
builder.Services.AddSingleton<IAppLogger>(sp => sp.GetRequiredService<MongoLogger>());


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Ticket Management API",
        Version = "v1",
        Description = "API zur Verwaltung von Tickets und Benutzern ",
        Contact = new OpenApiContact
        {
            Name = "Projektteam",
            Email = "support@spiratec.com"
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ticket Management API v1");
        c.RoutePrefix = ""; // Swagger UI 
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
