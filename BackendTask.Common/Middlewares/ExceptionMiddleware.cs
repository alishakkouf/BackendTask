using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BackendTask.Shared;
using BackendTask.Shared.Exceptions;
using BackendTask.Shared.ResultDtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace BackendTask.Common.Middlewares
{
    public static class ExceptionMiddleware
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var factory = app.ApplicationServices.GetService<IStringLocalizerFactory>();
                    var localizer = factory.Create(typeof(CommonResource));
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        var exception = contextFeature.Error;
                        Log.Error(exception, "An unhandled exception occurred");

                        int code;
                        string message;
                        ErrorResultDto error;
                        switch (exception)
                        {
                            case ValidationException validationException:
                                code = (int)HttpStatusCode.BadRequest;
                                error = new ErrorResultDto(localizer[validationException.Message], string.Join(", ", validationException.ValidationResult.MemberNames));
                                break;
                            case EntityNotFoundException notFoundException:
                                code = (int)HttpStatusCode.NotFound;
                                error = new ErrorResultDto(localizer["EntityNotFound{1}", notFoundException.EntityName, notFoundException.EntityId]);
                                break;
                            case BusinessException businessException:
                                code = (int)HttpStatusCode.BadRequest;
                                error = new ErrorResultDto(businessException.Message);
                                break;
                            default:
                                code = (int)HttpStatusCode.InternalServerError;
                                error = new ErrorResultDto(localizer["InternalServerError"]);
                                break;
                        }

                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = code;

                        var result = JsonConvert.SerializeObject(
                            new WrappedResultDto(error, code),
                            new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }
                        );

                        await context.Response.WriteAsync(result);

                    }
                });
            });
        }
    }
}
