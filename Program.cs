using Microsoft.EntityFrameworkCore;

using TCSA.WebAPI.FlightData.Data;
using TCSA.WebAPI.FlightData.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<FlightsDbContext>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseAuthentication();
    app.UseAuthorization();

    using var scope = app.Services.CreateScope();

    var context = scope.ServiceProvider.GetRequiredService<FlightsDbContext>();

    context.SeedData();
}

app.MapControllers();

app.Run();