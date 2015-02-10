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
        #region deklaracje

        CreationFrame frame;
        Synapse synapse;
        Color background;

        float start;
        float finish;
        float change;
        float step;

        #endregion

        #region logika

        public CreationData(Synapse synapse, CreationFrame frame, float change, float start, float finish)
        {
            this.synapse = synapse;
            this.frame = frame;
            this.change = change;
            this.start = start;
            this.finish = finish;

            Height = 35;
            Width = 160;
            initializeGraphics();

            MouseEnter += new EventHandler(mouseEnter);
            MouseLeave += new EventHandler(mouseLeave);
            background = SystemColors.Control;
        }

        protected override void tick(object sender, EventArgs e)
        {
            Graphics g = buffer.Graphics;
            g.Clear(background);

            Pen pen = new Pen(Brushes.DarkSlateGray, 2);
            float y = 14;

            Circle left = new Circle(new PointF(64, y), 12);
            Circle right = new Circle(new PointF(100, y), 12);

            left.draw(g, start, finish - start, pen);
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

            g.DrawString(frame.Frame.ToString(), new Font("Calibri", 14, FontStyle.Bold), Brushes.SaddleBrown, 28, y, format);
            g.DrawString(change.ToString(), new Font("Arial", 12, FontStyle.Bold), brush, 136, y, format);

            buffer.Render(graphics);
        }

        private void mouseEnter(object sender, EventArgs e)
        {
            background = SystemColors.ControlLight;
        }

        private void mouseLeave(object sender, EventArgs e)
        {
            background = SystemColors.Control;
        }

        #endregion

        #region właściwości

        public Synapse Synapse
        {
            get
            {
                return synapse;
            }
        }

        public float Change
        {
            get
            {
                return change;
            }
        }

        public int Frame
        {
            get
            {
                return frame.Frame;
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
            set
            {
                start = value;
            }
        }

        public float Weight
        {
            get
            {
                return finish;
            }
            set
            {
                finish = value;
            }
        }

        #endregion
    }
}
