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

        protected Display display;
        protected ShiftedNeuron shift;

        protected static Brain brain;
        protected static Size size;
        protected static Rectangle area;

        protected static int frames;
        protected bool animation;

        int density = 20;

        public static event EventHandler factorChanged;
        public static event EventHandler sizeChanged;

        #endregion

        public Presentation(Display display)
        {
            this.display = display;
            display.Controls.Add(this);

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

        public override void resize()
        {
            base.resize();

            if (brain == null)
                return;

            balanceSize();

            if (area.Height != 0)
            {
                float factor = (float)Height / area.Height;

                if (Height > Width)
                    factor = (float)Width / area.Width;

                display.calculateShift(factor);
            }

            area.Width = Width;
            area.Height = Height;

            AnimatedElement.Graphics = buffer.Graphics;
            AnimatedElement.Area = area;

            sizeChanged(this, null);
        }

        protected void balanceSize()
        {
            int optimum = (int)Math.Pow(brain.Neurons.Count + 1, 1.5) * density;
            float min = Height;

            if (Width < min)
                min = Width;

            if (optimum > min)
            {
                size.Width = optimum;
                size.Height = optimum;

                factorChanged((int)(min * 100 / optimum), null);
                sizeChanged(this, null);
            }
            else
                factorChanged(1, null);
        }

        public void changeDensity(int value)
        {
            int width = size.Width;
            density = (int)Math.Sqrt(100 * value);
            balanceSize();

            float factor = (float)size.Width / width;
            display.changeSize(factor);
        }

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

        public virtual void add(char key) { }

        public virtual void erase() { }

        public virtual void enter() { }

        public virtual void space() { }

        public virtual void delete() { }

        #endregion

        #region właściwości

        public Brain Brain
        {
            set
            {
                brain = value;
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