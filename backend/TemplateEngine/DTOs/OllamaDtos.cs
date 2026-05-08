namespace TemplateEngine.DTOs;

public class OllamaRequest
{

    #region Properties

    public string Model { get; set; } = string.Empty;
    public MessageDto[] Messages { get; set; } = Array.Empty<MessageDto>();

    public bool Stream { get; set; } = false;

    #endregion

}

public class MessageDto
{

    #region Properties

    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    #endregion

}

public class OllamaResponse
{

    #region Properties

    public MessageDto Message { get; set; } = new();

    #endregion

}
