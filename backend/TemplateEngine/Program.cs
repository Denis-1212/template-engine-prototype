using TemplateEngine.Configuration;
using TemplateEngine.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowReactApp",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddScoped<ExplicitTemplateProcessor>();
builder.Services.AddHttpClient<AiTemplateProcessor>();

AppSettings appSettings = builder.Configuration.Get<AppSettings>() ?? new AppSettings();

builder.WebHost.UseUrls($"http://{appSettings.Host}:{appSettings.Port}");

WebApplication app = builder.Build();

app.UseCors("AllowReactApp");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
