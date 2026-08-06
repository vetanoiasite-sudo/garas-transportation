namespace GarasTransport.Application.Responses;

// Response envelopes — faithful to the documented CoreApi contract (API.md §4).
// PascalCase keys are intentional (the Flutter/Next clients depend on them
// byte-for-byte). System.Text.Json serializes these property names as-is.

public class ErrorItem
{
    public string ErrorCode { get; set; } = string.Empty;
    public string ErrorMSG { get; set; } = string.Empty;
}

public class PaginationHeader
{
    public int CurrentPage { get; set; }
    public int ItemsPerPage { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }
}

public class BaseResponse
{
    public bool Result { get; set; }
    public List<ErrorItem> Errors { get; set; } = new();
}

public class BaseResponseWithData<T> : BaseResponse
{
    public T? Data { get; set; }
}

public class BaseResponseWithDataAndHeader<T> : BaseResponseWithData<T>
{
    public PaginationHeader PaginationHeader { get; set; } = new();
}

/// <summary>Write/ack response (mirrors PostResponseModel: adds Id + Message).</summary>
public class PostResponse : BaseResponse
{
    public object? Id { get; set; }
    public string? Message { get; set; }
}

public static class Envelope
{
    public static BaseResponseWithData<T> Success<T>(T data) =>
        new() { Result = true, Errors = new(), Data = data };

    public static BaseResponseWithDataAndHeader<T> SuccessList<T>(T data, PaginationHeader pagination) =>
        new() { Result = true, Errors = new(), Data = data, PaginationHeader = pagination };

    public static PostResponse SuccessWrite(object? id = null, string? message = null) =>
        new() { Result = true, Errors = new(), Id = id, Message = message };

    public static BaseResponse Fail(string errorCode, string errorMsg) =>
        new() { Result = false, Errors = new() { new ErrorItem { ErrorCode = errorCode, ErrorMSG = errorMsg } } };

    public static PaginationHeader Pagination(int currentPage, int itemsPerPage, int totalItems) => new()
    {
        CurrentPage = currentPage,
        ItemsPerPage = itemsPerPage,
        TotalItems = totalItems,
        TotalPages = Math.Max(1, (int)Math.Ceiling(totalItems / (double)Math.Max(1, itemsPerPage))),
    };
}
