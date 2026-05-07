using TemplateEngine.DTOs;
using TemplateEngine.Models;

namespace TemplateEngine.Services
{
    public class AiTemplateProcessor
    {
        private readonly HttpClient _httpClient;
        private readonly string _model;
        private readonly ILogger<AiTemplateProcessor> _logger;

        public AiTemplateProcessor(HttpClient httpClient, IConfiguration configuration, ILogger<AiTemplateProcessor> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            var ollamaUrl = configuration["Ollama:Url"] ?? "http://localhost:11434";
            var timeoutMinutes = configuration.GetValue("Ollama:TimeoutMinutes", 2);
            _model = configuration["Ollama:Model"] ?? "qwen2.5:7b";

            _httpClient.BaseAddress = new Uri(ollamaUrl);
            _httpClient.Timeout = TimeSpan.FromMinutes(timeoutMinutes);
        }

        public async Task<string> ProcessTemplateAsync(User user, string template)
        {
            var systemPrompt = """
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

            var userPrompt = $"""
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
                    new MessageDto { Role = "system", Content = systemPrompt },
                    new MessageDto { Role = "user", Content = userPrompt }
                ],
                Stream = false
            };

            try
            {
                _logger.LogInformation("Отправка запроса к Ollama модели {Model}", _model);

                var response = await _httpClient.PostAsJsonAsync("/api/chat", request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Ошибка Ollama API: {StatusCode} - {Content}", response.StatusCode, errorContent);
                    throw new HttpRequestException($"Ollama API error: {response.StatusCode}");
                }

                var result = await response.Content.ReadFromJsonAsync<OllamaResponse>();
                var answer = result?.Message.Content ?? string.Empty;

                _logger.LogInformation("Получен ответ от Ollama модели");
                return answer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке шаблона через AI");
                throw new Exception("Не удалось обработать шаблон через AI модель. Убедитесь, что Ollama запущен и модель доступна.", ex);
            }
        }
    }
}
