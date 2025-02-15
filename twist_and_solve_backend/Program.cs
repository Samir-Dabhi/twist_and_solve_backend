using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using twist_and_solve_backend.Data;
using twist_and_solve_backend.Models;
using twist_and_solve_backend.Services;
using FluentValidation.AspNetCore;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);

// Load JWT settings from appsettings.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var token = context.Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrEmpty(token) && !token.StartsWith("Bearer "))
                {
                    context.Token = token; // Directly assign the raw token
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configure Swagger with JWT authentication
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Twist and Solve API", Version = "v1" });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter the JWT token like this: Bearer {your_token_here}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<CloudinaryService>(); 

//builder.Services.AddControllers()
//    .AddFluentValidation(fv =>
//        fv.RegisterValidatorsFromAssemblies(new[] { Assembly.GetExecutingAssembly() }));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<JwtService>();

// Register repositories
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<AlgorithmRepository>();
builder.Services.AddScoped<AchievementRepository>();
builder.Services.AddScoped<FeedbackRepository>();
builder.Services.AddScoped<LessonRepository>();
builder.Services.AddScoped<SettingsRepository>();
builder.Services.AddScoped<SolveRepository>();
builder.Services.AddScoped<VideoRepository>();
builder.Services.AddScoped<UserAchievementRepository>();
builder.Services.AddScoped<UserProgressRepository>();

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); // Ensure authentication middleware is enabled
app.UseAuthorization();

app.MapControllers();

app.Run();