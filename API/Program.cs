using API.Middleware;
using Core.interfaces;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<StoreContext>(opt =>
{
	opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// ✅ Cart Service
builder.Services.AddScoped<ICartService, CartService>();

// ✅ Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
	var connString = builder.Configuration.GetConnectionString("redis")
		?? throw new Exception("Redis connection string not found");

	var options = ConfigurationOptions.Parse(connString, true);

	// ⭐ IMPORTANT FIX
	options.AbortOnConnectFail = false;

	return ConnectionMultiplexer.Connect(options);
});


// CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("CorsPolicy", policy =>
	{
		policy
			.WithOrigins("http://localhost:4200", "https://localhost:4200")
			.AllowAnyHeader()
			.AllowAnyMethod();
	});
});

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("CorsPolicy");
app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

// DB Migration & Seed
try
{
	using var scope = app.Services.CreateScope();
	var context = scope.ServiceProvider.GetRequiredService<StoreContext>();
	await context.Database.MigrateAsync();
	await StoreContextSeed.SeedAsync(context);
}
catch (Exception ex)
{
	Console.WriteLine(ex.Message);
}

app.Run();
