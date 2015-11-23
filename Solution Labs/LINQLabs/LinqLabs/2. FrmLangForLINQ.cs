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
            int n1 = 100;
            int n2 = 200;
            MessageBox.Show(n1 +"," + n2);

            Swap(ref n1, ref n2);
           MessageBox.Show(n1 +"," + n2);

           //==========================
           string s1 = "aaa";
           string s2 = "bbb";

           MessageBox.Show(s1 + "," + s2);
           Swap(ref s1, ref s2);
           MessageBox.Show(s1 + "," + s2);


        }

        void Swap(ref int n1, ref int n2)
        {
            int temp = n2;
            n2 = n1;
            n1 = temp;
        }
        void Swap(ref string n1, ref string n2)
        {
            string temp = n2;
            n2 = n1;
            n1 = temp;
        }
        void Swap(ref  Point n1, ref Point n2)
        {
            Point temp = n2;
            n2 = n1;
            n1 = temp;
        }

        void SwapObject(ref object n1, ref object n2)
        {
            object temp = n2;
            n2 = n1;
            n1 = temp;
        }


        void SwapAnyType<T>(ref T n1, ref T n2)
        {

            T temp = n2;
            n2 = n1;
            n1 = temp;

        }
        private void button5_Click(object sender, EventArgs e)
        {
           int n1 = 100;
            int n2 = 200;

            MessageBox.Show(n1 + "," + n2);
            object o1 = n1;
            object o2 = n2;

            SwapObject(ref o1, ref o2);

            n1 = (int) o1;
            n2 = (int)o2;
            MessageBox.Show(n1 + "," + n2);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int n1 = 100;
            int n2 = 200;
            MessageBox.Show(n1 + "," + n2);

            //SwapAnyType<int>(ref n1, ref n2);
            SwapAnyType(ref n1, ref n2); //推斷型別

            MessageBox.Show(n1 + "," + n2);
        }

        bool Test(int n)
        {
            return n > 5;
        }


        //Step 1: create delegate Class
        //Step 2: create delegate Object
        //Step 3; Invoke 

        public delegate bool TestDelegate(int n);

        private void button2_Click(object sender, EventArgs e)
        {
            int n = 4;
            //bool result =  Test(n);

           
            //C# 1.0 具名方法
            TestDelegate delegateObj_1 = Test; //new TestDelegate(Test);
            bool result = delegateObj_1(n);//.Invoke(n);
            MessageBox.Show("result = " + result);
           
            //C# 2.0 匿名方法
            TestDelegate delegateobj_2 = delegate (int n1) 
                                                  {
                                                    //.....
                                                      return n1 > 5;
                                                  };
            result= delegateobj_2.Invoke(2);
            MessageBox.Show("result = " + result);


            //ButtonX Click 
            this.buttonX.Click += buttonX_Click;
            this.buttonX.Click += new EventHandler(xxx);
            this.buttonX.Click += delegate(object sender1, EventArgs e1) 
                                             {
                                                 MessageBox.Show("匿名方法");
                                             };

            //C# 3.0 匿名方法簡潔板 labmda expression
            //Lambda 運算式是建立委派最簡單的方法 (參數型別也沒寫 / return 也沒寫 => 非常高階的抽象)

            //TestDelegate delegateObj_3 = (int n1) => { return n1 > 5; };
            TestDelegate delegateObj_3 = n1 => n1 > 5;
            result= delegateObj_3.Invoke(3);

            MessageBox.Show("result = " + result);





        }

        private void xxx(object sender, EventArgs e)
        {
            MessageBox.Show("xxx");
          
        }

        void buttonX_Click(object sender, EventArgs e)
        {
            MessageBox.Show("buttonX click");
          
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            
            List<int> LargeList =  MyWhere(nums, n => n > 5);
            List<int> EvenList = MyWhere(nums, n => n %2==0);
            List<int> OddList = MyWhere(nums, n => n % 2 == 1);

            foreach (int n in LargeList)
            {
                this.listBox1.Items.Add(n);
            }


            foreach (int n in EvenList)
            {
                this.listBox2.Items.Add(n);
            }
          

        }

        List <int> MyWhere(int[] nums, TestDelegate delegateObj )
        {
            List<int> list1 = new List<int>();
            //.....

            foreach (int n in nums)
            {
                if (delegateObj.Invoke(n))
                {
                    list1.Add(n);
                }
                
            }
            return list1;

        }

        private void button13_Click(object sender, EventArgs e)
        {
            MyList myList1 = new MyList();
           IEnumerable<int> q = myList1.OddIterator();

            foreach (int n in q)
            {
                this.listBox1.Items.Add(n);
            }


        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            //public static class Enumerable
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
            IEnumerable<int> q1= nums.Where<int>(n => n > 5);

            foreach (int n in q1)
            {
                this.listBox2.Items.Add(n);
            }
        }
  
        private void button45_Click(object sender, EventArgs e)
        {

            //var
            //1. 懶得寫
            //2. 難寫
            //3. 匿名型別 (不得不寫)
            int i=88;
          
            var j=88;

          var s = "xxxx";
          var p = new Point();


        }

        private void button43_Click(object sender, EventArgs e)
        {
          var p1 = new { x = 9, y = 7, z = 7 };
          var p2 = new { a = 8, b = 7 };
          var p3 = new { x = 9, y = 7, z = 7 };
          
          //  錯誤	7	無法指定屬性或索引子 'AnonymousType#1.x' -- 其為唯讀	E:\LINQ\Solution Labs\LINQLabs\LinqLabs\2. FrmLangForLINQ.cs	280	11	LINQLabs
          //p1.x = 88;

          this.listBox1.Items.Add(p1.GetType());
          this.listBox1.Items.Add(p2.GetType());
          this.listBox1.Items.Add(p3.GetType());

          //===========================================================
          int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
          var q = from n in nums
                  where n > 5
                  select new { N = n, Square = n * n, Cube = n * n * n };

          foreach ( var item in q)
          {
              this.listBox2.Items.Add(string.Format("{0} - {1} - {2}", item.N, item.Square, item.Cube));

          }
          //=========================================================
          var q1 = nums.Where(n => n > 5).Select(n => new { N = n, Square = n * n, Cube = n * n * n });

          this.dataGridView1.DataSource = q1.ToList();
        }

        private void button41_Click(object sender, EventArgs e)
        { 
            //() Constructor overloads
            Point p = new Point(100, 200);

            //===============================
            //{ } 物件初始化
            MyPoint p1 = new MyPoint { x = 7, y = 8, a = 7, b = 4 };

            List<MyPoint> list = new List<MyPoint>();
           
            list.Add(new MyPoint { x = 99, y = 99 });
            list.Add(new MyPoint { x = 7, y = 8, a = 7, b = 4 });
            list.Add(new  MyPoint { a = 7, b = 4 });

            this.dataGridView1.DataSource = list;

        }

        private void button40_Click(object sender, EventArgs e)
        {

            //具名型別陣列
            Point[] pts = new Point[]{ 
                                 new Point(10,10), 
                                 new Point(20, 20) 
                                };
  
            //匿名型別陣列

            var arr = new[] {
                                new { x = 1, y = 1 }, 
                                new { x = 2, y = 2 }
                             };


            foreach (var item in arr)
            {
                listBox1.Items.Add(item.x + ", " + item.y);

            }
            this.dataGridView1.DataSource = arr;
        }

        private void button44_Click(object sender, EventArgs e)
        {
            string s = "abcd111";
            int lenght = s.WordCount();
            MessageBox.Show("Length  = " + lenght);

            int count= MyStringExtend.WordCount(s);
            MessageBox.Show("count  = " + count);
            //==========================================


            char ch = s.Chars(2);
            MessageBox.Show("ch  = " + ch);
        
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.productsTableAdapter1.Fill(this.northwindDataSet1.Products);
            this.dataGridView1.DataSource = this.northwindDataSet1.Products;

            var q = from p in this.northwindDataSet1.Products
                    where  !p.IsUnitPriceNull() && p.UnitPrice > 30 
                          && p.ProductName.ToLower().StartsWith("c")
                    select p;

            this.dataGridView1.DataSource= q.ToList();
            //===============================

            var q1 = from p in this.northwindDataSet1.Products
                     where !p.IsUnitPriceNull() && p.UnitPrice > 30
                         && p.ProductName.ToLower().StartsWith("c")
                     select new { PName = p.ProductName, p.UnitPrice, p.UnitsInStock, TotalPrice = p.UnitPrice * p.UnitsInStock };

            this.dataGridView2.DataSource = q1.ToList();

        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            this.ordersTableAdapter1.Fill(this.northwindDataSet1.Orders);
            var q = from o in this.northwindDataSet1.Orders
                    where o.OrderDate.Year == 1996
                    select new
                    {
                        o.OrderID,
                        o.OrderDate,
                        o.OrderDate.Year,
                        o.OrderDate.Month,
                        Season = NumOfSeason(o.OrderDate)
                    };

            this.dataGridView1.DataSource = q.ToList();


        }

        private string NumOfSeason(DateTime dateTime)
        {
            if (dateTime.Month <= 3)
                return "第1季";
            else if (dateTime.Month <= 6)
                return "第2季";
            else if (dateTime.Month <= 9)
                return "第3季";
            else
                return "第4季";
        }

      
       
    }
}


public static class MyStringExtend
{
    public static int WordCount(this string s)
    {
        return s.Length;
    }

    public static char Chars(this string s, int index)
    {
        return s[index];
    }

}


//class MyString : String
//{

//}

class MyPoint
{
    //class var - Field
    public int x, y;

    private int m_a;
  
    //property
    public int a
    {
        get 
        { 
            //.,,...
            return m_a;
        }
        set 
        {
           //...
           m_a =value;
        }
    }
    //自動實作屬性
    public int b
    {
        get;
        set;
    }


}
class MyList
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
