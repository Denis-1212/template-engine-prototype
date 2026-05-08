namespace TemplateEngine.Configuration
{
    public class AppSettings
    {
        public string Host { get; set; } = "0.0.0.0";
        public int Port { get; set; } = 5000;
        public OllamaSettings Ollama { get; set; } = new();
    }

    public class OllamaSettings
    {
        public string Url { get; set; } = "http://localhost:11434";
        public string Model { get; set; } = "qwen2.5:7b";
        public int TimeoutMinutes { get; set; } = 2;
    }
}
