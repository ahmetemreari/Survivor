using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Survivor.Data;
using Survivor.Models;
using System;
using System.Linq;

namespace Survivor.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : ControllerBase
    {
        private readonly SurvivorDbContext _context;

        public CategoryController(SurvivorDbContext context)
        {
            _context = context;
        }

        // GET: api/categories - Tüm kategorileri listele
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var categories = _context.Categories
                .Where(c => !c.IsDeleted)
                .Select(c => new CategoryDTO
                {
                    KategoriID = c.Id.ToString(),
                    KategoriAdi = c.KategoriAd
                })
                .ToList();

            if (categories == null || !categories.Any())
                return NotFound("Hiç kategori bulunamadı.");

            return Ok(categories);
        }

        // GET: api/categories/{id} - Belirli bir kategoriyi getir
        [HttpGet("{id}")]
        public IActionResult GetCategoryById(int id)
        {
            var category = _context.Categories
                .Where(c => c.Id == id && !c.IsDeleted)
                .Select(c => new CategoryDTO
                {
                    KategoriID = c.Id.ToString(),
                    KategoriAdi = c.KategoriAd
                })
                .FirstOrDefault();

            if (category == null)
                return NotFound($"ID {id} olan kategori bulunamadı.");

            return Ok(category);
        }

        // POST: api/categories - Yeni bir kategori oluştur
        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryDTO categoryDto)
        {
            if (categoryDto == null || string.IsNullOrWhiteSpace(categoryDto.KategoriAdi))
                return BadRequest("Geçerli bir kategori adı sağlanamadı.");

            var newCategory = new Category
            {
                KategoriAd = categoryDto.KategoriAdi,
                CreatedAt = DateTime.Now,
                ModifiedDate = DateTime.Now,
                IsDeleted = false
            };

            _context.Categories.Add(newCategory);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCategoryById), new { id = newCategory.Id }, new CategoryDTO
            {
                KategoriID = newCategory.Id.ToString(),
                KategoriAdi = newCategory.KategoriAd
            });
        }

        // PUT: api/categories/{id} - Belirli bir kategoriyi güncelle
        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryDTO categoryDto)
        {
            if (categoryDto == null || string.IsNullOrWhiteSpace(categoryDto.KategoriAdi))
                return BadRequest("Kategori adı boş olamaz.");

            var category = _context.Categories.FirstOrDefault(c => c.Id == id && !c.IsDeleted);
            if (category == null)
                return NotFound($"ID {id} olan kategori bulunamadı.");

            category.KategoriAd = categoryDto.KategoriAdi;
            category.ModifiedDate = DateTime.Now;

            _context.SaveChanges();

            return Ok(new CategoryDTO
            {
                KategoriID = category.Id.ToString(),
                KategoriAdi = category.KategoriAd
            });
        }

        // DELETE: api/categories/{id} - Belirli bir kategoriyi sil
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.FirstOrDefault(c => c.Id == id && !c.IsDeleted);
            if (category == null)
                return NotFound($"ID {id} olan kategori bulunamadı.");

            category.IsDeleted = true;
            _context.SaveChanges();

            return Ok($"ID {id} olan kategori başarıyla silindi.");
        }
    }
}
