using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    class Presentation : Layer
    {
        #region deklaracje

        protected ShiftedNeuron shift;
        protected static Size size;
        protected static Padding padding;
        protected static Rectangle area;

        protected bool animation;

        #endregion

        public Presentation()
        {
            MouseDown += new System.Windows.Forms.MouseEventHandler(this.mouseDown);
            MouseUp += new System.Windows.Forms.MouseEventHandler(this.mouseUp);
            MouseMove += new System.Windows.Forms.MouseEventHandler(this.mouseMove);
        }

        #region obsługa zdarzeń myszy

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

        #endregion

        #region funkcje sterujące

        public bool started()
        {
            return animation;
        }

        public virtual void start() { }

        public virtual void stop() { }

        public virtual void forth() { }

        public virtual void back() { }

        public virtual void changeFrame(int frame) { }

        public virtual void changePace(int pace) { }

        public virtual void add(int key) { }

        public virtual void erase() { }

        public virtual void confirm() { }

        public virtual void space() { }

        #endregion

        #region właściwości

        public Padding Padding
        {
            set
            {
                padding = value;
                AnimatedElement.Padding = new PointF(padding.Left, padding.Top);
            }
        }

        public Size Size
        {
            get
            {
                return size;
            }
        }

        #endregion
    }
}