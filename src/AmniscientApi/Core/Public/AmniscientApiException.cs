using System;

namespace AmniscientApi;

/// <summary>
/// Base exception class for all exceptions thrown by the SDK.
/// </summary>
public class AmniscientApiException(string message, Exception? innerException = null)
    : Exception(message, innerException);
