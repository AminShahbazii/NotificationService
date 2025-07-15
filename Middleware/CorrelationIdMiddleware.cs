using MassTransit;
using Serilog.Context;
using MassTransit.Logging;
using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;

namespace NotificationService.Middleware
{
    public class CorrelationIdMiddleware
    {
  
        private const string CorrelationIdHeader = "X-Correlation-ID";
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
           
        }

        public async Task Invoke(HttpContext context)
        {
            var correlationId = context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationIdValues)
                                    ? correlationIdValues.ToString()
                                    : Guid.NewGuid().ToString();

            context.Response.Headers[CorrelationIdHeader] = correlationId;

            context.Items[CorrelationIdHeader] = correlationId;

            using (Serilog.Context.LogContext.PushProperty("CorrelationId", correlationId))
            {
                await _next(context);
            }
        }
    }
    
}
