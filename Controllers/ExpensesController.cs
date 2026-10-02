using DimDim.Web.Data;
using DimDim.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DimDim.Web.Controllers;

public class ExpensesController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.Expenses.Include(x => x.DimGroup)
        .OrderByDescending(x => x.ExpenseDate).ToListAsync());

    public async Task<IActionResult> Create()
    {
        await LoadGroups();
        return View(new Expense());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Expense expense)
    {
        if (!ModelState.IsValid) { await LoadGroups(expense.DimGroupId); return View(expense); }
        db.Expenses.Add(expense);
        await db.SaveChangesAsync();
        TempData["Success"] = "Despesa criada com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();
        var expense = await db.Expenses.FindAsync(id);
        if (expense is null) return NotFound();
        await LoadGroups(expense.DimGroupId);
        return View(expense);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Expense expense)
    {
        if (id != expense.Id) return NotFound();
        if (!ModelState.IsValid) { await LoadGroups(expense.DimGroupId); return View(expense); }
        db.Update(expense);
        await db.SaveChangesAsync();
        TempData["Success"] = "Despesa atualizada.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();
        var expense = await db.Expenses.Include(x => x.DimGroup).FirstOrDefaultAsync(x => x.Id == id);
        return expense is null ? NotFound() : View(expense);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();
        var expense = await db.Expenses.Include(x => x.DimGroup).FirstOrDefaultAsync(x => x.Id == id);
        return expense is null ? NotFound() : View(expense);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var expense = await db.Expenses.FindAsync(id);
        if (expense is not null) db.Expenses.Remove(expense);
        await db.SaveChangesAsync();
        TempData["Success"] = "Despesa removida.";
        return RedirectToAction(nameof(Index));
    }

    private async Task LoadGroups(int? selectedId = null)
    {
        ViewBag.DimGroupId = new SelectList(await db.DimGroups.OrderBy(x => x.Name).ToListAsync(), "Id", "Name", selectedId);
    }
}
