using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    class ResultSequence : Sequence
    {
        List<SequenceNeuron> neurons;

        int frame;
        int interval;

        bool animation;
        bool disappear;

        public ResultSequence()
        {
            neurons = new List<SequenceNeuron>();
        }

        public void setData(List<AnimatedNeuron> neurons)
        {
            foreach (AnimatedNeuron an in neurons)
                this.neurons.Add(new SequenceNeuron(an.Neuron));

            disappear = false;
        }

        public void setInterval(int value)
        {
            interval = value;
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
                    add(sn);

            if (sequence.Count == count)
            {
                if (count == 1)
                    clear();
                else if (animation)
                {
                    foreach (SequenceNeuron sn in sequence)
                        sn.disappear(interval);

                    disappear = true;
                }
                else
                    clear();
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

        public override void clear()
        {
            neurons.Clear();
            base.clear();
        }

        public void animate(bool value)
        {
            animation = value;
        }

        public void frameChanged(object sender, EventArgs e)
        {
            int frame = (int)sender;

            if(frame == 1)
            {
                frame = 1;
                clear();
                return;
            }

            if (frame > this.frame)
                tick(frame);
            else
                undo();

            this.frame = frame;
        }
    }
}
