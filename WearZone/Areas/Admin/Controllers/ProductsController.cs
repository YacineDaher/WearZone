using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WearZone.Data;
using WearZone.Models;

namespace WearZone.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index(string searchTerm, string category, string gender, bool? isActive)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(p => p.Name.Contains(searchTerm) || p.SKU.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category.Name == category);
            }

            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(p => p.Gender == gender);
            }

            if (isActive.HasValue)
            {
                query = query.Where(p => p.IsActive == isActive.Value);
            }

            var products = await query.OrderByDescending(p => p.CreatedAt).ToListAsync();

            ViewBag.Categories = await _context.Categories.ToListAsync();

            return View(products);
        }

        // GET: Admin/Products/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name");
            return View(new Product());
        }

        // POST: Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Name,Description,Brand,CategoryId,Price,OriginalPrice,StockQuantity," +
                  "Gender,SubCategory,Color,Size,AgeGroup," +
                  "ImageUrl,SecondaryImageUrl,BackImageUrl,SideImageUrl," +
                  "IsActive,IsNewArrival,IsTrending")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.CreatedAt = DateTime.Now;
                product.SKU = GenerateSKU();

                _context.Add(product);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Product created successfully!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var updated = await TryUpdateModelAsync(product, "",
                p => p.Name, p => p.Description, p => p.Brand, p => p.CategoryId,
                p => p.Price, p => p.OriginalPrice, p => p.StockQuantity,
                p => p.Gender, p => p.SubCategory, p => p.Color, p => p.Size, p => p.AgeGroup,
                p => p.ImageUrl, p => p.SecondaryImageUrl, p => p.BackImageUrl, p => p.SideImageUrl,
                p => p.IsActive, p => p.IsNewArrival, p => p.IsTrending);

            if (updated)
            {
                try
                {
                    product.UpdatedAt = DateTime.Now;
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Product updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(id))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }

            ViewBag.Categories = new SelectList(await _context.Categories.ToListAsync(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Admin/Products/Delete/5.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction(nameof(Index));
            }

            var hasOrderHistory = await _context.OrderItems.AnyAsync(oi => oi.ProductId == id);

            if (hasOrderHistory)
            {
                product.IsActive = false;
                product.UpdatedAt = DateTime.Now;
                TempData["Success"] = "This product appears in past orders, so it was deactivated instead of deleted.";
            }
            else
            {
                _context.Products.Remove(product);
                TempData["Success"] = "Product deleted successfully!";
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Products/ToggleActive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.IsActive = !product.IsActive;
                product.UpdatedAt = DateTime.Now;
                await _context.SaveChangesAsync();

                return Json(new { success = true, isActive = product.IsActive });
            }

            return Json(new { success = false });
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        private string GenerateSKU()
        {
            return "WZ-" + DateTime.Now.ToString("yyyyMMdd") + "-" + Random.Shared.Next(1000, 9999);
        }
    }
}