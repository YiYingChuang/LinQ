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

namespace LinqLabs
{
    public partial class FrmRelation_Tool : Form
    {
        public FrmRelation_Tool()
        {
            InitializeComponent();
        }
        global::NorthwindEntityModel.northwindEntities db = new northwindEntities();
        private void FrmRelation_Tool_Load(object sender, EventArgs e)
        {
            this.categoryBindingSource.DataSource = db.Categories.ToList();
        }

        private void categoryBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            //((Category)this.categoryBindingSource.Current).Products 的 Products 是導覽屬性
            this.dataGridView1.DataSource = ((Category)this.categoryBindingSource.Current).Products.ToList();
        }
    }
}
