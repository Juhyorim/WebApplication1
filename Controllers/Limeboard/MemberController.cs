using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Controllers.Limeboard
{
    public class MemberController : Controller
    {
        private readonly MyAppContext _context;

        public MemberController(MyAppContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var Member = await _context.Members.Include(m => m.Boards)
                .ToListAsync();

            return View(Member);
        }
    }
}
