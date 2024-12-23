//kategori 
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Survivor.Models
{
    public class Category : BaseEntity
    {
        [JsonPropertyName("Kategori Adı")]
        public string KategoriAd { get; set; }
        public ICollection<Competitor> Yarismacilar { get; set; }
    }
}
