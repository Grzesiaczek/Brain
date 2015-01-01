using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class BalancedNeuron
    {
        AnimatedNeuron neuron;
        PointF shift;
        PointF position;

        static float k = 250;

        public BalancedNeuron(AnimatedNeuron neuron)
        {
            this.neuron = neuron;
            position = new PointF(neuron.Position.X, neuron.Position.Y);
        }

        public void draw(int frame)
        {
            neuron.draw(frame);
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

        public void attract(AnimatedElement n, double factor)
        {
            PointF delta = diff(position, n.Position);
            shift.X += (float)(factor * delta.X / k);
            shift.Y += (float)(factor * delta.Y / k);

            if (n is AnimatedReceptor)
            {
                shift.X += (float)(factor * delta.X * Math.Abs(delta.X) / (k * k));
                shift.Y += (float)(factor * delta.Y * Math.Abs(delta.Y) / (k * k));
            }
        }

        public void repulse(PointF pos, double factor)
        {
            PointF delta = diff(position, pos);
            float distance = delta.X * delta.X + delta.Y * delta.Y;

            if (distance < 1)
                distance = 1;

            float force = Math.Min((float)(k * k * factor / distance), 100);

            shift.X += (float)(force * delta.X / Math.Sqrt(distance));
            shift.Y += (float)(force * delta.Y / Math.Sqrt(distance));
        }

        public void repulse(SizeF size, double factor)
        {
            PointF sub = new PointF(size.Width - position.X, size.Height - 10 - position.Y);

            shift.X += 4 * (float)(k * k * factor) * (1 / (position.X * position.X) - 1 / (sub.X * sub.X));
            shift.Y += 4 * (float)(k * k * factor) * (1 / (position.Y * position.Y) - 1 / (sub.Y * sub.Y));
        }

        public float update(float factor)
        {
            position.X += shift.X * factor;
            position.Y += shift.Y * factor;
            neuron.setPosition(position);

            float result = Math.Abs(shift.X);
            result += Math.Abs(shift.Y);
            return result;
        }

        public PointF getPosition()
        {
            return position;
        }

        public AnimatedNeuron getNeuron()
        {
            return neuron;
        }
    }
}
