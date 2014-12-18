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

    class Circle
    {
        PointF center;
        PointF position;
        PointF border;

        float radius;
        float diameter;

        public Circle(PointF center, float radius)
        {
            this.center = center;
            this.radius = radius;

            position = new PointF(center.X - radius, center.Y - radius);
            border = new PointF(position.X - 1, position.Y - 1);
            diameter = 2 * radius;
        }

        public bool click(PointF pos)
        {
            double x = pos.X - center.X;
            double y = pos.Y - center.Y;
                
            if(Math.Sqrt(x*x + y*y) < radius)
                return true;

            return false;
        }

        public void update(PointF center)
        {
            this.center = center;

            position = new PointF(center.X - radius, center.Y - radius);
            border = new PointF(position.X - 1, position.Y - 1);
        }

        public void draw(Graphics g, double value, Pen pen, String val)
        {
            draw(g, value, pen);
            drawState(g, val);
        }

        public void draw(Graphics g, Brush brush, Pen pen, String val)
        {
            draw(g, brush, pen);
            drawState(g, val);
        }

        public void draw(Graphics g, double value, Pen pen)
        {
            Brush inner = Brushes.LightYellow;
            Brush outer = Brushes.LightGreen;

            if (value < 0)
            {
                value = -value;
                inner = Brushes.LightSkyBlue;
                outer = Brushes.LightYellow;
            }

            g.FillEllipse(outer, position.X, position.Y, diameter, diameter);
            g.DrawEllipse(pen, border.X, border.Y, diameter + 2, diameter + 2);

            float r = radius * (1 - (float)value);
            float d = 2 * r; 
            float x = center.X - r;
            float y = center.Y - r;

            g.FillEllipse(inner, x, y, d, d);
        }

        void drawState(Graphics g, String val)
        {
            PointF position = new PointF((float)(center.X + 0.6),center.Y + 1);

            if (val[0] == '-')
                position.X -= 1.2f;

            StringFormat format = new StringFormat();
            format.LineAlignment = StringAlignment.Center;
            format.Alignment = StringAlignment.Center;
            g.DrawString(val, new Font("Arial", Config.Diameter / 4 + 3, FontStyle.Bold), Brushes.DarkSlateGray, position, format);
        }

        public void draw(Graphics g, Brush brush, Pen pen)
        {
            g.FillEllipse(brush, position.X, position.Y, diameter, diameter);
            g.DrawEllipse(pen, border.X, border.Y, diameter + 2, diameter + 2);
        }

        public PointF Center
        {
            get
            {
                return center;
            }
            set
            {
                center = value;
            }
        }
    }
}