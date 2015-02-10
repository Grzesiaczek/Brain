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

        public void add(char key)
        {
            built.Append(key);
            rename();
        }

        public bool erase()
        {
            built.Remove(built.Length - 1, 1);
            rename();

            if (built.Length == 0)
                return true;

            return false;
        }

        void rename()
        {
            name = built.ToString();
            width = 16 + (int)(8.5 * name.Length);
            rect.Width = width;
        }

        #endregion

        #region właściwości

        public int Length
        {
            get
            {
                return built.Length;
            }
        }

        #endregion
    }
}
