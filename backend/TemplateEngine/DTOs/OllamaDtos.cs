namespace TemplateEngine.DTOs
{
    public class OllamaRequest
    {
        public string Model { get; set; } = string.Empty;
        public MessageDto[] Messages { get; set; } = Array.Empty<MessageDto>();
        public bool Stream { get; set; } = false;
    }

    public class MessageDto
    {
        public string Role { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }

    public class OllamaResponse
    {
        public MessageDto Message { get; set; } = new();
    }
}
