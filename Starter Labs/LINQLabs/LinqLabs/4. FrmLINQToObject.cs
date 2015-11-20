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

            var q = from n in f

            foreach (var groups in q)
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
                ListViewGroup lvgroup = this.listView1.Groups.Add(groups.Key.ToString(), groups.Key.ToString());
                foreach (var item in groups)
                {
                    this.listView1.Items.Add(item.ToString()).Group = lvgroup;
                }

            }
        }

       

    

       
    }
}
