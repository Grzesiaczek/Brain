using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class SequenceNeuron : SequenceElement
    {
        Neuron neuron;

        List<bool> activity;
        bool fade = false;

        int frame = 1;
        int interval = 20;

        public SequenceNeuron(Neuron neuron) : base(neuron.Word)
        {
            this.neuron = neuron;
            activity = new List<bool>();
            changeType(SequenceElementType.Normal);
        }

        /*protected override void tick(object sender, EventArgs e)
        {
            if (fade)
                changeType(SequenceElementType.Activated);

            if (fade && frame++ == interval)
            {
                fade = false;
                changeType(SequenceElementType.Normal);
            }

            base.tick(sender, e);
        }*/

        public void disappear(int value)
        {
            fade = true;
            frame = 1;
            this.interval = value;
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

        public Neuron Neuron
        {
            get
            {
                return neuron;
            }
        }
    }
}
