// <copyright file="ControllerExtensions.cs" company="Synchron">
// Copyright (c) Synchron. All rights reserved.
// Licensed under the Proprietary license. See LICENSE file in the project root for full license information.
// </copyright>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

// Namespace olarak "Microsoft.AspNetCore.Mvc" verilmesinin sebebi, Controller sınıfı kullanılan her yerde alttaki extension'ların -using'e ilave bir namespace eklemeden- kullanılabilir olmasıdır.
namespace Microsoft.AspNetCore.Mvc;

public static class ControllerExtensions
{
    public static IActionResult ServiceResult<T>(this ControllerBase controller, IServiceResult<T> resultWithDto)
        => resultWithDto.HasErrors
            ? controller.StatusCode(resultWithDto.Errors.StatusCode, resultWithDto.Errors)
            : controller.Ok(resultWithDto.Data);

    /// <summary>
    ///   Returns error object from api.
    /// </summary>
    /// <param name="controller">this</param>
    /// <param name="modelState">ModelStateDictionary</param>
    /// <param name="httpStatusCode">Microsoft.AspNetCore.Http.StatusCodes (default=Status422UnprocessableEntity)</param>
    /// <returns>ErrorResult object</returns>
    public static IActionResult ErrorResult(
        this ControllerBase controller,
        ModelStateDictionary modelState,
        int httpStatusCode = StatusCodes.Status422UnprocessableEntity)
    {
        var errSum = new ErrorSummary("There are some errors in the model.");
        foreach (var key in modelState.Keys)
        {
            var value = modelState[key];
            foreach (var error in value.Errors)
            {
                errSum.Items.Add(new ErrorItem(key, error.ErrorMessage));
            }
        }

        return ErrorResult(controller, errSum, httpStatusCode);
    }

    /// <summary>
    ///   Returns error object from api.
    /// </summary>
    /// <param name="controller">this</param>
    /// <param name="errorMessage">Error message</param>
    /// <returns>ErrorResult object</returns>
    public static IActionResult NotAuthorizedResult(this ControllerBase controller, string errorMessage = null)
        => ErrorResult(controller, errorMessage ?? "You are not authorized for this process");

    /// <summary>
    ///   Returns error object from api.
    /// </summary>
    /// <param name="controller">this</param>
    /// <param name="errors">ErrorSummary object</param>
    /// <param name="httpStatusCode">Microsoft.AspNetCore.Http.StatusCodes (default=Status422UnprocessableEntity)</param>
    /// <returns>ErrorResult object</returns>
    public static IActionResult ErrorResult(
        this ControllerBase controller,
        IErrorSummary errors,
        int httpStatusCode = StatusCodes.Status422UnprocessableEntity)
        => controller.StatusCode(httpStatusCode, errors);

    /// <summary>
    ///   Returns error object from api.
    /// </summary>
    /// <param name="controller">this</param>
    /// <param name="errorMessage">Error message</param>
    /// <param name="httpStatusCode">Microsoft.AspNetCore.Http.StatusCodes (default=Status422UnprocessableEntity)</param>
    /// <returns>ErrorResult object</returns>
    public static IActionResult ErrorResult(
        this ControllerBase controller,
        string errorMessage,
        int httpStatusCode = StatusCodes.Status422UnprocessableEntity)
        => ErrorResult(controller, new ErrorSummary(errorMessage), httpStatusCode);

    /// <summary>
    ///   Returns error object from api.
    /// </summary>
    /// <param name="controller">this</param>
    /// <param name="activityId">Activity Id</param>
    /// <param name="errorMessage">Error message</param>
    /// <param name="httpStatusCode">Microsoft.AspNetCore.Http.StatusCodes (default=Status422UnprocessableEntity)</param>
    /// <returns>ErrorResult object</returns>
    public static IActionResult ErrorResult(
        this ControllerBase controller,
        string activityId,
        string errorMessage,
        int httpStatusCode = StatusCodes.Status422UnprocessableEntity)
        => ErrorResult(controller, new ErrorSummary(activityId, errorMessage), httpStatusCode);

    /// <summary>
    ///   Returns error object from api when action parameter model is null (or not matched).
    /// </summary>
    /// <param name="controller">this</param>
    /// <returns>ErrorResult object</returns>
    public static IActionResult ModelIsNullResult(this ControllerBase controller)
        =>
            ErrorResult(
                controller,
                new ErrorSummary("Parameter model is null or not matched"),
                StatusCodes.Status400BadRequest);
}
