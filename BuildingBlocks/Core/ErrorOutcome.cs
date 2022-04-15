namespace TodoApi.BuildingBlocks.Core;

public class ErrorOutcome
{
    public static ErrorResult createFailureResult(string requestId, string errorType, string[] errorCodes)
    {
        if (string.IsNullOrWhiteSpace(requestId))
            throw new ArgumentNullException(nameof(requestId));
        if (string.IsNullOrWhiteSpace(errorType))
            throw new ArgumentNullException(nameof(errorType));
        return new ErrorResult(requestId, errorType, errorCodes);
    }
    
}