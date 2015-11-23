using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WinFormsChartSamples
{
    public partial class FrmMyTest : Form
    {

       static void Main()
        {
            FrmMyTest f = new FrmMyTest();
            Application.Run(f);

        }

        public FrmMyTest()
        {
            InitializeComponent();
        }
      
        private void UpdateChartSettings()
        {
            // Set series chart type
            chart1.Series["Series1"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), comboBoxChartType.Text, true);
            chart1.Series["Series2"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), comboBoxChartType.Text, true);

            // Set point labels
            if (comboBoxPointLabels.Text != "None")
            {
                chart1.Series["Series1"].IsValueShownAsLabel = true;
                chart1.Series["Series2"].IsValueShownAsLabel = true;
                if (comboBoxPointLabels.Text != "Auto")
                {
                    chart1.Series["Series1"]["LabelStyle"] = comboBoxPointLabels.Text;
                    chart1.Series["Series2"]["LabelStyle"] = comboBoxPointLabels.Text;
                }
            }
            else
            {
                chart1.Series["Series1"].IsValueShownAsLabel = false;
                chart1.Series["Series2"].IsValueShownAsLabel = false;
            }

            // Set X axis margin
            chart1.ChartAreas["Default"].AxisX.IsMarginVisible = checkBoxShowMargin.Checked;
        }
        private void FrmMyTest_Load(object sender, EventArgs e)
        {
            comboBoxChartType.SelectedIndex = 0;
            comboBoxPointLabels.SelectedIndex = 0;
            checkBoxShow3D.Checked = false;

            // Populate series data
            Random random = new Random();
            for (int pointIndex = 0; pointIndex < 10; pointIndex++)
            {
                chart1.Series["Series1"].Points.AddY(random.Next(45, 95));
                chart1.Series["Series2"].Points.AddY(random.Next(5, 65));
            }

            UpdateChartSettings();
        }


        private void comboBoxChartType_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            UpdateChartSettings();
        }

        private void checkBoxShowMargin_CheckedChanged(object sender, System.EventArgs e)
        {
            UpdateChartSettings();
        }

        private void checkBoxShow3D_CheckedChanged(object sender, System.EventArgs e)
        {
            chart1.ChartAreas[0].Area3DStyle.Enable3D = checkBoxShow3D.Checked;
            if (checkBoxShow3D.Checked)
            {
                chart1.Series["Series1"].MarkerStyle = MarkerStyle.None;
                chart1.Series["Series2"].MarkerStyle = MarkerStyle.None;
                chart1.Series["Series1"].BorderWidth = 1;
                chart1.Series["Series2"].BorderWidth = 1;
            }
            else
            {
                chart1.Series["Series1"].MarkerStyle = MarkerStyle.Circle;
                chart1.Series["Series2"].MarkerStyle = MarkerStyle.Diamond;
                chart1.Series["Series1"].BorderWidth = 3;
                chart1.Series["Series2"].BorderWidth = 3;
            }
        }
    }
}
