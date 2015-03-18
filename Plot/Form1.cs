using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;

namespace Plot
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var plot1 = new OxyPlot.WindowsForms.PlotView();
            this.Controls.Add(plot1);
            plot1.Dock = DockStyle.Fill;

            var myModel = new PlotModel { Title = "Example 1" };
            myModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
            plot1.Model = myModel;
        }
    }
}
