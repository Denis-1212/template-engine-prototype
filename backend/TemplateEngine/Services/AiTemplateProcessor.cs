namespace TemplateEngine.Services;

using Configuration;

using DTOs;

using Microsoft.Extensions.Options;

using Models;

public class AiTemplateProcessor
{

    #region Fields

    private readonly HttpClient _httpClient;
    private readonly string _model;
    private readonly ILogger<AiTemplateProcessor> _logger;

    #endregion

    #region Constructors

    public AiTemplateProcessor(HttpClient httpClient, IOptions<AppSettings> options, ILogger<AiTemplateProcessor> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        OllamaSettings ollamaSettings = options.Value.Ollama;

        _model = ollamaSettings.Model;

        _httpClient.BaseAddress = new Uri(ollamaSettings.Url);
        _httpClient.Timeout = TimeSpan.FromMinutes(ollamaSettings.TimeoutMinutes);
    }

    #endregion

    #region Methods

    public async Task<string> ProcessTemplateAsync(User user, string template)
    {
        string systemPrompt = """
                              Ты — эксперт по русскому языку. Твоя задача — заполнить шаблон, просклоняв имя, фамилию и отчество в правильном падеже согласно контексту каждого предложения.

                              ПРАВИЛА:
                              1. Проанализируй каждое предложение в шаблоне.
                              2. Определи, какой падеж требуется для каждого вхождения {{lastname}}, {{name}}, {{middlename}}.
                              3. Замени {{lastname}}, {{name}}, {{middlename}} на соответствующие формы в правильном падеже.
                              4. Проверь правильность согласования слов в предложении.
                              5. Сохрани всё остальное форматирование без изменений.
                              6. Верни ТОЛЬКО заполненный шаблон, без JSON, без пояснений.

                              Верни ТОЛЬКО заполненный шаблон, без пояснений.
                              """;

        string userPrompt = $"""
                             Фамилия: {user.LastName}
                             Имя: {user.FirstName}
                             Отчество: {user.MiddleName}

                             Шаблон для заполнения:
                             {template}
                             """;

        var request = new OllamaRequest
        {
            Model = _model,
            Messages =
            [
                new MessageDto
                {
                    Role = "system",
                    Content = systemPrompt
                },
                new MessageDto
                {
                    Role = "user",
                    Content = userPrompt
                }
            ],
            Stream = false
        };

        try
        {
            _logger.LogInformation("Отправка запроса к Ollama модели {Model}", _model);

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/chat", request);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Ошибка Ollama API: {StatusCode} - {Content}", response.StatusCode, errorContent);
                throw new HttpRequestException($"Ollama API error: {response.StatusCode}");
            }

            var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
            string answer = result?.Message.Content ?? string.Empty;

            _logger.LogInformation("Получен ответ от Ollama модели");
            return answer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при обработке шаблона через AI");
            throw new Exception("Не удалось обработать шаблон через AI модель. Убедитесь, что Ollama запущен и модель доступна.", ex);
        }
    }

    #endregion

}
