using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class SerialNumber
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public int? ItemId { get; set; }
        [ForeignKey("ItemId")] //이게 어노테이션인듯
        public Item? Item { get; set; }
    }
}
