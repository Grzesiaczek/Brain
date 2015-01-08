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
        float x;
        float y;
        float angle;
        float length;
        float rotation;

        public void update(PointF p1, PointF p2)
        {
            x = p2.X - p1.X;
            y = p2.Y - p1.Y;

            length = (float)Math.Sqrt(x * x + y * y);
            angle = (float)Math.Acos(x / length);
            rotation = 0;

            if (y > 0)
                angle = -angle;
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
