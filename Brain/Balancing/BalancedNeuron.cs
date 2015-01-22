using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class BalancedNeuron : BalancedElement
    {
        AnimatedNeuron neuron;
        List<Vector> input;
        List<Vector> output;
        List<Vector> vectors;

        static float k = 320;

        public BalancedNeuron(AnimatedNeuron neuron)
        {
            shift = new PointF(0, 0);
            this.neuron = neuron;
            position = new PointF(neuron.Position.X, neuron.Position.Y);

            input = new List<Vector>();
            output = new List<Vector>();
            vectors = new List<Vector>();

            foreach (AnimatedSynapse synapse in neuron.Input)
                input.Add(synapse.Vector);

            foreach (AnimatedSynapse synapse in neuron.Output)
                output.Add(synapse.Vector);
        }

        public void draw(int frame)
        {
            neuron.draw(frame);
        }

        public void attract(AnimatedElement n, float factor)
        {
            PointF delta = diff(position, n.Position);
            shift.X += (float)(factor * delta.X / k);
            shift.Y += (float)(factor * delta.Y / k);

            if (n is AnimatedReceptor)
            {
                shift.X += (float)(8 * factor * delta.X * Math.Abs(delta.X * delta.X) / (k * k * k));
                shift.Y += (float)(8 * factor * delta.Y * Math.Abs(delta.Y * delta.Y) / (k * k * k));
            }
        }

        public void repulse(PointF pos, float factor)
        {
            PointF delta = diff(position, pos);
            float distance = delta.X * delta.X + delta.Y * delta.Y;

            if (distance < 1)
                distance = 1;

            float force = Math.Min((float)(k * k * factor / distance), 100);

            shift.X += (float)(force * delta.X / Math.Sqrt(distance));
            shift.Y += (float)(force * delta.Y / Math.Sqrt(distance));
        }

        public void repulse(float factor)
        {
            PointF sub = new PointF(size.Width - position.X, size.Height - 10 - position.Y);

            shift.X += 4 * (float)(k * k * factor) * (1 / (position.X * position.X) - 1 / (sub.X * sub.X));
            shift.Y += 4 * (float)(k * k * factor) * (1 / (position.Y * position.Y) - 1 / (sub.Y * sub.Y));
        }

        public void rotate()
        {
            int count = input.Count + output.Count;

            foreach(Vector v1 in input)
            {
                float rotation = 0;

                foreach(Vector v2 in input)
                {
                    if (v1 == v2)
                        continue;

                    rotation += diff(v1.Angle, v2.Angle);
                }

                foreach (Vector v2 in output)
                {
                    float angle = v2.Angle;

                    if (angle < Math.PI)
                        angle += (float)Math.PI;
                    else
                        angle -= (float)Math.PI;

                    rotation += diff(v1.Angle, angle);
                }

                v1.Rotation += rotation / count;
            }

            foreach (Vector v1 in output)
            {
                float rotation = 0;

                foreach (Vector v2 in output)
                {
                    if (v1 == v2)
                        continue;

                    rotation += diff(v1.Angle, v2.Angle);
                }

                foreach (Vector v2 in input)
                {
                    float angle = v2.Angle;

                    if (angle < Math.PI)
                        angle += (float)Math.PI;
                    else
                        angle -= (float)Math.PI;

                    rotation += diff(v1.Angle, angle);
                }

                v1.Rotation += rotation / count;
            }
        }

        float diff(float a1, float a2)
        {
            float angle = (float)(Math.PI + a1 - a2);
            float factor = 0.2f;
            angle = a2 - a1;
            float result = 0;

            if (angle < 0)
                angle += (float)(2 * Math.PI);

            angle -= (float)Math.PI;

            if(angle < 0)
            {
                if (angle < -3)
                    angle = -3;

                result = (float)(factor / Math.Pow(Math.PI - angle, 2));
            }
            else
            {
                if (angle > 3)
                    angle = 3;

                result = (float)(-factor / Math.Pow(Math.PI + angle, 2));
            }

            return result;
        }

        public override void move(float x, float y)
        {
            shift.X += x;
            shift.Y += y;
        }

        public float update(float factor)
        {
            position.X += Math.Min(shift.X * factor, 10);
            position.Y += Math.Min(shift.Y * factor, 10);
            neuron.Position = position;

            float result = Math.Abs(shift.X) + Math.Abs(shift.Y);
            shift = new PointF(0, 0);

            foreach (Vector vector in input)
                vector.Rotation = 0;

            return result;
        }

        public AnimatedNeuron Neuron
        {
            get
            {
                return neuron;
            }
        }
    }
}
