namespace WebApplication1.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!; //나중에 세팅할게 컴파일러야
        public List<Item>? Items { get; set; }
    }
}
