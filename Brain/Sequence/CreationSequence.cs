using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class CreationSequence : Sequence
    {
        List<CreationFrame> frames;

        public CreationSequence()
        {
            frames = new List<CreationFrame>();
        }

        public CreationSequence(List<CreationFrame> frames)
        {
            foreach (CreationFrame frame in frames)
                sequence.Add(frame.Neuron);

            this.frames = frames;
            arrange();
        }

        public CreationFrame get(int index)
        {
            return frames[index];
        }

        public CreationFrame add(Brain brain, int index)
        {
            if (builder == null)
                return null;

            CreationFrame frame = brain.add(this, builder, index);
            SequenceNeuron neuron = frame.Neuron;

            neuron.Top = 8;
            neuron.Left = builder.Left;
            neuron.changeType(SequenceElementType.Normal);
            position = neuron.Right + 10;

            builder = null;
            sequence.Add(neuron);
            frames.Add(frame);
            return frame;
        }

        public List<CreationFrame> Frames
        {
            get
            {
                return frames;
            }
        }
    }
}
