using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class AnimatedSynapse
    {
        AnimatedElement pre;
        AnimatedElement post;

        Synapse synapse;
        Synapse duplex;

        PointF start;
        PointF end;

        Circle sState;
        Circle sControl;

        Circle dState;
        Circle dControl;

        Graphics graphics;

        float sin, cos;
        float dx, dy;

        public AnimatedSynapse(AnimatedNeuron pre, AnimatedNeuron post, Synapse synapse, Graphics g)
        {
            this.pre = pre;
            this.post = post;
            this.synapse = synapse;

            graphics = g;
            duplex = null;

            pre.Output.Add(this);
            post.Input.Add(this);

            initialize();
        }

        public AnimatedSynapse(AnimatedReceptor pre, AnimatedNeuron post, Synapse synapse, Graphics g)
        {
            this.pre = pre;
            this.post = post;
            this.synapse = synapse;

            graphics = g;
            duplex = null;

            pre.Output = this;
            post.Input.Add(this);

            initialize();
        }

        public AnimatedSynapse(BinaryReader reader, List<AnimatedNeuron> neurons, Graphics g)
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

            initialize();
        }

        void initialize()
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

            sState = new Circle(new PointF(start.X + 0.8f * dx, start.Y + 0.8f * dy), 12);
            sControl = new Circle(new PointF(sState.Center.X - cos * 8, sState.Center.Y - sin * 8), 12);

            if (duplex == null)
                return;

            dState = new Circle(new PointF(start.X + end.X - sState.Center.X, start.Y + end.Y - sState.Center.Y), 12);
            dControl = new Circle(new PointF(start.X + end.X - sControl.Center.X, start.Y + end.Y - sControl.Center.Y), 12);
        }

        public void recalculate()
        {
            initialize();
        }

        public void animate(int frame, float factor)
        {
            draw(frame);

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

        public void draw(int frame)
        {
            Pen pen = new Pen(Brushes.Gray, 3);
            graphics.DrawLine(pen, start, end);

            pen = new Pen(Brushes.DarkBlue, 1);
            graphics.DrawLine(pen, start, end);
        }

        public void drawState(int frame)
        {
            Brush brush = Brushes.LightYellow;
            Pen pen = new Pen(Brushes.DarkSlateGray, 2);

            if (synapse.Activity[frame - 1])
                brush = Brushes.Red;

            sControl.draw(graphics, brush, pen);
            sState.draw(graphics, synapse.Weight, pen);

            if (duplex == null)
                return;

            dControl.draw(graphics, brush, pen);
            dState.draw(graphics, synapse.Weight, pen);
        }

        public void setDuplex(Synapse synapse)
        {
            duplex = synapse;

            dState = new Circle(new PointF(start.X + end.X - sState.Center.X, start.Y + end.Y - sState.Center.Y), 12);
            dControl = new Circle(new PointF(start.X + end.X - sControl.Center.X, start.Y + end.Y - sControl.Center.Y), 12);
        }

        public void save(BinaryWriter writer)
        {
            //writer.Write(pre.ID);
            //writer.Write(post.ID);
            //writer.Write(duplex);
        }

        public void updateGraphics(Graphics g)
        {
            initialize();
            graphics = g;
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

        public float getWeight()
        {
            float result = synapse.Weight;

            if (duplex != null)
                result += duplex.Weight;

            return result;
        }
    }
}
