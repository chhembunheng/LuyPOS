namespace LuyPOS.Api.Services;

public sealed class RequestValidationException(IReadOnlyCollection<string> errors)
    : Exception(string.Join(Environment.NewLine, errors))
{
    public IReadOnlyCollection<string> Errors { get; } = errors;
}
