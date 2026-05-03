using ExpenseTrackerApi.Data;
using ExpenseTrackerApi.DTOs;
using ExpenseTrackerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriesController(AppDbContext context)
    {
        _context = context;
    }

    // Create Category
    [HttpPost]
    public IActionResult CreateCategory(CreateCategoryDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState); 

        var userId = GetUserId();

        var category = new Category
        {
            Name = dto.Name,
            UserId = userId
        };

        _context.Categories.Add(category);
        _context.SaveChanges();

        return Ok(category);
    }

    // Get Categories
    [HttpGet]
    public IActionResult GetCategories()
    {
        var userId = GetUserId();

        var categories = _context.Categories
            .Where(c => c.UserId == userId)
            .ToList();

        return Ok(new
        {
            success = true,
            data = categories
        });
    }

    private int GetUserId()
    {
        return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}