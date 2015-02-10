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

        protected Padding margin;
        protected BufferedGraphics buffer;
        BufferedGraphicsContext context;

        public Layer()
        {
            Visible = false;
            SetStyle(ControlStyles.UserPaint, true);
            BackColor = SystemColors.Control;

            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(tick);
            timer.Interval = 25;
        }

        protected virtual void initializeGraphics()
        {
            Graphics graphics;
            BufferedGraphics buffer;

            graphics = CreateGraphics();
            graphics.FillRectangle(SystemBrushes.Control, graphics.VisibleClipBounds);

            context = BufferedGraphicsManager.Current;
            context.MaximumBuffer = new Size(Width + 1, Height + 1);

            buffer = context.Allocate(graphics, new Rectangle(0, 0, Width, Height));
            buffer.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            buffer.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            this.graphics = graphics;
            this.buffer = buffer;
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
            Height = Parent.Height - margin.Vertical;
            Width = Parent.Width - margin.Horizontal;
            initializeGraphics();
        }

        protected virtual void tick(object sender, EventArgs e) { }

        public void setMargin(Padding margin)
        {
            this.margin = margin;
            Location = new Point(margin.Left, margin.Top);

            Height = Parent.Height - margin.Vertical;
            Width = Parent.Width - margin.Horizontal;
        }
    }
}
