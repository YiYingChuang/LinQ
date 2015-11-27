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
namespace LINQ
{
    public partial class FrmLINQToDataSet : Form
    {
        public FrmLINQToDataSet()
        {
            InitializeComponent();
        }
        northwindEntities db = new northwindEntities();
        private void button14_Click(object sender, EventArgs e)
        {
            //var q = from n in db.Orders
            //        group n by n.OrderDate.Value.Year into o
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
