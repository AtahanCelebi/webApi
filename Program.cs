using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProductsAPI.Data;
using ProductsAPI.Models;
using ProductsAPI.Services;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Enable CORS
builder.Services.AddCors(options => {
    options.AddPolicy(MyAllowSpecificOrigins, policy => {
        policy.WithOrigins("http://127.0.0.1:5500")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddDbContext<MainDbContext>(options =>
    options.UseSqlite("Data Source=products.db;Pooling=False;Default Timeout=30"));


builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<MainDbContext>();
builder.Services.Configure<IdentityOptions>(options => {
    options.Password.RequiredLength = 4;
    options.Password.RequireDigit = false;

    options.User.RequireUniqueEmail = true;

    options.Lockout.MaxFailedAccessAttempts = 3; // login attempt
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // after x times timeout
});

builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
    x.RequireHttpsMetadata = false; // false ise http den gelen istekleri de kabul eder
    x.TokenValidationParameters = new TokenValidationParameters {
        ValidateIssuer = false, // eğer true olursa issuer kısmında ata.com'da gönderilmeli.
        ValidIssuer = "ata.com",

        ValidateAudience = false, // eğer true olursa bu api'nin hangi servislere ve kime hitap ettiği gönderilmeli
        ValidAudience = "a",

        ValidateIssuerSigningKey = true, // burada token'ı validate edicez, 
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("AppSettings:Secret").Value ?? string.Empty)),  // burada ters işlem yaparak algoritmayı vericez
        ValidateLifetime = true // eğer true olmazsa süre önemsemeden validate eder, expire olmaz
    };
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserSession, UserSession>();
builder.Services.AddScoped<IRiskServices, RiskServices>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseRouting(); // cors authentication ve authorization arasında olmalı, authrorization'dan önce olamaz
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();

app.Run();
