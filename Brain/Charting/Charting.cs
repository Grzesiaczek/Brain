using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Brain
{
    partial class Charting : Layer, Drawable
    {
        List<ChartedNeuron> neurons;
        QuerySequence query;
        ChartArea area;
        Brain brain;
        Control line;

        int length = 64;

        public Charting()
        {
            InitializeComponent();
            neurons = new List<ChartedNeuron>();
            Visible = false;

            area = new ChartArea();
            area.AxisX.Interval = 5;
            area.AxisX.MajorGrid.Enabled = false;
            area.BackColor = SystemColors.Control;

            area.AxisY.Interval = 1.0;
            area.AxisY.IntervalOffset = 0.2;
            area.AxisY.Minimum = -1.2;
            area.AxisY.Maximum = 2.4;

            chart.ChartAreas.Add(area);
            chart.BackColor = SystemColors.Control;

            Series series = new Series();
            series.Points.AddY(0);
            chart.Series.Add(series);
            chart.MouseClick += new MouseEventHandler(click);

            line = new Control();
            line.Visible = false;
            line.Width = 2;
            line.Height = chart.Height;
            line.BackColor = Color.ForestGreen;
            line.MouseClick += new MouseEventHandler(click);
            
            chart.Controls.Add(line);
        }

        public void loadBrain(Brain brain)
        {
            foreach (Neuron neuron in brain.Neurons)
            {
                ChartedNeuron chn = new ChartedNeuron(neuron);
                chn.showNeuron += new EventHandler(show);
                chn.hideNeuron += new EventHandler(hide);

                neurons.Add(chn);
                Controls.Add(chn);
                this.brain = brain;
            }

            Controls.SetChildIndex(chart, brain.Neurons.Count + 1);
        }

        public void addQuery(QuerySequence query)
        {
            this.query = query;
        }

        public override void resize()
        {
            Height = Parent.Height - margin.Vertical;
            Width = Parent.Width - margin.Horizontal;

            chart.Width = Width - 60;
            scrollBar.Width = chart.Width - 64;

            length = 5 * (Width / 80) + 4;
            area.AxisX.Maximum = area.AxisX.Minimum + length;
            //scrollBar.Maximum = Math.Max((brain.Length - length) / 5 + 10, 0);

            if (scrollBar.Value > scrollBar.Maximum - 9)
                scrollBar.Value = scrollBar.Maximum - 9;
        }

        #region interfejs drawable
        public override void show()
        {
            area.AxisX.Minimum = 0;
            area.AxisX.Maximum = length;

            scrollBar.Maximum = Math.Max((brain.Length - length) / 5 + 10, 0);
            base.show();

            int y = 32;

            foreach (ChartedNeuron neuron in neurons)
            {
                neuron.show();
                neuron.Top = y;
                neuron.Left = 24;
                y += 32;
            }
        }

        public override void hide()
        {
            base.hide();

            foreach (ChartedNeuron neuron in neurons)
                neuron.hide();
        }

        public override void save()
        {
            Bitmap bitmap = new Bitmap(Width, Height, CreateGraphics());
            bitmap.Save(Constant.Path + "test.png");
        }
#endregion

        void show(object sender, EventArgs e)
        {
            chart.Series.Add((Series)sender);
        }

        void hide(object sender, EventArgs e)
        {
            chart.Series.Remove((Series)sender);
        }

        private void scrollBar_ValueChanged(object sender, EventArgs e)
        {
            area.AxisX.Minimum = 5 * scrollBar.Value;
            area.AxisX.Maximum = area.AxisX.Minimum + length;
        }

        public void click(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                line.Visible = true;
                line.Left = e.X;
            }
            else
                line.Visible = false;
        }
    }
}