using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    abstract class Sequence : Layer
    {
        protected List<SequenceElement> sequence;

        public Sequence()
        {
            sequence = new List<SequenceElement>();
            BackColor = Color.FromArgb(255, 225, 225, 225);
        }

        #region logika

        public void add(SequenceElement element)
        {
            sequence.Add(element);
            Controls.Add(element);
        }

        public virtual void clear()
        {
            sequence.Clear();
            Controls.Clear();
        }

        #endregion

        #region funkcje klasy bazowej

        public override void resize()
        {
            Height = 50;
            Width = Parent.Width - margin.Horizontal + 30;
            initializeGraphics();
        }

        public override void hide()
        {
            Parent.Controls.Remove(this);
            base.hide();
        }

        protected override void tick(object sender, EventArgs e)
        {
            foreach (SequenceElement element in sequence)
                element.draw();
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
