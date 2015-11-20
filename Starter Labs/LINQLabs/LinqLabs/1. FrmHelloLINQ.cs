using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_01_HelloLINQ
{
    public partial class FrmHelloLINQ : Form
    {
        public FrmHelloLINQ()
        {
            InitializeComponent();
        }

        private void btuIEnumearble_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            //IEnumerable 公開能逐一查看非泛型集合內容一次的列舉值。
            //IEnumerable<T> 公開支援指定型別集合上簡單反覆運算的列舉值。

            //GetEnumerator() 傳回會逐一查看集合的列舉程式。
            //傳回: 
            //System.Collections.IEnumerator 物件，用於逐一查看集合。

            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            foreach (int n in nums)
            {
                this.listBox1.Items.Add(n);
            }

            //錯誤	1	foreach 陳述式不能用在型別 'int' 的變數上，因為 'int' 未包含 'GetEnumerator' 的公用定義	
            //  i 不是可列舉的
            //int i = 1;
            //foreach (int n in i)
            //{
            //    this.listBox1.Items.Add(n);
            //}

            this.listBox1.Items.Add("=============================");

            List<int> list = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            foreach (int n in list)
            {
                this.listBox1.Items.Add(n);
            }
            this.listBox1.Items.Add(string.Format("\r\n GetEnumerator(){0} \r\n", "===================="));

            System.Collections.IEnumerator en = nums.GetEnumerator();
            while (en.MoveNext())
            {
                this.listBox1.Items.Add(en.Current);
            }

            this.listBox1.Items.Add("=============================");

            List<int>.Enumerator en1 = list.GetEnumerator();
            while (en1.MoveNext())
            {
                this.listBox1.Items.Add(en1.Current);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();


            //Step 1:define source
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }; 
            

            //Step 2:define query
            //var r=...
            IEnumerable<int> q = from n in nums
                                 where n > 5 && n < 9
                                 select n;


            //Setp 3:execute query
            foreach (int n in q)
            {
                this.listBox1.Items.Add(n);
            }
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            //Step 1:define source
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //Step 2:define query
            IEnumerable<int> q = from n in nums
                                 where n % 2 == 0
                                 select n;
            //Setp 3:execute query
            foreach (int n in q)
            {
                this.listBox1.Items.Add(n);
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            //Step 1:define source
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //Step 2:define query
            //此時不會進入方法
            IEnumerable<int> q = from n in nums
                                 where IsEven(n)
                                 select n;
            //Setp 3:execute query
            //此時才會進入方法
            foreach (int n in q)
            {
                this.listBox1.Items.Add(n);
            }
        }

        private bool IsEven(int n)
        {
            if (n % 2 == 0)
            {
                return true;
            }
            else
            { 
                return false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            //Step 1:define source
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
             

            //Step 2:define query
            IEnumerable<Point> q = from n in nums
                                 select new Point(n, n * n);


            //Setp 3:execute query
            foreach (Point n in q)
            {
                this.listBox1.Items.Add(n);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            //Step 1:define source
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //Step 2:define query
            //此時不會進入方法
            //define query  (q 是一個  Iterator 物件)　, 如陣列集合一般 (陣列集合也是一個  Iterator 物件)
            //IEnumerable<int> q -  公開支援指定型別集合上簡單反覆運算的列舉值。
            IEnumerable<Point> q = from n in nums
                                 where IsEven(n)
                                 select new Point(n, n * n);
            
            //Step 3: Execute Query - foreach...
            //此時才會進入方法            
            //execute query(執行 iterator - 逐一查看集合的item)
            foreach (Point n in q)
            {
                this.listBox1.Items.Add(string.Format("{0},{1}", n.X, n.Y));
            }

            this.dataGridView1.DataSource = q.ToList();

            this.chart1.Series[0].XValueMember = "X";
            this.chart1.Series[0].YValueMembers = "Y";
            this.chart1.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            this.chart1.DataSource = q.ToList();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] str = { "Apple", "aaaaple", "bbapple", "ccApple", "yyy", "ZZApple" };

            var q = from n in str
                    where n.Length > 5 && n.Contains("Apple")
                    select n;
            
            foreach (string n in q)
            {
                this.listBox1.Items.Add(n);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            //public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);

            IEnumerable<Point> q = nums.Where(n => n > 5).Select(n => new Point(n, n * n));

            foreach (var n in q)
            {
                this.listBox1.Items.Add(n);
            }
            this.dataGridView1.DataSource = q.ToList();
        }

        private void button49_Click(object sender, EventArgs e)
        {
            //Linq 主要參考

            //1. 關於 Linq to Object
            //   參考 System.Core.dll (關於 Query) 
            //     System.Linq{}  (Enumerable /Querable Class – 擴充方法 )
            //     System.Linq.Expressions {} (Expression)

            //2. 關於 Linq to SQL
            //   參考 System.Data.Linq.dll (關於 Object Model) 
            //     System.Data.Linq {} (DataContext / Table <entity class>)
            //     System.Data.Linq.Mapping (ColumnAttribute….)

            //3 .關於 Linq to ADO.NET 
            //   參考 System.Data.DataSetExtensions.dll
            //      System.Data {}

            //4. 關於 Linq to XML  
            //   參考 System.XML.Linq.dll
            //       System.XML.Linq (XElement/XDocument......)
        }

   
    
    }
}
