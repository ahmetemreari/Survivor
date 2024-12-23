//ıd , datetime created ve modifiye , silme
namespace Survivor.Models;
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ModifiedDate { get; set; }
    public bool IsDeleted { get; set; }

}