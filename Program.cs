using Microsoft.EntityFrameworkCore;
using NailDesignerAPI.Services;
using NailDesignerAPI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder( args );

// Add services to the container
builder.Services.AddControllers();
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

var app = builder.Build();

// Configure the HTTP request pipeline
if( app.Environment.IsDevelopment() ) {
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
