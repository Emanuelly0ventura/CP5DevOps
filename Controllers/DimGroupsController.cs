using DimDim.Web.Data;
using DimDim.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DimDim.Web.Controllers;

public class DimGroupsController(AppDbContext db) : Controller
{
    public async Task<IActionResult> Index() => View(await db.DimGroups.OrderBy(x => x.Name).ToListAsync());

    public IActionResult Create() => View(new DimGroup());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DimGroup group)
    {
        if (!ModelState.IsValid) return View(group);
        db.DimGroups.Add(group);
        await db.SaveChangesAsync();
        TempData["Success"] = "Grupo criado com sucesso.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();
        var group = await db.DimGroups.FindAsync(id);
        return group is null ? NotFound() : View(group);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, DimGroup group)
    {
        if (id != group.Id) return NotFound();
        if (!ModelState.IsValid) return View(group);
        var existing = await db.DimGroups.FindAsync(id);
        if (existing is null) return NotFound();
        existing.Name = group.Name;
        existing.Description = group.Description;
        await db.SaveChangesAsync();
        TempData["Success"] = "Grupo atualizado.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();
        var group = await db.DimGroups.Include(x => x.Expenses).FirstOrDefaultAsync(x => x.Id == id);
        return group is null ? NotFound() : View(group);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();
        var group = await db.DimGroups.Include(x => x.Expenses).FirstOrDefaultAsync(x => x.Id == id);
        return group is null ? NotFound() : View(group);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var group = await db.DimGroups.FindAsync(id);
        if (group is not null) db.DimGroups.Remove(group);
        await db.SaveChangesAsync();
        TempData["Success"] = "Grupo e despesas relacionadas removidos.";
        return RedirectToAction(nameof(Index));
    }
}
