using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SZD.WebSite.Model;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SZDWebSite.Controllers
{
    public class ProductController : Controller
    {
        DBContext db;
        public ProductController(DBContext _db, IHostingEnvironment env)
        {
            db = _db;
        }
        // GET: /<controller>/
        public IActionResult index()
        {
            return View();
        }
        public IActionResult Detail(int id)
        {
            Product p = db.Products.SingleOrDefault(m => m.ID == id);


            return View(p);
        }
    }
}
