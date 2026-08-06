using GarasTransport.Application.Responses;

namespace GarasTransport.Api.Common;

/// <summary>
/// Maps Application-layer ServiceResults onto the FROZEN wire envelope
/// ({ Result, Errors, Data, PaginationHeader } / PostResponse). This is the only
/// place the two shapes meet — the public contract stays byte-for-byte intact.
/// </summary>
public static class ServiceResultMapper
{
    public static object ToEnvelope<T>(this ServiceResult<T> result)
    {
        if (!result.IsSuccess) return Fail(result.ErrorMessages);
        return Envelope.Success(result.Data!);
    }

    public static object ToEnvelope<T>(this ServiceResultWithHeader<T> result)
    {
        if (!result.IsSuccess) return Fail(result.ErrorMessages);
        return Envelope.SuccessList(result.Data!, result.Pagination);
    }

    public static object ToEnvelope(this ServiceWriteResult result)
    {
        if (!result.IsSuccess) return Fail(result.ErrorMessages);
        return Envelope.SuccessWrite(result.Id, result.Message);
    }

    private static BaseResponse Fail(List<ServiceError> errors) => new()
    {
        Result = false,
        Errors = errors.Select(e => new ErrorItem { ErrorCode = e.Code, ErrorMSG = e.Message }).ToList(),
    };
}
