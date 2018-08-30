using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SZDWebSite.ViewModels;
using SZD.WebSite.Model;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SZDWebSite.Controllers
{
    public class Other_pages : Controller
    {
        DBContext db;

        public Other_pages(DBContext _db, IHostingEnvironment env)
        {
            db = _db;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Product_index()
        {
            // 这个页面的Model需要包含ProductType的List<string>集合

            // 获取类型个数
            int TypeCount = db.ProductTypes.Count();
            // 获取到所有的Product类型
            List<ProductType> productType = db.ProductTypes.ToList();
            // 获取到所有的Product类型名称
            List<string> productTypeName = new List<string>();
            foreach (var item in productType)
            {
                productTypeName.Add(item.Type);
            }
            // 分别获取到每个类型的Solution的List集合
            List<Product> productList = new List<Product>();
            for (int i = 0; i < TypeCount; i++)
            {
                var thisName = productTypeName[i];
                // 获取一个Type的Solution加入到solutionList
                productList.AddRange(db.Products.Where(m => m.ProductTypes.Type.Equals(thisName)).OrderBy(m => m.Order));
            }
            ProductViewModel productViewModel = new ProductViewModel
            {
                ProductType = productTypeName,
                Product = productList
            };




            return View(productViewModel);
        }
        public IActionResult Solution_index()
        {
            // 这个页面的Model需要包含ProductType的List<string>集合

            // 获取类型个数
            int TypeCount = db.SolutionTypes.Count();
            // 获取到所有的Product类型
            List<SolutionType> solutionType = db.SolutionTypes.ToList();
            // 获取到所有的Product类型名称
            List<string> solutionTypeName = new List<string>();
            foreach (var item in solutionType)
            {
                solutionTypeName.Add(item.Type);
            }
            // 分别获取到每个类型的Solution的List集合
            List<Solution> solutionList = new List<Solution>();
            for (int i = 0; i < TypeCount; i++)
            {
                var thisName = solutionTypeName[i];
                // 获取一个Type的Solution加入到solutionList
                solutionList.AddRange(db.Solutions.Where(m => m.SolutionTypes.Type.Equals(thisName)).OrderBy(m => m.Order));
            }
            SolutionViewModel solutionViewModel = new SolutionViewModel
            {
                SolutionType = solutionTypeName,
                Solution = solutionList
            };
            
            return View(solutionViewModel);
        }
        public IActionResult News_index()
        {
            List<GsywViews> gsyw = db.News.Where(m => m.Type == 1).OrderByDescending(m => m.Date).ThenByDescending(m => m.ID).Select(r => new GsywViews
            {
                Nid = r.ID,
                Ntitle = r.Title,
                Ndate = r.Date
            }).ToList();
            List<HyzxViews> hyzx = db.News.Where(m => m.Type == 2).OrderByDescending(m => m.Date).ThenByDescending(m => m.ID).Select(r => new HyzxViews
            {
                Nid = r.ID,
                Ntitle = r.Title,
                Ndate = r.Date
            }).ToList();
            NewsViewModel newsViewModel = new NewsViewModel
            {
                Gsyw = gsyw,
                Hyzx = hyzx
            };
            ViewBag.gsywCount = gsyw.Count();
            ViewBag.hyzxCount = hyzx.Count();
            return View(newsViewModel);
        }
        public IActionResult About_index()
        {
            Company com = db.Companies.Single(m => m.ID == 1);
            return View(com);
        }

        public PartialNewsGsyw GetGsyw(int page)
        {
            List<GsywViews> gsyw = db.News.Where(m => m.Type == 1).OrderByDescending(m => m.Date).ThenByDescending(m => m.ID).Skip((page - 1) * 5).Take(5).Select(r => new GsywViews
            {
                Nid = r.ID,
                Ntitle = r.Title,
                Ndate = r.Date,
                Ndesc = r.Desc
            }).ToList();
            PartialNewsGsyw p = new PartialNewsGsyw
            {
                News = gsyw,
                Page = page
            };
            return p;
        }

        public PartialNewsHyzx GetHyzx(int page)
        {
            List<HyzxViews> hyzx = db.News.Where(m => m.Type == 2).OrderByDescending(m => m.Date).ThenByDescending(m => m.ID).Skip((page - 1) * 5).Take(5).Select(r => new HyzxViews
            {
                Nid = r.ID,
                Ntitle = r.Title,
                Ndate = r.Date,
                Ndesc = r.Desc
            }).ToList();
            PartialNewsHyzx p = new PartialNewsHyzx
            {
                News = hyzx,
                Page = page
            };
            return p;
        }
    }

    public class PartialNewsGsyw
    {
        public List<GsywViews> News;
        public int Page;
    }

    public class PartialNewsHyzx
    {
        public List<HyzxViews> News;
        public int Page;
    }

    public class GsywViews
    {
        public int Nid { get; set; }
        public string Ntitle { get; set; }
        public DateTime Ndate { get; set; }
        public string Ndesc { get; set; }
    }

    public class HyzxViews
    {
        public int Nid { get; set; }
        public string Ntitle { get; set; }
        public DateTime Ndate { get; set; }
        public string Ndesc { get; set; }
    }
}
