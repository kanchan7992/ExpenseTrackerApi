using System.ComponentModel.DataAnnotations;

public class CreateExpenseDto
{
    [Required]
    public decimal Amount { get; set; }

    public string? Description { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public int CategoryId { get; set; }
}