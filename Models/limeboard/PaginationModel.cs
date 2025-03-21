namespace WebApplication1.Models.limeboard
{
    public class PaginationModel<T>
    {
        public int PageSize { get; set; }
        public long? NextCursor { get; set; }     // 다음 페이지의 시작점
        public long? PreviousCursor { get; set; } // 이전 페이지의 시작점
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public int TotalItems { get; set; }       // 전체 아이템 수
        public IEnumerable<T> Items { get; set; }
    }
}
