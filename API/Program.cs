using API.Middleware;
using Core.Entities;
using Core.interfaces;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// -----------------------
// Add Services
// -----------------------

// Controllers & Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<StoreContext>(options =>
{
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("DefaultConnection")
	);
});

// Identity (for API)
builder.Services
	.AddIdentityApiEndpoints<AppUser>()
	.AddEntityFrameworkStores<StoreContext>();

// Authentication & Authorization
builder.Services.AddAuthentication(); // Identity handles cookie auth by default
builder.Services.AddAuthorization();

// Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// Cart Service
builder.Services.AddScoped<ICartService, CartService>();

// Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
	var connString = builder.Configuration.GetConnectionString("redis")
		?? throw new Exception("Redis connection string not found");

	var options = ConfigurationOptions.Parse(connString, true);
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
			.AllowAnyMethod()
		.AllowCredentials();
	});
});

var app = builder.Build();

// -----------------------
// Middleware
// -----------------------

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("CorsPolicy");

// **Authentication must come before Authorization**
app.UseAuthentication();
app.UseAuthorization();

// Exception handling
app.UseMiddleware<ExceptionMiddleware>();

// Map Controllers
app.MapControllers();
app.MapGroup("/api").MapIdentityApi<AppUser>();

// -----------------------
// Migrations & Seed
// -----------------------
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
