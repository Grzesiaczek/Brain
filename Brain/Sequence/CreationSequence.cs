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

        public CreationSequence(List<CreationFrame> frames)
        {
            foreach (CreationFrame frame in frames)
            {
                sequence.Add(frame.Neuron);
                Controls.Add(frame.Neuron);
                frame.Neuron.Visible = true;
            }

            this.frames = frames;
            arrange();
        }

        public CreationSequence()
        {
            frames = new List<CreationFrame>();
        }

        public CreationFrame get(int index)
        {
            return frames[index];
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
