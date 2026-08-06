namespace GarasTransport.Application.Responses;

/// <summary>
/// Application-layer result wrapper (doc §8.4). Services never throw for business
/// failures and never return raw nulls — they return a ServiceResult. The API
/// layer maps it onto the frozen wire envelope (Envelope.*) so the public
/// contract stays byte-for-byte identical.
/// </summary>
public class ServiceResult<T>
{
    public bool IsSuccess { get; set; } = true;
    public T? Data { get; set; }
    public List<ServiceError> ErrorMessages { get; set; } = new();

    public static ServiceResult<T> Success(T data) => new() { IsSuccess = true, Data = data };

    public static ServiceResult<T> Fail(string code, string message) => new()
    {
        IsSuccess = false,
        ErrorMessages = new() { new ServiceError { Code = code, Message = message } },
    };
}

/// <summary>Error with the legacy wire code (Err101 / Err404 / Err-P2 ...).</summary>
public class ServiceError
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

/// <summary>List result + pagination (maps onto BaseResponseWithDataAndHeader).</summary>
public class ServiceResultWithHeader<T> : ServiceResult<T>
{
    public PaginationHeader Pagination { get; set; } = new();

    public static ServiceResultWithHeader<T> SuccessList(T data, PaginationHeader pagination) =>
        new() { IsSuccess = true, Data = data, Pagination = pagination };
}

/// <summary>Write/ack result (maps onto PostResponse: Id + optional Message).</summary>
public class ServiceWriteResult : ServiceResult<object>
{
    public object? Id { get; set; }
    public string? Message { get; set; }

    public static ServiceWriteResult SuccessWrite(object? id = null, string? message = null) =>
        new() { IsSuccess = true, Id = id, Message = message };

    public static new ServiceWriteResult Fail(string code, string message) => new()
    {
        IsSuccess = false,
        ErrorMessages = new() { new ServiceError { Code = code, Message = message } },
    };
}
