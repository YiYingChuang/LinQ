using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Lab_02_CSharp_3._0
{
    public partial class FrmLangForLINQ : Form
    {
        public FrmLangForLINQ()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int x = 5;
            int y = 3;
            MessageBox.Show(string.Format("X = {0}, Y = {1}", x, y));
            Swap(ref x,ref y);
            MessageBox.Show(string.Format("X = {0}, Y = {1}", x, y));
        }

        private void Swap(ref int i,ref int j)
        {
            int temp = i;
            i = j;
            j = temp;            
        }      

        private void button5_Click(object sender, EventArgs e)
        {
            int x = 5;
            int y = 3;
            object ox = x;
            object oy = y;
            MessageBox.Show(string.Format("X = {0}, Y = {1}", x, y));
            SwapObject(ref ox, ref oy);
            x = (int)ox;
            y = (int)oy;
            MessageBox.Show(string.Format("X = {0}, Y = {1}", x, y));
        }

        private void SwapObject(ref object i, ref object j)
        {
            object temp = i;
            i = j;
            j = temp;
        }    

        private void button7_Click(object sender, EventArgs e)
        {
            int x = 5;
            int y = 3;
            MessageBox.Show(string.Format("X = {0}, Y = {1}", x, y));
           // SwapAnyType<int>(ref x, ref y);
            SwapAnyType(ref x, ref y);//自動推斷型別
            MessageBox.Show(string.Format("X = {0}, Y = {1}", x, y));
        }
        private void SwapAnyType<T>(ref T i, ref T j)
        {
            T temp = i;
            i = j;
            j = temp;
        }
        //Step1: creat delegate class
        //Step2: creat delegate object
        //Step3: Invoke

        public delegate bool TestDelegate(int i);
        private void button2_Click(object sender, EventArgs e)
        {
            int i = 10;
            //bool b = Test(i);

            //C# 1.0 具名方法
            TestDelegate tg_1 = Test;//new TestDelegate(Test);
            bool b = tg_1(i);//tg_1.Invoke(i);
            MessageBox.Show(string.Format("C# 1.0 \r\n{0} > 5 = {1}", i, b));


            //C# 2.0 匿名方法
            TestDelegate tg_2 = delegate (int j)
                                         {       
                                             return j > 5;       
                                         };
            bool b2 = tg_2.Invoke(i);
            MessageBox.Show(string.Format("C# 2.0 \r\n {0} > 5 = {1}", i, b2));

            //buttonx.Click
            this.buttonX.Click += buttonX_Click;
            this.buttonX.Click += new EventHandler(XXX);
            this.buttonX.Click += delegate(object sender1, EventArgs e1) 
                {
                    MessageBox.Show("匿名方法");
                };

            //C# 3.0 匿名方法簡潔板 labmda expression
            //Lambda 運算式是建立委派最簡單的方法 (參數型別也沒寫 / return 也沒寫 => 非常高階的抽象)
            TestDelegate tg_3 = n => n > 5;
            bool b3 = tg_3(i);
            MessageBox.Show(string.Format("C# 3.0 \r\n {0} > 5 = {1}", i, b3));     
            
            
      
        }

        private void XXX(object sender, EventArgs e)
        {
            MessageBox.Show("XXX");
        }

        void buttonX_Click(object sender, EventArgs e)
        {
            MessageBox.Show("buttonX_Click");
        }

        private bool Test(int i)
        {
            return i > 5;
        }
       
        List<int> MyWhere(int[] nums, TestDelegate dg)
        {
            List<int> l = new List<int>();
            foreach(int n in nums)
            {
                if (dg(n))
                {
                    l.Add(n);
                }
            }
            return l;
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            List<int> list_1 = MyWhere(nums, n => n > 5);
            List<int> list_2 = MyWhere(nums, n => n % 2 == 0);
            this.listBox1.DataSource = list_1.ToList();
            this.listBox2.DataSource = list_2.ToList();

            List<int> list_3 = MyWhere(nums, n => n % 2 != 0);
            this.listBox3.DataSource = list_3.ToList();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            MyClass m = new MyClass();
            IEnumerable<int> q = m.OddIterator();
            foreach(int n in q)
            {
                this.listBox1.Items.Add(n);
            }
           

        }

        private void button10_Click(object sender, EventArgs e)
        {
            //Where ...
            //public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate);

            //1. 泛型 (泛用方法)                   (ex.  void SwapAnyType<T>(ref T a, ref T b)
            //2. 委派參數 Lambda Expression (匿名方法簡潔版) (ex.  MyWhere(nums, n => n %2==0);
            //3. Iterator                            (ex.  OddIterator)
            //4. 擴充方法                            (ex.  MyStringExtend.WordCount(s);


            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            IEnumerable<int> q = from n in nums
                                 where n > 5
                                 select n;
            foreach (int n in q)
            {
                this.listBox1.Items.Add(n);
            }

            //=======================
            int[] nums1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var q1 = nums1.Where(n => n > 5);

            //this.listBox3.DataSource = q.ToList();

            foreach(int n in nums1)
            {
                this.listBox1.Items.Add(n);               
            }
        }

        private void button45_Click(object sender, EventArgs e)
        {
            MyProperty m = new MyProperty() { x = 1, y = 2, a = 3, b = 4 };
            List<MyProperty> list = new List<MyProperty> ( );
            list.Add(new MyProperty { x = 1, y = 2 });
            list.Add(new MyProperty { a = 3, b = 4 });
            list.Add(new MyProperty { x = 1, y = 2, a = 3, b = 4 });
            this.dataGridView1.DataSource = list;
        }

        private void button43_Click(object sender, EventArgs e)
        {
            var p1 = new { a = 1, b = 2 };
            var p2 = new { a = 1, b = 2, c = 3 };
            var p3 = new { a = 1, b = 2 };
            this.listBox1.Items.Add(p1.GetType());
            this.listBox1.Items.Add(p2.GetType());
            this.listBox1.Items.Add(p3.GetType());

            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            var q = from n in nums
                    where n > 5
                    select new { N = n, Square = n * n, Cube = n * n * n };
            foreach (var item in q)
            {
                this.listBox2.Items.Add(string.Format("{0}-{1}-{2}",item.N,item.Square,item.Cube));
            }

            //============================
            var q1 = nums.Where(n => n > 5).Select(n => new { N = n, Square = n * n, Cube = n * n * n });
            this.dataGridView1.DataSource = q1.ToList();
        }

        private void button44_Click(object sender, EventArgs e)
        {
            string s = "abcd";
            int length = s.LengthCount();
            int i = 99999;
            int intLength = i.IntLengthCount();
            char ch = s.Chars(2);
            MessageBox.Show(string.Format("LengthCount= {0}, IntLengthCount= {1}, Charts= {2}", length, intLength, ch));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.productsTableAdapter1.Fill(this.northwindDataSet1.Products);
            //this.dataGridView1.DataSource = this.northwindDataSet1.Products;

            var q = from p in this.northwindDataSet1.Products
                    where !p.IsUnitPriceNull() && p.UnitPrice > 30
                           && p.ProductName.ToUpper().StartsWith("C")
                    select p;

            this.dataGridView1.DataSource = q.ToList();

            var q1 = from p in this.northwindDataSet1.Products
                     where !p.IsUnitPriceNull() && p.UnitPrice > 30
                            && p.ProductName.ToUpper().StartsWith("C")
                     select new { p.ProductID, p.ProductName, p.UnitPrice, p.UnitsInStock, TotalPrice = p.UnitPrice * p.UnitsInStock };

            this.dataGridView2.DataSource = q1.ToList();

            this.ordersTableAdapter1.Fill(this.northwindDataSet1.Orders);
            var q3 = from o in this.northwindDataSet1.Orders
                     where !o.IsCustomerIDNull() && o.CustomerID.ToUpper().StartsWith("S")
                     select o;
            this.dataGridView2.DataSource = q3.ToList();
        }
        
    }
    
}
public static class MyStringExtend 
{
    public static int LengthCount(this string x)
    {
       return x.Length;
    }
    public static char Chars(this string x, int index)
    {
        return x[index];
    }

    public static int IntLengthCount(this int x)
    {
        return Convert.ToString(x).Length;    
    }
}

public class MyProperty
{
    public int x, y;
    private int m_a;
    public int a
    {
        get
        {
            return m_a;
        }
        set
        {
            m_a = value;
        }
    }
   
    public int b
       
    {
        get;set;
    }
}


class MyClass
    {
        private int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

        public IEnumerable<int> OddIterator()
        {
            foreach (int n in nums)
            {
                if (n % 2 == 1)
                {
                    yield return n;
                }
            }
        }
    }
