using INT.Assessment.BLL.Implementations;
using INT.Assessment.BLL.Signatures;
using INT.Assessment.LOGGER;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IFileLogger, FileLogger>();

builder.Services.AddScoped<IBLLCommon, BLLCommon>();


var app = builder.Build();

app.UseCors("INT.Assessment.Cors");
_ = app.UseCors(builder =>
{
    _ = builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

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
