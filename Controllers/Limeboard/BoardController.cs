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

        public async Task<IActionResult> Index()
        {
            var Boards = await _context.Boards.Include(b => b.Member).ToListAsync();

            return View(Boards);
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
