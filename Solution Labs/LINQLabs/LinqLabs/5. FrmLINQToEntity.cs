using NorthwindEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LINQ.StartupLabs
{
    public partial class FrmLINQToEntity : Form
    {
        public FrmLINQToEntity()
        {
            InitializeComponent();

            db.Database.Log = Console.Write;
            db.Database.Log = MyWriteTSQL;

        }

        void MyWriteTSQL(string  TSQL)
        {
            this.richTextBox1.Text += TSQL;
        }
        //Note:
        //1 參考 EntityFramework.dll / EntityFrmework.SqlServer.dll
        //2. app.config 連接字串
  
        northwindEntities db = new northwindEntities();
           
        
        private void button6_Click(object sender, EventArgs e)
        {
           
            var q = from p in db.Products
                    where p.UnitPrice>30
                    select p;

            this.dataGridView1.DataSource = q.ToList();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            db.Database.Log = Console.Write;
        }

        private void button38_Click(object sender, EventArgs e)
        {
          

            //===========================================
            var q1 = from emp in db.Employees.AsEnumerable()
                     where (from o in emp.Orders
                            where o.OrderDate.Value >= DateTime.Parse("1997/1/1") && o.OrderDate.Value <= DateTime.Parse("1997/1/5")
                            select o.EmployeeID).Any()
                     select emp;

            this.dataGridView1.DataSource = q1.ToList();

            var q2 = from emp in db.Employees.AsEnumerable()
                     where emp.Orders.Any(o=>o.OrderDate.Value >= DateTime.Parse("1997/1/1") && o.OrderDate.Value <= DateTime.Parse("1997/1/5"))
                     select emp;

            this.dataGridView2.DataSource = q2.ToList();

        }

        private void button50_Click(object sender, EventArgs e)
        {
            //類似 T-SQL 中的 EXISTS
            // 有下訂單的客戶
            var custs = from c in db.Customers where c.Orders.Any() select c;
            this.dataGridView1.DataSource = custs.ToList();

            //訂單中送貨城市是 London 的客戶
            var custs1 = from c in db.Customers
                         where (from o in c.Orders
                                where o.ShipCity == "London"
                                select 0).Any()
                         select c;
            this.dataGridView2.DataSource = custs1.ToList();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            //T-SQL 多難

            //每個分類群組的 unitPrice 都大於 20
            var q1 = from p in db.Products
                     group p by p.CategoryID into g
                     where g.All(p => p.UnitPrice >= 10)
                     from x in g
                     select new { x.CategoryID, ProdName = x.ProductName, x.UnitPrice };

            this.dataGridView1.DataSource = q1.ToList();
            this.dataGridView2.DataSource = q1.Select(c => c.CategoryID).Distinct().ToList();
         
        }

        private void button23_Click(object sender, EventArgs e)
        {
            var q = from p in db.Products
                    where p.UnitPrice > 15 && p.Order_Details.Sum(o => o.Quantity) < 200
                    select p;
            this.dataGridView1.DataSource = q.ToList();

     
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //join
            var q = from p in db.Products
                    where p.Category != null
                    select new
                    {
                        
                      p.ProductID,
                        p.ProductName,
                        p.UnitPrice,
                        p.UnitsInStock,
                        TotalPrice = (p.UnitPrice * p.UnitsInStock),
                        p.Category.CategoryName
                    };

           

            this.dataGridView1.DataSource = q.ToList();

         
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var q = from p in db.Products//.AsEnumerable()
                    where p.Category != null
                    group p by p.Category.CategoryName into g
                    select new
                    {
                        CategoryName = g.Key,
                        MyCount = g.Count(),
                        Avg = g.Average(p => p.UnitPrice),
                        Max = g.Max(p => p.UnitPrice),
                        Min = g.Min(p => p.UnitPrice)
                    };

            this.dataGridView1.DataSource = q.ToList();
        }

        private void button3_Click(object sender, EventArgs e)
        {
        //    db.

            this.dataGridView1.DataSource = db.Sales_by_Year(new DateTime(1990, 1, 1), DateTime.Now);
           
        }

        private void button68_Click(object sender, EventArgs e)
        {
            //看看 T-SQL 很難寫

            var q = from o in db.Orders
                    group o by new { year = o.OrderDate.Value.Year, month = o.OrderDate.Value.Month } into g
                    select new { g.Key, orderCount = g.Count(), TimeOrder = g };

            this.dataGridView1.DataSource = q.ToList();
            treeView1.Nodes.Clear();
            foreach (var o in q)
            {
                TreeNode x = treeView1.Nodes.Add(string.Format("{0} 年 {1} 月", o.Key.year, o.Key.month));

                foreach (var o1 in o.TimeOrder)
                {
                    x.Nodes.Add(string.Format("\t{0}-{1}-{2}", o1.OrderID, o1.ShipCity, o1.OrderDate.HasValue ? o1.OrderDate.Value.ToShortDateString() : "null"));

                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //Note : AsEnumerable()
            var q = from o in db.Orders.AsEnumerable()
                    group o by new { year = o.OrderDate.Value.Year, 季 = o.OrderDate.Season(), month = o.OrderDate.Value.Month } into g
                    select new { g.Key, orderCount = g.Count(), TimeOrder = g };

            this.dataGridView1.DataSource = q;
            treeView1.Nodes.Clear();
            foreach (var o in q)
            {
                TreeNode x = treeView1.Nodes.Add(string.Format("{0} 年 {1} 季 {2} 月", o.Key.year, o.Key.季, o.Key.month));

                foreach (var o1 in o.TimeOrder)
                {
                    x.Nodes.Add(string.Format("\t{0}-{1}-{2}", o1.OrderID, o1.ShipCity, o1.OrderDate.HasValue ? o1.OrderDate.Value.ToShortDateString() : "null"));

                }
            }
        }
  
        private void button15_Click(object sender, EventArgs e)
        {
            var YearGroup = from o in db.Orders.AsEnumerable()
                            group o by o.OrderDate.Value.Year into g
                            select new { g.Key, orders = g };

            var SeasonGroup = from x in YearGroup
                              from o in x.orders
                              group o by o.OrderDate.Season() into g
                              select new { g.Key, orders = g };


            var MonthGroup = "";


            this.dataGridView1.DataSource = YearGroup.ToList();
            this.dataGridView2.DataSource = SeasonGroup.ToList();



            var YearSeasonMonthGroup = from o in db.Orders.AsEnumerable()
                                       group o by o.OrderDate.Value.Year into g
                                       select new
                                       {
                                           年 = g.Key,
                                           SeasonGroup = from o in g
                                                         group o by o.OrderDate.Season() into g1
                                                         select new
                                                         {
                                                             季 = g1.Key,
                                                             monthGroup = from o in g1
                                                                          group o by o.OrderDate.Value.Month into g2
                                                                          select new
                                                                          {
                                                                              月 = g2.Key,
                                                                              MonthGroup = g2
                                                                          }
                                                         }
                                       };


            treeView1.Nodes.Clear();
            foreach (var o in YearSeasonMonthGroup)
            {
                TreeNode x = treeView1.Nodes.Add(o.年.ToString());

                foreach (var o1 in o.SeasonGroup)
                {
                    TreeNode y = x.Nodes.Add(o1.季);

                    foreach (var o2 in o1.monthGroup)
                    {
                        TreeNode z = y.Nodes.Add(o2.月.ToString());
                        foreach (var o3 in o2.MonthGroup)
                        {
                            z.Nodes.Add(string.Format("{0} - {1} - {2}", o3.OrderID, o3.OrderDate, o3.Freight));
                        }
                    }
                }
            }
            
        }

        private void button33_Click(object sender, EventArgs e)
        {
            var q = db.Customers.OrderBy(c => c, new MyCustomerComparer()).Reverse();

            this.dataGridView2.DataSource = q.ToList();
        }

        private void button31_Click(object sender, EventArgs e)
        {
            //使用指定的比較子，依遞增順序排序序列中的項目。


            //note AsEnumerable()
            var q = db.Customers.AsEnumerable().OrderBy(c => c, new MyCustomerComparer()).Select(c => new { c.Country, c.City, c.CustomerID }); ;

            this.dataGridView1.DataSource = q.ToList();
        }

        private void button36_Click(object sender, EventArgs e)
        {
            var q2 = db.Customers.OrderBy(c => c.Country).ThenBy(c => c.City).Select(c => new { c.Country, c.City, c.CustomerID });


            this.dataGridView2.DataSource = q2.ToList();
        }

        private void button34_Click(object sender, EventArgs e)
        {
            var q = db.Orders.OrderBy(o => o.OrderDate.Value);
            this.dataGridView1.DataSource = q.ToList();


            var q1 = from c in db.Customers
                     orderby c.CustomerID ascending, c.City descending
                     select new { c.CustomerID, c.Country, c.City };
            this.dataGridView1.DataSource = q1.ToList();

            var q2 = db.Customers.OrderBy(c => c.Country).ThenBy(c => c.City).Select(c => new { c.Country, c.City, c.CustomerID });


            this.dataGridView2.DataSource = q2.ToList();
        }

        class MyCustomerComparer : IComparer<Customer>
        {

            public int Compare(Customer x, Customer y)
            {
                if (string.Compare(x.Country, y.Country, true) < 0) //忽略 大小寫
                    return -1;
                else if (string.Compare(x.Country, y.Country) > 0)
                    return 1;
                else
                    if (string.Compare(x.City, y.City) < 0)
                        return -1;
                    else if (string.Compare(x.City, y.City) > 0)
                        return 1;
                    else
                        return (x.CustomerID.CompareTo(y.CustomerID));
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {

            //強迫到 DB 整個重抓 - new NorthwindDataContext

            //但若 Entity 已存在 就強迫 Refresh
            //Way 1:
            //NorthwindDataContext db = new NorthwindDataContext();

            //Way 2:
            //ObjectContext

            IObjectContextAdapter IContext = (DbContext)db;
            ObjectContext objectContext = IContext.ObjectContext;
            ObjectStateManager objectStateManager = objectContext.ObjectStateManager;

            objectContext.Refresh(System.Data.Entity.Core.Objects.RefreshMode.StoreWins, db.Products);

            this.dataGridView3.DataSource = db.Products.ToList();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //explicit inner join - 太 sql
            var q = from c in db.Categories
                    join p in db.Products on c.CategoryID equals p.CategoryID
                    select new { c.CategoryName, p.ProductID, p.ProductName, p.UnitPrice };

            dataGridView1.DataSource = q.ToList();

            //=======================
            //left outer join
            var q2 = from c in db.Categories
                     join p in db.Products on c.CategoryID equals p.CategoryID into g

                     select new { MyKey = c.CategoryName, MyGroup = g, MyCount = g.Count() };
            this.dataGridView2.DataSource = q2.ToList();

            //======================================================
            foreach (var group in q2)
            {
                TreeNode node = this.treeView1.Nodes.Add(group.MyKey.ToString());

                foreach (var item in group.MyGroup)
                {
                    node.Nodes.Add(item.ToString());
                }
            }

        }

        private void button11_Click(object sender, EventArgs e)
        {
            //SelectMany
            //inner join  (Implicit inner join)
            //Note : 這是較 Linq 較直覺的寫法,join 太 sql 了

            var q = from c in db.Categories
                    from p in c.Products
                    select new { c.CategoryName, p.ProductID, p.ProductName, p.UnitPrice };

            //q = DataContext.Categories.SelectMany(c => c.Products, (c, p) => new { c.CategoryName, p.ProductID, p.ProductName, p.UnitPrice }).ToList();

            this.dataGridView1.DataSource = q.ToList();

            //======================================
            //cross join
            var q2 = from c in db.Categories
                     from p in db.Products
                     select new { c.CategoryName, p.ProductID, p.ProductName, p.UnitPrice };

            MessageBox.Show("cross join count = " + q2.Count());
            this.dataGridView2.DataSource = q2.ToList();

        }

        private void button16_Click(object sender, EventArgs e)
        {
       
            var q = from p in db.Products
                    where p.Category != null
                    select new
                    {
                        p.ProductID,
                        p.ProductName,
                        p.UnitPrice,
                        p.UnitsInStock,
                        TotalPrice = (p.UnitPrice * p.UnitsInStock),
                        p.Category.CategoryName
                    };


            this.dataGridView1.DataSource = q.ToList();


            //====================
            //left outer join
            var q2 = from p in db.Products
                     select new
                     {
                         p.ProductID,
                         p.ProductName,
                         p.UnitPrice,
                         p.UnitsInStock,
                         TotalPrice = (p.UnitPrice * p.UnitsInStock),
                         Category = p.Category == null ? "unknown" : p.Category.CategoryName
                     };


            this.dataGridView2.DataSource = q2.ToList();

        }

        private void button43_Click(object sender, EventArgs e)
        {
            var q = from p in db.Products
                    where p.Category != null
                    group p by p.Category.CategoryName into g
                    select new
                    {
                        CategoryName = g.Key,
                        MyCount = g.Count(),
                        Avg = g.Average(p => p.UnitPrice),
                        Max = g.Max(p => p.UnitPrice),
                        Min = g.Min(p => p.UnitPrice)
                    };

            this.dataGridView1.DataSource = q.ToList();


            //treeview1 / ListView ()

          
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var q = from c in  db.Customers
                    group c by c.Country into g
                    select new
                    {
                        country = g.Key,
                        cityGroup = (from c in g group c by c.City)
                    };


            foreach (var countryGroup in q)
            {
                var node = this.treeView1.Nodes.Add(countryGroup.country);

                int count = 0;
                foreach (var cityGroup in countryGroup.cityGroup)
                {

                    var node2 = node.Nodes.Add(string.Format("{0} ({1})", cityGroup.Key, cityGroup.Count()));
                    count += cityGroup.Count();
                    foreach (var city in cityGroup)
                    {
                        node2.Nodes.Add(string.Format("{0} ({1})", city.CustomerID, city.City));
                    }

                }
                node.Text = string.Format("{0} ({1})", countryGroup.country, count);
            }
        }

        private void button55_Click(object sender, EventArgs e)
        {
            db.Products.Add(new Product
                    {
                        ProductName = "Test " + DateTime.Now.ToLongTimeString(),
                        Discontinued = true
                    });

            db.SaveChanges();

        }

        private void button57_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = db.Products.ToList();
        }

        private void button56_Click(object sender, EventArgs e)
        {
            //update
            var product = (from p in db.Products
                           where p.ProductName.Contains("Test")
                           select p).FirstOrDefault();

            if (product == null) return;
            product.ProductName = "Test" + product.ProductName;

            this.db.SaveChanges();
        }

        private void button53_Click(object sender, EventArgs e)
        {
            //delete one product
            var product = (from p in db.Products
                           where p.ProductName.Contains("Test")
                           select p).FirstOrDefault();

            if (product == null) return;
            this.db.Products.Remove(product);
            this.db.SaveChanges();

        }

        private void button54_Click(object sender, EventArgs e)
        {
            //delete all
            var q = from p in db.Products
                    where p.ProductName.Contains("Test")
                    select p;

            foreach (var product in q)
            {
                this.db.Products.Remove(product);
            }

            this.db.SaveChanges();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = db.Products.ToList();
        }
   
    }
    public static class DateTimeExtension
    {
        public static string Season(this DateTime? source)
        {
            switch (source.Value.Month)
            {
                case 1:
                case 2:
                case 3:
                    return "第一季";
                case 4:
                case 5:
                case 6:
                    return "第二季";
                case 7:
                case 8:
                case 9:
                    return "第三季";
                case 10:
                case 11:
                case 12:
                    return "第四季";
            }
            return "";


        }
    }


}
