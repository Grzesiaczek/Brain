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

        public MainLayer(GroupBox groupBox) : base(groupBox) 
        {
            layer.Location = new Point(10, 110);

            layer.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mouseClick);
            layer.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mouseClick);
            layer.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mouseDown);
            layer.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseUp);
            layer.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mouseMove);
        }

        private void mouseClick(object sender, MouseEventArgs e)
        {
            if (shift != null && mode == Mode.Manual)
                shift.activate();
        }

        protected abstract void mouseDown(object sender, MouseEventArgs e);

        protected virtual void mouseMove(object sender, MouseEventArgs e)
        {
            if (shift == null)
                return;

            shift.move(e.X, e.Y);
            redraw();
        }

        private void mouseUp(object sender, MouseEventArgs e)
        {
            if (shift == null)
                return;

            shift.save();
            shift = null;

            //neuronShifted(this, new EventArgs());
            redraw();
        }

        protected abstract void animate();
        protected abstract void redraw();

        public event EventHandler neuronShifted;
    }
}
