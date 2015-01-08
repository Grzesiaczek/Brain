using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    abstract class AnimatedElement
    {
        protected PointF position;
        protected float radius;
        protected static Graphics graphics;
        protected static Size size;

        public void update(SizeF factor)
        {
            position = new PointF(Position.X * factor.Width, Position.Y * factor.Height);
        }

        public static Graphics Graphics
        {
            set
            {
                graphics = value;
            }
        }

        public static Size Size
        {
            set
            {
                size = value;
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
            }
        }

        public abstract PointF Position
        {
            get;
            set;
        }
    }
}
