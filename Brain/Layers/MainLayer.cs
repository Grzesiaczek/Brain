using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    abstract class MainLayer : Layer
    {
        protected ShiftedNeuron shift;
        protected bool sequenceBar = true;

        public override void resize()
        {
            Height = Parent.Height - 58;
            Width = Parent.Width - 168;

            if (sequenceBar)
                Height -= 100;

            initializeGraphics();
        }

        protected override void tick(object sender, EventArgs e)
        {

        }

        public MainLayer(Control parent) : base(parent)
        {
            if (sequenceBar)
                Location = new Point(10, 110);
            else
                Location = new Point(10, 10);

            MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseClick);
            MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mouseClick);
            MouseDown += new System.Windows.Forms.MouseEventHandler(this.mouseDown);
            MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseUp);
            MouseMove += new System.Windows.Forms.MouseEventHandler(this.mouseMove);
        }

        private void mouseClick(object sender, MouseEventArgs e)
        {
            if (shift != null && mode == Mode.Manual)
                shift.activate();
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

            //neuronShifted(this, new EventArgs());
        }

        public event EventHandler neuronShifted;
    }
}
