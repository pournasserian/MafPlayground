namespace MafPlayground.Models;

public class ChatAgent
{
    public required string Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ConversationId { get; set; }
    public string? Instructions { get; set; }
    public float? Temperature { get; set; }
    public int? MaxOutputTokens { get; set; }
    public float? TopP { get; set; }
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
