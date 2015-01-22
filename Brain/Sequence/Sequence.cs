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
        SequenceBar sequenceBar;

        public Sequence()
        {
            sequence = new List<SequenceElement>();
            BackColor = Color.FromArgb(255, 225, 225, 225);
        }

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

        public void show(SequenceBar bar, SequenceBarType type)
        {
            Height = bar.Height / 2;
            Width = bar.Width;

            sequenceBar = bar;
            bar.Controls.Add(this);
            base.show();

            switch(type)
            {
                case SequenceBarType.Lower:
                    Location = new Point(0, Height);
                    break;

                case SequenceBarType.Upper:
                    Location = new Point(0, 0);
                    break;
            }
        }

        public override void hide()
        {
            sequenceBar.Controls.Remove(this);
            base.hide();
        }

        protected override void tick(object sender, EventArgs e)
        {
            foreach (SequenceElement element in sequence)
                element.draw();
        }

        public int Count
        {
            get
            {
                return sequence.Count;
            }
        }
    }
}
