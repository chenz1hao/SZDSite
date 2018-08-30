using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SZDWebSite.ViewModels;
using Microsoft.AspNetCore.Hosting;
using SZD.WebSite.Model;

namespace SZDWebSite.Controllers
{
    public class HomeController : Controller
    {
        DBContext db;
       
        public HomeController(DBContext _db, IHostingEnvironment env)
        {
            db = _db;
        }
        public IActionResult Index()
        {
            List<ViewModels.GsywViews> gsyw = db.News.Where(m => m.Type == 1).OrderByDescending(m => m.Date).ThenByDescending(m => m.ID).Take(4).Select(r => new ViewModels.GsywViews
            {
                Nid = r.ID,
                Ntitle = r.Title,
                Ndesc = r.Desc,
                Ndate = r.Date.ToString()
            }).ToList();
            HomeNewsModel homeNewsModel = new HomeNewsModel
            {
                IndexGsywViewsList = gsyw
            };
            return View(homeNewsModel);
        }
        public FileContentResult getImage(int id)
        {
            News news = db.News.Single(m => m.ID == id);

            return File(news.Image, "Image/png");
        }
    }
}