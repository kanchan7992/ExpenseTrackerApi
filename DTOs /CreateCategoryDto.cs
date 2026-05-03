using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerApi.DTOs;

public class CreateCategoryDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
}