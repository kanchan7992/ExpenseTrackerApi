namespace ExpenseTrackerApi.Models;
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public int UserId { get; set; }
    public User User { get; set; }

    public List<Expense> Expenses { get; set; }
}