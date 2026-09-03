using Microsoft.EntityFrameworkCore;
using NailDesignerAPI.Services;
using NailDesignerAPI;

var builder = WebApplication.CreateBuilder( args );

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer( builder.Configuration.GetConnectionString( "DefaultConnection" ) )
);

builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<ServiceTypeService>();
builder.Services.AddScoped<ServiceAddOnService>();
builder.Services.AddScoped<AppointmentService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if( app.Environment.IsDevelopment() ) {
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
