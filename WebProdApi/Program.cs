using WebProdApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//Register Context
builder.Services.AddScoped<AdoNetContext>(); // Dependecy Injection

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Configurer CORS in the API (if MVC and API are in different domains)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMvc",
        builder => builder.WithOrigins("https://localhost:44310") // URL del MVC
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
var app = builder.Build();
app.UseCors("AllowMvc");


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
