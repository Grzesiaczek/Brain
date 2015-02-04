using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class SequenceReceptor : SequenceElement
    {
        Receptor receptor;
        bool active;

        public SequenceReceptor(Sequence sequence, Receptor receptor)
        {
            this.receptor = receptor;
            name = receptor.Name;

            Location = new Point(10 + 90 * sequence.Count, 8);

            sequence.add(this);
            changeType(SequenceElementType.Receptor);
        }

        public void tick(int frame)
        {
            if(frame == 0)
            {
                active = false;
                return;
            }

            active = receptor.Activity[frame - 1];

            if(active)
                changeType(SequenceElementType.ActiveReceptor);
            else
                changeType(SequenceElementType.Receptor);
        }
    }
}
