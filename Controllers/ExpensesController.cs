using ExpenseTrackerApi.Data;
using ExpenseTrackerApi.DTOs;
using ExpenseTrackerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExpensesController : ControllerBase
{
    private readonly AppDbContext _context;

    public ExpensesController(AppDbContext context)
    {
        _context = context;
    }

    // ➤ Add Expense
    [HttpPost]
    public IActionResult AddExpense(CreateExpenseDto dto)
    {
       if (!ModelState.IsValid)
          return BadRequest(ModelState);
        
        var userId = GetUserId();

        // Validate category belongs to user
        var category = _context.Categories
            .FirstOrDefault(c => c.Id == dto.CategoryId && c.UserId == userId);

        if (category == null)
            return BadRequest("Invalid category");

        var expense = new Expense
        {
            Amount = dto.Amount,
            Description = dto.Description,
            Date = dto.Date,
            CategoryId = dto.CategoryId,
            UserId = userId
        };

        _context.Expenses.Add(expense);
        _context.SaveChanges();

        
            return Ok(new
            {
                expense.Id,
                expense.Amount,
                expense.Description,
                expense.Date,
                expense.CategoryId
            });
    }

    // Get Expenses
    [HttpGet]
    public IActionResult GetExpenses(
        int? categoryId,
        decimal? minAmount,
        decimal? maxAmount,
        DateTime? startDate,
        DateTime? endDate,
        int page = 1,
        int pageSize = 5,
        bool sortDesc = true)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
       
        var userId = GetUserId();

        var query = _context.Expenses
            .Where(e => e.UserId == userId)
            .AsQueryable();

        // Filters
        if (categoryId.HasValue)
            query = query.Where(e => e.CategoryId == categoryId.Value);

        if (minAmount.HasValue)
            query = query.Where(e => e.Amount >= minAmount.Value);

        if (maxAmount.HasValue)
            query = query.Where(e => e.Amount <= maxAmount.Value);

        if (startDate.HasValue)
            query = query.Where(e => e.Date >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(e => e.Date <= endDate.Value);

        // Sorting
        query = sortDesc
            ? query.OrderByDescending(e => e.Date)
            : query.OrderBy(e => e.Date);

        // Total count BEFORE pagination
        var totalCount = query.Count();

        // Pagination
        var expenses = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(e => new
            {
                e.Id,
                e.Amount,
                e.Description,
                e.Date,
                Category = e.Category.Name
            })
            .ToList();

        return Ok(new
        {
            totalCount,
            page,
            pageSize,
            data = expenses
        });
    }
    
    [HttpGet("summary")]
    public IActionResult GetMonthlySummary(int month, int year)
    {
        var userId = GetUserId();

        var query = _context.Expenses
            .Where(e => e.UserId == userId &&
                        e.Date.Month == month &&
                        e.Date.Year == year);

        // Total spent
        var totalSpent = query.Sum(e => e.Amount);

        // Category-wise grouping
        var categoryWise = query
            .GroupBy(e => e.Category.Name)
            .Select(g => new
            {
                category = g.Key,
                total = g.Sum(e => e.Amount)
            })
            .ToList();

        return Ok(new
        {
            totalSpent,
            categoryWise
        });
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}