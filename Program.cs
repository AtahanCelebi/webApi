using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ProductsContext>(x => x.UseSqlite("Data Source=products.db"));

builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<ProductsContext>();
builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequiredLength = 4;
    options.Password.RequireDigit = false;

    options.User.RequireUniqueEmail = true;

    options.Lockout.MaxFailedAccessAttempts = 3; // login attempt
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // after x times timeout
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
