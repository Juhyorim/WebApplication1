using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.limeboard;
using Microsoft.Extensions.Logging;
using System.Diagnostics;


namespace WebApplication1.Controllers.Limeboard
{
    public class BoardController : Controller
    {
        //private readonly ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());

        private readonly MyAppContext _context;

        public BoardController(MyAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(long? cursor = null, bool previous = false, int pageSize = 10)
        {
            pageSize = 10;

            // 전체 게시글 수 조회
            var totalItems = await _context.Boards.CountAsync();

            // 기본 쿼리 - ID 기준 내림차순 정렬 (최신 게시글 먼저)
            var query = _context.Boards
                .Include(b => b.Member)
                .OrderByDescending(b => b.Id);

            // 현재 페이지의 아이템
            IQueryable<Board> currentPageQuery;

            if (cursor == null)
            {
                // 첫 페이지인 경우
                currentPageQuery = query.Take(pageSize + 1); // 다음 페이지 존재 여부 확인을 위해 +1
            }
            else if (previous)
            {
                // 이전 페이지로 이동하는 경우
                // 커서보다 ID가 큰 항목들을 pageSize+1개 가져와서 역순으로 정렬
                currentPageQuery = query
                    .Where(b => b.Id > cursor)
                    .Take(pageSize + 1)
                    .OrderBy(b => b.Id); // 임시로 오름차순 정렬
            }
            else
            {
                // 다음 페이지로 이동하는 경우
                currentPageQuery = query
                    .Where(b => b.Id < cursor)
                    .Take(pageSize + 1);
            }

            // 결과 가져오기
            var currentPageItems = await currentPageQuery.ToListAsync();

            // 이전 페이지에서 온 경우 다시 내림차순으로 정렬
            if (previous && cursor != null)
            {
                currentPageItems = currentPageItems.OrderByDescending(b => b.Id).ToList();
            }

            // 다음/이전 페이지 존재 여부 확인 및 추가 항목 제거
            bool hasNextPage = currentPageItems.Count > pageSize;
            bool hasPreviousPage = cursor != null;

            if (hasNextPage)
            {
                currentPageItems = currentPageItems.Take(pageSize).ToList();
            }

            // 다음/이전 커서 설정
            long? nextCursor = hasNextPage ? currentPageItems.Last().Id : null;
            long? previousCursor = currentPageItems.Any() ? currentPageItems.First().Id : null;

            // 페이지네이션 모델 생성
            var paginationModel = new PaginationModel<Board>
            {
                PageSize = pageSize,
                NextCursor = hasNextPage ? nextCursor : null,
                PreviousCursor = hasPreviousPage ? previousCursor : null,
                HasNextPage = hasNextPage,
                HasPreviousPage = hasPreviousPage,
                TotalItems = totalItems,
                Items = currentPageItems
            };

            // ViewBag에 정보 추가
            ViewBag.HasNextPage = hasNextPage;
            ViewBag.HasPreviousPage = hasPreviousPage;
            ViewBag.NextCursor = nextCursor;
            ViewBag.PreviousCursor = previousCursor;

            //var Boards = await _context.Boards.Include(b => b.Member).ToListAsync();

            return View(paginationModel);
        }

        public async Task<IActionResult> Info(int id)
        {
            var Board = await _context.Boards.Include(b => b.Member).FirstOrDefaultAsync(b => b.Id == id);
            return View(Board);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Title, Content")] Board board)
        {
            //ILogger logger = factory.CreateLogger("Program");

            board.MemberId = 1; //no login dummy


            if (ModelState.IsValid)
            {
                Debug.WriteLine("@@@@@@isValid");

                _context.Boards.Add(board);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            } else
            {
                Debug.WriteLine("@@@is Not valid");
            }

             return View(board);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var board = await _context.Boards.FirstOrDefaultAsync(b => b.Id == id);

            return View(board);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Title, Content")] Board board)
        {
            var originBoard = await _context.Boards.FirstOrDefaultAsync(b => b.Id == id);
            if (originBoard == null)
            {
                return NotFound();
            }

            originBoard.Title = board.Title;
            originBoard.Content = board.Content;

            if (ModelState.IsValid)
            {
                _context.Update(originBoard); // board 대신 originBoard 사용
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(board);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var board = await _context.Boards.FirstOrDefaultAsync(b => b.Id == id);

            return View(board);
        }

        [HttpPost, ActionName("Delete")] //오버로딩으로 불가능하니까 ActionName으로 구분
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var originBoard = await _context.Boards.FirstOrDefaultAsync(b => b.Id == id);
            if (originBoard != null)
            {
                _context.Boards.Remove(originBoard);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
