using BKConnect.Common;
using BKConnect.Middleware;
using BKConnect.Service.Jwt;
using BKConnectBE;
using BKConnectBE.Model;
using BKConnectBE.Repository;
using BKConnectBE.Repository.Users;
using BKConnectBE.Services.Authorizations;
using BKConnectBE.Services.Email;
using Microsoft.EntityFrameworkCore;
using WebApplication1.ChatHub;

var builder = WebApplication.CreateBuilder(args);

Settings settings = builder.Configuration.GetSection("Settings").Get<Settings>();
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

var connectionString = settings.SqlServer.ConnectionString;
connectionString = connectionString.Replace("{Host}", settings.SqlServer.Host);
connectionString = connectionString.Replace("{DbName}", settings.SqlServer.DbName);
connectionString = connectionString.Replace("{User}", settings.SqlServer.User);
connectionString = connectionString.Replace("{Password}", settings.SqlServer.Password);

builder.Services.AddDbContext<BKConnectContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAuthorizationService, AuthorizationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

builder.Services.AddSignalR();

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthorization();

app.UseMiddleware<ResultMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chatHub");
});

app.Run();
