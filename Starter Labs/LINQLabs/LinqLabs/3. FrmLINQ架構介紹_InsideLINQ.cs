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
            //List<Array[]> arrlist = new List<Array[]>();
            
            //arrlist.Add(new Point(100, 100));
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.productsTableAdapter1.Fill(this.northwindDataSet1.Products);
            

            var q = (from p in this.northwindDataSet1.Products
                     where !p.IsUnitPriceNull() && p.UnitPrice > 30 
                            //&& p.ProductName.ToUpper().StartsWith("C")
                     orderby p.UnitPrice descending
                     select p).Take(10);

            this.dataGridView1.DataSource = q.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            MessageBox.Show("Sum=" +  nums.Sum());
            MessageBox.Show("Avg=" + nums.Average());

            this.productsTableAdapter1.Fill(this.northwindDataSet1.Products);
            this.dataGridView1.DataSource = this.northwindDataSet1.Products;
            MessageBox.Show(this.northwindDataSet1.Products.Average(p => p.UnitPrice).ToString("c2"));
        }

        

    }
}