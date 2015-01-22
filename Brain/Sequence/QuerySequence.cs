using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class QuerySequence : Sequence
    {
        public void tick(int frame)
        {
            foreach (SequenceElement element in sequence)
                ((SequenceReceptor)element).tick(frame);
        }
    }
}
