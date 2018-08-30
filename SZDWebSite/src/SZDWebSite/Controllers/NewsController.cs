using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SZDWebSite.ViewModels;
using Microsoft.AspNetCore.Hosting;
using SZD.WebSite.Model;
using Microsoft.AspNetCore.Hosting.Internal;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SZDWebSite.Controllers
{
    public class NewsController : Controller
    {
        DBContext db;
        
        public NewsController(DBContext _db, IHostingEnvironment env)
        {
            db = _db;
        }
        // GET: /<controller>/
        public IActionResult GsywDetail(int id)
        {
            News thisNews, nextNews, preNews;
            List<News> allGsyw = db.News.Where(m => m.Type == 1).ToList();
            int k = 0;
            while (id != allGsyw[k].ID)
            {
                k++;
                if (k == allGsyw.Count())
                {
                    return RedirectToAction("News", "Solution");
                }
            }
            int authorID = db.News.SingleOrDefault(m => m.ID == id).UID;
            string author = db.Users.SingleOrDefault(m => m.ID == authorID).Username;
            if (k == 0) //第一条
            {
                preNews = null;
                thisNews = allGsyw[0];
                nextNews = allGsyw[1];
            }
            else if (k == allGsyw.Count() - 1) // 最后一条
            {
                nextNews = null;
                thisNews = allGsyw[k];
                preNews = allGsyw[k - 1];
            }
            else //位于中间 有前有后
            {
                thisNews = allGsyw[k];
                preNews = allGsyw[k - 1];
                nextNews = allGsyw[k + 1];
            }
            NewsDetailModel newsDetailModel = new NewsDetailModel
            {
                ThisNews = thisNews,
                NextNews = nextNews,
                PreNews = preNews,
                Author = author
            };
            return View(newsDetailModel);
        }
        public IActionResult HyzxDetail(int id)
        {
            News thisNews, nextNews, preNews;
            List<News> allHyzx = db.News.Where(m => m.Type == 2).ToList();
            int k = 0;
            while (id != allHyzx[k].ID)
            {
                k++;
                if (k == allHyzx.Count())
                {
                    return RedirectToAction("News","Solution");
                }
            }
            int uid = allHyzx[k].ID;
            int authorID = db.News.SingleOrDefault(m => m.ID == id).UID;
            string author = db.Users.SingleOrDefault(m => m.ID == authorID).Username;
            if (k == 0) //第一条
            {
                preNews = null;
                thisNews = allHyzx[0];
                nextNews = allHyzx[1];
            }
            else if (k == allHyzx.Count() - 1) // 最后一条
            {
                nextNews = null;
                thisNews = allHyzx[k];
                preNews = allHyzx[k - 1];
            }
            else //位于中间 有前有后
            {
                thisNews = allHyzx[k];
                preNews = allHyzx[k - 1];
                nextNews = allHyzx[k + 1];
            }
            NewsDetailModel newsDetailModel = new NewsDetailModel
            {
                ThisNews = thisNews,
                NextNews = nextNews,
                PreNews = preNews,
                Author = author
            };
            return View(newsDetailModel);
        }
    }
}
