using NPetrovich;
using System.Text.Json.Serialization;

namespace TemplateEngine.Models
{
    public class TemplateRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Template { get; set; } = string.Empty;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ProcessingType ProcessingType { get; set; } = ProcessingType.Explicit;
        public Gender? Gender { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProcessingType
    {
        Explicit, 
        Ai       
    }
}

