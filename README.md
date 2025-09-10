# UseValidate

Lightweight action filter–based request validation for ASP.NET Core MVC controllers.

Use simple attribute annotations to validate `[FromBody]` and `[FromQuery]` parameters with your own validator classes.

## Contents
- [Installation](#installation)
- [What is it?](#what-is-it)
- [Examples](#examples)
- [How it works](#how-it-works)
- [Validating request body](#validating-request-body)
- [Validating query parameters](#validating-query-parameters)
- [Validator requirements](#validator-requirements)
- [Contributing](#contributing)
- [License](#license)

## Installation

```
dotnet add package UseValidator

// if you use FluentValidation
// dotnet add package UseValidatorExtension.FluentValidation

```

## What is it?
**UseValidator** provides two attributes you can put on controller actions:
- **UseBodyValidator** — validates a body parameter (e.g., `[FromBody] MyDto body`)
- **UseQueryValidator** — validates a query parameter object (e.g., `[FromQuery] MyQuery query`)

You supply a validator type via the Validator property.

If validation fails, the action short-circuits and returns HTTP 400 with an array of error messages.


## Examples

```

[HttpPost]
[UseBodyValidator(Validator = typeof(CreateUserValidator))] // <=======
public IActionResult Create([FromBody] CreateUserRequest body)
{
    // If validation failed, this code won't be reached.
    userService.CreateUser(body);
    return Ok();
}


[HttpGet]
[UseQueryValidator(Validator = typeof(SearchQueryValidator))] // <=======
public IActionResult Search([FromQuery] SearchQuery query)
{
    // If validation failed, this code won't be reached.
    var results = searchService.Search(query);
    return Ok(results);
}

```

## How it works

Under the hood, both attributes derive from `UseValidator` (an ActionFilterAttribute):

- The filter extracts the specified action argument by binding source type (`FromQuery`, `FromBody`).
- It creates an instance of your validator type (using an empty public constructor).
- It calls ValidatePayload(payload) via reflection.
- If the `result is null or IsValid == false`, it returns `BadRequestObjectResult(errors)` and prevents the action from
  running.

## Validating request body
Use `UseBodyValidator` to validate a `[FromBody]` parameter.
```
[HttpPost("/items")]
[UseBodyValidator(Validator = typeof(MyDtoValidator))]
public IActionResult Create([FromBody] MyDto dto) => Ok();
```

## Validating query parameters
Use `UseQueryValidator` to validate a `[FromQuery]` parameter object.
```
[HttpGet("/search")]
[UseQueryValidator(Validator = typeof(SearchQueryValidator))]
public IActionResult Search([FromQuery] SearchQuery q) => Ok();
```

## Validator requirements
- Implement IValidator<T> and return a ValidationResult.
- Ensure an empty public constructor is present.
- Put your validation logic in ValidatePayload.

```
using UseValidator;

public class CreateUserValidator : IValidator<CreateUserRequest>
{
    public ValidationResult ValidatePayload(CreateUserRequest payload)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(payload.Username))
            errors.Add("Username is required");

        if (payload.Age < 18)
            errors.Add("Age must be at least 18");

        return new ValidationResult
        {
            IsValid = errors.Count == 0,
            Errors = errors
        };
    }
}
```

## Fluent Validation Extension

If you prefer using [FluentValidation](https://fluentvalidation.net/), you can use the `UseValidatorExtension.FluentValidation` package.

Install the package:

```
dotnet add package UseValidatorExtension.FluentValidation
```

Then, create a validator by inheriting from `BaseValidator<T>`:

```csharp
using FluentValidation;
using UseValidatorExtension.FluentValidation;
    
public class CreateUserFluentValidator : BaseValidator<CreateUserRequest>
{
    public CreateUserFluentValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required");

        RuleFor(x => x.Age)
            .GreaterThanOrEqualTo(18).WithMessage("Age must be at least 18");
    }
}

// in your controller

[HttpPost("/users")]
[UseBodyValidator(Validator = typeof(CreateUserFluentValidator))]
public IActionResult Create([FromBody] CreateUserRequest body) => Ok();
```

## Contributing
Issues and PRs are welcome. Please include tests for new features.

If you’d like to contribute, please follow these steps:

1. **Fork** the repository
2. **Create a new branch** for your feature or fix

   ```bash
   git checkout -b feature/your-feature-name
   ```

3. Make your changes and commit them with a clear message

   ```bash
   git commit -m "description of feature or fix"
   ```

4. Push to your fork

   ```bash
   git push origin feature/your-feature-name
   ```

5. Open a Pull Request to the main branch of this repository

   Please make sure your changes follow the existing code style and include tests if applicable.

If you’re not sure where to start, feel free to check the issues

## License

This project is licensed under the [MIT License](LICENSE).
