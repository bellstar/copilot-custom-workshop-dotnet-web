using MeowWorld.Data;
using MeowWorld.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MeowWorld.Controllers;

public class CatsController(AppDbContext context, ILogger<CatsController> logger) : Controller
{
    public async Task<IActionResult> Index()
    {
        try
        {
            var cats = await context.Cats
                .OrderByDescending(cat => cat.CreatedAt)
                .ToListAsync();

            return View(cats);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "猫一覧の取得中にエラーが発生しました。");
            return Problem("猫一覧の取得に失敗しました。");
        }
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        try
        {
            var cat = await context.Cats
                .FirstOrDefaultAsync(catEntity => catEntity.Id == id.Value);

            if (cat is null)
            {
                return NotFound();
            }

            return View(cat);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "猫詳細(Id: {CatId})の取得中にエラーが発生しました。", id.Value);
            return Problem("猫詳細の取得に失敗しました。");
        }
    }

    public IActionResult Create()
    {
        try
        {
            return View();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "猫作成画面の表示中にエラーが発生しました。");
            return Problem("猫作成画面の表示に失敗しました。");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Age,Breed,Description")] Cat cat)
    {
        if (!ModelState.IsValid)
        {
            return View(cat);
        }

        try
        {
            cat.CreatedAt = DateTime.Now;

            context.Cats.Add(cat);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "猫の作成中にエラーが発生しました。Name: {CatName}", cat.Name);
            return Problem("猫の作成に失敗しました。");
        }
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        try
        {
            var cat = await context.Cats.FindAsync(id.Value);
            if (cat is null)
            {
                return NotFound();
            }

            return View(cat);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "猫編集画面(Id: {CatId})の表示中にエラーが発生しました。", id.Value);
            return Problem("猫編集画面の表示に失敗しました。");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age,Breed,Description")] Cat cat)
    {
        if (id != cat.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(cat);
        }

        try
        {
            var existingCat = await context.Cats.FindAsync(id);
            if (existingCat is null)
            {
                return NotFound();
            }

            existingCat.Name = cat.Name;
            existingCat.Age = cat.Age;
            existingCat.Breed = cat.Breed;
            existingCat.Description = cat.Description;

            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!CatExists(cat.Id))
            {
                return NotFound();
            }

            logger.LogError(ex, "猫編集(Id: {CatId})の同時実行エラーが発生しました。", cat.Id);
            return Problem("猫編集の保存に失敗しました。");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "猫編集(Id: {CatId})の保存中にエラーが発生しました。", cat.Id);
            return Problem("猫編集の保存に失敗しました。");
        }
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null)
        {
            return NotFound();
        }

        try
        {
            var cat = await context.Cats
                .FirstOrDefaultAsync(catEntity => catEntity.Id == id.Value);

            if (cat is null)
            {
                return NotFound();
            }

            return View(cat);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "猫削除画面(Id: {CatId})の表示中にエラーが発生しました。", id.Value);
            return Problem("猫削除画面の表示に失敗しました。");
        }
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var cat = await context.Cats.FindAsync(id);
            if (cat is null)
            {
                return NotFound();
            }

            context.Cats.Remove(cat);
            await context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "猫削除(Id: {CatId})中にエラーが発生しました。", id);
            return Problem("猫削除に失敗しました。");
        }
    }

    private bool CatExists(int id)
    {
        return context.Cats.Any(cat => cat.Id == id);
    }
}