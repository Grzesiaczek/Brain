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
        SequenceNeuron active;

        bool paused;
        int index;

        public CreationSequence(List<CreationFrame> frames)
        {
            this.frames = frames;
            int col = 0;

            active = null;
            index = -1;

            foreach (CreationFrame frame in frames)
            {
                frame.Neuron.Location = new Point(10 + 100 * col++, 8);
                sequence.Add(frame.Neuron);
                Controls.Add(frame.Neuron);
            }
        }

        public CreationFrame next()
        {
            if(paused)
            {
                paused = false;
                return frames[index];
            }

            if (++index == frames.Count)
            {
                index = -1;
                return null;
            }

            if (active != null)
                active.changeType(SequenceElementType.Normal);

            active = frames[index].Neuron;
            active.changeType(SequenceElementType.Active);

            return frames[index];
        }

        public void pause()
        {
            paused = true;
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
