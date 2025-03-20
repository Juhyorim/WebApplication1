using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models.limeboard
{
    public class Member
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime JoinAt { get; set; } = DateTime.Now;
        public List<Board>? Boards { get; set; }
    }
}
