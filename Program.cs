using Microsoft.AspNetCore.Authentication.JwtBearer;
using BKConnectBE;
using BKConnectBE.Model;
using BKConnectBE.Repository;
using Microsoft.EntityFrameworkCore;
using WebApplication1.ChatHub;
using Microsoft.IdentityModel.Tokens;
using BKConnectBE.Service;
using BKConnectBE.Repository.Users;
using BKConnect.Service;
using BKConnect.Middleware;
using BKConnectBE.Common;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
Settings settings = builder.Configuration.GetSection("Settings").Get<Settings>();
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string[] origins = { "http://localhost:5173", "https://chat-friend-three.vercel.app" };
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
                    .WithOrigins(origins)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
        );
});

var connectionString = settings.sqlServer.ConnectionString;
connectionString = connectionString.Replace("{Host}", settings.sqlServer.Host);
connectionString = connectionString.Replace("{DbName}", settings.sqlServer.DbName);
connectionString = connectionString.Replace("{User}", settings.sqlServer.User);
connectionString = connectionString.Replace("{Password}", settings.sqlServer.Password);

builder.Services.AddDbContext<BKConnectContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddSignalR();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(settings.jwtConfig.AccessTokenKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ResultMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chatHub");
});

app.Run();
