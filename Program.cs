using BKConnect.Middleware;
using BKConnect.Service.Jwt;
using BKConnectBE;
using BKConnectBE.Common;
using BKConnectBE.Model;
using BKConnectBE.Repository;
using BKConnectBE.Repository.Users;
using BKConnectBE.Service.Authentication;
using BKConnectBE.Service.Email;
using BKConnectBE.Service.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.ChatHub;
using BKConnectBE.Repository.RefreshTokens;
using BKConnectBE.Repository.Rooms;
using BKConnectBE.Service.Rooms;
using BKConnectBE.Repository.Classes;
using BKConnectBE.Service.Classes;
using BKConnectBE.Service.Faculites;
using BKConnectBE.Service.Relationships;
using BKConnectBE.Repository.Relationships;
using BKConnectBE.Repository.Messages;
using BKConnectBE.Service.Messages;
using BKConnectBE.Repository.FriendRequests;
using BKConnectBE.Service.FriendRequests;
using BKConnectBE.Service.WebSocket;
using BKConnectBE.Repository.Notifications;
using BKConnectBE.Service.Notifications;
using BKConnectBE.Service.Files;
using BKConnectBE.Repository.Files;
using Hangfire;
using BKConnectBE.Service.BackgroundJobs;
using BKConnectBE.Filter;
using Hangfire.Dashboard;
using BKConnectBE.Service.VideoCalls;

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

var connectionString = settings.SqlServer.ConnectionString;
connectionString = connectionString.Replace("{Host}", settings.SqlServer.Host);
connectionString = connectionString.Replace("{DbName}", settings.SqlServer.DbName);
connectionString = connectionString.Replace("{User}", settings.SqlServer.User);
connectionString = connectionString.Replace("{Password}", settings.SqlServer.Password);

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
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();
builder.Services.AddScoped<IRelationshipRepository, RelationshipRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IFacultyService, FacultyService>();
builder.Services.AddScoped<IRelationshipService, RelationshipService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IFriendRequestService, FriendRequestService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IVideoCallService, VideoCallService>();
builder.Services.AddScoped<IWebSocketService, WebSocketService>();
builder.Services.AddScoped<IBackgroundJobService, BackgroundJobService>();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddSignalR();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(settings.JwtConfig.AccessTokenKey)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(connectionString));
builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseWebSockets();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();
app.UseWebSockets();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new [] { new DashboardAuthorizeFilter() },
    IsReadOnlyFunc = (DashboardContext context) => true
});

app.UseMiddleware<ResultMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<ChatHub>("/chatHub");
    endpoints.MapHangfireDashboard();
});

GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute
{
    Attempts = Constants.ATTEMPTS_IN_HANGFIRE,
    DelaysInSeconds = new int[] { Constants.DELAY_IN_HANGFIRE }
});

app.Run();
