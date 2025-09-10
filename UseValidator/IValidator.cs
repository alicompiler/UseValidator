namespace UseValidator;

/// <summary>
///     Defines a simple contract for validating an incoming payload of type <typeparamref name="T" />.
///     Implementations should inspect the provided payload and return a <see cref="ValidationResult" />
///     describing whether the payload is valid and, if not, which validation errors occurred.
/// </summary>
/// <typeparam name="T">The type of the payload to validate (e.g., a request DTO).</typeparam>
public interface IValidator<in T>
{
    /// <summary>
    ///     Validates the specified payload instance and returns the result.
    /// </summary>
    /// <param name="payload">The payload instance to validate. Must be of type <typeparamref name="T" />.</param>
    /// <returns>
    ///     A <see cref="ValidationResult" /> indicating whether validation succeeded (<see cref="ValidationResult.IsValid" />)
    ///     and containing any validation error messages (<see cref="ValidationResult.Errors" />) when it did not.
    /// </returns>
    public ValidationResult ValidatePayload(T payload);
}
