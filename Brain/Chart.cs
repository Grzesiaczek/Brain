using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    class Chart : Layer
    {
        List<ChartedNeuron> neurons;

        public Chart()
        {
            neurons = new List<ChartedNeuron>();
            initializeGraphics();
        }

        public void setNeurons(List<Neuron> neurons)
        {
            int x = (int)(graphics.VisibleClipBounds.Width) - 120;
            int y = 120;

            foreach(Neuron n in neurons)
            {
                Point location = new Point(x, y);
                this.neurons.Add(new ChartedNeuron(n, this, location));
                y += 40;

                if(y > 240) break;
            }
        }

        protected override void changeSize()
        {
            Height = Parent.Height - 58;
            Width = Parent.Width - 168;
            initializeGraphics();
        }

        protected override void tick(object sender, EventArgs e)
        {
            redraw();
        }

        public void redraw()
        {
            buffer.Graphics.Clear(SystemColors.Control);

            foreach (ChartedNeuron n in neurons)
                n.draw(buffer.Graphics);

            buffer.Render(graphics);
        }
    }

    class ChartedNeuron
    {
        CheckBox checkBox;
        Neuron neuron;
        bool visible;

        public ChartedNeuron(Neuron neuron, Chart parent, Point location)
        {
            this.neuron = neuron;
            visible = false;

            checkBox = new CheckBox();
            checkBox.Location = location;
            checkBox.Text = neuron.Word;
            checkBox.CheckedChanged += new EventHandler(changeVisibility);

            parent.Controls.Add(checkBox);
        }

        public void draw(Graphics g)
        {
            if (!visible)
                return;

            int x1 = 40;
            int x2 = 48;
            float y1 = countY(neuron.Activity[0].Value);
            float y2 = countY(neuron.Activity[1].Value);

            for (int i = 2; i < 50; i++)
            {
                g.DrawLine(Pens.Red, x1, y1, x2, y2);

                x1 = x2;
                y1 = y2;

                x2 = x1 + 16;
                y2 = countY(neuron.Activity[i].Value);
            }
        }

        private void changeVisibility(object sender, EventArgs e)
        {
            visible = checkBox.Checked;
        }

        public float countY(double value)
        {
            return 240 - (float)(Math.Min(value, 2) * 100);
        }
    }
}
