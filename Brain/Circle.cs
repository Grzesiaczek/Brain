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
        #region deklaracje

        PointF center;
        PointF position;
        PointF border;

        float radius;
        float diameter;

        #endregion

        public Circle(PointF center, float radius)
        {
            this.center = center;
            this.radius = radius;

            position = new PointF(center.X - radius, center.Y - radius);
            border = new PointF(position.X - 1, position.Y - 1);
            diameter = 2 * radius;
        }

        #region logika

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

        #endregion

        #region rysowanie

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

        public void draw(Graphics g, float value, float change, Pen pen)
        {
            Brush inner = Brushes.LightYellow;
            Brush outer = Brushes.LightGreen;
            Brush middle = Brushes.DodgerBlue;

            float start = 1.0f - value;
            float end = start - change;

            if (change < 0)
            {
                middle = Brushes.HotPink;
                start -= change;
                end += change;
            }

            g.FillEllipse(outer, position.X, position.Y, diameter, diameter);
            g.DrawEllipse(pen, border.X, border.Y, diameter + 2, diameter + 2);

            float r1 = radius * start;
            float d1 = 2 * r1;
            float x1 = center.X - r1;
            float y1 = center.Y - r1;

            float r2 = radius * end;
            float d2 = 2 * r2;
            float x2 = center.X - r2;
            float y2 = center.Y - r2;

            g.FillEllipse(middle, x1, y1, d1, d1);
            g.FillEllipse(inner, x2, y2, d2, d2);
        }

        void drawState(Graphics g, String val)
        {
            PointF position = new PointF(center.X + radius / 24, center.Y + radius / 12);

            if (val[0] == '-')
                position.X -= 1.2f;

            g.DrawString(val, new Font("Arial", radius / 2 + 4, FontStyle.Bold), Brushes.DarkSlateGray, position, Constant.Format);
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
        
        #endregion

        #region właściwości

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

        #endregion
    }
}
