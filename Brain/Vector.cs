using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class Vector
    {
        PointF start;
        PointF end;

        float x;
        float y;

        float sin;
        float cos;

        float angle;
        float length;
        float rotation;

        public void update(PointF p1, PointF p2)
        {
            start = p1;
            end = p2;

            x = p2.X - p1.X;
            y = p2.Y - p1.Y;

            length = (float)Math.Sqrt(x * x + y * y);
            
            sin = y / length;
            cos = x / length;

            angle = (float)Math.Acos(cos);
            rotation = 0;

            if (y > 0)
                angle = -angle;
        }

        public void draw(Graphics g)
        {
            Pen pen = new Pen(Brushes.Gray, 3);
            g.DrawLine(pen, start, end);

            pen = new Pen(Brushes.DarkBlue, 1);
            g.DrawLine(pen, start, end);
        }

        public void draw(Graphics g, float factor)
        {
            float ex = start.X + factor * x;
            float ey = start.Y + factor * y;
            PointF end = new PointF(ex, ey);

            Pen pen = new Pen(Brushes.Gray, 3);
            g.DrawLine(pen, start, end);

            pen = new Pen(Brushes.DarkBlue, 1);
            g.DrawLine(pen, start, end);
        }

        public void drawSignal(Graphics g, float factor)
        {
            Pen pen = new Pen(Brushes.OrangeRed, 6);

            float sx = start.X + factor * x;
            float sy = start.Y + factor * y;
            float ex = sx + cos * 16;
            float ey = sy + sin * 16;

            g.DrawLine(pen, sx, sy, ex, ey);
        }

        public PointF getPoint(PointF point, int length)
        {
            return new PointF(point.X + cos * length, point.Y + sin * length);
        }

        public PointF Start
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

        public PointF End
        {
            get
            {
                return end;
            }
            set
            {
                end = value;
            }
        }

        public float X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }

        public float Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }

        public float Angle
        {
            get
            {
                return angle;
            }
            set
            {
                angle = value;
            }
        }

        public float Length
        {
            get
            {
                return length;
            }
            set
            {
                length = value;
            }
        }

        public float Rotation
        {
            get
            {
                return rotation;
            }
            set
            {
                rotation = value;
            }
        }
    }
}
