using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models.limeboard;
using Microsoft.Extensions.Logging;


namespace WebApplication1.Controllers.Limeboard
{
    public class BoardController : Controller
    {
        private readonly ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());

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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Title, Content")] Board board)
        {
            ILogger logger = factory.CreateLogger("Program");

            board.MemberId = 1;


            if (ModelState.IsValid)
            {
                logger.LogInformation("@@@@@@isValid");

                _context.Boards.Add(board);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            } else
            {
                logger.LogInformation("@@@is Not valid");
            }

                return View(board);
        }
    }
}
