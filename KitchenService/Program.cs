using KitchenService.BackgroundServices;
using KitchenService.Data;
using KitchenService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<ICookService, CookService>();
builder.Services.AddDbContext<KitchenDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("KitchenService")));
builder.Services.AddHostedService<RabbitMQBackgroundConsumerService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<KitchenDbContext>();
    dbContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
