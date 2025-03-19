using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ItemsController : Controller //base controller를 상속받음 필수로
    {
        public IActionResult Overview()
        {
            var item = new Item() { Name = "mouse" };

            return View(item);
        }

        //http://localhost:5076/items/edit/1
        //http://localhost:5076/items/edit?id=4
        public IActionResult Edit(int id) //pattern을 id로 지정했기 때문에 다른 이름으로 하면 안됨. edit/1이 안먹힘
        {
            return Content("id = " + id);
        }
    }
}
