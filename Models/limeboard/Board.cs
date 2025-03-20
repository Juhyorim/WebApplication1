using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.limeboard
{
    public class Board
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        //@ManyToOne
        public int? MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Member? Member { get; set; }
    }
}
