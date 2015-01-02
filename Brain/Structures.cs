using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Brain
{
    enum Mode { Auto, Creation, Manual, Query }

    class NeuronData
    {
        bool active;
        double initial;
        double impulse;
        double relaxation;
        double original;
        double value;

        public NeuronData(bool active, double initial, double impulse, double relaxation, double value)
        {
            this.active = active;
            this.initial = initial;
            this.impulse = impulse;
            this.relaxation = relaxation;
            this.value = value;
            original = value;
        }

        public NeuronData(BinaryReader reader)
        {
            active = reader.ReadBoolean();
            initial = reader.ReadDouble();
            impulse = reader.ReadDouble();
            relaxation = reader.ReadDouble();
            value = reader.ReadDouble();
        }

        public NeuronData()
        {
            active = false;
            initial = 0;
            impulse = 0;
            relaxation = 0;
            value = 0;
        }

        public void save(BinaryWriter writer)
        {
            writer.Write(active);
            writer.Write(initial);
            writer.Write(impulse);
            writer.Write(relaxation);
            writer.Write(value);
        }

        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
            }
        }

        public double Initial
        {
            get
            {
                return initial;
            }
            set
            {
                initial = value;
            }
        }

        public double Impulse
        {
            get
            {
                return impulse;
            }
            set
            {
                impulse = value;
            }
        }

        public double Relaxation
        {
            get
            {
                return relaxation;
            }
            set
            {
                relaxation = value;
            }
        }

        public double Original
        {
            get
            {
                return original;
            }
            set
            {
                original = value;
            }
        }

        public double Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }
    }

    class ReceptorData
    {
        String word;
        int alpha;
        int beta;
        double value;

        public ReceptorData(XmlNode node)
        {
            word = node.InnerText;

            try
            {
                alpha = Int32.Parse(node.Attributes["alpha"].Value);
                beta = Int32.Parse(node.Attributes["beta"].Value);
                value = Double.Parse(node.Attributes["value"].Value, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch(Exception)
            {
                alpha = 6;
                beta = 2;
                value = 0.8;
            }
        }

        public String Word
        {
            get
            {
                return word;
            }
            set
            {
                word = value;
            }
        }

        public int Alpha
        {
            get
            {
                return alpha;
            }
            set
            {
                alpha = value;
            }
        }

        public int Beta
        {
            get
            {
                return beta;
            }
            set
            {
                beta = value;
            }
        }

        public double Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }
    }

    class FrameEventArgs : EventArgs
    {
        int frame;

        public FrameEventArgs(int frame)
        {
            this.frame = frame;
        }

        public int Frame
        {
            get
            {
                return frame;
            }
        }
    }

    class CreationData
    {
        Synapse synapse;
        int frame;

        float start;
        float finish;
        float step;

        public CreationData(Synapse synapse, int frame, float start, float finish)
        {
            this.synapse = synapse;
            this.frame = frame;
            this.start = start;
            this.finish = finish;
        }

        public void draw(Graphics g, int line)
        {
            Pen pen = new Pen(Brushes.DarkSlateGray, 2);
            float y = 50 + 36 * line;

            Circle left = new Circle(new PointF(64, y), 12);
            Circle right = new Circle(new PointF(100, y), 12);

            left.draw(g, start, pen);
            right.draw(g, finish, pen);

            int change = (int)((finish - start) * 100);
            Brush brush = Brushes.Red;
            y += 2;

            if (change > 0)
                brush = Brushes.Green;
            else
                change = -change;

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            g.DrawString(frame.ToString(), new Font("Calibri", 14, FontStyle.Bold), Brushes.DarkSlateGray, 28, y, format);
            g.DrawString(change.ToString(), new Font("Arial", 12, FontStyle.Bold), brush, 136, y, format);
        }

        public Synapse Synapse
        {
            get
            {
                return synapse;
            }
        }

        public float Step
        {
            get
            {
                return step;
            }
            set
            {
                step = value;
            }
        }

        public float Start
        {
            get
            {
                return start;
            }
        }

        public float Weight
        {
            get
            {
                return finish;
            }
        }
    }
}