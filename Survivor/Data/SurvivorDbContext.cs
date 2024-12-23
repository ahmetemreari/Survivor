using Microsoft.EntityFrameworkCore;
using Survivor.Models;

namespace Survivor.Data
{
    public class SurvivorDbContext : DbContext
    {
        public DbSet<Category> Categories { get; set; } // Kategoriler
        public DbSet<Competitor> Competitors { get; set; } // Yarışmacılar

        public SurvivorDbContext(DbContextOptions<SurvivorDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category ve Competitor ilişkisi
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Yarismacilar) // Category => Çok Yarışmacı
                .WithOne(c => c.KategoriAd)  // Competitor => Tek Kategori
                .HasForeignKey(c => c.KategoriID); // Foreign Key: KategoriID

            // Varsayılan değerler ve diğer ayarlar
            modelBuilder.Entity<Category>()
                .Property(c => c.KategoriAd)
                .IsRequired() // Kategori adı zorunlu
                .HasMaxLength(100); // Maksimum uzunluk

            modelBuilder.Entity<Competitor>()
                .Property(c => c.YarismaciAdi)
                .IsRequired() // Yarışmacı adı zorunlu
                .HasMaxLength(50); // Maksimum uzunluk

            modelBuilder.Entity<Competitor>()
                .Property(c => c.YarismaciSoyadi)
                .IsRequired() // Yarışmacı soyadı zorunlu
                .HasMaxLength(50); // Maksimum uzunluk

            // BaseEntity alanlarının varsayılan değerleri
            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                var createdAtProperty = entity.FindProperty("CreatedAt");
                if (createdAtProperty != null)
                {
                    createdAtProperty.SetDefaultValueSql("GETDATE()");
                }

                var modifiedDateProperty = entity.FindProperty("ModifiedDate");
                if (modifiedDateProperty != null)
                {
                    modifiedDateProperty.SetDefaultValueSql("GETDATE()");
                }
            }
        }
    }
}
