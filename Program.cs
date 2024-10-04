using Microsoft.EntityFrameworkCore;
using Reddit;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
     .AddJsonOptions(options =>
     {
         options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
     });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection"));
    options.LogTo(Console.WriteLine, LogLevel.Information);
    options.UseLazyLoadingProxies();
});

builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>(); // Replace "Program" with the actual type if different

    try
    {
        await next();
    }
    catch (Exception ex) {
        Console.WriteLine("Exception occured");
        Console.WriteLine(ex.ToString());
    }
});

app.MapControllers();

app.Run();
