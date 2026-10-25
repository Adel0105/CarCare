using System.ComponentModel.DataAnnotations;
using CarCare.Domain.Enums;

namespace CarCare.Application.Expenses;

public class CreateExpenseDto : IValidatableObject
{
    public DateOnly Date { get; set; }

    [Range(typeof(decimal), "0", "79228162514264337593543950335")]
    public decimal Amount { get; set; }

    public ExpenseCategory Category { get; set; }

    public string? Description { get; set; }

    [Range(0, int.MaxValue)]
    public int? Mileage { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Date == default)
        {
            yield return new ValidationResult(
                "Date is required.",
                [nameof(Date)]);
        }

        if (!Enum.IsDefined(Category))
        {
            yield return new ValidationResult(
                "Category is not a valid expense category.",
                [nameof(Category)]);
        }
    }
}
