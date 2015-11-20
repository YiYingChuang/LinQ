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

            IEnumerable< IGrouping<int, int>> q = from n in nums
                                              group n by (n % 2);

            this.dataGridView1.DataSource= q.ToList();


            foreach (var group in q)
            {
                TreeNode node= this.treeView1.Nodes.Add(group.Key.ToString());
                
                foreach (var item in group)
                {
                    node.Nodes.Add(item.ToString());
                }
            }

            //============================================

            foreach (var group in q)
            {
                ListViewGroup lvGroup= this.listView1.Groups.Add(group.Key.ToString(), group.Key.ToString());   
                foreach (var item in group)
                {
                    this.listView1.Items.Add(item.ToString()).Group = lvGroup;
                }
            }


        }

        private void button7_Click(object sender, EventArgs e)
        {

            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 , 11, 101};

            var q = from n in nums
                    group n by (n % 2) into g
                    select new {MyKey = (g.Key==1?"奇數":"偶數"), MyCount= g.Count(), MyGroup = g};

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
                                              

        }

        private void button2_Click(object sender, EventArgs e)
        {

            int[] nums = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 101 };

            var q = from n in nums
                    group n by MyKey(n) into g
                    select new { MyKey =g.Key, MyCount = g.Count(), MyGroup = g };

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
            FileInfo[] files= dir.GetFiles();

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
                ListViewGroup lvGroup = this.listView1.Groups.Add(group.MyKey.ToString(),headerText);//  group.MyKey.ToString());
                foreach (var item in group.MyGroup)
                {
                    this.listView1.Items.Add(item.ToString()).Group = lvGroup;
                }
            }


        }

    

       
    }
}
