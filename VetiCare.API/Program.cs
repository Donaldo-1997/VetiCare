using Microsoft.EntityFrameworkCore;
using VetiCare.DataAccess.Context;

var builder = WebApplication.CreateBuilder(args);

// Entity Framework Core
builder.Services.AddDbContext<VetiCareDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => Results.Redirect("/swagger"));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
