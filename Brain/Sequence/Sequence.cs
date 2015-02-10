using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    class Sequence : Layer
    {
        protected List<SequenceElement> sequence;
        protected BuiltElement builder;
        protected int position = 10;

        public Sequence()
        {
            sequence = new List<SequenceElement>();
            BackColor = Color.FromArgb(255, 225, 225, 225);
        }

        #region logika

        public void add(SequenceElement element)
        {
            sequence.Add(element);
        }

        public virtual void clear()
        {
            sequence.Clear();
            Controls.Clear();
        }

        public void arrange()
        {
            int position = 10;

            foreach(SequenceElement element in sequence)
            {
                element.Top = 8;
                element.Left = position;
                position = element.Right + 10;
            }
        }

        #endregion

        #region budowa

        public void add(char key)
        {
            if (builder == null)
            {
                builder = new BuiltElement("");
                builder.Left = position;
                builder.Top = 8;
            }

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

        #endregion

        #region funkcje klasy bazowej

        public override void resize()
        {
            Height = 50;
            Width = Parent.Width - margin.Horizontal + 30;
            initializeGraphics();
        }

        public override void show()
        {
            base.show();
            SequenceElement.Graphics = buffer.Graphics;
        }

        protected override void tick(object sender, EventArgs e)
        {
            buffer.Graphics.Clear(BackColor);

            foreach (SequenceElement element in sequence)
                element.draw();

            if (builder != null)
                builder.draw();

            buffer.Render(graphics);
        }

        #endregion

        #region właściwości

        public int Count
        {
            get
            {
                return sequence.Count;
            }
        }

        #endregion
    }
}
