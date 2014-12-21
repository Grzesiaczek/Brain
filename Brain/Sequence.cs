using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    class Sequence : Layer
    {
        //List<SequenceElement> elements;

        List<SequenceNeuron> neurons;
        List<SequenceNeuron> sequence;

        int frame;
        bool animation;
        bool disappear;

        public Sequence(GroupBox groupBox) : base(groupBox)
        {
            //elements = new List<SequenceElement>();
            neurons = new List<SequenceNeuron>();
            sequence = new List<SequenceNeuron>();
        }

        public void setData(List<AnimatedNeuron> neurons, List<AnimatedReceptor> receptors)
        {
            //foreach (AnimatedReceptor ar in receptors)
                //elements.Add(new SequenceReceptor(ar));

            foreach (AnimatedNeuron an in neurons)
                this.neurons.Add(new SequenceNeuron(an));

            disappear = false;
        }

        public void tick(int frame)
        {
            if (animation)
                frame++;

            if (disappear)
            {
                sequence.Clear();
                disappear = false;
            }

            int count = sequence.Count;

            foreach (SequenceNeuron sn in neurons)
                if (sn.tick(frame))
                    sequence.Add(sn);

            if (sequence.Count == count)
            {
                if(count == 1)
                    sequence.Clear();
                else
                {
                    foreach (SequenceNeuron sn in sequence)
                        sn.disappear();

                    disappear = true;
                }
           }
                

            foreach (SequenceNeuron sn in neurons)
            {
                if (sequence.Contains(sn))
                    sn.add(true);
                else
                    sn.add(false);
            }
        }

        void undo()
        {
            sequence.Clear();

            foreach (SequenceNeuron sn in neurons)
                if (sn.undo())
                    sequence.Add(sn);

        }

        void clear()
        {
            sequence.Clear();
        }

        public void animate(bool value)
        {
            animation = value;
        }

        protected override void changeSize()
        {
            layer.Height = 48;
            layer.Width = layer.Parent.Width - 148;
            initializeGraphics();
        }

        protected override void tick(object sender, EventArgs e)
        {
            buffer.Graphics.Clear(Color.FromArgb(255, 225, 225, 225));

            SequenceNeuron.Count = 0;
            SequenceReceptor.Count = 0;

            foreach (SequenceNeuron sn in sequence)
                sn.draw(buffer.Graphics);
            /*
            foreach (SequenceReceptor sr in receptors)
                sr.draw(buffer.Graphics);*/

            buffer.Render(graphics);
        }

        public void frameChanged(object sender, FrameEventArgs e)
        {
            if(e.Frame == 1)
            {
                frame = 1;
                clear();
                return;
            }

            if (e.Frame > frame)
                tick(e.Frame);
            else
                undo();

            frame = e.Frame;
        }
    }

    

    class SequenceNeuron : SequenceElement
    {
        AnimatedNeuron neuron;
        static int count;

        List<bool> activity;
        bool fade = false;
        int frame = 1;

        public SequenceNeuron(AnimatedNeuron neuron) : base(neuron)
        {
            this.neuron = neuron;
            activity = new List<bool>();

            brush = Brushes.GreenYellow;
            pen = Pens.Thistle;
        }

        public override void draw(Graphics g)
        {
            if (fade)
                brush = Brushes.LightSkyBlue;

            if (frame++ == 20)
            {
                fade = false;
                brush = Brushes.GreenYellow;
            } 

            x = 40 + 100 * count++;
            base.draw(g);
        }

        public void disappear()
        {
            fade = true;
            frame = 1;
        }

        public bool tick(int frame)
        {
            bool result = neuron.Activity[frame - 1].Active;
            return result;
        }

        public bool undo()
        {
            activity.RemoveAt(activity.Count - 1);
            return activity.Last();
        }

        public void add(bool value)
        {
            activity.Add(value);
        }

        public static int Count
        {
            set
            {
                count = value;
            }
        }
    }

    class SequenceReceptor : SequenceElement
    {
        AnimatedReceptor receptor;
        static int count;

        public SequenceReceptor(AnimatedReceptor receptor) : base(receptor)
        {
            this.receptor = receptor;

            brush = Brushes.Orchid;
            pen = Pens.Purple;
        }

        public override void draw(Graphics g)
        {
            x = 40 + 80 * count++;
            base.draw(g);
        }

        public static int Count
        {
            set
            {
                count = value;
            }
        }
    }

    abstract class SequenceElement
    {
        protected Brush brush;
        protected Pen pen;

        protected Font font;
        protected StringFormat format;

        protected AnimatedElement element;
        protected int x;

        public SequenceElement(AnimatedElement element)
        {
            this.element = element;
            font = new Font("Arial", 12, FontStyle.Bold);

            format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;
        }

        public virtual void draw(Graphics g)
        {
            int y = 8;

            Rectangle rect = new Rectangle(x, y, 80, 32);
            g.FillRectangle(brush, rect);
            g.DrawRectangle(pen, rect);
            g.DrawString(element.Name, font, Brushes.CadetBlue, rect, format);
        }
    }
}
