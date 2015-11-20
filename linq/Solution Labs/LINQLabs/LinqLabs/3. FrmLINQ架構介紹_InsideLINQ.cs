using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab_03
{
    public partial class FrmLINQ架構介紹_InsideLINQ : Form
    {
        public FrmLINQ架構介紹_InsideLINQ()
        {
            InitializeComponent();
        }

        private void button30_Click(object sender, EventArgs e)
        {
            System.Collections.ArrayList arrList = new System.Collections.ArrayList();
           
            arrList.Add(new Point(100, 100));
            arrList.Add(new Point(100, 100));

            //((Point) arrList[0]).X

            var q = from p in arrList.Cast<Point>()
                    select p;

            this.dataGridView1.DataSource = q.ToList();


           

        }

        private void button4_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {

            //I. 延遲查詢 (deferred execution)
            //定義時不會估算
            //使用時才估算


            int[] nums = new int[] { 1, 2,3,4,5,6,7,8,9,10 };

            int i = 0;
            var q = from n in nums
                    select ++i;

            //foreach 執行 Query
            foreach (var v in q)
            {
                listBox1.Items.Add(string.Format("v = {0}, i = {1}", v, i));
            }
            listBox1.Items.Add("===========================================");



            //=======================================================
            //II. 立即執行 (Immedate execution)
            //1  foreach 執行 Query
            //2  call Convert Operator :  .ToList()
            //3. Call Aggregation Operator : Min/Max

            i = 0;
            var q1 = (from n in nums
                      select ++i).ToList();

            foreach (var v in q1)
            {
                listBox1.Items.Add(string.Format("v = {0}, i = {1}", v, i));
            }  
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.productsTableAdapter1.Fill(this.northwindDataSet1.Products);
            this.dataGridView1.DataSource = this.northwindDataSet1.Products;

            var q = (from p in this.northwindDataSet1.Products
                     where !p.IsUnitPriceNull() && p.UnitPrice > 30
                     //&& p.ProductName.ToLower().StartsWith("c")
                     orderby p.UnitPrice descending
                     select p).Take(5);


            this.dataGridView1.DataSource = q.ToList();
        }

        private void button54_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5 ,6, 7,8,9,10};
            MessageBox.Show("SUM =" + nums.Sum());

            MessageBox.Show("Avg =" + nums.Average());
           
            this.productsTableAdapter1.Fill(this.northwindDataSet1.Products);
            this.dataGridView1.DataSource = this.northwindDataSet1.Products;

            MessageBox.Show(this.northwindDataSet1.Products.Average(p => p.UnitPrice).ToString("c2"));


        }

        

    }
}