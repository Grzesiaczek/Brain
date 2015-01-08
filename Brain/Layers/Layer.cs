using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brain
{
    abstract class Layer : Control
    {
        protected Graphics graphics;
        protected System.Windows.Forms.Timer timer;

        protected Mode mode;
        protected BufferedGraphics buffer;
        BufferedGraphicsContext context;

        public Layer(Control parent)
        {
            if(parent != null)
                parent.Controls.Add(this);

            mode = Mode.Auto;
            Visible = false;
            Location = new Point(10, 10);
            VisibleChanged += new EventHandler(visibilityChanged);

            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(tick);
            timer.Interval = 25;
        }

        protected virtual void initializeGraphics()
        {
            graphics = CreateGraphics();
            graphics.FillRectangle(SystemBrushes.Control, graphics.VisibleClipBounds);

            context = BufferedGraphicsManager.Current;
            context.MaximumBuffer = new Size(Width + 1, Height + 1);

            buffer = context.Allocate(graphics, new Rectangle(0, 0, Width, Height));
            buffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        }

        public void setMode(Mode mode)
        {
            this.mode = mode;
        }
        
        protected virtual void visibilityChanged(object sender, EventArgs e)
        {
            if (!Visible)
            {
                timer.Stop();
                return;
            }

            resize();
            timer.Start();
        }

        public abstract void resize();

        protected abstract void tick(object sender, EventArgs e);
    }
}
