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
//using LinqLabs;

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

            this.dataGridView1.DataSource = q.ToList();


            foreach (var group in q)
            {
                TreeNode node = this.treeView1.Nodes.Add(group.Key.ToString());

                foreach (var item in group)
                {
                    node.Nodes.Add(item.ToString());
                }
            }

            //============================================

            foreach (var group in q)
            {
                ListViewGroup lvGroup = this.listView1.Groups.Add(group.Key.ToString(), group.Key.ToString());
                foreach (var item in group)
                {
                    this.listView1.Items.Add(item.ToString()).Group = lvGroup;
                }
            }


        }

        private void button7_Click(object sender, EventArgs e)
        {

            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 101 };

            var q = from n in nums
                    group n by (n % 2) into g
                    select new { MyKey = (g.Key == 1 ? "奇數" : "偶數"), MyCount = g.Count(), MyGroup = g };

            this.dataGridView1.DataSource = q.ToList();

            foreach (var group in q)
            {
                TreeNode node = this.treeView1.Nodes.Add(group.MyKey.ToString());

                foreach (var item in group.MyGroup)
                {
                    node.Nodes.Add(item.ToString());
                }
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            //explicit inner join - 太 sql
            var q = from c in DataContext.Categories
                    join p in DataContext.Products on c.CategoryID equals p.CategoryID
                    select new { c.CategoryName, p.ProductID, p.ProductName, p.UnitPrice };

            dataGridView1.DataSource = q.ToList();

            //=======================
            //left outer join
            var q2 = from c in DataContext.Categories
                     join p in DataContext.Products on c.CategoryID equals p.CategoryID into g

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

        private void button2_Click(object sender, EventArgs e)
        {

            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 101 };

            var q = from n in nums
                    group n by MyKey(n) into g
                    select new { MyKey = g.Key, MyCount = g.Count(), MyGroup = g };

            this.dataGridView1.DataSource = q.ToList();

            foreach (var group in q)
            {
                TreeNode node = this.treeView1.Nodes.Add(group.MyKey.ToString());

                foreach (var item in group.MyGroup)
                {
                    node.Nodes.Add(item.ToString());
                }
            }
        }

        private string MyKey(int n)
        {
            if (n <= 4)
                return "Small";
            else if (n <= 10)
                return "Medium";
            else
                return "Large";
        }

        private void button38_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new System.IO.DirectoryInfo(@"c:\windows");
            FileInfo[] files = dir.GetFiles();

            var q = from f in files
                    group f by f.Extension into g
                    select new { MyKey = g.Key, MyCount = g.Count(), MyGroup = g };

            this.dataGridView1.DataSource = q.ToList();

            foreach (var group in q)
            {
                TreeNode node = this.treeView1.Nodes.Add(group.MyKey.ToString());

                foreach (var item in group.MyGroup)
                {
                    node.Nodes.Add(item.ToString());
                }
            }

            //============================================

            foreach (var group in q)
            {
                string headerText = string.Format("{0} ({1})", group.MyKey, group.MyCount);
                ListViewGroup lvGroup = this.listView1.Groups.Add(group.MyKey.ToString(), headerText);//  group.MyKey.ToString());
                foreach (var item in group.MyGroup)
                {
                    this.listView1.Items.Add(item.ToString()).Group = lvGroup;
                }
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = "This is a book. this is an apple.    this   is a pen.";

            string[] words = s.Split(new char[] { '.', ' ' }, StringSplitOptions.RemoveEmptyEntries);


            IEnumerable<IGrouping<string, string>> q = from w in words
                                                       group w by (w);




            var q1 = from w in words
                     group w by w.ToUpper() into g
                     select new { MyKey = g.Key, MyCount = g.Count(), MyGroup = g };



            this.dataGridView1.DataSource = q1.ToList();

            foreach (var group in q1)
            {
                TreeNode node = this.treeView1.Nodes.Add(group.MyKey.ToString());

                foreach (var item in group.MyGroup)
                {
                    node.Nodes.Add(item.ToString());
                }
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {

            System.IO.DirectoryInfo dir = new DirectoryInfo(@"c:\windows");
            FileInfo[] files = dir.GetFiles();

            var q = from f in files
                    let s = f.Extension.ToUpper()
                    where s == ".EXE"
                    select f;

            this.dataGridView2.DataSource = q.ToList();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string s = "This is a book. this is an apple.    this   is a pen.";

            string[] words = s.Split(new char[] { '.', ' ' }, StringSplitOptions.RemoveEmptyEntries);

            MessageBox.Show("count = " + words.Where(w => w == "is").Count());

        }

        private void Test_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = DataContext.Products.ToList();
            this.dataGridView2.DataSource = DataContext.Categories.ToList();


            Nullable<bool> b; //bool? b;
            b = true; b = false; b = null;
            //if (b.HasValue) { ....}

            bool b1;
            b1 = true; b1 = false;

            //===============================
            //導覽屬性
            this.dataGridView3.DataSource = DataContext.Categories.First().Products.ToList();

            MessageBox.Show(DataContext.Products.First().Category.CategoryName);

        }

        private void button22_Click(object sender, EventArgs e)
        {
            var q1 = from p in DataContext.Products
                     orderby p.UnitPrice, p.UnitsInStock
                     select p;

            this.dataGridView1.DataSource = q1.ToList();


            var q2 = from p in DataContext.Products
                     orderby p.UnitPrice, p.UnitsInStock descending
                     select p;

            this.dataGridView2.DataSource = q2.ToList();
            //==============================


            var q3 = DataContext.Products.OrderBy(p => p.UnitPrice).ThenByDescending(p => p.UnitsInStock);
            this.dataGridView3.DataSource = q3.ToList();



        }

        private void button23_Click(object sender, EventArgs e)
        {
            //============================
            //自訂 compare logic
            var q3 = DataContext.Products.OrderBy(p => p, new MyComparer()).ToList();
            this.dataGridView2.DataSource = q3.ToList();
        }

        class MyComparer : IComparer<Product>
        {

            public int Compare(Product x, Product y)
            {
                if (x.UnitPrice < y.UnitPrice)
                    return -1;
                else if (x.UnitPrice > y.UnitPrice)
                    return 1;
                else
                    return string.Compare(x.ProductName[0].ToString(), y.ProductName[0].ToString(), true);

            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            //SelectMany
            //inner join  (Implicit inner join)
            //Note : 這是較 Linq 較直覺的寫法,join 太 sql 了

            var q = from c in DataContext.Categories
                    from p in c.Products
                    select new { c.CategoryName, p.ProductID, p.ProductName, p.UnitPrice };

            //q = DataContext.Categories.SelectMany(c => c.Products, (c, p) => new { c.CategoryName, p.ProductID, p.ProductName, p.UnitPrice }).ToList();

            this.dataGridView1.DataSource = q.ToList();

            //======================================
            //cross join
            var q2 = from c in DataContext.Categories
                     from p in DataContext.Products
                     select new { c.CategoryName, p.ProductID, p.ProductName, p.UnitPrice };

            MessageBox.Show("cross join count = " + q2.Count());
            this.dataGridView2.DataSource = q2.ToList();

            //=======================================





        }

        private void button16_Click(object sender, EventArgs e)
        {
            // join
            var q = from p in DataContext.Products
                    where p.Category != null
                    select new
                    {
                        p.ProductID,
                        p.ProductName,
                        p.UnitPrice,
                        p.UnitsInStock,
                        TotalPrice = (p.UnitPrice * p.UnitsInStock).ToString("c2"),
                        p.Category.CategoryName
                    };


            this.dataGridView1.DataSource = q.ToList();


            //====================
            //left outer join
            var q2 = from p in DataContext.Products
                     select new
                    {
                        p.ProductID,
                        p.ProductName,
                        p.UnitPrice,
                        p.UnitsInStock,
                        TotalPrice = (p.UnitPrice * p.UnitsInStock).ToString("c2"),
                        Category = p.Category == null ? "unknown" : p.Category.CategoryName
                    };


            this.dataGridView2.DataSource = q2.ToList();


        }

        private void button10_Click(object sender, EventArgs e)
        {


        }

        private void button11_Click(object sender, EventArgs e)
        {
            var q = from p in DataContext.Products
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

            this.chart1.DataSource = q.ToList();

            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            chart1.Series[0].XValueMember = "CategoryName";
            chart1.Series[0].YValueMembers = "Min";

            chart1.Series[1].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            chart1.Series[1].XValueMember = "CategoryName";
            chart1.Series[1].YValueMembers = "Max";




        }

        private void button8_Click(object sender, EventArgs e)
        {
            var q = from p in DataContext.Products
                    where p.Category != null
                    group p by MyPriceKey(p.UnitPrice) into g
                    select new
                    {
                        MyKey = g.Key,
                        MyCount = g.Count()

                    };

            this.dataGridView1.DataSource = q.ToList();


            //treeview1 / ListView ()

            this.chart1.DataSource = q.ToList();

            chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
            chart1.Series[0].XValueMember = "MyKey";
            chart1.Series[0].YValueMembers = "MyCount";
        }

        private string MyPriceKey(decimal UnitPrice)
        {
            if (UnitPrice <= 10)
                return "1.低價";
            else if (UnitPrice <= 40)
                return "2.中價";
            else
                return "3.高價";
        }

        private void button18_1_Click(object sender, EventArgs e)
        {
            // MessageBox.Show(DataContext.Categories.First().CategoryName);

            // MessageBox.Show(DataContext.Categories.ElementAt(4).CategoryName);

            Category c = DataContext.Categories.ElementAtOrDefault(3);
            if (c == null)
            {
                //.....
                MessageBox.Show("null");
            }
            else
            {
                MessageBox.Show(c.CategoryName);
            }

        }

        int[] nums1 = { 1, 3, 4, 5 };

        int[] nums2 = { 1, 4, 5, 6, 7, 8, 1 };

        private void button45_Click(object sender, EventArgs e)
        {


            var result1 = nums1.Intersect(nums2); //交集
            var result2 = nums2.Union(nums2);  //聯集

            var result3 = nums2.Except(new int[] { 1, 5 }); //-

            var result4 = nums2.Distinct();


        }

        private void button48_Click(object sender, EventArgs e)
        {
            bool result = nums2.Any(n => n > 7);

            int[] nums = { 2, 4, 6, 8, 9};
            var result1 = nums.All(n => n % 2 == 0);
            bool result3 = nums1.Contains(4);

        }

        private void button47_Click(object sender, EventArgs e)
        {

        }

        private void button27_Click(object sender, EventArgs e)
        {
            this.dataGridView1.DataSource = DataContext.Categories.ElementAt(2).Products.ToList();
            this.dataGridView2.DataSource = DataContext.Categories.ElementAt(3).Products.ToList();

            this.dataGridView3.DataSource = DataContext.Categories.ElementAt(3).Products.DefaultIfEmpty().ToList();

            //================================


            //SelectMany
            //inner join  (Implicit inner join)
            //Note : 這是較 Linq 較直覺的寫法,join 太 sql 了

            var q = from c in DataContext.Categories
                    from p in c.Products
                    select new { c.CategoryName, p.ProductID, p.ProductName, p.UnitPrice };

            //q = DataContext.Categories.SelectMany(c => c.Products, (c, p) => new { c.CategoryName, p.ProductID, p.ProductName, p.UnitPrice }).ToList();

            this.dataGridView1.DataSource = q.ToList();

            //left outer join
            var q1 = from c in DataContext.Categories
                     from p in c.Products.DefaultIfEmpty()
                     select new {c.CategoryName, ProductID = p == null ? -1 : p.ProductID, ProductName = p == null ? "" : p.ProductName };

            this.dataGridView2.DataSource = q1.ToList();
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        const int RowsPerPage = 4;
        int currentPage = -1;

        private void button9_Click(object sender, EventArgs e)
        {
            int TotalPage = DataContext.Products.Count() / RowsPerPage + ((DataContext.Products.Count() % RowsPerPage == 0) ? 0 : 1);
            if (currentPage >= TotalPage - 1) return;

           currentPage += 1;
            this.dataGridView1.DataSource = DataContext.Products.Skip(currentPage * RowsPerPage).Take(RowsPerPage).ToList();//this.bindingSource1;

        }

        private void button4_Click(object sender, EventArgs e)
        {

            if (currentPage == 0) return;

            currentPage -= 1;
            this.dataGridView1.DataSource = DataContext.Products.Skip(currentPage * RowsPerPage).Take(RowsPerPage).ToList(); ;//this.bindingSource1;

        }

        private void button24_Click(object sender, EventArgs e)
        {
            // PLINQ 簡介
            //什麼是平行查詢？
            //主要差別在於 PLINQ 會嘗試充分運用系統上的所有處理器。 它的作法是將資料來源分割成多個區段，然後以平行方式，以個別的背景工作執行緒在多個處理器上對每個區段執行查詢。 在許多情況下，平行執行可讓查詢速度快許多。

            var source = Enumerable.Range(1, 10000);

           var evenNums = from num in source.AsParallel()
                           where num % 2 == 0
                           select num;
           foreach (var n in evenNums)
           {
               Console.WriteLine(n);
           }

         
        }

        private void button50_Click(object sender, EventArgs e)
        {

        }

        private void button25_Click(object sender, EventArgs e)
        {

            var source = Enumerable.Range(1, 10000);

            var evenNums = from num in source.AsParallel()
                           where num % 2 == 0
                           select num;
            foreach (var n in evenNums)
            {
                Console.WriteLine(n);
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {

            var source = Enumerable.Range(1, 10000);

            var evenNums = from num in source
                           where num % 2 == 0
                           select num;
            foreach (var n in evenNums)
            {
                Console.WriteLine(n);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {

        }

        private void button30_Click(object sender, EventArgs e)
        {

        }
    }

}