using Microsoft.AspNetCore.Mvc;
using PatientReferralManagementAPI.DTO.Common;
using System.Text.Json;

namespace PatientReferralManagementAPI.Validators
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomValidation(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = new Dictionary<string, List<string>>();

                    foreach (var modelState in context.ModelState)
                    {
                        var rawKey = modelState.Key;

                        // take last field name (handle nested object)
                        var fieldName = rawKey.Contains(".")
                            ? rawKey.Split('.').Last()
                            : rawKey;

                        foreach (var error in modelState.Value.Errors)
                        {
                            string message = null;

                            // JSON parsing error
                            if (error.Exception is JsonException ||
                                error.ErrorMessage.Contains("could not be converted"))
                            {
                                message = $"Invalid format for {fieldName}";
                            }
                            else if (!error.ErrorMessage.Contains("dto field"))
                            {
                                message = error.ErrorMessage;
                            }

                            message = message?.Replace("'", "");

                            if (message == null)
                                continue;

                            if (!errors.ContainsKey(fieldName))
                                errors[fieldName] = new List<string>();

                            errors[fieldName].Add(message);
                        }
                    }

                    // remove duplicate message per field
                    foreach (var key in errors.Keys.ToList())
                    {
                        errors[key] = errors[key].Distinct().ToList();
                    }

                    var response = new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Validation failed",
                        Errors = errors,
                        Data = null
                    };

                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }
    }
}
