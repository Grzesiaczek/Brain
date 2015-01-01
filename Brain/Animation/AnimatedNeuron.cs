using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class AnimatedNeuron : AnimatedElement
    {
        Graphics graphics;
        Neuron neuron;

        Circle circle;
        SizeF size;

        List<AnimatedSynapse> input;
        List<AnimatedSynapse> output;

        int id;
        bool active = false;
        bool collision = false;
        bool shifted = false;

        bool label = true;
        bool state = true;

        int frame = 0;
        static int count = 0;

        public AnimatedNeuron() { }

        public AnimatedNeuron(Neuron n, Graphics g, PointF pos)
        {
            neuron = n;
            graphics = g;
            size = new SizeF(g.VisibleClipBounds.Width, g.VisibleClipBounds.Height);

            id = ++count;
            createCircle(pos);

            input = new List<AnimatedSynapse>();
            output = new List<AnimatedSynapse>();
        }

        public AnimatedNeuron(BinaryReader reader, Graphics g)
        {
            id = reader.ReadInt32();
            createCircle(new PointF(reader.ReadSingle(), reader.ReadSingle()));
            graphics = g;

            int count = reader.ReadInt32();
            List<NeuronData> data = new List<NeuronData>(count);

            neuron = new Neuron(data);
            neuron.setLength(count);

            for (int i = 0; i < count; i++)
                data.Add(new NeuronData(reader));

            input = new List<AnimatedSynapse>();
            output = new List<AnimatedSynapse>();
        }

        public void setPosition(PointF pos)
        {
            circle.update(pos);
        }

        public void setRadius(float radius)
        {
            circle.Radius = radius;
        }

        public void checkCollision(List<AnimatedNeuron> neurons)
        {
            collision = false;

            foreach (AnimatedNeuron an in neurons)
            {
                if (an == this)
                    continue;

                double dx = Position.X - an.Position.X;
                double dy = Position.Y - an.Position.Y;

                if (Math.Sqrt(dx * dx + dy * dy) < Config.Diameter)
                {
                    collision = true;
                    break;
                }
            }

            if (Position.X < Config.Radius || Position.X > size.Width - Config.Radius)
                collision = true;

            if (Position.Y < Config.Radius || Position.Y > size.Height - Config.Radius)
                collision = true;
        }

        public void activate(bool shifted)
        {
            if (shifted)
                this.shifted = true;
            else
                neuron.activate();
        }

        public void save(PointF original)
        {
            if(collision)
            {
                setPosition(new PointF(original.X, original.Y));
                recalculate();
            }

            shifted = false;
        }

        public void recalculate()
        {
            foreach (AnimatedSynapse s in input)
                s.recalculate();

            foreach (AnimatedSynapse s in output)
                s.recalculate();
        }

        public void createCircle(PointF pos)
        {
            circle = new Circle(pos, Config.Radius);
        }

        public void animate(int number, int frame, double factor)
        {
            NeuronData data = neuron.Activity[number];
            double delta = factor * data.Relaxation;

            if (factor > 0.75)
            {
                factor = 4 * (factor - 0.75);
                delta += factor * data.Impulse;
            }

            active = true;
            draw(data.Initial + delta);
        }

        public void draw(Graphics g)
        {
            circle.draw(g);

            if (Radius == Config.Radius)
                drawLabel(g);
        }

        public void draw(int number)
        {
            draw(neuron.Activity[number - 1].Value);
        }

        void draw(double value)
        {
            Pen pen = new Pen(Brushes.Purple, 3);

            if (shifted)
            {
                if(collision)
                    pen = new Pen(Brushes.IndianRed, 3);
                else
                    pen = new Pen(Brushes.Green, 3);
            }

            if (value >= 1)
            {
                Brush brush;
                value = 1;

                if (active)
                {
                    if (frame++ % 8 < 4)
                        brush = Brushes.Red;
                    else
                        brush = Brushes.Orange;
                }
                else
                    brush = Brushes.OrangeRed;

                if (state)
                    circle.draw(graphics, brush, pen, "100");
                else
                    circle.draw(graphics, brush, pen);
            }
            else
            {
                if (state)
                    circle.draw(graphics, value, pen, ((int)(value * 100)).ToString());
                else
                    circle.draw(graphics, value, pen);
            }

            if (label)
                drawLabel(graphics);
        }

        void drawLabel(Graphics graphics)
        {
            int width = neuron.Word.Length * 8 + 5;
            float x = circle.Center.X - width / 2;
            float y = circle.Center.Y + Config.Radius + 5;

            graphics.FillRectangle(Brushes.AliceBlue, x, y, width, 14);
            graphics.DrawRectangle(new Pen(SystemBrushes.ButtonFace, 2), x, y, width, 14);
            graphics.DrawString(neuron.Word, new Font("Miriam Fixed", 9, FontStyle.Bold), Brushes.DarkSlateBlue, x + 2, y + 2);
        }

        public void save(BinaryWriter writer)
        {
            writer.Write(id);
            writer.Write(circle.Center.X);
            writer.Write(circle.Center.Y);
            writer.Write(neuron.Length);

            foreach (NeuronData data in neuron.Activity)
                data.save(writer);
        }

        public bool click(PointF pos)
        {
            return circle.click(pos);
        }

        public void create()
        {
            circle.Radius = Config.Radius;
        }

        public void setSynapses(List<AnimatedSynapse> input, List<AnimatedSynapse> output)
        {
            this.input = input;
            this.output = output;

            foreach (AnimatedSynapse s in input)
            {
                s.Post = this;
                s.recalculate();
            }
                
            foreach (AnimatedSynapse s in output)
            {
                s.Pre = this;
                s.recalculate();
            }
        }

        public Neuron getNeuron()
        {
            return neuron;
        }

        public Graphics getGraphics()
        {
            return graphics;
        }

        public void updateGraphics(Graphics g)
        {
            float fx = g.VisibleClipBounds.Width / size.Width;
            float fy = g.VisibleClipBounds.Height / size.Height;

            setPosition(new PointF(Position.X * fx, Position.Y * fy));
            size = new SizeF(g.VisibleClipBounds.Width, g.VisibleClipBounds.Height);
            graphics = g;
        }

        public List<NeuronData> Activity
        {
            get
            {
                return neuron.Activity;
            }
        }

        public String Name
        {
            get
            {
                return neuron.Word;
            }
        }

        public float Radius
        {
            get
            {
                return circle.Radius;
            }
        }

        public PointF Position
        {
            get
            {
                return circle.Center;
            }
            set
            {
                circle.Center = value;
            }
        }

        public List<AnimatedSynapse> Input
        {
            get
            {
                return input;
            }
            set
            {
                input = value;
            }
        }

        public List<AnimatedSynapse> Output
        {
            get
            {
                return output;
            }
            set
            {
                output = value;
            }
        }

        public bool Label
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
            }
        }

        public bool State
        {
            get
            {
                return state;
            }
            set
            {
                state = value;
            }
        }

        public int ID
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
    }
}
