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
    class Layer : UserControl, Drawable
    {
        protected Graphics graphics;
        protected System.Windows.Forms.Timer timer;

        protected BufferedGraphics buffer;
        BufferedGraphicsContext context;

        public Layer()
        {
            Visible = false;
            SetStyle(ControlStyles.UserPaint, true);
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
        
        public virtual void show()
        {
            resize();
            timer.Start();
            Visible = true;
        }

        public virtual void hide()
        {
            Visible = false;
            timer.Stop();
        }

        public virtual void save(){ }

        public virtual void resize()
        {
            initializeGraphics();
        }

        protected virtual void tick(object sender, EventArgs e) { }
    }
}
