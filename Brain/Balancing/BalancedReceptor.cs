using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class BalancedReceptor : BalancedElement
    {
        AnimatedReceptor receptor;

        static float k = 80;
        static float k2 = k * k;
        int wall;

        public BalancedReceptor(AnimatedReceptor ar)
        {
            receptor = ar;
            wall = ar.getWall();
            shift = new PointF(0, 0);
            position = new PointF(ar.Position.X, ar.Position.Y);
        }

        public void attract(float factor)
        {
            PointF delta = diff(position, receptor.Output.Post.Position);
            float x = (float)(factor * delta.X * Math.Abs(delta.X) / k2);
            float y = (float)(factor * delta.Y * Math.Abs(delta.Y) / k2);

            shift.X += x;
            shift.Y += y;
        }

        public void repulse(BalancedReceptor r, float factor)
        {
            PointF delta = diff(position, r.position);

            float distance = delta.X * delta.X + delta.Y * delta.Y;
            float force = Math.Min((float)(k2 * factor / distance), 0.5f);

            shift.X += (float)(force * delta.X / Math.Sqrt(distance));
            shift.Y += (float)(force * delta.Y / Math.Sqrt(distance));
        }

        public override void move(float x, float y)
        {
            shift.X += x;
            shift.Y += y;
        }

        public float update(float factor)
        {
            if (wall == 0)
                position.X += shift.X * factor;
            else
                position.Y += shift.Y * factor;

            receptor.Position = position;
            shift = new PointF(0, 0);

            if (wall == 0)
                return Math.Abs(shift.X);

            return Math.Abs(shift.Y);
        }
    }
}
