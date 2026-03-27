using System.ComponentModel.DataAnnotations;

namespace MafPlayground.Models;

public class ChatAgent
{
    [Required(ErrorMessage = "Id is required.")]
    public required string Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ConversationId { get; set; }
    public string? Instructions { get; set; }
    [Range(0, 2, ErrorMessage = "Temperature must be between 0 and 2.")]
    public float? Temperature { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "MaxOutputTokens must be at least 1.")]
    public int? MaxOutputTokens { get; set; }
    [Range(0, 1, ErrorMessage = "TopP must be between 0 and 1.")]
    public float? TopP { get; set; }
    [Range(0, int.MaxValue, ErrorMessage = "TopK must be 0 or greater.")]
    public int? TopK { get; set; }
    public float? FrequencyPenalty { get; set; }
    public float? PresencePenalty { get; set; }
    public long? Seed { get; set; }
    public string? ModelId { get; set; }
    public List<string>? StopSequences { get; set; }
    public bool? AllowMultipleToolCalls { get; set; }
    public bool? AllowBackgroundResponses { get; set; }
    //public ResponseContinuationToken? ContinuationToken { get; set; }
    //public AdditionalPropertiesDictionary? AdditionalProperties { get; set; }
    //public ReasoningOptions? Reasoning { get; set; }
    //public ChatResponseFormat? ResponseFormat { get; set; }
    //public ChatToolMode? ToolMode { get; set; }
}
