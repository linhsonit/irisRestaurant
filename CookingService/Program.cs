using CookingService.BackgroundServices;
using CookingService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddDbContext<CookingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CookingDb")));
builder.Services.AddHostedService<RabbitMQBackgroundConsumerService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CookingDbContext>();
    dbContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
