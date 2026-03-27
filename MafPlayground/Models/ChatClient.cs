using System.ComponentModel.DataAnnotations;

namespace MafPlayground.Models;

public class ChatClient
{
    [Required(ErrorMessage = "Id is required.")]
    public required string Id { get; set; }
    public string Description { get; set; } = string.Empty;
    [Required(ErrorMessage = "Provider is required.")]
    public required string Provider { get; set; }
    public string Model { get; set; } = "openai/gpt-4o-mini";
    public string Endpoint { get; set; } = "https://openrouter.ai/api/v1";
    public string OrganizationId { get; set; } = string.Empty;
    public string ProjectId { get; set; } = string.Empty;
    [Range(0, int.MaxValue, ErrorMessage = "MaxRetries must be 0 or greater.")]
    public int? MaxRetries { get; set; } // Retry policy, maximum number of retries to attempt
    public TimeSpan? NetworkTimeout { get; set; }
    public bool? EnableDistributedTracing { get; set; }
}
