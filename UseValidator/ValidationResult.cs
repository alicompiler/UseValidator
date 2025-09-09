namespace UseValidator;

public class ValidationResult
{
    public required bool IsValid { get; init; }
    public required List<string> Errors { get; init; }
}
