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
        Animation animation;
        List<SequenceElement> elements;
        List<SequenceElement> active;

        public Sequence(GroupBox groupBox) : base(groupBox)
        {
            //resize();
            elements = new List<SequenceElement>();
            elements.Add(new SequenceNeuron());
            elements.Add(new SequenceReceptor());
        }

        protected override void tick(object sender, EventArgs e)
        {
            redraw();
        }

        public void redraw()
        {
            foreach (SequenceElement sel in active)
                sel.draw();
        }

        public void tick(int frame)
        {
            active.Clear();
            SequenceElement.Count = 0;

            foreach (SequenceElement sel in elements)
                if (sel.active(frame))
                    active.Add(sel);
        }
    }

    abstract class SequenceElement
    {
        static protected int count;
        protected Pen pen;

        public virtual void draw()
        {
            RectangleF rectangle = new Rectangle(10 + 60 * count, 10, 60, 40);
            count++;
        }

        public abstract bool active(int frame);

        public static int Count
        {
            set
            {
                count = value;
            }
        }
    }

    class SequenceNeuron : SequenceElement
    {
        AnimatedNeuron neuron;

        public SequenceNeuron()
        {
            pen = Pens.Thistle;
        }

        public override void draw()
        {
            
        }

        public override bool active(int frame)
        {
            return neuron.Activity[frame - 1].Active;
        }
    }

    class SequenceReceptor : SequenceElement
    {
        AnimatedReceptor receptor;

        public SequenceReceptor(AnimatedReceptor receptor)
        {
            this.receptor = receptor;
            pen = Pens.Purple;
        }

        public override void draw()
        {
            
        }

        public override bool active(int frame)
        {
            return receptor.Activity[frame - 1];
        }
    }
}
