using Microsoft.AspNetCore.Mvc;
using TemplateEngine.Models;
using TemplateEngine.Services;

namespace TemplateEngine.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemplateController(
        ExplicitTemplateProcessor explicitProcessor,
        AiTemplateProcessor aiProcessor,
        ILogger<TemplateController> logger)
        : ControllerBase
    {

        [HttpPost("process")]
        public async Task<IActionResult> ProcessTemplate([FromBody] TemplateRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Template))
                return BadRequest("Template cannot be empty");

            if (string.IsNullOrWhiteSpace(request.FirstName) ||
                string.IsNullOrWhiteSpace(request.LastName) ||
                string.IsNullOrWhiteSpace(request.MiddleName))
                return BadRequest("FirstName, LastName, and MiddleName are required");

            try
            {
                var user = new User(
                    request.FirstName,
                    request.LastName,
                    request.MiddleName,
                    request.Gender
                );

                string result;

                switch (request.ProcessingType)
                {
                    case ProcessingType.Explicit:
                        logger.LogInformation("Processing template with explicit inflection");
                        result = explicitProcessor.ProcessTemplate(user, request.Template);
                        break;
                    case ProcessingType.Ai:
                        logger.LogInformation("Processing template with AI model");
                        result = await aiProcessor.ProcessTemplateAsync(user, request.Template);
                        break;
                    default:
                        return BadRequest("Invalid processing type");
                }

                var response = new TemplateResponse
                {
                    Preview = result
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing template");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
