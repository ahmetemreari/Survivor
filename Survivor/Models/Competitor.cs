//competitor ad , soyad , category id , category
namespace Survivor.Models;
public class Competitor : BaseEntity
{
    public string YarismaciAdi { get; set; }
    public string YarismaciSoyadi { get; set; }
    public int KategoriID { get; set; }
    public Category KategoriAd { get; set; }
}
