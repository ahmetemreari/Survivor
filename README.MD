# Survivor API Projesi

Bu proje, **Survivor yarışmacıları** ve **kategorileri** arasında bir ilişki kuran, CRUD (Create, Read, Update, Delete) işlemleri sağlayan bir API uygulamasıdır. Proje, yarışmacıları ve kategorileri yönetmek için çeşitli endpointler sunmaktadır.

## Özellikler

- Yarışmacı ve kategori arasında **ilişki yönetimi**.
- **CRUD işlemleri** (Ekleme, Okuma, Güncelleme, Silme).
- Veritabanı yönetimi için **Entity Framework Core** kullanımı.
- Navigasyon özellikleriyle ilişkili veriler için `Include` desteği.
- Silinen kayıtlar için **soft delete** mekanizması.
- **Hata yönetimi** ve uygun durum kodları.

---

## Kullanılan Teknolojiler

- **ASP.NET Core 6.0** - Web API altyapısı
- **Entity Framework Core** - Veritabanı erişimi ve yönetimi
- **SQL Server** - Veritabanı yönetimi
- **Dependency Injection** - Bağımlılıkların yönetimi

---

## Nasıl Çalıştırılır?

### Gereksinimler
- .NET SDK 6.0+
- SQL Server
- Visual Studio veya Visual Studio Code

### Kurulum

1. **Kaynak Kodunu İndir**
   ```bash
   git clone https://github.com/ahmetemreari/survivor-api.git
   cd survivor-api
