using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WebApiCors.Data;
using WebApiCors.Filters;
using WebApiCors.Helpers;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddCors();
builder.Services.AddDbContext<UserContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("ValidJwtToken", policy => policy.Requirements.Add(new ValidTokenRequirement("")));
//});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(opt => opt
    .WithOrigins(new[] {"http://localhost:3000", "http://localhost:8080", "http://localhost:4200" })
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    );
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
