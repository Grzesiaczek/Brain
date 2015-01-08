using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    abstract class BalancedElement
    {
        protected PointF position;
        protected PointF shift;
        protected static SizeF size;

        protected PointF diff(PointF P1, PointF P2)
        {
            float x = P1.X - P2.X;
            float y = P1.Y - P2.Y;

            return new PointF(x, y);
        }

        public abstract void move(float x, float y);

        public PointF Position
        {
            get
            {
                return position;
            }
        }

        public static Size Size
        {
            set
            {
                size = value;
            }
        }
    }
}
