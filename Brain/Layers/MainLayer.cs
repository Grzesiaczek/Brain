using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    class MainLayer : Layer
    {
        protected ShiftedNeuron shift;
        static SequenceBar sequenceBar;

        public MainLayer()
        {
            MouseDown += new System.Windows.Forms.MouseEventHandler(this.mouseDown);
            MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseUp);
            MouseMove += new System.Windows.Forms.MouseEventHandler(this.mouseMove);
        }

        public void relocate()
        {
            if (sequenceBar.Active)
                Location = new Point(10, 110);
            else
                Location = new Point(10, 10);

            resize();
        }

        public override void resize()
        {
            Height = Parent.Height - 58;
            Width = Parent.Width - 168;

            if (sequenceBar.Active)
                Height -= 100;

            initializeGraphics();
        }

        public override void show()
        {
            relocate();
            timer.Start();
            Visible = true;
        }

        protected virtual void mouseDown(object sender, MouseEventArgs e) { }

        protected virtual void mouseMove(object sender, MouseEventArgs e)
        {
            if (shift == null)
                return;

            shift.move(e.X, e.Y);
        }

        private void mouseUp(object sender, MouseEventArgs e)
        {
            if (shift == null)
                return;

            shift.save();
            shift = null;
        }

        public static SequenceBar SequenceBar
        {
            get
            {
                return sequenceBar;
            }
            set
            {
                sequenceBar = value;
            }
        }
    }
}