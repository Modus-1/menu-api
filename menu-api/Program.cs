using menu_api;
using menu_api.Context;
using menu_api.Repositories;
using menu_api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MenuContext>(
    options => options.UseSqlServer(
        builder.Configuration["ConnectionStrings:LocalMenuConnStr"]
        )
    );

var AllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:12345", "http://localhost:3002");
                          builder.WithMethods("GET", "POST", "PUT", "PATCH", "DELETE", "OPTIONS");
                          builder.WithHeaders("Content-Type", "Accept");
                      });
});

builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IIngredientRepository, IngredientRepository>();
builder.Services.AddScoped<IMenuItemIngredientRepository, MenuItemIngredientRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();


builder.Services.AddControllers();

builder.Services.AddTransient<DatabaseSeeder>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Data Seeding start
var app = builder.Build();
SeedData(app);
void SeedData(IHost application)
{
    var scopedFactory = application.Services.GetService<IServiceScopeFactory>();
    if (scopedFactory is null)
        return;

    using var scope = scopedFactory.CreateScope();
    var service = scope.ServiceProvider.GetService<DatabaseSeeder>();
    service?.SeedCategories();
    service?.SeedMenuItems();
}
// Data Seeding end


// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseCors(AllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();

app.Run();
