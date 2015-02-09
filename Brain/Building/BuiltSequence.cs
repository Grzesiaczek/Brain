using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class BuiltSequence : Sequence
    {
        BuiltElement builder;

        public void add(int key)
        {
            builder.add(key);
        }

        public bool erase()
        {
            if (!builder.erase())
                return false;

            if (sequence.Count == 0)
                return true;

            SequenceElement last = sequence.Last<SequenceElement>();
            builder = new BuiltElement(last);
            sequence.Remove(last);

            return false;
        }

        public void addNeuron()
        {

        }

        public CreationSequence getCreation()
        {
            CreationSequence result = new CreationSequence();

            return result;
        }

        public void getQuery()
        {

        }
    }
}
