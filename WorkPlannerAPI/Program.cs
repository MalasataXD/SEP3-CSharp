using System.Text;
using Application.DAOInterfaces;
using Application.Logic;
using Application.LogicInterfaces;
using DatabaseConnection.DAOs;
using RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// NOTE: We have added these!
builder.Services.AddScoped<Sender>();
builder.Services.AddScoped<Receiver>();
builder.Services.AddScoped<IWorkerDao, WorkerDao>();
builder.Services.AddScoped<IWorkShiftDao, WorkShiftDao>();

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


// NOTE: We have added this!
app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    .AllowCredentials());



app.MapControllers();

app.Run();