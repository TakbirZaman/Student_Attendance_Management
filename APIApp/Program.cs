using BLL.Services;
using DAL;
using DAL.EF;
using DAL.Repos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/*
builder.Services.AddScoped<ClassRepo>();
builder.Services.AddScoped<StudentRepo>();
builder.Services.AddScoped<AttendanceRepo>();
*/

builder.Services.AddScoped<DataAccessFactory>();
builder.Services.AddScoped<ClassService>();
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<AttendanceService>();



// DbContext
builder.Services.AddDbContext<SMSContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DbConn"));
    // opt.EnableSensitiveDataLogging(); // optional, useful in development
});

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
