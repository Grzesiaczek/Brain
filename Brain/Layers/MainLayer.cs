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

        public MainLayer()
        {
            Location = new Point(10, 110);

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

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainLayer
            // 
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ResumeLayout(false);
        }
    }
}
