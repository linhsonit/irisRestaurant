using Microsoft.EntityFrameworkCore;
using OrderingService.Data;
using OrderingService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddDbContext<OrderingDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("OrderingDb"))
    );

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
    dbContext.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
app.UseAuthorization();

app.MapControllers();

app.Run();
