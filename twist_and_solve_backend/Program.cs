using twist_and_solve_backend.Data;
using twist_and_solve_backend.Validators;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


//builder.Services.AddControllers()
//    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AchievementValidator>());
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
