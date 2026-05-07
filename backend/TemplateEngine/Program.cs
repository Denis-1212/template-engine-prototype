using TemplateEngine.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddScoped<ExplicitTemplateProcessor>();
builder.Services.AddHttpClient<AiTemplateProcessor>();

var app = builder.Build();

app.UseCors("AllowReactApp");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();


