using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    abstract class Layer
    {
        protected GroupBox layer;
        protected Graphics graphics;
        protected System.Windows.Forms.Timer timer;

        protected Mode mode;
        protected BufferedGraphics buffer;
        BufferedGraphicsContext context;

        public Layer(GroupBox groupBox)
        {
            mode = Mode.Auto;
            layer = groupBox;
            layer.VisibleChanged += new EventHandler(visibilityChanged);
            layer.Location = new Point(10, 10);

            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(tick);
            timer.Interval = 25;
        }

        protected void initializeGraphics()
        {
            graphics = layer.CreateGraphics();
            graphics.FillRectangle(SystemBrushes.Control, graphics.VisibleClipBounds);

            int height = (int)graphics.VisibleClipBounds.Height;
            int width = (int)graphics.VisibleClipBounds.Width;

            context = BufferedGraphicsManager.Current;
            context.MaximumBuffer = new Size(width + 1, height + 1);

            buffer = context.Allocate(graphics, new Rectangle(0, 0, width, height));
            buffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        }

        public void setMode(Mode mode)
        {
            this.mode = mode;
        }

        public virtual void resize()
        {
            changeSize();
        }

        protected virtual void visibilityChanged(object sender, EventArgs e)
        {
            if (!layer.Visible)
            {
                timer.Stop();
                return;
            }

            resize();
            timer.Start();
        }

        protected abstract void changeSize();

        protected abstract void tick(object sender, EventArgs e);
    }
}
