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
    enum Format { Button }
    enum Mode { Chart, Creation, Manual, Query }
    enum SequenceBarType { Lower, Upper }
    enum SequenceElementType { Normal, Active, Activated, Receptor, ActiveReceptor}

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
}