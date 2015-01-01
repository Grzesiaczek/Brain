using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class BalancedReceptor
    {
        AnimatedReceptor receptor;
        PointF shift;
        PointF position;

        static float k = 80;
        int wall;

        public BalancedReceptor(AnimatedReceptor ar)
        {
            receptor = ar;
            wall = ar.getWall();
            position = new PointF(ar.Position.X, ar.Position.Y);
        }

        public void zero()
        {
            shift = new PointF(0, 0);
        }

        PointF diff(PointF P1, PointF P2)
        {
            float x = P1.X - P2.X;
            float y = P1.Y - P2.Y;

            return new PointF(x, y);
        }

        public void attract(double factor)
        {
            PointF delta = diff(position, receptor.Output.Post.Position);
            shift.X += (float)(factor * delta.X / k);
            shift.Y += (float)(factor * delta.Y / k);
        }

        public void repulse(BalancedReceptor r, double factor)
        {
            PointF delta = diff(position, r.position);

            float distance = delta.X * delta.X + delta.Y * delta.Y;
            float force = Math.Min((float)(k * k * factor / distance), 0.5f);

            shift.X += (float)(force * delta.X / Math.Sqrt(distance));
            shift.Y += (float)(force * delta.Y / Math.Sqrt(distance));
        }

        public PointF getPosition()
        {
            return position;
        }

        public float update(float factor)
        {
            if (wall == 0)
                position.X += shift.X * factor;
            else
                position.Y += shift.Y * factor;

            receptor.setPosition(position);

            if (wall == 0)
                return Math.Abs(shift.X);

            return Math.Abs(shift.Y);
        }
    }
}
