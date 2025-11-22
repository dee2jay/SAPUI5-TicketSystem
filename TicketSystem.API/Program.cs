using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using TicketManagementSystem.API.OptionsSetup;
using TicketManagementSystem.Application.Command.UserCommands;
using TicketManagementSystem.Application.CommandHandler.UserCommandHandlers;
using TicketManagementSystem.Application.Dispatcher;
using TicketManagementSystem.Application.EventHandlers.TicketEventHandlers;
using TicketManagementSystem.Application.EventHandlers.UserEventHandler;
using TicketManagementSystem.Application.Events.TicketEvents;
using TicketManagementSystem.Application.Events.UserEvents;
using TicketManagementSystem.Application.Interfaces;
using TicketManagementSystem.Application.Mapping;
using TicketManagementSystem.Application.Publisher;
using TicketManagementSystem.Application.Security;
using TicketManagementSystem.Application.Services;
using TicketManagementSystem.Application.Services.AssignmentService;
using TicketManagementSystem.Application.Services.MailService;
using TicketManagementSystem.Domain.Models;
using TicketManagementSystem.Infrastructure.Interface;
using TicketManagementSystem.Infrastructure.Logging;
using TicketManagementSystem.Infrastructure.Persistence;
using TicketManagementSystem.Infrastructure.Persistence.Repository;

var builder = WebApplication.CreateBuilder(args);


// === Authentication Configurations ===
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

// === Assignment Rules Configurations ===
builder.Services.ConfigureOptions<AssignmentRulesOptionsSetup>();

// === Repositories ===
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();




builder.Services.AddScoped<IEventHandler<UserCreatedEvent>, RegisterUserEventHandler>();
builder.Services.AddScoped<IEventHandler<UserLoginEvent>, LoginUserEventHandler>();
builder.Services.AddScoped<IEventHandler<UserLogoutEvent>, LogoutUserEventHandler>();


builder.Services.AddScoped<ICommandHandler<RegisterUserCommand, User>, UserRegisterCommandHandler>();
builder.Services.AddScoped<ICommandHandler<LoginUserCommand, string>, UserLoginCommandHandler>();
builder.Services.AddScoped<ICommandHandlerBase<LogoutUserCommand>, UserLogoutCommandHandler>();


// === Event Publisher & Dispatcher ===
builder.Services.AddScoped<EventPublisher>();
builder.Services.AddScoped<IEventPublisher>(sp => sp.GetRequiredService<EventPublisher>());
builder.Services.AddScoped<IEventDispatcher, EventDispatcher>();


builder.Services.AddAutoMapper(profile => profile.AddProfile(typeof(TicketMappingProfile)));
builder.Services.AddAutoMapper(profile => profile.AddProfile(typeof(UserMappingProfile)));

// === Services ===
builder.Services.AddScoped<ITicketAssignmentService, TicketAssignmentService>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.AddScoped<ISmtpSettingsProvider, SmtpSettingProvider>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// === Command Handler and Event Handlers===
builder.Services.AddScoped<IEventHandler<TicketCreatedEvent>, TicketCreatedEventHandler>();
builder.Services.AddScoped<IEventHandler<TicketUpdatedEvent>, TicketUpdatedEventHandler>();
builder.Services.AddScoped<IEventHandler<AttachmentAddedToTicketEvent>, AttachmentAddedToTicketEventHandler>();
builder.Services.AddScoped<IEventHandler<CommentAddedToTicketEvent>, CommentAddedTicketEventHandler>();
builder.Services.AddScoped<IEventHandler<TicketPriorityChangedEvent>, TicketPriorityChangedEventHandler>();
builder.Services.AddScoped<IEventHandler<TicketStatusChangedEvent>, TicketStatusChangedEventHandler>();
builder.Services.AddScoped<IEventHandler<TicketOwnerChangedEvent>, TicketOwnerChangedEventHandler>();


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
    options.AddDefaultPolicy(cors =>
    {
        cors.WithOrigins()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddAuthorization();

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
    // JWT Authorization  Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer {token}' to access the secure endpoints"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] { }
        }
    });

});

// Health Checks
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy("API is running"));

builder.Services.AddHttpContextAccessor();


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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
