using System.ComponentModel.DataAnnotations;

namespace ClinicalFlow.DTOs.Patients;

public sealed class DateOfBirthValidationAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null)
            return true;

        if (value is not DateOnly dateOfBirth)
            return false;

        return dateOfBirth <= DateOnly.FromDateTime(DateTime.UtcNow);
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} cannot be in the future.";
    }
}