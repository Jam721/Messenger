using Messenger.Application.Interfaces;
using Messenger.Application.Interfaces.Auth;
using Messenger.Application.Interfaces.Services;
using Messenger.Application.Services;
using Messenger.Core.Stores;
using Messenger.Extensions;
using Messenger.Infrastructure;
using Messenger.Persistence.DataBases;
using Messenger.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;

// Подключаем базовые сервисы
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddControllers();

// Настройка JWT-опций
services.Configure<JwtOptions>(config.GetSection(nameof(JwtOptions)));

// Настройка базы данных
services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(config.GetConnectionString("Database"));
});

services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000") // Разрешаем запросы с фронтенда React
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // Включаем отправку куки
        });
});


// Регистрация сервисов и репозиториев
services.AddScoped<IPostRepository, PostRepository>();
services.AddScoped<IPostService, PostService>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<IUserService, UserService>();
services.AddScoped<IJwtProvider, JwtProvider>();
services.AddScoped<IPasswordHasher, PasswordHasher>();

// Добавляем аутентификацию через ваше расширение
services.AddApiAuthentication(services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>());

var app = builder.Build();

// Swagger для режима разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowReactApp");

// Правильный порядок подключения middleware
app.UseAuthentication();
app.UseAuthorization();

// Маршрутизация контроллеров
app.MapControllers();

app.Run();