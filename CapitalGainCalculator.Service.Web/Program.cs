using System.Text.Json.Serialization;
using CapitalGainCalculator.Service.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();   
builder.Services.AddControllers().AddJsonOptions(options => 
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddCapitalGainCalcServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.UseExceptionHandler(exceptionHandlerApp => 
{
    exceptionHandlerApp.Run(async context => 
    {
        if (context.Response.StatusCode == StatusCodes.Status500InternalServerError)
        {
            await context.Response.WriteAsync("Internal Server Error");
        }
    });
});
app.Run();