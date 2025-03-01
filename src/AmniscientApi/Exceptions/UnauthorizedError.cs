namespace AmniscientApi;

/// <summary>
/// This exception type will be thrown for any non-2XX API responses.
/// </summary>
public class UnauthorizedError(UnauthorizedErrorBody body)
    : AmniscientApiApiException("UnauthorizedError", 401, body)
{
    /// <summary>
    /// The body of the response that triggered the exception.
    /// </summary>
    public new UnauthorizedErrorBody Body => body;
}
