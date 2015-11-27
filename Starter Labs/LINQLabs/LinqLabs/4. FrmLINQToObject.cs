using MyDataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Starter
{
    public partial class FrmLINQ_To_XXX : Form
    {
        public FrmLINQ_To_XXX()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            IEnumerable<IGrouping<int, int>> q = from n in nums
                                                 group n by (n % 2);
            //var q = from n in nums
            //        group n by (n % 2);

            this.dataGridView1.DataSource = q.ToList();

            foreach(var groups in q)
            {
                //if( groups.Key==0)

                TreeNode node = this.treeView1.Nodes.Add(groups.Key.ToString());
                foreach (var item in groups)
                {
                    node.Nodes.Add(item.ToString());
                }
                
            }

            foreach (var groups in q)
            {
                ListViewGroup lvgroup= this.listView1.Groups.Add(groups.Key.ToString(), groups.Key.ToString());
                foreach (var item in groups)
                {
                    this.listView1.Items.Add(item.ToString()).Group = lvgroup;
                }

            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var q = from n in nums
                    group n by (n % 2) into g
                    select new {Key=(g.Key==1?"奇數":"偶數"),Count= g.Count(),Group=g};

            this.dataGridView1.DataSource = q.ToList();

            foreach (var g in q)
            {
                TreeNode node = this.treeView1.Nodes.Add(g.Key.ToString());
                foreach (var item in g.Group)
                {
                    node.Nodes.Add(item.ToString());
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var q = from n in nums
                    group n by Group(n) into g
                    select new { Key = g.Key, Count = g.Count(), Group = g };

            this.dataGridView1.DataSource = q.ToList();

            foreach (var g in q)
            {
                TreeNode node = this.treeView1.Nodes.Add(g.Key.ToString());
                foreach (var item in g.Group)
                {
                    node.Nodes.Add(item.ToString());
                }

            }
        }

        private string Group(int n)
        {
            if (n < 5)
                return "small";
            else if (n < 10)
                return "medium";
            else
                return "Large";
        }

        private void button38_Click(object sender, EventArgs e)
        {
            
            FileInfo f = new FileInfo(@"C:\Users\III\下載");

            //var q = from n in f

            //foreach (var groups in q)
            //{
            //    //if( groups.Key==0)

            //    TreeNode node = this.treeView1.Nodes.Add(groups.Key.ToString());
            //    foreach (var item in groups)
            //    {
            //        node.Nodes.Add(item.ToString());
            //    }

            //}

            //foreach (var groups in q)
            //{
            //    ListViewGroup lvgroup = this.listView1.Groups.Add(groups.Key.ToString(), groups.Key.ToString());
            //    foreach (var item in groups)
            //    {
            //        this.listView1.Items.Add(item.ToString()).Group = lvgroup;
            //    }

            //}
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string s = "This is a blook. This is an apple. Thia is a pen.";
            string[] words= s.Split(new char[] {'.', ' '}, StringSplitOptions.RemoveEmptyEntries);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = "This is a blook. This is an apple. Thia is a pen.";
            string[] words= s.Split(new char[] {'.', ' '}, StringSplitOptions.RemoveEmptyEntries);
            var q = from n in words
                    group n by (n);

            var q1 = from n in words
                     group n by n into g
                     select new { MyKey = g.Key, MyCount = g.Count(), MyGroup = g };
            this.dataGridView1.DataSource = q1.ToList();

            foreach (var n in q1)
            {
                TreeNode t = this.treeView1.Nodes.Add(n.MyKey);
                foreach (var m in n.MyGroup)
                {
                    t.Nodes.Add(m);
                }
            }
        }

        private void Test_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = DataContext.Products.ToList();
            this.dataGridView2.DataSource = DataContext.Categories.ToList();

            MessageBox.Show(DataContext.Categories.First().CategoryName);
            this.dataGridView3.DataSource = DataContext.Categories.First().Products.ToList();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            var q = from p in DataContext.Products
                    orderby p.UnitPrice, p.UnitsInStock descending
                    select p;

            var q1 = DataContext.Products.OrderBy(p => p.UnitPrice).ThenByDescending(p => p.UnitsInStock);
            this.dataGridView1.DataSource = q.ToList();
            this.dataGridView2.DataSource = q1.ToList();
        }

        private void button20_Click(object sender, EventArgs e)
        {
            //inner join
            var q = from c in DataContext.Categories
                    join p in DataContext.Products on c.CategoryID equals p.CategoryID
                    select new { c.CategoryName, p.ProductID, p.ProductName, p.UnitPrice, p.UnitsInStock };

            this.dataGridView1.DataSource = q.ToList();

            //left outer join
            var q1 = from c in DataContext.Categories
                     join p in DataContext.Products on c.CategoryID equals p.CategoryID into g
                     select new { c.CategoryName, MyCount = g.Count(), MyGroup = g };
            this.dataGridView2.DataSource = q1.ToList();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            //select many--inner join
            var q = from c in DataContext.Categories
                    from p in c.Products
                    select new { c.CategoryName, p.ProductID, p.ProductName, p.UnitPrice };

            q = DataContext.Categories.SelectMany(c => c.Products, (c, p) => new { c.CategoryName, p.ProductID, p.ProductName, p.UnitPrice });
            this.dataGridView1.DataSource = q.ToList();

            //select many--cross join
            var q1 = from c in DataContext.Categories
                    from p in DataContext.Products
                    select new { c.CategoryName, p.ProductID, p.ProductName, p.UnitPrice };
            this.dataGridView2.DataSource = q1.ToList();

        }

        private void button16_Click(object sender, EventArgs e)
        {
            //inner join
            var q = from p in DataContext.Products
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

            //left outer join
            var q1 = from p in DataContext.Products
                    select new
                    {
                        p.ProductID,
                        p.ProductName,
                        p.UnitPrice,
                        p.UnitsInStock,
                        TotalPrice =(p.UnitPrice * p.UnitsInStock).ToString("c2"),
                        Category = p.Category == null ? "" : p.Category.CategoryName
                    };
            this.dataGridView1.DataSource = q1.ToList();

        }

        private void button13_Click(object sender, EventArgs e)
        {
           
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var q = from p in DataContext.Products
                    where p.CategoryID != null
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

            this.chart1.DataSource = q.ToList();
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            this.chart1.Series[0].Name = "Max";
            this.chart1.Series[0].XValueMember = "CategoryName";
            this.chart1.Series[0].YValueMembers = "Max";
            this.chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            this.chart1.Series[1].Name = "Min";
            this.chart1.Series[1].XValueMember = "CategoryName";
            this.chart1.Series[1].YValueMembers = "Min";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var q = from p in DataContext.Products
                    where p.CategoryID != null
                    group p by MyWay(p.UnitPrice) into g
                    select new
                    {
                        CategoryName = g.Key,
                        MyCount = g.Count(),
                        Max = g.Max(p => p.UnitPrice),
                        MyGroup=g
                    };
            this.dataGridView1.DataSource = q.ToList();
            
            foreach(var p in q)
            {
               TreeNode t= this.treeView1.Nodes.Add(p.CategoryName.ToString());
                foreach (var item in p.MyGroup)
                {
                    t.Nodes.Add(string.Format("{0}--{1}",item.ProductName,item.UnitPrice));
                }
            }
        }

        private string MyWay(decimal p)
        {
            if (p <= 30)
                return "低價位";
            else if (p <= 60)
                return "中價位";
            else
                return "高價位";
        }

        private void button18_1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(DataContext.Products.ElementAt(0).Category.CategoryName.ToString());

            MessageBox.Show(DataContext.Products.ElementAt(0).Category.CategoryName.ToString());

            MessageBox.Show(DataContext.Products.ElementAtOrDefault(6).Category.CategoryName.ToString());
        }

        int RowsPerPage=3;
        int PageNumber = -1;
        private void button9_Click(object sender, EventArgs e)
        {
            PageNumber += 1;
            this.dataGridView1.DataSource = DataContext.Products.Skip(PageNumber*RowsPerPage).Take(RowsPerPage).ToList();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PageNumber -= 1;
            this.dataGridView1.DataSource = DataContext.Products.Skip(PageNumber * RowsPerPage).Take(RowsPerPage).ToList();
        }

       

    

       
    }
}
