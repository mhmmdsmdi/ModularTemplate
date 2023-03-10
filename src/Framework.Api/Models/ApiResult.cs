using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Common.Extensions;

namespace Framework.Api.Models;

public class ApiResult
{
    public bool IsSuccess { get; set; }
    public ApiResultStatusCode StatusCode { get; set; }

    public string Message { get; set; }
    public string RequestId { get; }

    public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, string message = null)
    {
        IsSuccess = isSuccess;
        StatusCode = statusCode;
        Message = message ?? statusCode.ToDisplay();
        RequestId = Activity.Current.Id;
    }
}

public class ApiResult<TData> : ApiResult
    where TData : class
{
    public TData Data { get; set; }

    public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, TData data, string message = null)
        : base(isSuccess, statusCode, message)
    {
        Data = data;
    }
}

public enum ApiResultStatusCode
{
    [Display(Name = "Success")]
    Success = 200,

    [Display(Name = "Server Error")]
    ServerError = 500,

    [Display(Name = "Bad Request Error")]
    BadRequest = 400,

    [Display(Name = "Not Found")]
    NotFound = 404,

    [Display(Name = "Request Process Error")]
    EntityProcessError = 422,

    [Display(Name = "Authentication Error")]
    UnAuthorized = 401,

    [Display(Name = "Authorization Error")]
    Forbidden = 403,

    [Display(Name = "Not Acceptable")]
    NotAcceptable = 406,

    [Display(Name = "Failed Dependency")]
    FailedDependency = 424
}