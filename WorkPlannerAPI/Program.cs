using Application.DAOInterfaces;
using Application.Logic;
using Application.LogicInterfaces;
using FileData;
using FileData.DAOs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// NOTE: We have added these!
builder.Services.AddScoped<FileContext>();
builder.Services.AddScoped<IWorkerDao, WorkerFileDao>();
builder.Services.AddScoped<IWorkShiftDao, WorkShiftFileDao>();
builder.Services.AddScoped<IWorkerLogic,WorkerLogic>();
builder.Services.AddScoped<IWorkShiftLogic,WorkShiftLogic>();


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