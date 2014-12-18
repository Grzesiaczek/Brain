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
        protected Graphics graphics;
        protected Neuron neuron;

        Circle circle;

        List<AnimatedSynapse> input;
        List<AnimatedSynapse> output;

        int id;
        bool active = false;
        bool label = true;
        bool state = true;
        int frame = 0;
        static int count = 0;

        public AnimatedNeuron() { }

        public AnimatedNeuron(Neuron n, Graphics g, PointF pos)
        {
            neuron = n;
            graphics = g;

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

        public void createCircle(PointF pos)
        {
            circle = new Circle(pos, Config.Radius);
        }

        public void animate(int number, int frame, double factor)
        {
            NeuronData data = neuron.Activity[number - 1];
            double delta = factor * data.Relaxation;

            if (factor > 0.75)
            {
                factor = 4 * (factor - 0.75);
                delta += factor * data.Impulse;
            }

            active = true;
            draw(1, data.Initial + delta);
        }

        public virtual void draw(int number)
        {
            try
            {
                draw(1, neuron.Activity[number - 1].Value);
            }
            catch (Exception) { }
        }

        protected void draw(int mode, double value)
        {
            Pen pen = new Pen(Brushes.Purple, 3);

            switch (mode)
            {
                case 2:
                    pen = new Pen(Brushes.Green, 3);
                    break;
                case 3:
                    pen = new Pen(Brushes.IndianRed, 3);
                    break;
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
                drawLabel();
        }

        void drawLabel()
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

        public void setGraphics(Graphics g)
        {
            graphics = g;
        }

        public List<NeuronData> Activity
        {
            get
            {
                return neuron.Activity;
            }
        }

        public float Radius
        {
            get
            {
                return Config.Radius;
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
