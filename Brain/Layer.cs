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

        protected virtual void visibilityChanged(object sender, EventArgs e)
        {
            if (!layer.Visible)
            {
                timer.Stop();
                return;
            }

            layer.Height = layer.Parent.Height - 58;
            layer.Width = layer.Parent.Width - 148;

            timer.Start();
        }

        public virtual void resize()
        {
            layer.Height = layer.Parent.Height - 58;
            layer.Width = layer.Parent.Width - 148;

            if (mode == Mode.Query)
                layer.Height -= 60;

            initializeGraphics();
        }

        public void setMode(Mode mode)
        {
            Mode old = this.mode;
            this.mode = mode;

            if(mode == Mode.Query)
            {
                layer.Location = new Point(10, 70);
                resize();
            }            
            else if (old == Mode.Query)
            {
                layer.Location = new Point(10, 10);
                resize();
            }
        }

        protected abstract void tick(object sender, EventArgs e);
    }
}
