using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Survivor.Data;
using Survivor.Models;
using System;
using System.Linq;

namespace Survivor.Controllers
{
    [ApiController]
    [Route("api/competitors")]
    public class CompetitorController : ControllerBase
    {
        private readonly SurvivorDbContext _context;

        public CompetitorController(SurvivorDbContext context)
        {
            _context = context;
        }

        // GET: api/competitors - Tüm yarışmacıları listele
        [HttpGet]
        public IActionResult GetAllCompetitors()
        {
            try
            {
                var competitors = _context.Competitors
                    .Where(c => !c.IsDeleted)
                    .Include(c => c.KategoriAd)
                    .Select(c => new CompetitorDTO
                    {
                        YarismaciID = c.Id.ToString(),
                        YarismaciAdi = c.YarismaciAdi,
                        YarismaciSoyadi = c.YarismaciSoyadi,
                        KategoriID = c.KategoriAd != null ? c.KategoriAd.Id.ToString() : null
                    })
                    .ToList();

                if (!competitors.Any())
                    return NotFound("Hiç yarışmacı bulunamadı.");

                return Ok(competitors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        // GET: api/competitors/{id} - Belirli bir yarışmacıyı getir
        [HttpGet("{id}")]
        public IActionResult GetCompetitorById(int id)
        {
            try
            {
                var competitor = _context.Competitors
                    .Include(c => c.KategoriAd)
                    .Where(c => c.Id == id && !c.IsDeleted)
                    .Select(c => new CompetitorDTO
                    {
                        YarismaciID = c.Id.ToString(),
                        YarismaciAdi = c.YarismaciAdi,
                        YarismaciSoyadi = c.YarismaciSoyadi,
                        KategoriID = c.KategoriAd != null ? c.KategoriAd.Id.ToString() : null
                    })
                    .FirstOrDefault();

                if (competitor == null)
                    return NotFound($"ID {id} olan yarışmacı bulunamadı.");

                return Ok(competitor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        // POST: api/competitors - Yeni bir yarışmacı oluştur
        [HttpPost]
        public IActionResult CreateCompetitor([FromBody] CompetitorDTO competitorDto)
        {
            try
            {
                if (competitorDto == null || string.IsNullOrWhiteSpace(competitorDto.YarismaciAdi) || string.IsNullOrWhiteSpace(competitorDto.YarismaciSoyadi))
                    return BadRequest("Geçerli bir yarışmacı bilgisi sağlanamadı.");

                var category = _context.Categories.FirstOrDefault(c => c.Id.ToString() == competitorDto.KategoriID && !c.IsDeleted);
                if (category == null)
                    return NotFound($"Kategori ID {competitorDto.KategoriID} bulunamadı.");

                var newCompetitor = new Competitor
                {
                    YarismaciAdi = competitorDto.YarismaciAdi,
                    YarismaciSoyadi = competitorDto.YarismaciSoyadi,
                    KategoriID = category.Id,
                    CreatedAt = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    IsDeleted = false
                };

                _context.Competitors.Add(newCompetitor);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetCompetitorById), new { id = newCompetitor.Id }, new CompetitorDTO
                {
                    YarismaciID = newCompetitor.Id.ToString(),
                    YarismaciAdi = newCompetitor.YarismaciAdi,
                    YarismaciSoyadi = newCompetitor.YarismaciSoyadi,
                    KategoriID = category.Id.ToString()
                });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, $"Veritabanı güncelleme hatası: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        // PUT: api/competitors/{id} - Belirli bir yarışmacıyı güncelle
        [HttpPut("{id}")]
        public IActionResult UpdateCompetitor(int id, [FromBody] CompetitorDTO competitorDto)
        {
            try
            {
                if (competitorDto == null || string.IsNullOrWhiteSpace(competitorDto.YarismaciAdi) || string.IsNullOrWhiteSpace(competitorDto.YarismaciSoyadi))
                    return BadRequest("Geçerli bir yarışmacı bilgisi sağlanamadı.");

                var competitor = _context.Competitors
                    .Include(c => c.KategoriAd)
                    .FirstOrDefault(c => c.Id == id && !c.IsDeleted);

                if (competitor == null)
                    return NotFound($"ID {id} olan yarışmacı bulunamadı.");

                var category = _context.Categories.FirstOrDefault(c => c.Id.ToString() == competitorDto.KategoriID && !c.IsDeleted);
                if (category == null)
                    return NotFound($"Kategori ID {competitorDto.KategoriID} bulunamadı.");

                competitor.YarismaciAdi = competitorDto.YarismaciAdi;
                competitor.YarismaciSoyadi = competitorDto.YarismaciSoyadi;
                competitor.KategoriID = category.Id;
                competitor.ModifiedDate = DateTime.Now;

                _context.SaveChanges();

                return Ok(new CompetitorDTO
                {
                    YarismaciID = competitor.Id.ToString(),
                    YarismaciAdi = competitor.YarismaciAdi,
                    YarismaciSoyadi = competitor.YarismaciSoyadi,
                    KategoriID = competitor.KategoriID.ToString()
                });
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, $"Veritabanı güncelleme hatası: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }

        // DELETE: api/competitors/{id} - Belirli bir yarışmacıyı sil
        [HttpDelete("{id}")]
        public IActionResult DeleteCompetitor(int id)
        {
            try
            {
                var competitor = _context.Competitors.FirstOrDefault(c => c.Id == id && !c.IsDeleted);
                if (competitor == null)
                    return NotFound($"ID {id} olan yarışmacı bulunamadı.");

                competitor.IsDeleted = true;
                _context.SaveChanges();

                return Ok($"ID {id} olan yarışmacı başarıyla silindi.");
            }
            catch (DbUpdateException dbEx)
            {
                return StatusCode(500, $"Veritabanı güncelleme hatası: {dbEx.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Sunucu hatası: {ex.Message}");
            }
        }
    }
}
