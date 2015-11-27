using MyLINQ;
using NorthwindDataSetModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyLINQ
{
    public partial class FrmLinqToDataSet : Form
    {
        public FrmLinqToDataSet()
        {
            InitializeComponent();

      

            this.customersTableAdapter1.Fill(this.northwindDataSet1.Customers);
            this.ordersTableAdapter1.Fill(this.northwindDataSet1.Orders);
  this.productsTableAdapter1.Fill(this.northwindDataSet1.Products);
  this.categoriesTableAdapter1.Fill(this.northwindDataSet1.Categories);


        }

        private void button14_Click(object sender, EventArgs e)
        {

            //Note : For DataSet 以下 OK (for entity 就不 OK)
            this.dataGridView3.DataSource = this.northwindDataSet1.Products;

          
            var q = from p in this.northwindDataSet1.Products
                    //where p.UnitPrice > 30
                    select p;

            this.dataGridView1.DataSource = q.ToList();


            //1. Var (隱含型別區域變數)
            //2. 轉成匿名型別 (來源型別轉換投射成任意目標型別 )
            var q1 = from p in this.northwindDataSet1.Products
                     where !p.IsUnitPriceNull() && p.UnitPrice > 30
                     select new { prodID = p.ProductID, prodName = p.ProductName, p.UnitPrice, p.UnitsInStock, TotalPrice = p.UnitPrice * p.UnitsInStock };

            this.dataGridView2.DataSource = q1.ToList();
        }

        
        private void button18_Click(object sender, EventArgs e)
        {
            var q = from c in this.northwindDataSet1.Customers
                    group c by c.Country into g
                    select new
                    {
                        country = g.Key,
                        cityGroup = (from c in g group c by c.City)
                    };


            foreach (var countryGroup in q)
            {
                var node= this.treeView1.Nodes.Add(countryGroup.country);

                int count = 0;
                foreach (var cityGroup in countryGroup.cityGroup)
                {
                    
                      var node2= node.Nodes.Add(  string.Format("{0} ({1})",  cityGroup.Key , cityGroup.Count()));
                      count += cityGroup.Count();
                      foreach (var  city in cityGroup)
                      {
                          node2.Nodes.Add(string.Format("{0} ({1})", city.CustomerID , city.City));
                      }

                }
                node.Text = string.Format("{0} ({1})", countryGroup.country, count);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            this.ordersTableAdapter1.Fill(this.northwindDataSet1.Orders);

            var q = from o in this.northwindDataSet1.Orders
                    group o by o.OrderDate.Year into g
                    select new { Key = g.Key, Count = g.Count(), MyGroup = g };

            this.dataGridView1.DataSource = q.ToList();


            //==============================================
            foreach (var group in q)
            {
                TreeNode node = this.treeView1.Nodes.Add(string.Format("{0} ({1})", group.Key, group.Count));

                foreach (var item in group.MyGroup)
                {
                    node.Nodes.Add(item.OrderID.ToString());
                }
            }
            foreach (var group in q)
            {
                ListViewGroup lvGroup = this.listView1.Groups.Add(group.Key.ToString(), group.Key.ToString());
                foreach (var item in group.MyGroup)
                {
                    this.listView1.Items.Add(item.OrderID.ToString()).Group = lvGroup;
                }
            }
        }

   

        int Page = -1;
        int RowsPerPage = 5;
        private void button4_Click(object sender, EventArgs e)
        {
            if (Page == 0) return;
            Page -= 1;
            this.dataGridView1.DataSource = this.northwindDataSet1.Products.Skip(Page * RowsPerPage).Take(RowsPerPage).ToList() ;
      
        }

        private void button9_Click(object sender, EventArgs e)
        {

            int TotalPage = this.northwindDataSet1.Products.Count / RowsPerPage + ((this.northwindDataSet1.Products.Count % RowsPerPage == 0) ? 0 : 1);
            if (Page >= TotalPage - 1) return;

            Page += 1;
       this.dataGridView1.DataSource=  this.northwindDataSet1.Products.Skip(Page * RowsPerPage).Take(RowsPerPage).ToList();
      
            
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void dataGridView2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            //Note:　DataSet schema CategoryID  Null will throw exception
            //Datset 一直要去判斷 Is...NUll() , 很麻煩 , (除非能關掉改掉schema)
            //DBNull - How to get rid of annoying exception throwing   
            //Solution 1:去判斷 Is...NUll()
            //Solution 2:　DB design 不允許 null

            //inner join 
            //var products= this.northwindDataSet1.Products.Where(p => !(p.IsCategoryIDNull())) ;
            //var q0 = from c in this.northwindDataSet1.Categories
            //         join p in products on c.CategoryID equals  p.CategoryID
            //         select new { c.CategoryName, p.ProductID, p.ProductName };

            var q = from c in this.northwindDataSet1.Categories
                    join p in this.northwindDataSet1.Products on c.CategoryID equals  (p.IsCategoryIDNull() ? 0 : p.CategoryID)
                    group p by c.CategoryName into g
                    select new { g.Key, Avg = g.Average(p => p.UnitPrice) };

            this.dataGridView1.DataSource = q.ToList();

            //into g left outer join
            var q1 = from c in this.northwindDataSet1.Categories
                     join p in this.northwindDataSet1.Products.Where(p => !p.IsCategoryIDNull()) on c.CategoryID equals p.CategoryID into g
                     select new { c.CategoryName, Avg = g.Count()==0?0: g.Average(p => p.UnitPrice) };
            this.dataGridView2.DataSource = q1.ToList();

        }

        private void button13_Click(object sender, EventArgs e)
        {
            //var q2 = from c in this.northwindDataSet1.Categories
            //         //  from p in c.GetChildRows("FK_Products_Categories").Cast<NorthwindDataSet.ProductsRow>()  //cast 成 strong type 可抓 UnitPrice
            //         from NorthwindDataSet.ProductsRow p in c.GetChildRows("FK_Products_Categories")  //cast 成 strong type 可抓 UnitPrice
            //         group p by c.CategoryName into g
            //         select new { g.Key, Avg = g.Average(p => p.UnitPrice) };

            //this.dataGridView2.DataSource = q2.ToList();

            //Select Many       - inner join  //DataSet 用 GetChildRows()/GetProductsRow()
            var q1 = from c in this.northwindDataSet1.Categories
                        from p in   c.GetProductsRows()
                    group p by c.CategoryName into g
                    select new { g.Key, Avg = g.Average(p => p.UnitPrice) };

            this.dataGridView1.DataSource = q1.ToList();



        }

        private void button11_Click(object sender, EventArgs e)
        {
            //點標記法 , DataSet 用 GetChildRows()/GetProductsRow()  / GetParentRow()/ CategoriesRow
            //類導覽屬性
   //Datset 一直要去判斷 Is...NUll() , 很麻煩 (因為 DBnull 會丟exception)
        

            var q = from p in this.northwindDataSet1.Products
                    group p by (p.IsCategoryIDNull() ? "Null" : p.CategoriesRow.CategoryName) into g
                    select new { g.Key, Avg = g.Average(p => p.IsUnitPriceNull()?0:p.UnitPrice) };

            this.dataGridView1.DataSource = q.ToList();


            var q1 = from p in this.northwindDataSet1.Products
                     group p by (NorthwindDataSet.CategoriesRow)p.GetParentRow("FK_Products_Categories") into g
                     select new { g.Key, Avg = g.Average(p => p.IsUnitPriceNull() ? 0 : p.UnitPrice) };

            this.dataGridView2.DataSource = q1.ToList();
        


        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            //if 已經 join 好的adapter
            //this.categoryProductsTableAdapter1.Fill(this.northwindDataSet1.CategoryProducts);
            //var q = from p in this.northwindDataSet1.CategoryProducts
            //        group p by p.CategoryName into g
            //        select new { g.Key, Avg = g.Average(p => p.UnitPrice) };

            //this.dataGridView1.DataSource = q.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DataTable table1;
            table1 = this.northwindDataSet1.Products;


            var q = from NorthwindDataSet.ProductsRow row in table1.Rows
                    select row;
            this.dataGridView1.DataSource = q.ToList();


          
            //Query 來源物件 ProductTable OK ,  but Table1  不 OK
            var q1 = from NorthwindDataSet.ProductsRow row in  this.northwindDataSet1.Products
                     select row;
            this.dataGridView1.DataSource = q1.ToList();

            var q2 = from NorthwindDataSet.ProductsRow row in table1.AsEnumerable()
                    select row;
            this.dataGridView2.DataSource = q2.ToList();
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

      
    }
}
