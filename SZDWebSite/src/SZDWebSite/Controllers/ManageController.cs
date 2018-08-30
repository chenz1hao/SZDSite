using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using SZDWebSite.ViewModels;
using SZD.WebSite.Model;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace SZDWebSite.Controllers
{
    public class ManageController : Controller
    {
        DBContext db;
        public ManageController(DBContext _db, IHostingEnvironment env)
        {
            db = _db;
        }
        // GET: /<controller>/
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult LoginSuccess()
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                ViewBag.tips = "您还没有登录，请先登录";
                return View("Login");
            }
            else
            {
                ViewBag.username = username;
                return View();
            }
        }
        [HttpPost]
        public IActionResult LoginAction(User loginUser)
        {
            User user = db.Users.SingleOrDefault(m => m.Username == loginUser.Username);
            if (user == null)
            {
                ViewBag.tips = "用户不存在";
            }
            else if (user.Password != loginUser.Password)
            {
                ViewBag.tips = "密码错误";
            }
            else
            {
                HttpContext.Session.SetString("username", loginUser.Username);
                return RedirectToAction("LoginSuccess");
            }
            return View("Login");
        }
        [HttpGet]
        public IActionResult LoginAction()
        {
            return View("Login");
        }
        public IActionResult Index(int? page)
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                ViewBag.tips = "您还没有登录，请先登录";
                return View("Login");
            }
            else
            {
                ViewBag.username = username;
                if (page == null)
                {
                    page = 1;
                }
                List<ManageIndexView> news = db.News.OrderByDescending(m => m.Date).ThenByDescending(m => m.ID).Skip((page.Value - 1) * 10).Select(r => new ManageIndexView
                {
                    Nid = r.ID,
                    Ntitle = r.Title,
                    Ndate = r.Date,
                    Ntype = r.Type,
                    Nauthor = db.Users.FirstOrDefault(m => m.ID == r.ID).Username,
                    Ndesc = r.Desc
                }).Take(10).ToList();

                ViewBag.newsCount = news.Count();

                ViewBag.tempPage = page;

                return View(news);
            }
        }

        public IActionResult PublishNews()
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                ViewBag.tips = "您还没有登录，请先登录";
                return View("Login");
            }
            else
            {
                ViewBag.username = username;
                return View();
            }
        }
        public IActionResult CreateNews(string html, string txt, string title, string desc, string type, string uusername, byte[] imageBytes, DateTime date)
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                ViewBag.tips = "您还没有登录，请先登录";
                return View("Login");
            }
            else
            {
                User user = db.Users.SingleOrDefault(u => u.Username == uusername);
                int uid = user.ID;
                db.News.Add(new News()
                {
                    Title = title,
                    Html = html,
                    Desc = desc,
                    UID = uid,
                    Type = type.Equals("gsyw") ? 1 : (type.Equals("hyzx") ? 2 : 3),
                    Date = date,
                    Image = imageBytes
                });
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public IActionResult Exit()
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                ViewBag.tips = "您没有登录，无法退出";
            }
            else
            {
                ViewBag.tips = username + " 已安全退出";
                HttpContext.Session.Remove("username");
            }
            return View("Login");
        }
        [HttpGet]
        public IActionResult EditNews(int id)
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                ViewBag.tips = "您还没有登录，请先登录";
                return View("Login");
            }
            else
            {
                ViewBag.username = username;
                News news = db.News.SingleOrDefault(m => m.ID == id);
                return View(news);
            }
        }
        [HttpPost]
        public void EditNews(int nid, string html, string txt, string title, string desc, string type, string uusername, DateTime date)
        {
            string username = HttpContext.Session.GetString("username");
            if (username != null)
            {
                News news = db.News.SingleOrDefault(m => m.ID == nid);
                news.Date = date;
                news.Html = html;
                news.Title = title;
                news.Desc = desc;
                news.Type = type.Equals("gsyw") ? 1 : (type.Equals("hyzx") ? 2 : 3);
                news.UID = db.Users.SingleOrDefault(u => u.Username == uusername).ID;
                db.SaveChanges();
            }
        }
        [HttpPost]
        public void EditNewsWithCover(int nid, string html, string txt, string title, string desc, string type, string uusername, byte[] imageBytes, DateTime date)
        {
            string username = HttpContext.Session.GetString("username");
            if (username != null)
            {
                News news = db.News.SingleOrDefault(m => m.ID == nid);
                news.Date = date;
                news.Html = html;
                news.Title = title;
                news.Desc = desc;
                news.Type = type.Equals("gsyw") ? 1 : (type.Equals("hyzx") ? 2 : 3);
                news.UID = db.Users.SingleOrDefault(u => u.Username == uusername).ID;
                news.Image = imageBytes;
                db.SaveChanges();
            }
        }

        public IActionResult DeleteNews(int id)
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                ViewBag.tips = "您还没有登录，请先登录";
                return View("Login");
            }
            else
            {
                db.News.Remove(db.News.SingleOrDefault(m => m.ID == id));
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public IActionResult Gsjj()
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                ViewBag.tips = "您还没有登录，请先登录";
                return View("Login");
            }
            else
            {
                ViewBag.username = username;
                string Gsjj = db.Companies.Single(m => m.ID == 1).Introduction;
                ViewBag.Gsjj = Gsjj;
                return View();
            }
        }

        public IActionResult Culture()
        {
            string username = HttpContext.Session.GetString("username");
            if (username != null)
            {
                ViewBag.username = username;
                string Culture = db.Companies.Single(m => m.ID == 1).Culture;
                ViewBag.Culture = Culture;
                return View();
            }
            return null;
        }
        public IActionResult Recruit()
        {
            string username = HttpContext.Session.GetString("username");
            if (username != null)
            {
                ViewBag.username = username;
                string recruit = db.Companies.Single(m => m.ID == 1).Recruit;
                ViewBag.Recruit = recruit;
                return View();
            }
            return null;
        }
        [HttpPost]
        public IActionResult EditGsjj(string gsjj)
        {
            string username = HttpContext.Session.GetString("username");
            if (username != null)
            {
                Company com = db.Companies.Single(m => m.ID == 1);
                com.Introduction = gsjj;
                db.SaveChanges();
                return View("Gsjj");
            }
            return null;
        }
        [HttpPost]
        public IActionResult EditCulture(string culture)
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                ViewBag.tips = "您还没有登录，请先登录";
                return View("Login");
            }
            else
            {
                Company com = db.Companies.Single(m => m.ID == 1);
                com.Culture = culture;
                db.SaveChanges();
                return View("Culture");
            }
        }
        [HttpPost]
        public IActionResult EditRecruit(string recruit)
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                ViewBag.tips = "您还没有登录，请先登录";
                return View("Login");
            }
            else
            {
                Company com = db.Companies.Single(m => m.ID == 1);
                com.Recruit = recruit;
                db.SaveChanges();
                return View("Recruit");
            }
        }

        [HttpPost]
        public byte[] UploadImage(IFormFile image)
        {
            string username = HttpContext.Session.GetString("username");
            if (username == null)
            {
                ViewBag.tips = "您还没有登录，请先登录";
                return null;
            }
            else
            {
                if (image != null)
                {
                    var stream = image.OpenReadStream();
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);
                    return bytes;
                }
                return null;
            }
        }

        public IActionResult Solution()
        {
            string username = HttpContext.Session.GetString("username");
            if (username != null)
            {
                ViewBag.username = username;
                // 获取类型个数
                int TypeCount = db.SolutionTypes.Count();
                // 获取到所有的Solution类型
                List<SolutionType> solutionType = db.SolutionTypes.ToList();
                // 获取到所有的Solution类型名称
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


                return View(solutionList);
            }
            ViewBag.tips = "您还没有登录，请先登录";
            return View("Login");
        }

        public IActionResult SolutionType()
        {
            string username = HttpContext.Session.GetString("username");
            if (username != null)
            {
                ViewBag.username = username;
                List<SolutionType> solutionTypes = db.SolutionTypes.ToList();
                return View(solutionTypes);
            }
            ViewBag.tips = "您还没有登录，请先登录";
            return View("Login");
        }

        [HttpPost]
        public void AddSolutionType(string TypeName, string TypeRemark)
        {
            SolutionType solutionType = new SolutionType
            {
                Type = TypeName,
                Remark = (TypeRemark == null ? "无" : TypeRemark)
            };
            db.SolutionTypes.Add(solutionType);
            db.SaveChanges();
            return;
        }

        [HttpPost]
        public void SaveSolutionType(string nowID, string TypeName, string TypeRemark)
        {
            int ID = Convert.ToInt32(nowID.Substring(2));
            SolutionType solutionType = db.SolutionTypes.SingleOrDefault(m => m.ID == ID);
            solutionType.Type = TypeName;
            solutionType.Remark = TypeRemark == null ? "无" : TypeRemark;
            db.SaveChanges();
            return;
        }

        [HttpPost]
        public void DeleteType(string ID)
        {
            int tempID = Convert.ToInt32(ID.Substring(2));
            db.SolutionTypes.Remove(db.SolutionTypes.SingleOrDefault(m => m.ID == tempID));
            db.SaveChanges();
            return;
        }

        public IActionResult AddSolution()
        {
            string username = HttpContext.Session.GetString("username");
            if (username != null)
            {
                ViewBag.username = username;
                List<SolutionType> solutionList = db.SolutionTypes.ToList();
                List<string> solutionTypeName = new List<string>();
                foreach (var item in solutionList)
                {
                    solutionTypeName.Add(item.Type);
                }
                return View(solutionTypeName);
            }
            ViewBag.tips = "您还没有登录，请先登录";
            return View("Login");
        }

        [HttpPost]
        public void PublishSolution(string solutionName, string solutionType, string solutionHTML, byte[] solutionImage, int solutionSort, string solutionDesc)
        {

            var typeID = db.SolutionTypes.SingleOrDefault(m => m.Type == solutionType).ID;
            Solution solution = new Solution
            {
                Name = solutionName,
                TypeID = typeID,
                Html = solutionHTML,
                Order = solutionSort,
                Desc = solutionDesc
            };
            db.Solutions.Add(solution);
            int thisTypeCount = db.Solutions.Where(m => m.TypeID == typeID).Count();
            if (solutionSort == thisTypeCount + 1) //放在最后一个，不用处理
            {
                db.SaveChanges();
            }
            else  //如果不是放在最后一个，把所有solution中Order字段值大于等于solutionSort的Order值分别加一
            {
                var thisTypeSolutionList = db.Solutions.Where(m => m.TypeID == typeID);
                var thisTypeSolutionListShouldPlusOne = thisTypeSolutionList.Where(m => m.Order >= solutionSort);
                foreach (var item in thisTypeSolutionListShouldPlusOne)
                {
                    item.Order += 1;
                }
                db.SaveChanges();
            }


            //添加图片
            int ID = db.Solutions.SingleOrDefault(m => m.Name == solutionName).ID;
            db.SolutionImages.Add(new SolutionImage
            {
                SolutionID = ID,
                Image = solutionImage
            });
            db.SaveChanges();

            return;
        }

        [HttpPost]
        public int GetSolutionCount(string solutionTypeName)
        {
            int solutionID = db.SolutionTypes.SingleOrDefault(m => m.Type.Equals(solutionTypeName)).ID;
            return db.Solutions.Where(m => m.TypeID == solutionID).Count();
        }

        [HttpGet]
        public IActionResult EditSolution(int ID)
        {
            string username = HttpContext.Session.GetString("username");
            if (username != null)
            {
                Solution solution = db.Solutions.SingleOrDefault(m => m.ID == ID);
                List<string> SolutionType = new List<string>();
                List<SolutionType> solutionType = db.SolutionTypes.ToList();
                foreach (var item in solutionType)
                {
                    SolutionType.Add(item.Type);
                }
                EditSolutionModel editSolutionModel = new EditSolutionModel
                {
                    Solution = solution,
                    SolutionType = SolutionType
                };
                ViewBag.username = username;
                return View(editSolutionModel);
            }
            else
            {
                ViewBag.tips = "您还没有登录，请先登录";
                return View("Login");
            }
        }

        public FileContentResult getImage(int SolutionID)
        {
            SolutionImage solutionImage = db.SolutionImages.SingleOrDefault(m => m.SolutionID == SolutionID);
            return File(solutionImage.Image, "Image/png");
        }

        public FileContentResult getProductImage(int ProductID)
        {
            ProductImage productImage = db.ProductImages.SingleOrDefault(m => m.ProductID == ProductID);
            return File(productImage.Image, "Image/png");
        }


        // 不修改图片且不修改类型时
        [HttpPost]
        public void EditSolution(int id, string solutionName, string solutionType, string solutionHTML, int solutionSort, string solutionDesc)
        {

            Solution s = db.Solutions.SingleOrDefault(m => m.ID == id);

            int oldOrder = s.Order;

            if (oldOrder != solutionSort) // 如果修改了序号
            {
                Solution temp = db.Solutions.Where(m => m.TypeID == s.TypeID).SingleOrDefault(m => m.Order == solutionSort);
                int tempOrder = temp.Order;
                temp.Order = s.Order;
                s.Order = tempOrder;
            }

            s.Desc = solutionDesc;
            s.Name = solutionName;
            s.Html = solutionHTML;
            db.SaveChanges();

            return;
        }

        // 修改了解决方案的类型，没有修改图片
        [HttpPost]
        public void EditSolutionWithType(int id, string solutionName, string solutionType, string solutionHTML, int solutionSort, string solutionDesc)
        {
            Solution s = db.Solutions.SingleOrDefault(m => m.ID == id);
            int oldOrder = s.Order;
            // 将原类别Order值之后的Order值全部减一
            string oldType = s.SolutionTypes.Type;
            List<Solution> solutionOldTypeList = db.Solutions.Where(m => m.SolutionTypes.Type.Equals(oldType)).ToList();
            foreach (var item in solutionOldTypeList)
            {
                if (item.Order > oldOrder)
                {
                    item.Order -= 1;
                }
            }

            // 将新类型Order值大于等于的Order值全部加一
            List<Solution> solutionNewTypeList = db.Solutions.Where(m => m.SolutionTypes.Type.Equals(solutionType)).ToList();
            foreach (var item in solutionNewTypeList)
            {
                if (item.Order >= solutionSort)
                {
                    item.Order += 1;
                }
            }

            // 修改该产品各项信息
            s.Name = solutionName;
            s.TypeID = db.SolutionTypes.SingleOrDefault(m => m.Type.Equals(solutionType)).ID;
            s.Order = solutionSort;
            s.Html = solutionHTML;
            s.Desc = solutionDesc;

            db.SaveChanges();
            return;
        }
        
        // // 等待修改这个方法 2018/8/30 0:09
        [HttpPost]
        public void EditSolutionWithImage(int id, string solutionName, string solutionType, string solutionHTML, byte[] solutionImage, int solutionSort, string solutionDesc)
        {
            Solution s = db.Solutions.SingleOrDefault(m => m.ID == id);
            int oldOrder = s.Order;
            if (oldOrder != solutionSort) //修改了product的order
            {
                Solution temp = db.Solutions.SingleOrDefault(m => m.Order == solutionSort);
                int tempOrder = temp.Order;
                temp.Order = s.Order;
                s.Order = tempOrder;
                db.SaveChanges();
            }

            // 修改解决方案的各项信息
            s.Name = solutionName;
            s.TypeID = db.SolutionTypes.SingleOrDefault(m => m.Type.Equals(solutionType)).ID;
            s.Order = solutionSort;
            s.Html = solutionHTML;
            s.Desc = solutionDesc;

            SolutionImage si = db.SolutionImages.SingleOrDefault(m => m.SolutionID == s.ID);
            si.Image = solutionImage;
            db.SaveChanges();

            return;
        }

        [HttpPost]
        public void EditSolutionWithImageAndType(int id, string solutionName, string solutionType, string solutionHTML, byte[] solutionImage, int solutionSort, string solutionDesc)
        {
            Solution s = db.Solutions.SingleOrDefault(m => m.ID == id);
            int oldOrder = s.Order;
            // 将原类别Order值之后的Order值全部减一
            string oldType = s.SolutionTypes.Type;
            List<Solution> solutionOldTypeList = db.Solutions.Where(m => m.SolutionTypes.Type.Equals(oldType)).ToList();
            foreach (var item in solutionOldTypeList)
            {
                if (item.Order > oldOrder)
                {
                    item.Order -= 1;
                }
            }

            // 将新类型Order值大于等于的Order值全部加一
            List<Solution> solutionNewTypeList = db.Solutions.Where(m => m.SolutionTypes.Type.Equals(solutionType)).ToList();
            foreach (var item in solutionNewTypeList)
            {
                if (item.Order >= solutionSort)
                {
                    item.Order += 1;
                }
            }

            // 各项信息
            s.Name = solutionName;
            s.TypeID = db.SolutionTypes.SingleOrDefault(m => m.Type.Equals(solutionType)).ID;
            s.Order = solutionSort;
            s.Html = solutionHTML;
            s.Desc = solutionDesc;

            // 修改图片
            SolutionImage si = db.SolutionImages.SingleOrDefault(m => m.SolutionID == s.ID);
            si.Image = solutionImage;

            db.SaveChanges();


            return;
        }

        public IActionResult DeleteSolution(int id)
        {
            int thisOrder = db.Solutions.SingleOrDefault(m => m.ID == id).Order;
            //删除solution
            db.Solutions.Remove(db.Solutions.SingleOrDefault(m => m.ID == id));
            //删除封面图片
            db.SolutionImages.Remove(db.SolutionImages.SingleOrDefault(m => m.SolutionID == id));

            //找出这个类型的solution
            int typeID = db.Solutions.SingleOrDefault(m => m.ID == id).TypeID;
            List<Solution> solutionList = db.Solutions.Where(m => m.TypeID == typeID).ToList();

            //将solutionList中Order值大于thisOrder全部减一
            foreach (var item in solutionList)
            {
                if (item.Order > thisOrder)
                {
                    item.Order -= 1;
                }
            }
            db.SaveChanges();

            return RedirectToAction("Solution");
        }

        [HttpPost]
        public bool IsSolutionRepeat(string SolutionName)
        {
            Solution temp = db.Solutions.SingleOrDefault(m => m.Name.Equals(SolutionName));
            
            if(temp == null)
            {
                return false;
            }
            return true;
        }

        [HttpPost]
        public bool IsProductRepeat(string ProductName)
        {
            Product temp = db.Products.SingleOrDefault(m => m.Name.Equals(ProductName));

            if(temp == null)
            {
                return false;
            }
            return true;
        }

        public IActionResult ProductType()
        {
            string username = HttpContext.Session.GetString("username");
            if (username != null)
            {
                ViewBag.username = username;
                List<ProductType> productTypes = db.ProductTypes.ToList();
                return View(productTypes);
            }
            ViewBag.tips = "您还没有登录，请先登录";
            return View("Login");
        }

        public IActionResult Product()
        {
            string username = HttpContext.Session.GetString("username");
            if (username != null)
            {
                ViewBag.username = username;
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


                return View(productList);
            }
            ViewBag.tips = "您还没有登录，请先登录";
            return View("Login");
        }
        

        [HttpPost]
        public void AddProductType(string TypeName, string TypeRemark)
        {
            ProductType productType = new ProductType
            {
                Type = TypeName,
                Remark = (TypeRemark == null ? "无" : TypeRemark)
            };
            db.ProductTypes.Add(productType);
            db.SaveChanges();
            return;
        }

        [HttpPost]
        public void DeleteProductType(string ID)
        {
            int tempID = Convert.ToInt32(ID.Substring(2));
            db.ProductTypes.Remove(db.ProductTypes.SingleOrDefault(m => m.ID == tempID));
            db.SaveChanges();
            return;
        }


        [HttpPost]
        public void SaveProductType(string nowID, string TypeName, string TypeRemark)
        {
            int ID = Convert.ToInt32(nowID.Substring(2));
            ProductType productType = db.ProductTypes.SingleOrDefault(m => m.ID == ID);
            productType.Type = TypeName;
            productType.Remark = TypeRemark == null ? "无" : TypeRemark;
            db.SaveChanges();
            return;
        }

        public IActionResult AddProduct()
        {
            string username = HttpContext.Session.GetString("username");
            if (username != null)
            {
                ViewBag.username = username;
                List<ProductType> productList = db.ProductTypes.ToList();
                List<string> productTypeName = new List<string>();
                foreach (var item in productList)
                {
                    productTypeName.Add(item.Type);
                }
                return View(productTypeName);
            }
            ViewBag.tips = "您还没有登录，请先登录";
            return View("Login");
        }

        [HttpPost]
        public void PublishProduct(string productName, string productType, string productHTML, byte[] productImage, int productSort)
        {

            var typeID = db.ProductTypes.SingleOrDefault(m => m.Type == productType).ID;
            Product product = new Product
            {
                Name = productName,
                TypeID = typeID,
                Html = productHTML,
                Order = productSort
            };
            db.Products.Add(product);
            int thisTypeCount = db.Products.Where(m => m.TypeID == typeID).Count();
            if (productSort == thisTypeCount + 1) //放在最后一个，不用处理
            {
                db.SaveChanges();
            }
            else  //如果不是放在最后一个，把所有solution中Order字段值大于等于solutionSort的Order值分别加一
            {
                var thisTypeProductList = db.Products.Where(m => m.TypeID == typeID);
                var thisTypeProductListShouldPlusOne = thisTypeProductList.Where(m => m.Order >= productSort);
                foreach (var item in thisTypeProductListShouldPlusOne)
                {
                    item.Order += 1;
                }
                db.SaveChanges();
            }


            //添加图片
            int ID = db.Products.SingleOrDefault(m => m.Name == productName).ID;
            db.ProductImages.Add(new ProductImage
            {
                ProductID = ID,
                Image = productImage
            });
            db.SaveChanges();

            return;
        }

        [HttpPost]
        public int GetProductCount(string productTypeName)
        {
            int productID = db.ProductTypes.SingleOrDefault(m => m.Type.Equals(productTypeName)).ID;
            return db.Products.Where(m => m.TypeID == productID).Count();
        }


        //DeleteProduct
        public IActionResult DeleteProduct(int id)
        {
            int thisOrder = db.Products.SingleOrDefault(m => m.ID == id).Order;
            //删除solution
            db.Products.Remove(db.Products.SingleOrDefault(m => m.ID == id));
            //删除封面图片
            db.ProductImages.Remove(db.ProductImages.SingleOrDefault(m => m.ProductID == id));

            //找出这个类型的solution
            int typeID = db.Products.SingleOrDefault(m => m.ID == id).TypeID;
            List<Product> productList = db.Products.Where(m => m.TypeID == typeID).ToList();

            //将solutionList中Order值大于thisOrder全部减一
            foreach (var item in productList)
            {
                if (item.Order > thisOrder)
                {
                    item.Order -= 1;
                }
            }
            db.SaveChanges();

            return RedirectToAction("Product");
        }


        [HttpGet]
        public IActionResult EditProduct(int ID)
        {
            string username = HttpContext.Session.GetString("username");
            if (username != null)
            {
                Product product = db.Products.SingleOrDefault(m => m.ID == ID);
                List<string> ProductType = new List<string>();
                List<ProductType> productType = db.ProductTypes.ToList();
                foreach (var item in productType)
                {
                    ProductType.Add(item.Type);
                }
                EditProductModel editProductModel = new EditProductModel
                {
                    Product = product,
                    ProductType = ProductType
                };
                ViewBag.username = username;
                return View(editProductModel);
            }
            else
            {
                ViewBag.tips = "您还没有登录，请先登录";
                return View("Login");
            }
        }


        // 不修改图片且不修改产品类型时
        [HttpPost]
        public void EditProduct(int id, string productName, string productHTML, int productSort)
        {
            
            Product p = db.Products.SingleOrDefault(m => m.ID == id);
            
            int oldOrder = p.Order;

            if (oldOrder != productSort) // 如果修改了序号
            {
                Product temp = db.Products.Where(m => m.TypeID == p.TypeID).SingleOrDefault(m => m.Order == productSort);
                int tempOrder = temp.Order;
                temp.Order = p.Order;
                p.Order = tempOrder;
            }

            p.Name = productName;
            p.Html = productHTML;
            db.SaveChanges();

            return;
        }
        // 修改了类别但没有修改图片时的产品
        [HttpPost]
        public void EditProductWithType(int id, string productName, string productType, string productHTML, int productSort)
        {
            Product p = db.Products.SingleOrDefault(m => m.ID == id);
            int oldOrder = p.Order;
            // 将原类别Order值之后的Order值全部减一
            string oldType = p.ProductTypes.Type;
            List<Product> productOldTypeList = db.Products.Where(m => m.ProductTypes.Type.Equals(oldType)).ToList();
            foreach (var item in productOldTypeList)
            {
                if (item.Order > oldOrder)
                {
                    item.Order -= 1;
                }
            }

            // 将新类型Order值大于等于的Order值全部加一
            List<Product> productNewTypeList = db.Products.Where(m => m.ProductTypes.Type.Equals(productType)).ToList();
            foreach(var item in productNewTypeList)
            {
                if(item.Order >= productSort)
                {
                    item.Order += 1;
                }
            }

            // 修改该产品各项信息
            p.Name = productName;
            p.TypeID = db.ProductTypes.SingleOrDefault(m => m.Type.Equals(productType)).ID;
            p.Order = productSort;
            p.Html = productHTML;

            db.SaveChanges();
            return;
        }

        // 修改了产品图片但是没有修改类型时
        [HttpPost]
        public void EditProductWithImage(int id, string productName, string productType, string productHTML, byte[] productImage, int productSort)
        {
            Product p = db.Products.SingleOrDefault(m => m.ID == id);
            int oldOrder = p.Order;
            if(oldOrder != productSort) //修改了product的order
            {
                Product temp = db.Products.SingleOrDefault(m => m.Order == productSort);
                int tempOrder = temp.Order;
                temp.Order = p.Order;
                p.Order = tempOrder;
                db.SaveChanges();
            }
            // 修改产品各项信息
            p.Name = productName;
            p.TypeID = db.ProductTypes.SingleOrDefault(m => m.Type.Equals(productType)).ID;
            p.Order = productSort;
            p.Html = productHTML;

            ProductImage pi = db.ProductImages.SingleOrDefault(m => m.ProductID == p.ID);
            pi.Image = productImage;
            db.SaveChanges();

            return;
        }

        //修改了图片又修改了类型
        [HttpPost]
        public void EditProductWithImageAndType(int id, string productName, string productType, string productHTML, byte[] productImage, int productSort)
        {
            Product p = db.Products.SingleOrDefault(m => m.ID == id);
            int oldOrder = p.Order;
            // 将原类别Order值之后的Order值全部减一
            string oldType = p.ProductTypes.Type;
            List<Product> productOldTypeList = db.Products.Where(m => m.ProductTypes.Type.Equals(oldType)).ToList();
            foreach (var item in productOldTypeList)
            {
                if (item.Order > oldOrder)
                {
                    item.Order -= 1;
                }
            }

            // 将新类型Order值大于等于的Order值全部加一
            List<Product> productNewTypeList = db.Products.Where(m => m.ProductTypes.Type.Equals(productType)).ToList();
            foreach (var item in productNewTypeList)
            {
                if (item.Order >= productSort)
                {
                    item.Order += 1;
                }
            }

            // 修改该产品各项信息
            p.Name = productName;
            p.TypeID = db.ProductTypes.SingleOrDefault(m => m.Type.Equals(productType)).ID;
            p.Order = productSort;
            p.Html = productHTML;

            // 修改图片
            ProductImage pi = db.ProductImages.SingleOrDefault(m => m.ProductID == p.ID);
            pi.Image = productImage;

            db.SaveChanges();


            return;
        }
        
        [HttpPost]
        public bool IsProductRepeatExceptThis(string ThisName, string ProductName)
        {
            if (ThisName.Equals(ProductName))
            {
                return false;
            }else
            {
                Product p = db.Products.SingleOrDefault(m => m.Name.Equals(ProductName));
                if(p == null)
                {
                    return false;
                }else
                {
                    return true;
                }
            }
        }

        [HttpPost]
        public bool IsSolutionRepeatExceptThis(string ThisName, string SolutionName)
        {
            if (ThisName.Equals(SolutionName))
            {
                return false;
            }
            else
            {
                Solution s = db.Solutions.SingleOrDefault(m => m.Name.Equals(SolutionName));
                if (s == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}