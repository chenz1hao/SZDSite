using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SZD.WebSite.Model
{
    public class DBContext : DbContext
    {
        public static string ConnectionString = "ConnectionString";

        public DBContext() : base(ConnectionString)
        {

        }

        public DBContext(string connString) : base(connString) { }
        public DbSet<User> Users { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<SolutionType> SolutionTypes { get; set; }
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<SolutionImage> SolutionImages { get; set; }
    }
}
