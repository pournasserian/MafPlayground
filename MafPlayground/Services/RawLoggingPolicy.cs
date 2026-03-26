using System.ClientModel.Primitives;
using System.Text;

namespace MafPlayground.Services;

public class RawLoggingPolicy : PipelinePolicy
{
    public override void Process(PipelineMessage message, IReadOnlyList<PipelinePolicy> pipeline, int currentIndex)
    {
        // 1. Log Request before it goes out
        LogRequest(message);

        // 2. Move to the next policy in the chain
        ProcessNext(message, pipeline, currentIndex);

        // 3. Log Response after it comes back
        LogResponse(message);
    }

    public override async ValueTask ProcessAsync(PipelineMessage message, IReadOnlyList<PipelinePolicy> pipeline, int currentIndex)
    {
        // 1. Log Request
        await LogRequestAsync(message);

        // 2. Move to the next policy
        await ProcessNextAsync(message, pipeline, currentIndex);

        // 3. Log Response
        await LogResponseAsync(message);
    }

    private static void LogRequest(PipelineMessage message)
    {
        if (message.Request.Content != null)
        {
            // For simple logging, ToString() on BinaryContent usually works
            // but for production, you'd handle the stream carefully.
            Console.WriteLine($"[REQ URL]: {message.Request.Uri}");
            // Use a helper to read the content without disposing the stream
            using var stream = new MemoryStream();
            message.Request.Content.WriteTo(stream, default);
            var body = Encoding.UTF8.GetString(stream.ToArray());
            Console.WriteLine($"[REQ BODY]: {body}");
        }
    }

    private static void LogResponse(PipelineMessage message)
    {
        if (message.Response != null && message.Response.Content != null)
        {
            // Note: In some client versions, you may need to 'buffer' the response
            // if you want to read it and still let the UI process it.
            var body = message.Response.Content.ToString();
            Console.WriteLine($"[RESP STATUS]: {message.Response.Status}");
            Console.WriteLine($"[RESP BODY]: {body}");
        }
    }

    // Async versions follow the same logic...
    private static async Task LogRequestAsync(PipelineMessage message)
    {
        LogRequest(message);
    }

    private static async Task LogResponseAsync(PipelineMessage message)
    {
        LogResponse(message);
    }
}