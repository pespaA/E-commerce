using System.Net;
using Microsoft.AspNetCore.Mvc;
using Shared.ErrorModels;

namespace E_commerce.Factories
{
    public class ApiResponseFactory
    {
        public static IActionResult CustomValidationErrors(ActionContext actionContext)
        {
            //Get All Errors in ModelStatue
            var errors = actionContext.ModelState.Where(error => error.Value.Errors.Any())
                .Select(error => new ValidationError
                {
                    Filed=error.Key,//Id
                    Errors= error.Value.Errors.Select(e=>e.ErrorMessage),

                });
            // Create Custom Response
            var respone = new ValidationErrorsResponse
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                ErrorMessage = "There is A proplem With Validation",
                Errors = errors,
            };
            // Return 
            return new BadRequestObjectResult(respone);
        }
    }
}
