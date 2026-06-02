namespace LuyPOS.Application.Common.Exceptions;

public sealed class ValidationException(IReadOnlyCollection<string> errors)
    : Exception(string.Join(Environment.NewLine, errors))
{
    public IReadOnlyCollection<string> Errors { get; } = errors;
}
