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
        Neuron neuron;
        Circle circle;

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

        public AnimatedNeuron(Neuron n, PointF pos)
        {
            neuron = n;
            radius = 24;

            id = ++count;
            createCircle(pos);

            input = new List<AnimatedSynapse>();
            output = new List<AnimatedSynapse>();
        }

        public AnimatedNeuron(BinaryReader reader)
        {
            id = reader.ReadInt32();
            createCircle(new PointF(reader.ReadSingle(), reader.ReadSingle()));

            int count = reader.ReadInt32();
            List<NeuronData> data = new List<NeuronData>(count);

            neuron = new Neuron(data);
            neuron.setLength(count);

            for (int i = 0; i < count; i++)
                data.Add(new NeuronData(reader));

            input = new List<AnimatedSynapse>();
            output = new List<AnimatedSynapse>();
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

                if (Math.Sqrt(dx * dx + dy * dy) < Constant.Diameter)
                {
                    collision = true;
                    break;
                }
            }

            if (Position.X < Constant.Radius || Position.X > size.Width - Constant.Radius)
                collision = true;

            if (Position.Y < Constant.Radius || Position.Y > size.Height - Constant.Radius)
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
                Position = new PointF(original.X, original.Y);
                recalculate();
            }

            shifted = false;
        }

        public void recalculate()
        {
            foreach (AnimatedSynapse s in input)
                s.calculate();

            foreach (AnimatedSynapse s in output)
                s.calculate();
        }

        public void createCircle(PointF pos)
        {
            circle = new Circle(pos, radius);
            position = pos;
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

        public void draw()
        {
            draw((float)0);
        }

        public void draw(Graphics g)
        {
            circle.draw(g);

            if (radius == Constant.Radius && label)
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

        void drawLabel(Graphics g)
        {
            int width = neuron.Word.Length * 8 + 5;
            float x = position.X - width / 2;
            float y = position.Y + radius + 5;

            g.FillRectangle(Brushes.AliceBlue, x, y, width, 14);
            g.DrawRectangle(new Pen(SystemBrushes.ButtonFace, 2), x, y, width, 14);
            g.DrawString(neuron.Word, new Font("Miriam Fixed", 9, FontStyle.Bold), Brushes.DarkSlateBlue, x + 2, y + 2);
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
            Radius = Constant.Radius;
        }
        
        public Neuron Neuron
        {
            get
            {
                return neuron;
            }
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

        public override PointF Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                circle.update(value);
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

        public override float Radius
        {
            get
            {
                return radius;
            }
            set
            {
                radius = value;
                circle.Radius = radius;
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
