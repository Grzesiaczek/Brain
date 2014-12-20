using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class GraphDrawing
    {
        List<DrawnNeuron> neurons;
        List<DrawnReceptor> receptors;
        List<AnimatedSynapse> synapses;

        System.Windows.Forms.Timer timer;
        Graphics buffer;

        float alpha;
        float beta;
        float delta;
        float step;

        int steps;

        public GraphDrawing(List<AnimatedNeuron> neurons, List<AnimatedSynapse> synapses, List<AnimatedReceptor> receptors, int steps)
        {
            this.neurons = new List<DrawnNeuron>();
            this.receptors = new List<DrawnReceptor>();

            foreach (AnimatedNeuron neuron in neurons)
                this.neurons.Add(new DrawnNeuron(neuron));

            foreach (AnimatedReceptor receptor in receptors)
                this.receptors.Add(new DrawnReceptor(receptor));

            this.synapses = synapses;
            this.steps = steps;

            alpha = -0.2f;
            beta = 2.0f;
            step = 0.2f;
        }

        public void animate(Graphics g)
        {
            buffer = g;

            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(tick);
            timer.Interval = 25;
            timer.Start();
        }

        void tick(object sender, EventArgs e)
        {
            int length = steps;

            if(delta < 0.5)
                length = (int)(steps / Math.Max(0.1f, delta) / 2);

            for (int i = 0; i < length; i++)
                calculate();

            if (Math.Abs(delta) < 0.01)
            {
                timer.Stop();
                balanceFinished(this, new EventArgs());
            }

            foreach(DrawnNeuron n in neurons)
                n.draw(1);

            foreach (AnimatedSynapse s in synapses)
                s.recalculate();

            drawing(this, new EventArgs());
        }

        void calculate()
        {
            delta = 0;

            foreach (DrawnNeuron n1 in neurons)
            {
                n1.zero();
                n1.repulse(buffer.VisibleClipBounds.Size, beta / (2 * neurons.Count));

                foreach (DrawnNeuron n2 in neurons)
                {
                    if (n1 == n2)
                        continue;

                    n1.repulse(n2.getPosition(), beta);
                }
                
                foreach(DrawnReceptor r in receptors)
                    n1.repulse(r.getPosition(), beta / 2);

                foreach (AnimatedSynapse s in n1.getNeuron().Output)
                    n1.attract(s.Post, alpha + alpha * s.getWeight());

                foreach (AnimatedSynapse s in n1.getNeuron().Input)
                    n1.attract(s.Pre, alpha + alpha * s.getWeight());
            }

            foreach (DrawnReceptor r1 in receptors)
            {
                r1.zero();
                r1.attract(alpha);

                foreach (DrawnReceptor r2 in receptors)
                {
                    if (r1 == r2)
                        continue;

                    r1.repulse(r2, beta);
                }
            }

            foreach (DrawnNeuron neuron in neurons)
                delta += neuron.update(step);

            foreach (DrawnReceptor r in receptors)
                delta += r.update(step);
        }

        public event EventHandler drawing;
        public event EventHandler balanceFinished;
    }

    class DrawnNeuron
    {
        AnimatedNeuron neuron;
        PointF shift;
        PointF position;

        static float k = 120;

        public DrawnNeuron(AnimatedNeuron neuron)
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

            if(n is AnimatedReceptor)
            {
                shift.X += (float)(factor * delta.X * Math.Abs(delta.X) / (k * k));
                shift.Y += (float)(factor * delta.Y * Math.Abs(delta.Y) / (k * k));
            }
        }

        public void repulse(PointF pos, double factor)
        {
            PointF delta = diff(position, pos);
            float distance = delta.X * delta.X + delta.Y * delta.Y;

            if (distance == 0)
            {
                shift.X += 1.5f;
                return;
            }

            float force = (float)(k * k * factor / distance);

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

    class DrawnReceptor
    {
        AnimatedReceptor receptor;
        PointF shift;
        PointF position;

        static float k = 80;
        int wall;

        public DrawnReceptor(AnimatedReceptor ar)
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

        public void repulse(DrawnReceptor r, double factor)
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
            if(wall == 0)
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