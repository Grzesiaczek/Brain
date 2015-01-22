using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class BalancedSynapse : BalancedElement
    {
        AnimatedSynapse synapse;

        BalancedElement pre;
        BalancedElement post;

        static float k = 100;

        public BalancedSynapse(AnimatedSynapse synapse, Dictionary<AnimatedElement, BalancedElement> map)
        {
            this.synapse = synapse;
            pre = map[synapse.Pre];
            post = map[synapse.Post]; 
        }

        public void repulse(BalancedNeuron neuron, float factor)
        {
            if (neuron == pre || neuron == post)
                return;

            PointF pos = new PointF(neuron.Position.X - synapse.Pre.Position.X, neuron.Position.Y - synapse.Pre.Position.Y);

            float a = synapse.Vector.Y;
            float b = synapse.Vector.X;
            float c = b * pos.X + a * pos.Y;
            float x, y;
            float eq; // punkt równowagi

            if(a == 0)
            {
                x = c / b;
                y = 0;
                eq = x / b;
            }
            else if (b == 0)
            {
                x = 0;
                y = c / a;
                eq = y / a;
            }
            else
            {
                x = c * b / (synapse.Vector.Length * synapse.Vector.Length);
                eq = x / b;
                y = a * eq;
            }

            if (eq < 0 || eq > 1)
                return;

            float force = 0;
            float tau = 0;
            float distance = Math.Abs(b * pos.Y - a * pos.X) / synapse.Vector.Length;

            if (distance < Constant.Radius)
            {
                tau = k / Constant.Diameter;
                force = 3 * factor * tau * tau;
            }
            else if (distance < Constant.Diameter)
            {
                tau = k / Constant.Radius;
                force = factor * tau * tau * (2.5f * Constant.Radius - distance) / Constant.Diameter;
            }
            else
            {
                tau = k / distance;
                force = factor * tau * tau;
            }

            PointF shift = new PointF(force * (pos.X - x) / distance, force * (pos.Y - y) / distance);
            neuron.move(shift.X, shift.Y);

            eq = - eq;
            post.move(shift.X * eq, shift.Y * eq);

            eq = -1 - eq;
            pre.move(shift.X * eq, shift.Y * eq);
        }

        public void rotate()
        {
            if (pre is BalancedReceptor)
                return;

            if (Math.Abs(synapse.Vector.Rotation) < 0.01)
            {
                synapse.Vector.Rotation = 0;
                return;
            }

            float shift = synapse.Vector.Rotation * synapse.Vector.Length / 50;
            float x = (float)(shift * Math.Sin(synapse.Vector.Angle));
            float y = (float)(shift * Math.Cos(synapse.Vector.Angle));

            post.move(x, y);
            pre.move(-x, -y);
        }

        public override void move(float x, float y)
        {/*
            shift.X += value.X;
            shift.Y += shift.Y;*/
        }

        public AnimatedSynapse Synapse
        {
            get
            {
                return synapse;
            }
        }
    }
}
