using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class AnimatedSynapse : AnimatedElement
    {
        AnimatedElement pre;
        AnimatedElement post;

        PointF start;
        PointF end;
        Vector vector;

        SynapseState synapse;
        SynapseState duplex;

        float sin, cos;
        float dx, dy;
        

        public AnimatedSynapse(AnimatedNeuron pre, AnimatedNeuron post, Synapse synapse)
        {
            this.pre = pre;
            this.post = post;
            this.synapse = new SynapseState(synapse);

            duplex = null;
            vector = new Vector();

            pre.Output.Add(this);
            post.Input.Add(this);

            calculate();
        }

        public AnimatedSynapse(AnimatedReceptor pre, AnimatedNeuron post, Synapse synapse)
        {
            this.pre = pre;
            this.post = post;
            this.synapse = new SynapseState(synapse);

            duplex = null;
            vector = new Vector();

            pre.Output = this;
            post.Input.Add(this);

            calculate();
        }

        public AnimatedSynapse(BinaryReader reader, List<AnimatedNeuron> neurons)
        {/*
            int id = reader.ReadInt32();
            pre = neurons.Find(k => k.ID == id);

            id = reader.ReadInt32();
            post = neurons.Find(k => k.ID == id);

            graphics = g;
            duplex = reader.ReadBoolean();

            pre.Output.Add(this);
            post.Input.Add(this);
            */
            start = new PointF();
            end = new PointF();

            calculate();
        }

        public void calculate()
        {
            start = new PointF();
            end = new PointF();

            dx = post.Position.X - pre.Position.X;
            dy = post.Position.Y - pre.Position.Y;
            float r = (float)Math.Sqrt(dx * dx + dy * dy);

            cos = dx / r;
            sin = dy / r;

            start.X = pre.Position.X + pre.Radius * cos;
            start.Y = pre.Position.Y + pre.Radius * sin;

            end.X = post.Position.X - post.Radius * cos;
            end.Y = post.Position.Y - post.Radius * sin;

            dx = end.X - start.X;
            dy = end.Y - start.Y;

            Circle state = new Circle(new PointF(start.X + 0.8f * dx, start.Y + 0.8f * dy), 12);
            Circle control = new Circle(new PointF(state.Center.X - cos * 8, state.Center.Y - sin * 8), 12);
            synapse.load(state, control);

            vector.update(pre.Position, post.Position);

            if (duplex == null)
                return;

            state = new Circle(new PointF(start.X + end.X - synapse.State.X, start.Y + end.Y - synapse.State.Y), 12);
            control = new Circle(new PointF(start.X + end.X - synapse.Control.X, start.Y + end.Y - synapse.Control.Y), 12);
            duplex.load(state, control);
        }

        public void animate(int frame, float factor)
        {
            draw();

            int length = 16;
            float x = length * cos;
            float y = length * sin;
            float x1, y1;
            float x2, y2;

            Pen pen = new Pen(Brushes.OrangeRed, 6);

            if (synapse.Activity[frame - 1])
            {
                x1 = factor * dx + start.X - x;
                y1 = factor * dy + start.Y - y;
                x2 = x1 + 2 * x;
                y2 = y1 + 2 * y;

                graphics.DrawLine(pen, x1, y1, x2, y2);
            }

            if (duplex != null && duplex.Activity[frame - 1])
            {
                x1 = end.X - x - factor * dx;
                y1 = end.Y - y - factor * dy;
                x2 = x1 + 2 * x;
                y2 = y1 + 2 * y;

                graphics.DrawLine(pen, x1, y1, x2, y2);
            }
        }

        public void draw()
        {
            Pen pen = new Pen(Brushes.Gray, 3);
            graphics.DrawLine(pen, start, end);

            pen = new Pen(Brushes.DarkBlue, 1);
            graphics.DrawLine(pen, start, end);
        }

        public void draw(Graphics g)
        {
            Pen pen = new Pen(Brushes.Gray, 3);
            g.DrawLine(pen, start, end);

            pen = new Pen(Brushes.DarkBlue, 1);
            g.DrawLine(pen, start, end);
        }

        public void draw(Graphics g, float factor)
        {
            float x = start.X + factor * (end.X - start.X);
            float y = start.Y + factor * (end.Y - start.Y);
            PointF finish = new PointF(x, y);

            Pen pen = new Pen(Brushes.Gray, 3);
            g.DrawLine(pen, start, finish);

            pen = new Pen(Brushes.DarkBlue, 1);
            g.DrawLine(pen, start, finish);
        }

        public void drawState(Graphics g)
        {
            synapse.draw(g);

            if (duplex != null && duplex.Weight > 0)
                duplex.draw(g);
        }

        public void drawState(int frame)
        {
            synapse.draw(graphics, frame);

            if (duplex != null)
                duplex.draw(graphics, frame);
        }

        public void create(CreationData data)
        {
            if (data.Synapse == synapse.Synapse)
            {
                synapse.History.Add(data);
                synapse.Weight = data.Weight;
            }
            else
            {
                duplex.History.Add(data);
                duplex.Weight = data.Weight;
            } 
        }

        public void create()
        {
            synapse.create();

            if (duplex != null)
                duplex.create();
        }

        public void setDuplex(Synapse synapse)
        {
            duplex = new SynapseState(synapse);

            Circle state = new Circle(new PointF(start.X + end.X - this.synapse.State.X, start.Y + end.Y - this.synapse.State.Y), 12);
            Circle control = new Circle(new PointF(start.X + end.X - this.synapse.Control.X, start.Y + end.Y - this.synapse.Control.Y), 12);
            duplex.load(state, control);
        }

        public void save(BinaryWriter writer)
        {
            //writer.Write(pre.ID);
            //writer.Write(post.ID);
            //writer.Write(duplex);
        }

        public bool active(Point location, bool duplex)
        {
            if (duplex)
                return this.duplex.active(location);

            return synapse.active(location);
        }

        public AnimatedElement Pre
        {
            get
            {
                return pre;
            }
            set
            {
                pre = value;
            }
        }

        public AnimatedElement Post
        {
            get
            {
                return post;
            }
            set
            {
                post = value;
            }
        }

        public Vector Vector
        {
            get
            {
                return vector;
            }
            set
            {
                vector = value;
            }
        }

        public bool isDuplex()
        {
            if (duplex == null)
                return false;

            return true;
        }

        public SynapseState getState(bool duplex)
        {
            if (duplex)
                return this.duplex;

            return synapse;
        }

        public Synapse Synapse
        {
            get
            {
                return synapse.Synapse;
            }
        }

        public Synapse Duplex
        {
            get
            {
                return duplex.Synapse;
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

        public float getWeight()
        {
            float result = synapse.Weight;

            if (duplex != null)
                result += duplex.Weight;

            return result;
        }
    }
}
