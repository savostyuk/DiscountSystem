﻿using FluentValidation.Results;

namespace DiscountSystem.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }
    public ValidationException() : base("One or more validation errors have occured. See the list below.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures) : this()
    {
        Errors = failures
            .GroupBy(f => f.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup =>  failureGroup.Key, failureGroup => failureGroup.ToArray());
    }
}
