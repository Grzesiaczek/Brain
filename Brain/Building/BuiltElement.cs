using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brain
{
    class BuiltElement : SequenceElement
    {
        StringBuilder built;

        #region konstruktory

        public BuiltElement(String name) : base(name)
        {
            changeType(SequenceElementType.Built);
            built = new StringBuilder(name);
        }

        public BuiltElement(SequenceElement element) : base(element.Name)
        {
            changeType(SequenceElementType.Built);
            built = new StringBuilder(name);
        }

        #endregion

        #region logika

        public void add(int key)
        {
            built.Append(key);
            name = built.ToString();
        }

        public bool erase()
        {
            built.Remove(built.Length - 1, 1);
            name = built.ToString();

            if (built.Length == 0)
                return true;

            return false;
        }

        public SequenceNeuron getNeuron()
        {
            return null;
        }

        #endregion
    }
}
