namespace LuyPOS.Api.Services;

public sealed class NotFoundException(string message) : Exception(message);
