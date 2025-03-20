using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!; 
        public double Price {  get; set; }
        public int? SerialNumberId { get; set; }
        //[ForeignKey("SerialNumberId")]
        public SerialNumber? SerialNumber { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
    }
}
