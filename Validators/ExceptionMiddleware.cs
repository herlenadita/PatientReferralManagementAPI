using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PatientReferralManagementAPI.DTO.Common;
using System.Net;
using System.Text.Json;
using static PatientReferralManagementAPI.Validators.Exceptions;

namespace PatientReferralManagementAPI.Validators
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IOptions<JsonOptions> jsonOptions)
        {
            _next = next;
            _logger = logger;
            _jsonOptions = jsonOptions.Value.JsonSerializerOptions;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                await Handle(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (BadRequestException ex)
            {
                _logger.LogWarning(ex, ex.Message);
                await Handle(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "Invalid JSON format");

                await Handle(context, HttpStatusCode.BadRequest,
                    "Invalid request format. Please check your input!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error");
                await Handle(context, HttpStatusCode.InternalServerError, "Internal server error");
            }
        }

        private async Task Handle(HttpContext context, HttpStatusCode code, string message)
        {
            context.Response.StatusCode = (int)code;
            context.Response.ContentType = "application/json";

            var response = new ApiResponse<string>
            {
                Success = false,
                Message = message,
                Data = null
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, _jsonOptions));
        }
    }
}
