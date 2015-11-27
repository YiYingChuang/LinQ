using NorthwindEntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            db.Database.Log = MyWrite;
        }
        northwindEntities db = new northwindEntities();

        void MyWrite(string TSQL)
        {
            this.richTextBox1.Text += TSQL;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            
            var q = from p in db.Products
                    select p;
            this.dataGridView1.DataSource = q.ToList();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            db.Database.Log = Console.Write;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var q = from p in db.Products
                    where p.CategoryID != null
                    select new
                    {
                        p.ProductID,
                        p.ProductName,
                        p.UnitPrice,
                        p.UnitsInStock,
                        TotalPrice = p.UnitPrice * p.UnitsInStock,
                        p.Category.CategoryName
                    };
            this.dataGridView1.DataSource = q.ToList();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button68_Click(object sender, EventArgs e)
        {
            //var q = from n in db.Orders
            //        orderby (n.OrderDate.Value.Year,n.OrderDate.Value.Season() ) 
            //        select new { MyKey = o.Key, MyCount = o.Count(), MyGroup = o };
            //this.dataGridView1.DataSource = q.ToList();
            //foreach (var p in q)
            //{
            //    TreeNode t = this.treeView1.Nodes.Add(p.MyKey.ToString());
            //    foreach (var item in p.MyGroup)
            //    {
            //        t.Nodes.Add(item.Customer.CompanyName);
            //    }
            //}
        }

 
    }
}
