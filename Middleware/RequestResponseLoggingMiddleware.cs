using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace YasiroRegrave.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            // リクエストをログに記録
            context.Request.EnableBuffering();
            var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
            _logger.LogInformation($"Incoming Request: {context.Request.Method} {context.Request.Path} {requestBody}");
            context.Request.Body.Position = 0;

            // 元のレスポンスボディストリームへのポインタをコピー
            var originalResponseBodyStream = context.Response.Body;

            // レスポンスをキャプチャするための新しいメモリーストリームを作成
            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                // リクエストの処理を継続
                await _next(context);

                // レスポンスをログに記録
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                _logger.LogInformation($"Outgoing Response: {context.Response.StatusCode} {responseBodyText}");
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                // 新しいメモリーストリームの内容を元のレスポンスボディストリームにコピー
                await responseBody.CopyToAsync(originalResponseBodyStream);
            }
        }
    }
}
