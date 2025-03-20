using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ItemsController : Controller //base controller를 상속받음 필수로
    {
        private readonly MyAppContext _context;

        public ItemsController(MyAppContext context)
        {
            _context = context;

        }

        //메인페이지에 아이템 다 나오게 ㅇㅇ select
        public async Task<IActionResult> Index()
        {
            var item = await _context.Items.Include(s => s.SerialNumber)
                                           .Include(c => c.Category)
                                           .ToListAsync();

            return View(item);
        }

        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id, Name, Price, CategoryId")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Items.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(item); //redirect index page
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");

            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
            return View(item);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name, Price, CategoryId")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(item);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(x => x.Id == id);
            return View(item);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        //이전 학습 코드
        //public IActionResult Overview()
        //{
        //    var item = new Item() { Name = "mouse" };

        //    return View(item);
        //}

        ////http://localhost:5076/items/edit/1
        ////http://localhost:5076/items/edit?id=4
        //public IActionResult Edit(int id) //pattern을 id로 지정했기 때문에 다른 이름으로 하면 안됨. edit/1이 안먹힘
        //{
        //    return Content("id = " + id);
        //}
    }
}
