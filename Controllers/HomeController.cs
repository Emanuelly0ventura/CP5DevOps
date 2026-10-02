using DimDim.Web.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DimDim.Web.Controllers;

public class HomeController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewBag.GroupCount = await db.DimGroups.CountAsync();
        ViewBag.ExpenseCount = await db.Expenses.CountAsync();
        ViewBag.Total = await db.Expenses.SumAsync(x => (decimal?)x.Amount) ?? 0m;
        ViewBag.RecentExpenses = await db.Expenses.Include(x => x.DimGroup)
            .OrderByDescending(x => x.ExpenseDate).Take(5).ToListAsync();
        return View();
    }
}
