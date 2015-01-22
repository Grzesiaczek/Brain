using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Brain
{
    class ChartedNeuron : Layer
    {
        Neuron neuron;
        Series series;

        Brush fontColor;
        Color background;
        Color light;

        Pen border;
        Pen active;
        Pen inactive;

        Rectangle rect;
        Rectangle normal;
        Rectangle pushed;

        static Color[] palette = { Color.Red, Color.Purple, Color.Orchid, Color.BlueViolet, Color.Goldenrod, Color.DarkGreen, Color.Maroon, Color.Navy,
                                     Color.HotPink, Color.Firebrick, Color.Crimson, Color.Indigo, Color.Khaki, Color.Lavender};
        static int index = 0;
        bool visible;

        public ChartedNeuron(Neuron neuron)
        {
            this.neuron = neuron;
            visible = false;

            series = new Series();
            series.Name = neuron.Name;
            series.ChartType = SeriesChartType.Line;

            MouseClick += new MouseEventHandler(click);
            MouseDoubleClick += new MouseEventHandler(click);
            MouseEnter += new EventHandler(mouseOn);
            MouseLeave += new EventHandler(mouseOff);

            Height = 20;
            Width = 60;

            normal = new Rectangle(-1, -1, Width, Height);
            pushed = new Rectangle(0, 0, Width, Height);

            active = new Pen(Brushes.Azure, 2);
            inactive = new Pen(Brushes.Purple, 2);
        }

        public override void show()
        {
            series.Points.Clear();
            series.Points.AddXY(0, 0);
            series.Color = palette[index];
            series.BorderWidth = 2;
            light = palette[index];

            visible = false;
            index = (index + 1) % palette.Length;

            background = SystemColors.ActiveBorder;
            fontColor = Brushes.DarkSlateBlue;

            border = inactive;
            rect = normal;

            double x = 0;

            foreach(NeuronData data in neuron.Activity)
            {
                if (data.Impulse == 0)
                {
                    if(data.Initial == 0)
                    {
                        series.Points.AddXY(++x, data.Value);
                        continue;
                    }

                    double basis = data.Value / data.Initial;

                    for (int i = 1; i < 5; i++)
                    {
                        double factor = 0.2 * i;
                        series.Points.AddXY(x + factor, data.Initial * Math.Pow(basis, factor));
                    }

                    series.Points.AddXY(++x, data.Value);
                }
                else
                {
                    double diff = data.Value - data.Initial;

                    for (int i = 1; i < 5; i++)
                    {
                        double factor = 0.2 * i;
                        series.Points.AddXY(x + factor, data.Initial + factor * diff);
                    }

                    if (data.Value < -0.9 * Constant.Beta)
                        series.Points.AddXY(++x - 0.04, data.Value * 0.96);
                    else
                        series.Points.AddXY(++x, data.Value);
                }
            }

            base.show();
        }

        public override void hide()
        {
            hideNeuron(series, null);
            base.hide();
        }

        void click(object sender, EventArgs e)
        {
            if (visible)
            {
                background = SystemColors.ActiveBorder;
                fontColor = Brushes.DarkSlateBlue;

                rect = normal;
                visible = false;
                hideNeuron(series, null);
            }
            else
            {
                background = light;
                fontColor = Brushes.AliceBlue;

                rect = pushed;
                visible = true;
                showNeuron(series, null);
            }
        }

        protected override void tick(object sender, EventArgs e)
        {
            Graphics g = buffer.Graphics;

            g.Clear(background);
            g.DrawRectangle(border, rect);
            g.DrawString(neuron.Word, SystemFonts.DialogFont, fontColor, rect, Constant.Format);

            buffer.Render(graphics);
        }

        void mouseOn(object sender, EventArgs e)
        {
            series.BorderWidth = 4;
            border = active;
        }

        void mouseOff(object sender, EventArgs e)
        {
            series.BorderWidth = 2;
            border = inactive;
        }

        public event EventHandler showNeuron;
        public event EventHandler hideNeuron;
    }
}
