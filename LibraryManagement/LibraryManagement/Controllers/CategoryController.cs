using LibraryManagement.Interfaces;
using LibraryManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers;

[Authorize(Roles = "SuperUser")]
public class CategoryController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var categories = await _categoryService.GetAllCategoriesAsync();
        return View(categories);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        await _categoryService.CreateCategoryAsync(category);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public async Task<IActionResult> Update(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound();
        return View(category);
    }

    [HttpPost]
    public async Task<IActionResult> Update(int id, Category category)
    {
        if (id != category.CategoryId)
            return NotFound();
        if (ModelState.IsValid)
        {
            await _categoryService.UpdateCategoryAsync(category);
            return RedirectToAction("Index");
        }
        return View(category);
    }
    /*[HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound();
        return View(category);
    }*/
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _categoryService.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound();
        return View(category);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _categoryService.DeleteCategoryAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
