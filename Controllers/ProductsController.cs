using Microsoft.AspNetCore.Mvc;
using day6_17.data;
using day6_17.Models;

namespace day6_17.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // READ: Hiển thị danh sách sản phẩm
        public IActionResult Index(string searchString, int page = 1)
        {
            int pageSize = 5;
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(p => p.Name.Contains(searchString));
            }

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            if (totalPages < 1) totalPages = 1;
            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var products = query
                .OrderByDescending(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentPage"] = page;
            ViewData["TotalPages"] = totalPages;

            return View(products);
        }

        // CREATE: Hiển thị form thêm mới
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // UPDATE: Hiển thị form sửa
        public IActionResult Edit(int id)
        {
            var p = _context.Products.Find(id);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpPost]
        public IActionResult Edit(Product updated)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Update(updated);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(updated);
        }

        // DELETE: Xóa sản phẩm
        public IActionResult Delete(int id)
        {
            var p = _context.Products.Find(id);
            if (p != null)
            {
                _context.Products.Remove(p);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}