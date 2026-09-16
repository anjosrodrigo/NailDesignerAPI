using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NailDesignerAPI;
using NailDesignerAPI.Services;
using NailDesignerAPI.Validators;
using System.Text;

var builder = WebApplication.CreateBuilder( args );

// Add services to the container
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions( options => {
        options.InvalidModelStateResponseFactory = context => {
            var errors = context.ModelState
                .Where( e => e.Value!.Errors.Count > 0 )
                .SelectMany( e => e.Value!.Errors )
                .Select( e => e.ErrorMessage )
                .ToList();

            return new BadRequestObjectResult( new { message = string.Join( " | ", errors ) } );
        };
    } );
builder.Services.AddOpenApi();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer( builder.Configuration.GetConnectionString( "DefaultConnection" ) )
);

// Add Services
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<ServiceTypeService>();
builder.Services.AddScoped<ServiceAddOnService>();
builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<AuthService>();


// Add JWT Authentication
var jwtKey = builder.Configuration[ "Jwt:Key" ]!;

builder.Services.AddAuthentication( JwtBearerDefaults.AuthenticationScheme )
    .AddJwtBearer( options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( jwtKey ) ),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true
        };
    } );

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<WhatsAppService>();
//builder.Services.AddHostedService<AppointmentReminderService>();
// Registra como Singleton para injeção no Controller
builder.Services.AddSingleton<AppointmentReminderService>();
// Registra como HostedService para rodar em background
builder.Services.AddHostedService( sp => sp.GetRequiredService<AppointmentReminderService>() );
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateClientValidator>();

// Add CORS
builder.Services.AddCors( options => {
    options.AddPolicy( "AllowFrontend", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    } );
} );

var app = builder.Build();

// Configure the HTTP request pipeline
if( app.Environment.IsDevelopment() ) {
    app.MapOpenApi();
}

//app.UseHttpsRedirection();
app.UseCors( "AllowFrontend" );
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
