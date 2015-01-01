using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class Circle
    {
        PointF center;
        PointF position;
        PointF border;

        float radius;
        float diameter;

        public Circle(PointF center, float radius)
        {
            this.center = center;
            this.radius = radius;

            position = new PointF(center.X - radius, center.Y - radius);
            border = new PointF(position.X - 1, position.Y - 1);
            diameter = 2 * radius;
        }

        public bool click(PointF pos)
        {
            double x = pos.X - center.X;
            double y = pos.Y - center.Y;

            if (Math.Sqrt(x * x + y * y) < radius)
                return true;

            return false;
        }

        public void update(PointF center)
        {
            this.center = center;
            position = new PointF(center.X - radius, center.Y - radius);
            border = new PointF(position.X - 1, position.Y - 1);
        }

        public void draw(Graphics g, double value, Pen pen, String val)
        {
            draw(g, value, pen);
            drawState(g, val);
        }

        public void draw(Graphics g, Brush brush, Pen pen, String val)
        {
            draw(g, brush, pen);
            drawState(g, val);
        }

        public void draw(Graphics g, double value, Pen pen)
        {
            Brush inner = Brushes.LightYellow;
            Brush outer = Brushes.LightGreen;

            if (value < 0)
            {
                value = -value;
                inner = Brushes.LightSkyBlue;
                outer = Brushes.LightYellow;
            }

            g.FillEllipse(outer, position.X, position.Y, diameter, diameter);
            g.DrawEllipse(pen, border.X, border.Y, diameter + 2, diameter + 2);

            float r = radius * (1 - (float)value);
            float d = 2 * r;
            float x = center.X - r;
            float y = center.Y - r;

            g.FillEllipse(inner, x, y, d, d);
        }

        void drawState(Graphics g, String val)
        {
            PointF position = new PointF((float)(center.X + 0.6), center.Y + 1);

            if (val[0] == '-')
                position.X -= 1.2f;

            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            g.DrawString(val, new Font("Arial", Config.Diameter / 4 + 3, FontStyle.Bold), Brushes.DarkSlateGray, position, format);
        }

        public void draw(Graphics g, Brush brush, Pen pen)
        {
            g.FillEllipse(brush, position.X, position.Y, diameter, diameter);
            g.DrawEllipse(pen, border.X, border.Y, diameter + 2, diameter + 2);
        }

        public void draw(Graphics g)
        {
            g.FillEllipse(Brushes.LightYellow, position.X, position.Y, diameter, diameter);
            g.DrawEllipse(Pens.Purple, border.X, border.Y, diameter + 2, diameter + 2);
        }

        public PointF Center
        {
            get
            {
                return center;
            }
            set
            {
                center = value;
            }
        }

        public float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
                diameter = 2 * radius;

                position = new PointF(center.X - radius, center.Y - radius);
                border = new PointF(position.X - 1, position.Y - 1);
            }
        }
    }
}
