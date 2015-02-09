using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class AnimatedVector : AnimatedElement
    {
        #region deklaracje

        AnimatedElement start;
        AnimatedElement end;

        PointF location;

        float sin;
        float cos;

        float angle;
        float length;
        float rotation;

        #endregion

        #region logika

        public AnimatedVector(AnimatedElement start, AnimatedElement end)
        {
            this.start = start;
            this.end = end;
        }

        public override void changePosition()
        {
            location.X = end.Location.X - start.Location.X;
            location.Y = end.Location.Y - start.Location.Y;

            position.X = end.Position.X - start.Position.X;
            position.Y = end.Position.Y - start.Position.Y;

            length = (float)Math.Sqrt(position.X * position.X + position.Y * position.Y);
            
            sin = position.Y / length;
            cos = position.X / length;

            angle = (float)Math.Acos(cos);
            rotation = 0;

            if (position.X > 0)
                angle = -angle;
        }

        public void udpate()
        {
            position.X = end.Position.X - start.Position.X;
            position.Y = end.Position.Y - start.Position.Y;

            length = (float)Math.Sqrt(position.X * position.X + position.Y * position.Y);

            sin = position.Y / length;
            cos = position.X / length;

            angle = (float)Math.Acos(cos);
            rotation = 0;
        }

        public PointF getLocation(int length, bool duplex = false)
        {
            if(duplex)
                return new PointF(start.Location.X + cos * length, start.Location.Y + sin * length);

            return new PointF(end.Location.X - cos * length, end.Location.Y - sin * length);
        }

        #endregion

        #region rysowanie

        public void draw()
        {
            Pen pen = new Pen(Brushes.Gray, 3);
            graphics.DrawLine(pen, start.Location, end.Location);

            pen = new Pen(Brushes.DarkBlue, 1);
            graphics.DrawLine(pen, start.Location, end.Location);
        }

        public void draw(float factor)
        {
            float ex = start.Location.X + factor * Location.X;
            float ey = start.Location.Y + factor * Location.Y;
            PointF end = new PointF(ex, ey);

            Pen pen = new Pen(Brushes.Gray, 3);
            graphics.DrawLine(pen, start.Location, end);

            pen = new Pen(Brushes.DarkBlue, 1);
            graphics.DrawLine(pen, start.Location, end);
        }

        public void drawSignal(float factor)
        {
            Pen pen = new Pen(Brushes.OrangeRed, 6);

            float sx = start.Location.X + factor * Location.X;
            float sy = start.Location.Y + factor * Location.Y;
            float ex = sx + cos * 16;
            float ey = sy + sin * 16;

            graphics.DrawLine(pen, sx, sy, ex, ey);
        }

        #endregion

        #region właściwości

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

        public override PointF Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
            }
        }

        public override PointF Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        #endregion
    }
}
