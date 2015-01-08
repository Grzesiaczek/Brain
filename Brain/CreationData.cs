using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    class CreationData : Layer
    {
        Synapse synapse;
        Color background;
        int frame;

        float start;
        float finish;
        float step;

        public CreationData(Synapse synapse, int frame, float start, float finish) : base(null)
        {
            this.synapse = synapse;
            this.frame = frame;
            this.start = start;
            this.finish = finish;

            Height = 35;
            Width = 160;

            MouseEnter += new EventHandler(mouseEnter);
            MouseLeave += new EventHandler(mouseLeave);
            background = SystemColors.Control;
        }

        public void redraw()
        {
            Graphics g = buffer.Graphics;
            g.Clear(background);

            Pen pen = new Pen(Brushes.DarkSlateGray, 2);
            float y = 14;

            Circle left = new Circle(new PointF(64, y), 12);
            Circle right = new Circle(new PointF(100, y), 12);

            left.draw(g, start, pen);
            right.draw(g, finish, pen);

            int change = (int)((finish - start) * 100);
            Brush brush = Brushes.Red;
            y += 2;

            if (change > 0)
                brush = Brushes.Green;
            else
                change = -change;

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            g.DrawString(frame.ToString(), new Font("Calibri", 15, FontStyle.Bold), Brushes.SaddleBrown, 28, y, format);
            g.DrawString(change.ToString(), new Font("Arial", 12, FontStyle.Bold), brush, 136, y, format);

            buffer.Render(graphics);
        }

        public override void resize()
        {
            initializeGraphics();
        }

        protected override void tick(object sender, EventArgs e)
        {
            redraw();
        }

        private void mouseEnter(object sender, EventArgs e)
        {
            background = SystemColors.ControlLight;
        }

        private void mouseLeave(object sender, EventArgs e)
        {
            background = SystemColors.Control;
        }

        public Synapse Synapse
        {
            get
            {
                return synapse;
            }
        }

        public float Step
        {
            get
            {
                return step;
            }
            set
            {
                step = value;
            }
        }

        public float Start
        {
            get
            {
                return start;
            }
        }

        public float Weight
        {
            get
            {
                return finish;
            }
        }
    }
}
