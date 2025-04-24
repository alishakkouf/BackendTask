using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using BackendTask.Shared.ResultDtos;

namespace BackendTask.Common.Middlewares
{
    internal class ResultWrapperMiddleware
    {
        private readonly RequestDelegate _next;

        public ResultWrapperMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var isApi = context.Request.Path.ToString().Contains("/api/");
            var isExport = context.Request.Path.ToString().Contains("Export");

            if (!isApi || isExport)
            {
                await _next(context);
                return;
            }

            var currentBody = context.Response.Body;

            using (var memoryStream = new MemoryStream())
            {
                try
                {
                    context.Response.Body = memoryStream;
                    await _next(context);

                    if (context.Response.HasStarted) return;

                    memoryStream.Seek(0, SeekOrigin.Begin);
                    var readToEnd = await new StreamReader(memoryStream).ReadToEndAsync();

                    var success = context.Response.StatusCode >= 200 && context.Response.StatusCode <= 299;

                    var wrappedResult = new WrappedResultDto(
                    success ? JsonConvert.DeserializeObject(readToEnd) : null,
                    context.Response.StatusCode,
                    success ? "Success" : readToEnd // If failed, assign the error message
                    );


                    context.Response.Body = currentBody;
                    context.Response.ContentType ??= "application/json";
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(wrappedResult,
                    new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }));

                }
                catch
                {
                    if (!context.Response.HasStarted)
                        context.Response.Body = currentBody;
                    throw;
                }
            }
        }
    }

    public static class ResultWrapperMiddlewareExtensions
    {
        public static IApplicationBuilder UseResultWrapper(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResultWrapperMiddleware>();
        }
    }
}
